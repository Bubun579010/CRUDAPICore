using CRUDApi.Models.Common;
using CRUDApi.Models.Payload;

namespace CRUDApi.Repository
{
    public interface IDepartment
    {
        Task<IEnumerable<GetDetails>> GetDepartments();
        Task<PagedResult<GetDetails>> GetDepartmentsPaged(int pageNumber, int pageSize, string sortBy, string sortOrder);
        Task<GetDetails> GetDepartment(int id);
        Task<GetDetails> AddDepartment(CreatePayload payload);
        Task<GetDetails> UpdateDepartment(UpdatePayload payload);
        Task<GetDetails> DeleteDepartment(int id);
    }
}
