using System;

namespace TimeManagement.Models
{
    public class EventAddDto
    {
        public string Name { get; set; }

        public string Time { get; set; }

        public string RepeatDays { get; set; } 

        public string Description { get; set; }
    }
}
