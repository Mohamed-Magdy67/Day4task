using Application.Responses;
using MediatR;

namespace Application.Commands
{
    public class CreateEmployeeCommand : IRequest<EmployeeResponse>
    {
        public string Name { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public int DepartmentId { get; set; }
    }
}
