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

      Get["/courses/{id}"] = chocolate => {
        Course newCourse = Course.Find(chocolate.id);
        Dictionary<string, object> model = new Dictionary<string, object>{};
        List<Student> allStudents = Student.GetAll();
        List<Student> enrolledStudents = newCourse.GetStudents();
        List<Department> allDepartments = Department.GetAll();
        List<Department> registeredDepartment = newCourse.GetAssignedDepartment();
        model.Add("registeredDepartment", registeredDepartment);
        model.Add("departments", allDepartments);
        model.Add("students", allStudents);
        model.Add("course", newCourse);
        model.Add("enrolledStudents", enrolledStudents);
        return View["course.cshtml", model];
      };
      Post["/courses/{id}"] = chocolate => {
        Course newCourse = Course.Find(chocolate.id);
        Student newStudent = Student.Find(Request.Form["student-id"]);
        newStudent.AddCourse(newCourse.GetId());
        Dictionary<string, object> model = new Dictionary<string, object>{};
        List<Student> allStudents = Student.GetAll();
        List<Student> enrolledStudents = newCourse.GetStudents();
        List<Department> allDepartments = Department.GetAll();
        List<Department> registeredDepartment = newCourse.GetAssignedDepartment();
        model.Add("registeredDepartment", registeredDepartment);
        model.Add("departments", allDepartments);
        model.Add("students", allStudents);
        model.Add("course", newCourse);
        model.Add("enrolledStudents", enrolledStudents);
        return View["course.cshtml", model];
      };

      Post["/courses/{id}/department"] = chocolate => {
        Course newCourse = Course.Find(chocolate.id);
        newCourse.AddDepartment(Request.Form["department-id"]);
        Dictionary<string, object> model = new Dictionary<string, object>{};
        List<Student> allStudents = Student.GetAll();
        List<Student> enrolledStudents = newCourse.GetStudents();
        List<Department> allDepartments = Department.GetAll();
        List<Department> registeredDepartment = newCourse.GetAssignedDepartment();
        model.Add("registeredDepartment", registeredDepartment);
        model.Add("departments", allDepartments);
        model.Add("students", allStudents);
        model.Add("course", newCourse);
        model.Add("enrolledStudents", enrolledStudents);
        return View["course.cshtml", model];
      };

      Get["/departments"] = _ =>{
        List<Department> allDepartments = Department.GetAll();
        return View["departments.cshtml", allDepartments];
      };

      Post["/departments"] = _ =>{
        Department newDepartment = new Department(Request.Form["department-name"], Request.Form["department-code"]);
        newDepartment.Save();
        List<Department> allDepartments = Department.GetAll();
        return View["departments.cshtml", allDepartments];
      };

      Get["/departments/{id}"] = chocolate => {
        Department newDepartment =Department.Find(chocolate.id);
        Dictionary<string, object> model = new Dictionary<string, object>{};
        List<Student> allStudents = newDepartment.GetStudents();
        List<Course> allCourses = newDepartment.GetCourses();
        model.Add("students", allStudents);
        model.Add("courses", allCourses);
        model.Add("department", newDepartment);
        return View["department.cshtml", model];
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

      Get["/students/{id}"] = chocolate => {
        Student newStudent = Student.Find(chocolate.id);
        Dictionary<string, object> model = new Dictionary<string, object>{};
        List<Course> allCourses = newStudent.GetCourses();
        List<Department> allDepartments = Department.GetAll();
        List<Department> registeredDepartment = newStudent.GetAssignedDepartment();
        model.Add("registeredDepartment", registeredDepartment);
        model.Add("departments", allDepartments);
        model.Add("student", newStudent);
        model.Add("courses", allCourses);
        return View["student.cshtml", model];
      };

      Post["/students/{id}/department"] = chocolate => {
        Student newStudent = Student.Find(chocolate.id);
        newStudent.AddDepartment(Request.Form["department-id"]);
        Dictionary<string, object> model = new Dictionary<string, object>{};
        List<Course> allCourses = newStudent.GetCourses();
        List<Department> allDepartments = Department.GetAll();
        List<Department> registeredDepartment = newStudent.GetAssignedDepartment();
        model.Add("registeredDepartment", registeredDepartment);
        model.Add("departments", allDepartments);
        model.Add("student", newStudent);
        model.Add("courses", allCourses);
        return View["student.cshtml", model];
      };
    }
  }
}
