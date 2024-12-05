using System;
using System.Collections.Generic;

namespace Jobify.BL.DALModels;

public partial class JobApp
{
    public int Id { get; set; }

    public int JobAdId { get; set; }

    public int StudentId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string CvFilepath { get; set; } = null!;

    public int StatusId { get; set; }

    public virtual JobAd JobAd { get; set; } = null!;

    public virtual ICollection<JobOffer> JobOffers { get; set; } = new List<JobOffer>();

    public virtual Status Status { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
