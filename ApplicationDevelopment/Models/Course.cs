using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ApplicationDevelopment.Models
{
    public class Course
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
        [Required]
        [Display(Name="Due Date of Course")]

        public DateTime DueDate { get; set; }
        [Required]
        [DisplayName("Course Name")]
        public string Name { get; set; }
        [ForeignKey("Category")]
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}