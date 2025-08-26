using Medical_Appoinment_System_API.DBConnectionContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Medical_Appoinment_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        public MedicineController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // GET: /api/medicines
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var medicines = await _dbContext.Medicines.ToListAsync();
            return Ok(medicines);
        }
    }
}
