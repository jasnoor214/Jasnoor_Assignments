using System;
using System.Collections.Generic;

namespace BEC.Models;

public  class Course
{
    public int CourseId { get; set; }

    public string CourseName { get; set; } = null!;

    public string? Description { get; set; }

    public string? Duration { get; set; }

    public int Cost { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
