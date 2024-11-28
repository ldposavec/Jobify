using System;
using System.Collections.Generic;

namespace Jobify.BL.DALModels;

public partial class Employer
{
    public int Id { get; set; }

    public int FirmId { get; set; }

    public int UserId { get; set; }

    public string? Position { get; set; }

    public virtual Firm Firm { get; set; } = null!;

    public virtual ICollection<JobAd> JobAds { get; set; } = new List<JobAd>();

    public virtual User User { get; set; } = null!;
}
