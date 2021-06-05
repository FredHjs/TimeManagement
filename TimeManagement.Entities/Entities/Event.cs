using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeManagement.Entities
{
    public class Event
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string Time { get; set; }

        public int RepeatDays { get; set; } // 二进制整数，从最低位开始第n位表示星期n，为1则在当天发生

        public string Description { get; set; }

        public string CourseID { get; set; }

        public Course Course { get; set; }
    }
}
