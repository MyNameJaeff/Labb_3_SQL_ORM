using System;
using System.Collections.Generic;

namespace Labb_3___SQL___ORM.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string CourseName { get; set; } = null!;

    public int TeacherId { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    public virtual Teacher Teacher { get; set; } = null!;
}
