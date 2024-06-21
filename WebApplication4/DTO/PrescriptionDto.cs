namespace WebApplication4.DTO;

public class PrescriptionDto
{
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public int PatientId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int DoctorId { get; set; }
    public List<MedicamentDto> Medicaments { get; set; }
}