using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationDevelopment.Models
{
    public class TraineeCourse
    {
        public int Id { get; set; }
        public string TraineeId { get; set; }
        public int CourseId { get; set; }
        public ApplicationUser Trainee { get; set; }
        public Course Course { get; set; }

    }
}