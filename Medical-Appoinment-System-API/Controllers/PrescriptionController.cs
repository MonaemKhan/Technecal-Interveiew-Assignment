using Aspose.Cells;
using Medical_Appoinment_System_API.DBConnectionContext;
using Medical_Appoinment_System_API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace Medical_Appoinment_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        public PrescriptionController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> AddPrescription([FromBody] List<PrescriptionDto> prescriptionDto)
        {
            try
            {
                foreach (var obj in prescriptionDto)
                {
                    var appointment = await _dbContext.Appointments
                    .FirstOrDefaultAsync(a => a.AppointmentNo == obj.AppointmentNo);
                    if (appointment == null)
                    {
                        return NotFound(new { Message = "Appointment not found." });
                    }
                    var prescription = new Model.Prescription
                    {
                        MedicineId = obj.MedicineId,
                        Dogaes = obj.Dosage,
                        StartDate = Convert.ToDateTime(obj.StartDate).ToString("yyyy-MM-dd"),
                        EndDate = Convert.ToDateTime(obj.EndDate).ToString("yyyy-MM-dd"),
                        Notes = obj.Notes,
                        AppointmentNo = obj.AppointmentNo
                    };

                    appointment.isAppointmentVIsited = "1";
                    _dbContext.Appointments.Update(appointment);
                    _dbContext.Prescribtions.Add(prescription);
                }
                await SavePdfFile(prescriptionDto);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing the request.", Details = ex.Message });
            }
        }

        private async Task SavePdfFile(List<PrescriptionDto> prescriptionDto)
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
                .FirstOrDefaultAsync(a => a.AppointmentNo == prescriptionDto[0].AppointmentNo);

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

            string[] columns = { "Medicine", "Dosage", "Start Date", "End Date"};

            Style colHeaderStyle = workbook.CreateStyle();
            colHeaderStyle.Font.IsBold = true;
            colHeaderStyle.Font.Color = Color.White;
            colHeaderStyle.ForegroundColor = Color.Gray;
            colHeaderStyle.Pattern = BackgroundType.Solid;
            colHeaderStyle.HorizontalAlignment = TextAlignmentType.Center;
            colHeaderStyle.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            colHeaderStyle.Borders[BorderType.BottomBorder].Color = Color.Black;

            for (int i = 0; i < columns.Length; i++)
            {
                Cell c = sheet.Cells[9, i];
                c.PutValue(columns[i]);
                c.SetStyle(colHeaderStyle);
            }
            

            Style dataStyle = workbook.CreateStyle();
            dataStyle.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            dataStyle.Borders[BorderType.BottomBorder].Color = Color.LightGray;
            dataStyle.HorizontalAlignment = TextAlignmentType.Center;

            int rowIndex = 10;
            foreach (var item in prescriptionDto)
            {
                var med = await _dbContext.Medicines.FirstOrDefaultAsync(x => x.Id == item.MedicineId);
                sheet.Cells[rowIndex, 0].PutValue(med!.Name);
                sheet.Cells[rowIndex, 1].PutValue(item.Dosage);
                sheet.Cells[rowIndex, 2].PutValue(Convert.ToDateTime(item.StartDate).ToString("dd-MMM-yyyy"));
                sheet.Cells[rowIndex, 3].PutValue(Convert.ToDateTime(item.EndDate).ToString("dd-MMM-yyyy"));
                
                for (int i = 0; i < columns.Length; i++)
                    sheet.Cells[rowIndex, i].SetStyle(dataStyle);

                rowIndex++;
            }

            sheet.PageSetup.Orientation = PageOrientationType.Portrait;
            sheet.PageSetup.PaperSize = PaperSizeType.PaperA4;
            sheet.PageSetup.CenterHorizontally = true;
            sheet.PageSetup.CenterVertically = false;

            sheet.AutoFitColumns();

            string fullPath = Path.Combine("..", "Reports");
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            fullPath = Path.Combine(fullPath, prescriptionDto[0].AppointmentNo + ".pdf");

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            // Also save as PDF if needed
            workbook.Save(fullPath, SaveFormat.Pdf);
        }
    }
}
