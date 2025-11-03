using CRUDApi.Data;
using CRUDApi.Models;
using CRUDApi.Models.Common;
using CRUDApi.Models.Payload;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace CRUDApi.Repository.Services
{
    public class DepartmentRepository : IDepartment
    {
        private readonly DeptDbContext _context;
        public DepartmentRepository(DeptDbContext context)
        {
            _context = context;
        }

        private static GetDetails Details(DepartmentEntity entity) => new GetDetails
        {
            ID = entity.ID,
            Name = entity.Name,
            Description = entity.Description, 
            Status = entity.Status
        };

        public async Task<IEnumerable<GetDetails>> GetDepartments()
        {
            var result = await _context.Departments.Where(D => D.Status == true).ToListAsync();
            return result.Select(Details);
        }
        public async Task<PagedResult<GetDetails>> GetDepartmentsPaged(int pageNumber, int pageSize, string sortBy = "ID", string sortOrder = "asc")
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;
            sortBy = string.IsNullOrWhiteSpace(sortBy) ? "ID" : sortBy.Trim();
            sortOrder = string.IsNullOrWhiteSpace(sortOrder) ? "asc" : sortOrder.Trim().ToLower();

            var query = _context.Departments.Where(D => D.Status == true).AsQueryable();

            var prop = typeof(DepartmentEntity).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (prop != null)
            {
                var parameter = Expression.Parameter(typeof(DepartmentEntity), "d");
                var propertyAccess = Expression.MakeMemberAccess(parameter, prop);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);

                string methodName = sortOrder == "desc" ? "OrderByDescending" : "OrderBy";

                var resultExp = Expression.Call(typeof(Queryable), methodName, new Type[] { typeof(DepartmentEntity), prop.PropertyType }, query.Expression, Expression.Quote(orderByExp));

                query = query.Provider.CreateQuery<DepartmentEntity>(resultExp);
            }
            else
            {
                query = sortOrder == "desc" ? query.OrderByDescending(d => d.ID) : query.OrderBy(d => d.ID);
            }

            var totalRecords = await query.CountAsync();

            var data = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            return new PagedResult<GetDetails>
            {
                Data = data.Select(Details),
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages
            };
        }

        public async Task<GetDetails> GetDepartment(int id)
        {
            var dept = await _context.Departments.Where(D => D.Status == true).FirstOrDefaultAsync(D => D.ID == id);
            if (dept == null || dept.Status == false)
                throw new Exception("Department does not exist.");

            return Details(dept);
        }
        public async Task<GetDetails> AddDepartment(CreatePayload payload)
        {

            bool nameExists = await _context.Departments.AnyAsync(D => D.Name.ToLower() == payload.Name.ToLower());
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

            await _context.Departments.AddAsync(dept);
            await _context.SaveChangesAsync();
            return Details(dept);
        }
        public async Task<GetDetails> UpdateDepartment(UpdatePayload payload)
        {

            var dept = await _context.Departments.FindAsync(payload.ID);
            if (dept == null)
                throw new Exception();

            bool nameExists = await _context.Departments.AnyAsync(D => D.Name.ToLower() == payload.Name.ToLower() && D.ID != payload.ID);
            if (nameExists)
                throw new InvalidOperationException("Department Name already exists.");

            dept.Name = payload.Name.ToUpper();
            dept.Description = payload.Description.ToUpper();
            dept.Status = payload.Status;
            dept.ModifiedOn = DateTime.Now;
            await _context.SaveChangesAsync();
            return Details(dept);
        }
        public async Task<GetDetails> DeleteDepartment(int id)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept == null)
                throw new Exception("Department does not exist.");

            dept.Status = false;
            dept.ModifiedOn = DateTime.Now;
            await _context.SaveChangesAsync();
            return Details(dept);
        }
    }
}
