using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Context;
using WebApplication4.DTO;
using WebApplication4.Models;

namespace WebApplication4.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PatientController : ControllerBase
{
    private readonly MasterContext _context;

    public PatientController(MasterContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> addRecipe([FromBody]  PrescriptionDto prescriptionDto)
    {
        if (prescriptionDto.Medicaments.Count > 10)
        {
            return BadRequest("recepta max 10 lekow");
        }
        var patient = await _context.Patients.FindAsync(prescriptionDto.PatientId);
        if (patient == null)
        {
            patient = new Patient
            {
                IdPatient = prescriptionDto.PatientId,
                FirstName = prescriptionDto.FirstName,
                LastName = prescriptionDto.LastName,
                Birthdate = DateTime.Now
            };
            _context.Add(patient);
        }

        var medicaments = await _context.Medicaments
            .Where(m => prescriptionDto.Medicaments
                .Select(md => md.Id)
                .Contains(m.IdMedicament)).ToListAsync();
        
        
        
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> getPatitents(int id)
    {
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
            .ThenInclude(p => p.IdDoctorNavigation)
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.PrescriptionMedicaments)
            .ThenInclude(pm => pm.IdMedicamentNavigation)
            .Where(p => p.IdPatient == id)
            .Select(p => new
            {
                p.IdPatient,
                p.FirstName,
                p.LastName,
                p.Birthdate,
                Prescriptions = p.Prescriptions.Select( pr => new
                {
                    pr.IdPrescription,
                    pr.Date,
                    pr.DueDate,
                    Doctor = new { pr.IdDoctor, pr.IdDoctorNavigation.FirstName,pr.IdDoctorNavigation.LastName},
                    Medicaments = pr.PrescriptionMedicaments.Select(pm => new
                    {
                        pm.IdMedicamentNavigation.IdMedicament,
                        pm.IdMedicamentNavigation.Name,
                        pm.Dose,
                        pm.IdMedicamentNavigation.Description
                    })
                })
                
                

            }).FirstOrDefaultAsync();
        return Ok(patient);
    }

}