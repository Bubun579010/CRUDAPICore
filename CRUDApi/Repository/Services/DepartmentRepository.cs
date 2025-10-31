using CRUDApi.Data;
using CRUDApi.Models;
using CRUDApi.Models.Common;
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
        public async Task<IEnumerable<Department>> GetDepartments()
        {
            try
            {
                return await _context.Departments.Where(D => D.Status == true).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<PagedResult<Department>> GetDepartmentsPaged(int pageNumber,int pageSize,string sortBy = "ID",string sortOrder = "asc")
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;
            sortBy = string.IsNullOrWhiteSpace(sortBy) ? "ID" : sortBy.Trim();
            sortOrder = string.IsNullOrWhiteSpace(sortOrder) ? "asc" : sortOrder.Trim().ToLower();

            var query = _context.Departments.Where(D => D.Status == true).AsQueryable();

            var prop = typeof(Department).GetProperty(sortBy,BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (prop != null)
            {
                var parameter = Expression.Parameter(typeof(Department), "d");
                var propertyAccess = Expression.MakeMemberAccess(parameter, prop);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);

                string methodName = sortOrder == "desc" ? "OrderByDescending" : "OrderBy";

                var resultExp = Expression.Call(typeof(Queryable),methodName,new Type[] { typeof(Department), prop.PropertyType },query.Expression,Expression.Quote(orderByExp));

                query = query.Provider.CreateQuery<Department>(resultExp);
            }
            else
            {
                query = sortOrder == "desc" ? query.OrderByDescending(d => d.ID) : query.OrderBy(d => d.ID);
            }

            var totalRecords = await query.CountAsync();

            var data = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var totalPages = (int)Math.Ceiling(totalRecords == 0 ? 1 : (double)totalRecords / pageSize);

            return new PagedResult<Department>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages
            };
        }

        public async Task<Department> GetDepartment(int id)
        {
            try
            {
                var dept = await _context.Departments.Where(D => D.Status == true).FirstOrDefaultAsync(D => D.ID == id);
                if (dept == null || dept.Status == false)
                    throw new Exception("The record does not exist.");
                return dept;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Department> AddDepartment(Department dept)
        {
            try
            {
                bool nameExists = await _context.Departments.AnyAsync(D => D.Name.ToLower() == dept.Name.ToLower());
                if (nameExists)
                    throw new InvalidOperationException("Department Name already exists.");

                dept.CreatedOn = DateTime.Now;
                dept.ModifiedOn = DateTime.Now;
                var department = await _context.Departments.AddAsync(dept);
                await _context.SaveChangesAsync();
                return department.Entity;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Department> UpdateDepartment(Department dept)
        {
            try
            {
                var department = await _context.Departments.FindAsync(dept.ID);
                if (department == null)
                    throw new Exception();

                bool nameExists = await _context.Departments.AnyAsync(D => D.Name.ToLower() == dept.Name.ToLower() && D.ID != dept.ID);
                if (nameExists)
                    throw new InvalidOperationException("Department Name already exists.");

                department.Name = dept.Name;
                department.Description = dept.Description;
                department.Status = dept.Status;
                department.ModifiedOn = DateTime.Now;
                await _context.SaveChangesAsync();
                return department;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Department> DeleteDepartment(int id)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept != null)
            {
                //_context.Departments.Remove(dept);
                dept.Status = false;
                await _context.SaveChangesAsync();
                return dept;
            }
            return null;
        }
    }
}
