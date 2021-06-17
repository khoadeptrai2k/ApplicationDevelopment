using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationDevelopment.Models
{
    public class TrainerCourse
    {
        public int Id { get; set; }
        public string TrainerId { get; set; }
        public int CourseId { get; set; }
        public ApplicationUser Trainer { get; set; }
        public Course Course { get; set; }
    }
}