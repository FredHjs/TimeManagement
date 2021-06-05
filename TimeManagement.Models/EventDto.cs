namespace TimeManagement.Models
{
    public class EventDto
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public string Time { get; set; }

        public string RepeatDays { get; set; } //字符串中包含的数字即为每周重复日，如“0”为每周一重复，“0，3”为周一、周四重复

        public string Description { get; set; }

        public string CourseID { get; set; }
    }
}
