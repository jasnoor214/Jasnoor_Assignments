using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModifiedProject.DTO;
using ModifiedProject.Services;

namespace ModifiedProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class EmpController : ControllerBase
    {
        private readonly IEmployee _employeeService;

        public EmpController(IEmployee employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<EmployeeDto>>> GetAll(int page = 1, int pageSize = 5)
        {
            var result = await _employeeService.GetAllEmployeesAsync(page, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetById(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);

            if (employee == null)
                return NotFound("Employee not found");

            return Ok(employee);
        }

        [HttpGet("basic")]
        public async Task<ActionResult<List<EmployeeBasicDto>>> GetBasicEmployeeList(
            int page = 1, int pageSize = 5, string? search = null)
        {
            var result = await _employeeService.GetAllEmployeeBasicInfoAsync(page, pageSize, search);
            return Ok(result);
        }

        [HttpPost]
       // [Authorize(Roles = "Admin,HR")]
        public async Task<ActionResult<EmployeeDto>> Create([FromForm] EmployeeDto employeeDto, IFormFile? image)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var added = await _employeeService.AddEmployeeAsync(employeeDto, image);
            return Ok(added);
        }

        [HttpPut("{id}")]
       // [Authorize(Roles = "Admin,HR")]
        public async Task<ActionResult<EmployeeDto>> Update(
            int id,
            [FromForm] EmployeeUpdateDto employeeDto,
            IFormFile? image)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _employeeService.UpdateEmployeeAsync(id, employeeDto, image);

            if (updated == null)
                return NotFound("Employee not found to update");

            return Ok(updated);
        }

        [HttpDelete("{id}")]
      //  [Authorize(Roles = "Admin")]
        public async Task<ActionResult<EmployeeDto>> Delete(int id)
        {
            var deleted = await _employeeService.DeleteEmployeeAsync(id);

            if (deleted == null)
                return NotFound("Employee not found to delete");

            return Ok(deleted);
        }

        [HttpGet("export/excel")]
       // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExportToExcel(string? search = null)
        {
            var employees = await _employeeService.GetAllEmployeeBasicInfoAsync(1, int.MaxValue, search);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Employees");

            worksheet.Cell(1, 1).Value = "First Name";
            worksheet.Cell(1, 2).Value = "Last Name";
            worksheet.Cell(1, 3).Value = "Email";
            worksheet.Cell(1, 4).Value = "Image URL";

            int row = 2;
            foreach (var emp in employees)
            {
                worksheet.Cell(row, 1).Value = emp.FirstName;
                worksheet.Cell(row, 2).Value = emp.LastName;
                worksheet.Cell(row, 3).Value = emp.Email;
                worksheet.Cell(row, 4).Value = emp.ImageUrl;
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Seek(0, SeekOrigin.Begin);

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Employees.xlsx");
        }
    }
}