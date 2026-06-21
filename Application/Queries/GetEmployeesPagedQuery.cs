using Application.Responses;
using MediatR;

namespace Application.Queries
{
    public class GetEmployeesPagedQuery : IRequest<PagedResult<EmployeeResponse>>
    {
        public string? Search { get; set; }
        public string? SortBy { get; set; }
        public bool IsDescending { get; set; } = false;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
