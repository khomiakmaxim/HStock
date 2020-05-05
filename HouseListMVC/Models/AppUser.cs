using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace HouseListMVC.Models
{
    public class AppUser : IdentityUser<int>
    {        

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        [StringLength(50, MinimumLength = 2)]
        [Required]
        public string LastName { get; set; }

        [Required]
        [Remote(action:"IsEmailInUse", controller:"Account")]
        [EmailAddress]        
        public override string Email { get; set; }
        
        [DataType(DataType.Password)]               
        public string Password { get; set; }    
        
        public bool IsActive { get; set; }

        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords dont match")]      
        public string ConfirmPassword { get; set; }

        [Display(Name = "Seller")]
        public bool Seller { get; set; }     

    }
}
