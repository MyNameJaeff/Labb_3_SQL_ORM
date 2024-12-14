using System;
using System.Collections.Generic;

namespace Labb_3___SQL___ORM.Models;

public partial class Teacher
{
    public int TeacherId { get; set; }

    public int PersonId { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    public virtual Person Person { get; set; } = null!;
}
