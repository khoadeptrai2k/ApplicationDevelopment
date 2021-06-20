using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationDevelopment.Models
{
    public class Trainer: ApplicationUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string WorkingPlace { get; set; }
        public string Phone { get; set; }
    }
}