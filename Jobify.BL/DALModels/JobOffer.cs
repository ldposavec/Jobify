using System;
using System.Collections.Generic;

namespace Jobify.BL.DALModels;

public partial class JobOffer
{
    public int Id { get; set; }

    public int JobAppId { get; set; }

    public DateTime? Date { get; set; }

    public int StatusId { get; set; }

    public virtual JobApp JobApp { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;
}
