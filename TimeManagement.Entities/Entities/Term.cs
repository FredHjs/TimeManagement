using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeManagement.Entities
{
    public class Term
    {
        public string ID { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Name { get; set; }

        public int TermNumber { get; set; }

        public List<Course> Courses { get; set; }
    }
}
