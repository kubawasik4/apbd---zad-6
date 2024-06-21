using System;
using System.Collections.Generic;

namespace WebApplication4.Models;

public partial class Patient
{
    public int IdPatient { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime Birthdate { get; set; }

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}
