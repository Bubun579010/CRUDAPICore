using CRUDApi.Models;
using CRUDApi.Models.Common;

namespace CRUDApi.Repository
{
    public interface IDepartment
    {
        Task<IEnumerable<DepartmentEntity>> GetDepartments();
        Task<PagedResult<DepartmentEntity>> GetDepartmentsPaged(int pageNumber, int pageSize, string sortBy, string sortOrder);
        Task<DepartmentEntity> GetDepartment(int id);
        Task<DepartmentEntity> AddDepartment(DepartmentEntity dept);
        Task<DepartmentEntity> UpdateDepartment(DepartmentEntity dept);
        Task<DepartmentEntity> DeleteDepartment(int id);
    }
}
