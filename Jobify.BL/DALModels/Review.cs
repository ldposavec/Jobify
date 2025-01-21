using System;
using System.Collections.Generic;

namespace Jobify.BL.DALModels;

public partial class Review
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int FirmId { get; set; }

    public int Grade { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Firm Firm { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
