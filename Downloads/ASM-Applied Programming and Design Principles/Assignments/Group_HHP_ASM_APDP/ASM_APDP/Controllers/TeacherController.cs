using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ASM_APDP.Models;

public class TeacherController : Controller
{
    private readonly string gradesFilePath = "wwwroot/Data/Grades.csv";
    private readonly string attendanceFilePath = "wwwroot/Data/Attendance.csv";

    // Trang TeacherHome sau khi Teacher đăng nhập
    public IActionResult TeacherHome()
    {
        return View();
    }

    // Phương thức xử lý đăng nhập của Teacher
    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        var teacher = Teacher.GetTeacherByUsername(email, password);
        if (teacher != null)
        {
            return RedirectToAction("TeacherHome", "Teacher");
        }

        ViewBag.Error = "Invalid login credentials.";
        return View("Login");
    }

    // Phương thức đọc dữ liệu từ Schedule.csv
    private List<Schedule> ReadSchedule()
    {
        string filePath = "wwwroot/Data/Schedule.csv";
        List<Schedule> schedules = new List<Schedule>();

        if (System.IO.File.Exists(filePath))
        {
            var lines = System.IO.File.ReadAllLines(filePath);
            foreach (var line in lines.Skip(1)) // Bỏ qua dòng tiêu đề
            {
                var parts = line.Split(',');
                if (parts.Length == 4)
                {
                    schedules.Add(new Schedule
                    {
                        SubjectName = parts[0],
                        TeacherName = parts[1],
                        Date = parts[2],
                        Time = parts[3]
                    });
                }
            }
        }
        return schedules;
    }

    // Hiển thị danh sách lịch học của giáo viên hiện tại
    public IActionResult ViewSchedule()
    {
        var schedules = ReadSchedule();
        return View(schedules);
    }

    // Hiển thị giao diện cập nhật hồ sơ giáo viên
    public IActionResult UpdateProfile()
    {
        string email = "teacher1@gmail.com"; // TODO: Lấy từ session/cookie
        string password = "123";

        var teacher = Teacher.GetTeacherByUsername(email, password);
        if (teacher != null)
        {
            return View(teacher);
        }

        return RedirectToAction("Login");
    }

    [HttpPost]
    public IActionResult UpdateProfile(Teacher updatedTeacher)
    {
        Teacher.UpdateTeacherProfile(updatedTeacher);
        ViewBag.Message = "Profile updated successfully!";
        return View(updatedTeacher);
    }

    // Hiển thị danh sách điểm số của học sinh
    public IActionResult ManageGrades()
    {
        var grades = ReadGradesFromCsv();
        return View(grades);
    }

    [HttpPost]
    public IActionResult UpdateGrades(List<Grade> grades)
    {
        WriteGradesToCsv(grades);
        return RedirectToAction("ManageGrades");
    }

    // Hiển thị danh sách điểm danh của học sinh
    public IActionResult ManageAttendance()
    {
        var attendance = ReadAttendanceFromCsv();
        return View(attendance);
    }

    [HttpPost]
    public IActionResult UpdateAttendance(List<Attendance> attendanceRecords)
    {
        WriteAttendanceToCsv(attendanceRecords);
        return RedirectToAction("ManageAttendance");
    }

    // Đọc danh sách điểm từ file CSV
    private List<Grade> ReadGradesFromCsv()
    {
        var grades = new List<Grade>();
        if (System.IO.File.Exists(gradesFilePath))
        {
            var lines = System.IO.File.ReadAllLines(gradesFilePath);
            foreach (var line in lines.Skip(1))
            {
                var values = line.Split(',');
                grades.Add(new Grade
                {
                    StudentID = values[0],
                    Course = values[1],
                    Score = int.Parse(values[2])
                });
            }
        }
        return grades;
    }

    // Ghi danh sách điểm vào file CSV
    private void WriteGradesToCsv(List<Grade> grades)
    {
        var csv = new StringBuilder();
        csv.AppendLine("StudentID,Course,Grade");
        foreach (var grade in grades)
        {
            csv.AppendLine($"{grade.StudentID},{grade.Course},{grade.Score}");
        }
        System.IO.File.WriteAllText(gradesFilePath, csv.ToString());
    }

    // Đọc danh sách điểm danh từ file CSV
    private List<Attendance> ReadAttendanceFromCsv()
    {
        var attendance = new List<Attendance>();
        if (System.IO.File.Exists(attendanceFilePath))
        {
            var lines = System.IO.File.ReadAllLines(attendanceFilePath);
            foreach (var line in lines.Skip(1))
            {
                var values = line.Split(',');
                attendance.Add(new Attendance
                {
                    StudentID = values[0],
                    Course = values[1],
                    Date = values[2],
                    Status = values[3]
                });
            }
        }
        return attendance;
    }

    // Ghi danh sách điểm danh vào file CSV
    private void WriteAttendanceToCsv(List<Attendance> attendanceRecords)
    {
        var csv = new StringBuilder();
        csv.AppendLine("StudentID,Course,Date,Status");
        foreach (var record in attendanceRecords)
        {
            csv.AppendLine($"{record.StudentID},{record.Course},{record.Date},{record.Status}");
        }
        System.IO.File.WriteAllText(attendanceFilePath, csv.ToString());
    }
}
