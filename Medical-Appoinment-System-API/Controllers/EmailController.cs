using Medical_Appoinment_System_API.DBConnectionContext;
using Medical_Appoinment_System_API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace Medical_Appoinment_System_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _configuration;
        public EmailController(AppDbContext dbContext,IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        [HttpGet("send")]
        public async Task<IActionResult> SendEmail([FromQuery] string EmailAddress , string appointmentNo)
        {
            try
            {
                var server = _configuration["EmailSettings:Server"];
                var port = _configuration["EmailSettings:Port"];
                var fromMail = _configuration["EmailSettings:FromMail"];
                var password = _configuration["EmailSettings:Password"];

                var smtpClient = new SmtpClient(server)
                {
                    Port = Convert.ToInt32(port),
                    Credentials = new NetworkCredential(fromMail, password),
                    EnableSsl = true,
                };
                var appointment = await _dbContext.Appointments.FirstOrDefaultAsync(x => x.AppointmentNo == appointmentNo);
                var patient = await _dbContext.Patients.FirstOrDefaultAsync(x => x.Id == appointment!.PatientId);
                var doctor = await _dbContext.Doctors.FirstOrDefaultAsync(x => x.Id == appointment!.DoctorId);

                var subject = "Appointment Detils of "+patient!.Name;
                var body = "<h3>Appointment Details</h3>" +
                           "<p><strong>Appointment No:</strong> " + appointment!.AppointmentNo + "</p>" +
                           "<p><strong>Patient Name:</strong> " + patient!.Name + "</p>" +
                           "<p><strong>Doctor Name:</strong> " + doctor!.Name + "</p>" +
                           "<p><strong>Date:</strong> " + appointment!.AppointmentDate + "</p>" +
                           "<p><strong>Visit Type:</strong> " + appointment!.VisitType + "</p>";

                if(appointment!.isAppointmentVIsited == "1")
                {
                    body = body + "<p><strong>Status:</strong> Visited</p>";
                }
                else
                {
                    body = body + "<p><strong>Status:</strong> Not Visited</p>";
                }

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(fromMail),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true, // if you want HTML body
                    };

                mailMessage.To.Add(EmailAddress);
                
                // Optional: Add attachment
                if (!string.IsNullOrEmpty(appointmentNo))
                {
                    string fullPath = Path.Combine("..", "Reports");
                    fullPath = Path.Combine(fullPath, appointmentNo + ".pdf");
                    mailMessage.Attachments.Add(new Attachment(fullPath));
                }

                await smtpClient.SendMailAsync(mailMessage);

                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
