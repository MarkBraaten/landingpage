using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace landing.Models
{
    public class User
    {
        [Key]
        public int UserId {get;set;}
        [Display(Name="First Name")]
        [MinLength(2)]
        [Required]
        public string FirstName {get;set;}
        [Required]
        [MinLength(2)]
        [Display(Name="Last Name")]
        public string LastName {get;set;}
        [Required]
        [EmailAddress]
        public string Email {get;set;}
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password {get;set;}
        [Compare("Password")]
        [DataType(DataType.Password)]
        [NotMapped]
        public string Confirm {get;set;}
        
    }
    public class LoginUser
    {
        [EmailAddress]
        [Required]
        [Display(Name="Email")]
        public string Email {get;set;}
        [DataType(DataType.Password)]
        [Display(Name="Password")]
        public string Password {get;set;}
    }

}