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
        public async Task<ActionResult> Get()
        {
            var depts = await _dept.GetDepartments();
            return Ok(depts);
        }

        [HttpGet("paged")]
        public async Task<ActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string sortBy = "ID", [FromQuery] string sortOrder = "asc")
        {
            var paged = await _dept.GetDepartmentsPaged(pageNumber, pageSize, sortBy, sortOrder);
            return Ok(paged);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            var dept = await _dept.GetDepartment(id);
            return Ok(dept);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody]CreatePayload payload)
        {
            var createDept = await _dept.AddDepartment(payload);
            return Ok(createDept);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody]UpdatePayload payload)
        {
            if (id != payload.ID)
                return BadRequest();

            var updateDept = await _dept.UpdateDepartment(payload);
            return Ok(updateDept);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleteDept = await _dept.DeleteDepartment(id);
            return Ok(deleteDept);
        }
    }
}
