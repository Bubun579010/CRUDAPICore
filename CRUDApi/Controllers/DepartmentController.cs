using CRUDApi.Models;
using CRUDApi.Models.Common;
using CRUDApi.Repository;
using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<IEnumerable<Payload>>> Get()
        {
            var depts = await _dept.GetDepartments();
            var result = depts.Select(D => new Payload { ID = D.ID, Name = D.Name, Description = D.Description, Status = D.Status });
            return Ok(result);
        }

        [HttpGet("paged")]
        public async Task<ActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string sortBy = "ID", [FromQuery] string sortOrder = "asc")
        {
            var paged = await _dept.GetDepartmentsPaged(pageNumber, pageSize, sortBy, sortOrder);
            var payloads = paged.Data.Select(D => new Payload { ID = D.ID, Name = D.Name, Description = D.Description, Status = D.Status });

            var result = new PagedResult<Payload> { Data = payloads.ToList(), TotalRecords = paged.TotalRecords, PageNumber = paged.PageNumber, PageSize = paged.PageSize, TotalPages = paged.TotalPages };
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Payload>> Get(int id)
        {
            var dept = await _dept.GetDepartment(id);

            var result = new Payload { ID = dept.ID, Name = dept.Name, Description = dept.Description, Status = dept.Status };
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Payload>> Create(Payload payload)
        {
            if (payload == null)
                return BadRequest();
            var entiry = new DepartmentEntity { Name = payload.Name, Description = payload.Description, Status = payload.Status };
            var createDept = await _dept.AddDepartment(entiry);

            var result = new Payload { ID = createDept.ID, Name = createDept.Name, Description = createDept.Description, Status = createDept.Status };
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Payload>> Update(int id, Payload payload)
        {
            if (id != payload.ID)
                return BadRequest();
            var entity = new DepartmentEntity { ID = payload.ID, Name = payload.Name, Description = payload.Description, Status = payload.Status };
            var updateDept = await _dept.UpdateDepartment(entity);

            var result = new Payload { ID = updateDept.ID, Name = updateDept.Name, Description = updateDept.Description, Status = updateDept.Status };
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<DepartmentEntity>> Delete(int id)
        {
            var deleteDept = await _dept.GetDepartment(id);
            if (id == deleteDept.ID)
                return await _dept.DeleteDepartment(id);
            return Ok();
        }
    }
}
