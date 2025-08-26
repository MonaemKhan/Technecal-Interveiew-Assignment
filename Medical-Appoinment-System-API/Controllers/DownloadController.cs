using Medical_Appoinment_System_API.DBConnectionContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Medical_Appoinment_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public DownloadController(AppDbContext dbContext) {
                    _dbContext = dbContext;
        }

        [HttpGet("file")]
        public IActionResult DownloadFile(string appointmentId)
        {
            string fullPath = Path.Combine("..", "Reports");
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            fullPath = Path.Combine(fullPath, appointmentId + ".pdf");

            if (System.IO.File.Exists(fullPath))
            {
                var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
                return File(fileStream, "application/pdf", $"{appointmentId}.pdf");
            }
            return Ok();
        }
    }
}
