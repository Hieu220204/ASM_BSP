using System;

namespace ASM_APDP.Models
{
    public class Attendance
    {
        public string StudentID { get; set; }
        public string StudentName { get; set; }
        public string Subject { get; set; }
        public string Course { get; set; }
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
        public string Status { get; set; }

        public Attendance()
        {
            StudentID = string.Empty;
            StudentName = string.Empty;
            Subject = string.Empty;
            Course = string.Empty;
            Status = string.Empty;
            Date = DateTime.Now;
            IsPresent = false;
        }
    }
}
