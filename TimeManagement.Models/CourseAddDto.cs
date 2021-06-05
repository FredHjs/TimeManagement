using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManagement.Models
{
    public class CourseAddDto
    {

        [Display(Name = "课程名称")]
        public string Name { get; set; }

        [Display(Name = "课程描述")]
        public string Discription { get; set; }

        [Display(Name = "成绩")]
        public double Grade { get; set; }
    }
}
