using CRUDApi.Models.Common;
using CRUDApi.Models.Payload;

namespace CRUDApi.Repository
{
    public interface IDepartmentService
    {
        Task<IEnumerable<GetDetails>> GetAllDepartments();
        Task<GetDetails> GetDepartmentById(int id);
        Task<GetDetails> CreateDepartment(CreatePayload payload);
        Task<GetDetails> UpdateDepartment(UpdatePayload payload);
        Task<GetDetails> DeleteDepartment(int id);
        Task<PagedResult<GetDetails>> GetDepartmentsPaged(int pageNumber, int pageSize, string sortBy, string sortOrder);
    }
}
