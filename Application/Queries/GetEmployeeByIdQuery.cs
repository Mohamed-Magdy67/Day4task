using Application.Responses;
using MediatR;

namespace Application.Queries
{
    public class GetEmployeeByIdQuery : IRequest<EmployeeResponse?>
    {
        public int Id { get; }

        public GetEmployeeByIdQuery(int id)
        {
            Id = id;
        }
    }
}
