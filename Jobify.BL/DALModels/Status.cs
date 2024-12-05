using System;
using System.Collections.Generic;

namespace Jobify.BL.DALModels;

public partial class Status
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<JobAd> JobAds { get; set; } = new List<JobAd>();

    public virtual ICollection<JobApp> JobApps { get; set; } = new List<JobApp>();

    public virtual ICollection<JobOffer> JobOffers { get; set; } = new List<JobOffer>();
}
