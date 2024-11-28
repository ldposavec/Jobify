using System;
using System.Collections.Generic;

namespace Jobify.BL.DALModels;

public partial class JobAd
{
    public int Id { get; set; }

    public int EmployerId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Salary { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int StatusId { get; set; }

    public virtual Employer Employer { get; set; } = null!;

    public virtual ICollection<JobApp> JobApps { get; set; } = new List<JobApp>();

    public virtual Status Status { get; set; } = null!;
}
