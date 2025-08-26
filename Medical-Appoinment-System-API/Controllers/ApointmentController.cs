using Aspose.Cells;
using Medical_Appoinment_System_API.DBConnectionContext;
using Medical_Appoinment_System_API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace Medical_Appoinment_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApointmentController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        public ApointmentController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: /api/appointments?pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string searchText = "", [FromQuery] string arrowDir = "", string sortValue = "",
            [FromQuery] string visitFilter = "", [FromQuery] string appStatus = "")
        {
            string upArrow = "↑";
            string downArrow = "↓";
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;

            // Base query
            var query = _dbContext.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .AsQueryable();

            if (!string.IsNullOrEmpty(visitFilter))
            {
                query = query.Where(a => a.VisitType == visitFilter);
            }

            if(!string.IsNullOrEmpty(appStatus))
            {
                query = query.Where(a => a.DoctorId == Convert.ToInt32(appStatus));                
            }

            // Apply search filter if provided
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                searchText = searchText.Trim().ToLower();
                query = query.Where(a =>
                    a.Patient!.Name.ToLower().Contains(searchText) ||
                    a.Doctor!.Name.ToLower().Contains(searchText)
                );
            }

            if (!string.IsNullOrEmpty(sortValue) && !string.IsNullOrEmpty(arrowDir))
            {
                if (sortValue == "pat")
                {
                    if (arrowDir == upArrow)
                        query = query.OrderBy(a => a.Patient.Name);
                    else if (arrowDir == downArrow)
                        query = query.OrderByDescending(a => a.Patient.Name);
                }
                else if (sortValue == "doc")
                {
                    if (arrowDir == upArrow)
                        query = query.OrderBy(a => a.Doctor.Name);
                    else if (arrowDir == downArrow)
                        query = query.OrderByDescending(a => a.Doctor.Name);
                }
                else if (sortValue == "date")
                {
                    if (arrowDir == upArrow)
                        query = query.OrderBy(a => a.AppointmentDate);
                    else if (arrowDir == downArrow)
                        query = query.OrderByDescending(a => a.AppointmentDate);
                }
                else if (sortValue == "apt")
                {
                    if (arrowDir == upArrow)
                        query = query.OrderBy(a => a.AppointmentNo);
                    else if (arrowDir == downArrow)
                        query = query.OrderByDescending(a => a.AppointmentNo);
                }
                else if (sortValue == "vis")
                {
                    if (arrowDir == upArrow)
                        query = query.OrderBy(a => a.VisitType);
                    else if (arrowDir == downArrow)
                        query = query.OrderByDescending(a => a.VisitType);
                }
                else if(sortValue == "dis")
                {
                    if (arrowDir == upArrow)
                        query = query.OrderBy(a => a.Diagnosis);
                    else if (arrowDir == downArrow)
                        query = query.OrderByDescending(a => a.Diagnosis);
                }
                else
                {
                    query = query.OrderByDescending(a => a.Id);
                }
            }
            else
            {
                query = query.OrderByDescending(a => a.Id);
            }

            var totalCount = await query.CountAsync();

            // Apply pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new
                {
                    a.Id,
                    a.AppointmentNo,
                    a.AppointmentDate,
                    a.VisitType,
                    a.Notes,
                    a.Diagnosis,
                    a.PatientId,
                    a.DoctorId,
                    a.isAppointmentVIsited,
                    Patient = new { a.PatientId, a.Patient!.Name },
                    Doctor = new { a.DoctorId, a.Doctor!.Name }
                })
                .ToListAsync();

            var result = new
            {
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Items = items
            };

            return Ok(result);
        }



        // GET: /api/appointments/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _dbContext.Appointments
            .Include(a => a.Patient).Include(a => a.Doctor).
            Select(a => new
            {
                a.Id,
                a.AppointmentNo,
                a.AppointmentDate,
                a.VisitType,
                a.Notes,
                a.Diagnosis,
                a.PatientId,
                a.DoctorId,
                a.isAppointmentVIsited,
                Patient = new { a.PatientId, a.Patient!.Name },
                Doctor = new { a.DoctorId, a.Doctor!.Name }
            })
            .FirstOrDefaultAsync(a => a.Id == id);
            return item is null ? NotFound() : Ok(item);
        }

        // POST: /api/appointments  (AppointmentNo auto-generated in DB)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AppointmentDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            // Validate Patient and Doctor existence
            var patientExists = await _dbContext.Patients.AnyAsync(p => p.Id == dto.PatientId);
            if (!patientExists) return BadRequest("Invalid PatientId");

            var doctorExists = await _dbContext.Doctors.AnyAsync(d => d.Id == dto.DoctorId);
            if (!doctorExists) return BadRequest("Invalid DoctorId");

            var entity = new Appointment
            {
                AppointmentNo = $"APT-{dto.PatientId}-{dto.DoctorId}-{DateTime.UtcNow.Year}{DateTime.UtcNow.Month}{DateTime.UtcNow.Hour}-{DateTime.UtcNow.Minute}{DateTime.UtcNow.Second}",
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                AppointmentDate = Convert.ToDateTime(dto.AppointmentDate).ToString("yyyy-MM-dd"),
                VisitType = dto.VisitType,
                Notes = dto.Notes,
                Diagnosis = dto.Diagnosis
            };

            _dbContext.Appointments.Add(entity);
            await _dbContext.SaveChangesAsync();
            await SavePdfFile(entity.AppointmentNo);

            await _dbContext.Entry(entity).ReloadAsync();

            return Ok(entity);
        }

        // PUT: /api/appointments/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] AppointmentDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var entity = await _dbContext.Appointments.FindAsync(id);
            if (entity is null) return NotFound();

            entity.PatientId = dto.PatientId;
            entity.DoctorId = dto.DoctorId;
            entity.AppointmentDate = Convert.ToDateTime(dto.AppointmentDate).ToString("yyyy-MM-dd");
            entity.VisitType = dto.VisitType;
            entity.Notes = dto.Notes;
            entity.Diagnosis = dto.Diagnosis;

            _dbContext.Appointments.Update(entity);
            await SavePdfFile(entity.AppointmentNo);
            await _dbContext.SaveChangesAsync();
            return Ok(entity);
        }

        // DELETE: /api/appointments/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _dbContext.Appointments.FindAsync(id);
            if (entity is null) return NotFound();

            _dbContext.Appointments.Remove(entity);
            await _dbContext.SaveChangesAsync();

            return Ok(entity);
        }

        // GET APOINTMENT DROPDOWN LIST: /api/appointments/dropdown
        [HttpGet("dropdown")]
        public async Task<IActionResult> GetAppointmentDropdown()
        {
            var items = await _dbContext.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a=>a.isAppointmentVIsited == "0")
                .OrderBy(a => a.AppointmentDate)
                .Select(a => new
                {
                    a.AppointmentNo,
                    display = "(" + a.Patient!.Name + ")"+a.AppointmentNo+"("+a.Doctor!.Name+ ")"
                })
                .ToListAsync();
            return Ok(items);
        }

        private async Task SavePdfFile(string appointmentNo)
        {
            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets[0];

            sheet.Cells.Merge(0, 0, 5, 10);
            Cell headerCell = sheet.Cells[0, 0];
            headerCell.PutValue("Prescription Report");

            Style headerStyle = workbook.CreateStyle();
            headerStyle.Font.Size = 18;
            headerStyle.Font.IsBold = true;

            headerCell.SetStyle(headerStyle);

            Style ss = workbook.CreateStyle();
            ss.Font.IsBold = true;

            var apoinment = await _dbContext.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.AppointmentNo == appointmentNo);

            var doctor = await _dbContext.Doctors
                .FirstOrDefaultAsync(d => d.Id == apoinment!.DoctorId);
            var patient = await _dbContext.Patients.FirstOrDefaultAsync(x => x.Id == apoinment!.PatientId);

            Cell Cell1 = sheet.Cells[5, 0];
            Cell1.PutValue("Patient");
            Cell1.SetStyle(ss);
            Cell1 = sheet.Cells[5, 1];
            Cell1.PutValue(":");
            Cell1 = sheet.Cells[5, 2];
            Cell1.PutValue(patient!.Name);


            Cell1 = sheet.Cells[6, 0];
            Cell1.PutValue("Doctor");
            Cell1.SetStyle(ss);
            Cell1 = sheet.Cells[6, 1];
            Cell1.PutValue(":");
            Cell1 = sheet.Cells[6, 2];
            Cell1.PutValue(doctor!.Name);


            Cell1 = sheet.Cells[7, 0];
            Cell1.PutValue("Date");
            Cell1.SetStyle(ss);
            Cell1 = sheet.Cells[7, 1];
            Cell1.PutValue(":");
            Cell1 = sheet.Cells[7, 2];
            Cell1.PutValue(Convert.ToDateTime(apoinment!.AppointmentDate).ToString("dd-MMM-yyyy"));

            Cell1 = sheet.Cells[8, 0];
            Cell1.PutValue("Visit Type");
            Cell1.SetStyle(ss);
            Cell1 = sheet.Cells[8, 1];
            Cell1.PutValue(":");
            Cell1 = sheet.Cells[8, 2];
            Cell1.PutValue(apoinment!.VisitType);

            sheet.AutoFitColumns();

            string fullPath = Path.Combine("..", "Reports");
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            fullPath = Path.Combine(fullPath, appointmentNo + ".pdf");

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            // Also save as PDF if needed
            workbook.Save(fullPath, SaveFormat.Pdf);
        }
    }
}
