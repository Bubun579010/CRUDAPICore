using Payload.Common;
using Payload.Payload;

namespace Service.Service.Interface
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
