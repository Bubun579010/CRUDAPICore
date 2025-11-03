using CRUDApi.Models;
using CRUDApi.Models.Common;
using CRUDApi.Models.Payload;
using CRUDApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CRUDApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartment _dept;
        public DepartmentController(IDepartment dept)
        {
            _dept = dept;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetDetails>>> Get()
        {
            var depts = await _dept.GetDepartments();
            var result = depts.Select(D => new GetDetails
            {
                ID = D.ID,
                Name = D.Name,
                Description = D.Description,
                Status = D.Status
            });
            return Ok(result);
        }

        [HttpGet("paged")]
        public async Task<ActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string sortBy = "ID", [FromQuery] string sortOrder = "asc")
        {
            var paged = await _dept.GetDepartmentsPaged(pageNumber, pageSize, sortBy, sortOrder);
            var details = paged.Data.Select(D => new GetDetails
            {
                ID = D.ID,
                Name = D.Name,
                Description = D.Description,
                Status = D.Status
            });

            var result = new PagedResult<GetDetails>
            {
                Data = details.ToList(),
                TotalRecords = paged.TotalRecords,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
                TotalPages = paged.TotalPages
            };
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetDetails>> Get(int id)
        {
            var dept = await _dept.GetDepartment(id);

            var result = new GetDetails
            {
                ID = dept.ID,
                Name = dept.Name,
                Description = dept.Description,
                Status = dept.Status
            };
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<CreatePayload>> Create([FromBody]CreatePayload payload)
        {
            var entiry = new DepartmentEntity
            {
                Name = payload.Name.ToUpper(),
                Description = payload.Description.ToUpper(),
                Status = payload.Status
            };
            var createDept = await _dept.AddDepartment(entiry);

            var result = new GetDetails
            {
                ID = createDept.ID,
                Name = createDept.Name,
                Description = createDept.Description,
                Status = createDept.Status
            };
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<UpdatePayload>> Update(int id, [FromBody]UpdatePayload payload)
        {
            if (id != payload.ID)
                return BadRequest(new { error = "ID in URL and body do not match." });
            var entity = new DepartmentEntity
            {
                ID = payload.ID,
                Name = payload.Name.ToUpper(),
                Description = payload.Description.ToUpper(),
                Status = payload.Status
            };
            var updateDept = await _dept.UpdateDepartment(entity);

            var result = new GetDetails
            {
                ID = updateDept.ID,
                Name = updateDept.Name,
                Description = updateDept.Description,
                Status = updateDept.Status
            };
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<GetDetails>> Delete(int id)
        {
            var dept = await _dept.GetDepartment(id);
            if (dept == null)
                return NotFound();

            var deleteDept = await _dept.DeleteDepartment(id);
            if (deleteDept == null)
                return BadRequest();

            var result = new GetDetails
            {
                ID = deleteDept.ID,
                Name = deleteDept.Name,
                Description = deleteDept.Description,
                Status = deleteDept.Status
            };
            return Ok(result);
        }
    }
}
