using Medical_Appoinment_System_API.DBConnectionContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Medical_Appoinment_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly AppDbContext _context;
        public PatientController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var patients = await _context.Patients
                                         .OrderBy(p=>p.Name)
                                         .Select(p=>new { p.Id,p.Name}).ToListAsync();
            return Ok(patients);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var patients = await _context.Patients.FirstOrDefaultAsync(x=>x.Id == id);
            return Ok(patients);
        }
    }
}
