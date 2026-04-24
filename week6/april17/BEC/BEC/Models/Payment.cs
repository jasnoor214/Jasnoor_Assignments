using System;
using System.Collections.Generic;

namespace BEC.Models;

public  class Payment
{
    public int PaymentId { get; set; }

    public int CourseId { get; set; }

    public int UserId { get; set; }

    public int TotalAmount { get; set; }

    public string Status { get; set; } = null!;

    public DateTime PaymentDate { get; set; }

    public string ModeOfPayment { get; set; } = null!;

    public virtual Course Course { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
