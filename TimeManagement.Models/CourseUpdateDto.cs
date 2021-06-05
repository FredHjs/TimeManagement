using System.ComponentModel.DataAnnotations;

namespace TimeManagement.Models
{
    public class CourseUpdateDto
    {
        [Display(Name = "课程名称")]
        public string Name { get; set; }

        [Display(Name = "课程描述")]
        public string Discription { get; set; }

        [Display(Name = "成绩")]
        public double Grade { get; set; }

        [Display(Name = "学期标识")]
        public string TermID { get; set; }
    }
}
