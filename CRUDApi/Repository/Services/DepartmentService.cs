using CRUDApi.Models;
using CRUDApi.Models.Common;
using CRUDApi.Models.Payload;

namespace CRUDApi.Repository.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IRepository<DepartmentEntity> _repository;

        public DepartmentService(IRepository<DepartmentEntity> repository)
        {
            _repository = repository;
        }

        private static GetDetails Details(DepartmentEntity entity) => new GetDetails
        {
            ID = entity.ID,
            Name = entity.Name,
            Description = entity.Description,
            Status = entity.Status
        };

        public async Task<IEnumerable<GetDetails>> GetAllDepartments()
        {
            var depts = await _repository.Find(D => D.Status == true);
            var result = depts.Select(Details);
            return result;
        }

        public async Task<GetDetails> GetDepartmentById(int id)
        {
            var dept = await _repository.GetById(id);
            if (dept == null || dept.Status == false)
                throw new Exception($"Department with ID {id} not found.");
            return Details(dept);
        }


        public async Task<GetDetails> CreateDepartment(CreatePayload payload)
        {
            bool nameExists = await _repository.Exists(D => D.Name.ToLower() == payload.Name.ToLower());
            if (nameExists)
                throw new InvalidOperationException("Department Name already exists.");

            var dept = new DepartmentEntity
            {
                Name = payload.Name.ToUpper(),
                Description = payload.Description.ToUpper(),
                Status = payload.Status,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };
            await _repository.Add(dept);
            return Details(dept);
        }

        public async Task<GetDetails> UpdateDepartment(UpdatePayload payload)
        {
            var dept = await _repository.GetById(payload.ID);
            if (dept == null)
                throw new Exception();
            bool nameExists = await _repository.Exists(D => D.Name.ToLower() == payload.Name.ToLower() && D.ID != payload.ID);
            if (nameExists)
                throw new InvalidOperationException("Department Name already exists.");


            dept.Name = payload.Name.ToUpper();
            dept.Description = payload.Description.ToUpper();
            dept.Status = payload.Status;
            dept.ModifiedOn = DateTime.Now;

            await _repository.Update(dept);
            return Details(dept);
        }

        public async Task<GetDetails> DeleteDepartment(int id)
        {
            var dept = await _repository.GetById(id);
            if (dept == null)
                throw new Exception("Department not found.");

            dept.Status = false;
            dept.ModifiedOn = DateTime.Now;
            await _repository.Delete(dept);

            return Details(dept);
        }

        public async Task<PagedResult<GetDetails>> GetDepartmentsPaged(int pageNumber, int pageSize, string sortBy, string sortOrder)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;
            sortBy = string.IsNullOrWhiteSpace(sortBy) ? "ID" : sortBy.Trim();
            sortOrder = string.IsNullOrWhiteSpace(sortOrder) ? "asc" : sortOrder.Trim().ToLower();

            var (data, totalRecords) = await _repository.GetPagedAsync(D => D.Status == true, sortBy, sortOrder, pageNumber, pageSize);

            var result = data.Select(Details);

            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            return new PagedResult<GetDetails>
            {
                Data = result,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages
            };
        }
    }
}
