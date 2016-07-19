using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace Registrar
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ =>
      {
        return View["index.cshtml"];
      };

      Get["/courses"] = _ =>
      {
        List<Course> allCourses = Course.GetAll();
        return View["courses.cshtml", allCourses];
      };

      Post["/courses"] = _ =>
      {
        Course newCourse = new Course(Request.Form["course-name"], Request.Form["Department"], Request.Form["course-number"]);
        newCourse.Save();
        List<Course> allCourses = Course.GetAll();
        return View["courses.cshtml", allCourses];
      };

      Get["/students"] = _ =>
      {
        List<Student> allStudents = Student.GetAll();
        return View["students.cshtml", allStudents];
      };

      Post["/students"] = _ =>
      {
        Student newStudent = new Student(Request.Form["student-name"], Request.Form["enrollment"]);
        newStudent.Save();
        List<Student> allStudents = Student.GetAll();
        return View["students.cshtml", allStudents];
      };
    }
  }
}
