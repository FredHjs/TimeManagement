using System.Collections.Generic;

namespace TimeManagement.Entities
{
    public class Course
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string Discription { get; set; }

        public double Grade { get; set; }

        public string TermID { get; set; }

        public Term Term { get; set; }

        public List<Event> Events { get; set; }


    }
}
