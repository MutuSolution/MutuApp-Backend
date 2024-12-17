using Domain;

namespace Application.Services;

public interface IEmployeeService
{
    Task<Employee> CreateEmployeeAsync(Employee employee);
    Task<List<Employee>> GetEmployeeListAsync();
    Task<Employee> GetEmployeeByIdAsync(int id);
    Task<Employee> UpdateEmployeeAsync(Employee employee);
    Task<int> DeleteEmployeeAsync(Employee employee);
}
