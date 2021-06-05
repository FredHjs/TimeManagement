using System;
using System.ComponentModel.DataAnnotations;

namespace TimeManagement.Models
{
    public class TermDto
    {
        [Display(Name = "标识")]
        public string ID { get; set; }

        [Display(Name = "开始时间")]
        public DateTime StartTime { get; set; }

        [Display(Name = "结束时间")]
        public DateTime EndTime { get; set; }

        [Display(Name = "学期名称")]
        public string Name { get; set; }

        [Display(Name = "学期编号")]
        public int TermNumber { get; set; }
    }
}
