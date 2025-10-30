using CRUDApi.Models;
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
        public async Task<ActionResult> Get()
        {
            return Ok(await _dept.GetDepartments());
        }

        [HttpGet("paged")]
        public async Task<ActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string sortBy = "ID", [FromQuery] string sortOrder = "asc")
        {
            var result = await _dept.GetDepartmentsPaged(pageNumber, pageSize, sortBy, sortOrder);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Department>> Get(int id)
        {
            return Ok(await _dept.GetDepartment(id));
        }

        [HttpPost]
        public async Task<ActionResult<Department>> Create(Department department)
        {
            if (department == null)
                return BadRequest();
            return await _dept.AddDepartment(department);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Department>> Update(int id, Department department)
        {
            if (id != department.ID)
                return BadRequest();
            var updateDept = await _dept.GetDepartment(id);
            if (updateDept == null)
                return NotFound();
            return await _dept.UpdateDepartment(department);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Department>> Delete(int id)
        {
            var deleteDept = await _dept.GetDepartment(id);
            if (id == deleteDept.ID)
                return await _dept.DeleteDepartment(id);
            return Ok();
        }
    }
}
