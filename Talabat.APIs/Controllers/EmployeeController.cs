using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications.Employee_Spec;

namespace Talabat.APIs.Controllers
{
    public class EmployeeController : BaseApiController
    {

        private readonly IGenericRepository<Employee> _employeeRepo;

        public EmployeeController(IGenericRepository<Employee> employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        //BaseUrl/api/Employee
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee()
        {
            var spec = new EmployeeWithDepartmentSpecifications();
            var employees = await _employeeRepo.GetAllWithSpecAsync(spec);
            return Ok(employees);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var spec = new EmployeeWithDepartmentSpecifications(id);

            var employee = await _employeeRepo.GetWithSpecAsync(spec);

            if (employee == null)
                return NotFound(new ApiResponse(404));

            return Ok(employee);
        }
    }
}
