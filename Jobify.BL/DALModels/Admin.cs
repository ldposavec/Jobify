using System;
using System.Collections.Generic;

namespace Jobify.BL.DALModels;

public partial class Admin
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
