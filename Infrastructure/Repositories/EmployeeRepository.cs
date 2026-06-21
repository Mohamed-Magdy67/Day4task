using Core.Entities;
using Core.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees
                .Include(e => e.Department)
                .ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Employee> AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<bool> UpdateAsync(Employee employee)
        {
            var existing = await _context.Employees.FindAsync(employee.Id);
            if (existing == null) return false;

            existing.Name = employee.Name;
            existing.Salary = employee.Salary;
            existing.DepartmentId = employee.DepartmentId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Employees.FindAsync(id);
            if (existing == null) return false;

            _context.Employees.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(IEnumerable<Employee> Items, int TotalCount)> GetEmployeesPagedAsync(
            string? search, 
            string? sortBy, 
            bool isDescending, 
            int pageNumber, 
            int pageSize)
        {
            var query = _context.Employees.Include(e => e.Department).AsQueryable();

            // 1. Searching
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(e => e.Name.ToLower().Contains(search) || 
                                         (e.Department != null && e.Department.Name.ToLower().Contains(search)));
            }

            // 2. Sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy.Trim().ToLower())
                {
                    case "name":
                        query = isDescending ? query.OrderByDescending(e => e.Name) : query.OrderBy(e => e.Name);
                        break;
                    case "salary":
                        query = isDescending ? query.OrderByDescending(e => e.Salary) : query.OrderBy(e => e.Salary);
                        break;
                    case "departmentname":
                    case "department":
                        query = isDescending 
                            ? query.OrderByDescending(e => e.Department != null ? e.Department.Name : string.Empty) 
                            : query.OrderBy(e => e.Department != null ? e.Department.Name : string.Empty);
                        break;
                    default:
                        query = query.OrderBy(e => e.Id);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(e => e.Id); // Default sort
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // 3. Pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
