using System;

namespace ASM_APDP.Models
{
    public class Grade
    {
        public string StudentID { get; set; }
        public string StudentName { get; set; }
        public string Subject { get; set; }
        public string Course { get; set; }
        public double Score { get; set; }
        public string GradeLetter { get; set; }

        public Grade()
        {
            StudentID = string.Empty;
            StudentName = string.Empty;
            Subject = string.Empty;
            Course = string.Empty;
            GradeLetter = string.Empty;
        }

        public void CalculateGrade()
        {
            if (Score >= 90) GradeLetter = "A";
            else if (Score >= 80) GradeLetter = "B";
            else if (Score >= 70) GradeLetter = "C";
            else if (Score >= 60) GradeLetter = "D";
            else GradeLetter = "F";
        }
    }
}
