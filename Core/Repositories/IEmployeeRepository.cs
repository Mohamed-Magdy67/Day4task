using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task<Employee> AddAsync(Employee employee);
        Task<bool> UpdateAsync(Employee employee);
        Task<bool> DeleteAsync(int id);
        Task<(IEnumerable<Employee> Items, int TotalCount)> GetEmployeesPagedAsync(
            string? search, 
            string? sortBy, 
            bool isDescending, 
            int pageNumber, 
            int pageSize);
    }
}
