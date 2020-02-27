using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CatalogOfProducts2.Models
{
    public class UserModel
    {

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First name required.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last name required.")]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email adress required.")]
        public string EmailAddress { get; set; }


        [DataType(DataType.PhoneNumber)]
        [Display(Name ="Phone Number")]
        public int PhoneNumber { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "You need to provide a long enough password.")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords are not identical.")]
        public string ConfirmPassword { get; set; }

    }
}