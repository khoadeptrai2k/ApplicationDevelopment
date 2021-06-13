using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApplicationDevelopment.ViewModel
{
    public class AdminChangePassViewModel
    {
        [Display(Name = "User Id")]
        public string UserId { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]

        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Comfirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match")]
        public string ComfirmPassword { get; set; }
    }
}