using Microsoft.EntityFrameworkCore;
using ModifiedProject.DTO;
using ModifiedProject.Models;
using System;

namespace ModifiedProject.Services
{
    public class EmployeeService : IEmployee
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmployeeService(
            ApplicationDbContext context,
            IWebHostEnvironment env,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetBaseUrl()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                throw new InvalidOperationException("No HttpContext available.");

            var request = httpContext.Request;
            return $"{request.Scheme}://{request.Host}";
        }

        public async Task<List<EmployeeDto>> GetAllEmployeesAsync(int pageNumber, int pageSize)
        {
            var employees = await _context.employees
                .OrderBy(e => e.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return employees.Select(MapEmployeeToDto).ToList();
        }

        public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
        {
            var employee = await _context.employees.FindAsync(id);
            if (employee == null)
                return null;

            return MapEmployeeToDto(employee);
        }

        public async Task<EmployeeDto> AddEmployeeAsync(EmployeeDto employeeDto, IFormFile? image)
        {
            var employee = new Employee
            {
                FirstName = employeeDto.FirstName!,
                LastName = employeeDto.LastName!,
                Email = employeeDto.Email!,
                Age = employeeDto.Age,
                ImagePath = "/uploads/default.jpg"
            };

            if (image != null && image.Length > 0)
            {
                employee.ImagePath = await SaveImageToUploadsAsync(image);
            }

            await _context.employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            return MapEmployeeToDto(employee);
        }

        public async Task<EmployeeDto?> UpdateEmployeeAsync(int id, EmployeeUpdateDto employeeDto, IFormFile? image)
        {
            var existingEmployee = await _context.employees.FindAsync(id);
            if (existingEmployee == null)
                return null;

            existingEmployee.FirstName = employeeDto.FirstName!;
            existingEmployee.LastName = employeeDto.LastName!;
            existingEmployee.Email = employeeDto.Email!;
            existingEmployee.Age = employeeDto.Age;

            if (image != null && image.Length > 0)
            {
                DeleteImageFile(existingEmployee.ImagePath);
                existingEmployee.ImagePath = await SaveImageToUploadsAsync(image);
            }

            await _context.SaveChangesAsync();

            return MapEmployeeToDto(existingEmployee);
        }

        public async Task<EmployeeDto?> DeleteEmployeeAsync(int id)
        {
            var employee = await _context.employees.FindAsync(id);
            if (employee == null)
                return null;

            var deletedEmployeeDto = MapEmployeeToDto(employee);

            DeleteImageFile(employee.ImagePath);
            _context.employees.Remove(employee);
            await _context.SaveChangesAsync();

            return deletedEmployeeDto;
        }

        public async Task<List<EmployeeBasicDto>> GetAllEmployeeBasicInfoAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            var query = _context.employees.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(e =>
                    e.FirstName.Contains(searchTerm) ||
                    e.LastName.Contains(searchTerm) ||
                    e.Email.Contains(searchTerm));
            }

            var employees = await query
                .OrderBy(e => e.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            string baseUrl = GetBaseUrl();

            return employees.Select(e => new EmployeeBasicDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                ImageUrl = string.IsNullOrEmpty(e.ImagePath)
                    ? $"{baseUrl}/uploads/default.jpg"
                    : $"{baseUrl}{e.ImagePath}"
            }).ToList();
        }

        private EmployeeDto MapEmployeeToDto(Employee employee)
        {
            string baseUrl = GetBaseUrl();

            return new EmployeeDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Age = employee.Age,
                ImagePath = string.IsNullOrEmpty(employee.ImagePath)
                    ? $"{baseUrl}/uploads/default.jpg"
                    : $"{baseUrl}{employee.ImagePath}"
            };
        }
        private void DeleteImageFile(string? imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath) || imagePath.Contains("default.jpg"))
                return;

            if (imagePath.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                var uri = new Uri(imagePath);
                imagePath = uri.AbsolutePath;
            }

            var fullPath = Path.Combine(
                _env.WebRootPath!,
                imagePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        private async Task<string> SaveImageToUploadsAsync(IFormFile image)
        {
            if (_env.WebRootPath == null)
            {
                var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                if (!Directory.Exists(wwwrootPath))
                    Directory.CreateDirectory(wwwrootPath);

                _env.WebRootPath = wwwrootPath;
            }

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var extension = Path.GetExtension(image.FileName);
            var imageName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(uploadsFolder, imageName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await image.CopyToAsync(stream);

            return $"/uploads/{imageName}";
        }
    }
}