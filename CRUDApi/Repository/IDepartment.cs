using CRUDApi.Models;
using CRUDApi.Models.Common;

namespace CRUDApi.Repository
{
    public interface IDepartment
    {
        Task<IEnumerable<Department>> GetDepartments();
        Task<PagedResult<Department>> GetDepartmentsPaged(int pageNumber, int pageSize, string sortBy, string sortOrder);
        Task<Department> GetDepartment(int id);
        Task<Department> AddDepartment(Department dept);
        Task<Department> UpdateDepartment(Department dept);
        Task<Department> DeleteDepartment(int id);
    }
}
