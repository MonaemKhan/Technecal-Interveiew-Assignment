using Medical_Appoinment_System_API.DBConnectionContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Medical_Appoinment_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly AppDbContext _context;
        public DoctorController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var doctors = await _context.Doctors
                                         .OrderBy(d => d.Name)
                                         .Select(d => new { d.Id, d.Name }).ToListAsync();
            return Ok(doctors);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var doctors = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == id);
            return Ok(doctors);
        }
    }
}
