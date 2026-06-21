using Application.Queries;
using Application.Responses;
using AutoMapper;
using Core.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers.Queries
{
    public class GetEmployeesPagedQueryHandler : IRequestHandler<GetEmployeesPagedQuery, PagedResult<EmployeeResponse>>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

        public GetEmployeesPagedQueryHandler(IEmployeeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedResult<EmployeeResponse>> Handle(GetEmployeesPagedQuery request, CancellationToken cancellationToken)
        {
            // Set defaults to avoid edge cases
            var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
            var pageSize = request.PageSize < 1 ? 10 : request.PageSize;

            var (items, totalCount) = await _repository.GetEmployeesPagedAsync(
                request.Search,
                request.SortBy,
                request.IsDescending,
                pageNumber,
                pageSize);

            var mappedItems = _mapper.Map<IEnumerable<EmployeeResponse>>(items);

            return new PagedResult<EmployeeResponse>(mappedItems, totalCount, pageNumber, pageSize);
        }
    }
}
