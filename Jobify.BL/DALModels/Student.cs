using System;
using System.Collections.Generic;

namespace Jobify.BL.DALModels;

public partial class Student
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public decimal? AverageGrade { get; set; }

    public int? YearOfStudy { get; set; }

    public string Jmbag { get; set; } = null!;

    public virtual ICollection<JobApp> JobApps { get; set; } = new List<JobApp>();

    public virtual User User { get; set; } = null!;
}
