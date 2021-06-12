using ApplicationDevelopment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationDevelopment.ViewModel
{
    public class UsersInRole
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string WorkingPlace { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public ApplicationUser  ApplicationUser { get; set; }
    }
}