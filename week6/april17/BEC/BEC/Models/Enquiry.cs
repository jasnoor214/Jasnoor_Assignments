using System;
using System.Collections.Generic;

namespace BEC.Models;

public  class Enquiry
{
    public int EnquiryId { get; set; }

    public DateTime EnquiryDate { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string EmailId { get; set; } = null!;

    public string EnquiryType { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
