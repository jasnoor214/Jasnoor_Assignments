using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace BEC.Models;

public  class User:IdentityUser
{
 
   
    
    public string UserRole { get; set; } = null!;

    public virtual ICollection<Enquiry> Enquiries { get; set; } = new List<Enquiry>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
