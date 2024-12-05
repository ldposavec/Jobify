using System;
using System.Collections.Generic;

namespace Jobify.BL.DALModels;

public partial class Firm
{
    public int Id { get; set; }

    public string FirmName { get; set; } = null!;

    public string Oib { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? Industry { get; set; }

    public virtual ICollection<Employer> Employers { get; set; } = new List<Employer>();
}
