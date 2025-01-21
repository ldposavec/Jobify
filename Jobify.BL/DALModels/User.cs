using System;
using System.Collections.Generic;

namespace Jobify.BL.DALModels;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Mail { get; set; } = null!;

    public string? Password { get; set; }

    public int UserTypeId { get; set; }

    public bool? IsEmailVerified { get; set; }

    public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();

    public virtual ICollection<Employer> Employers { get; set; } = new List<Employer>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual UserType UserType { get; set; } = null!;
}
