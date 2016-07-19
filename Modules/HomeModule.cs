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
        Dictionary<string, object> model = new Dictionary<string, object>{};
        List<Department> allDepartments = Department.GetAll();
        List<Course> allCourses = Course.GetAll();
        model.Add("departments", allDepartments);
        model.Add("courses", allCourses);
        return View["courses.cshtml", model];
      };

      Post["/courses"] = _ =>
      {
        Department newDepartment = Department.Find(Request.Form["department"]);
        Course newCourse = new Course(Request.Form["course-name"], newDepartment.GetDepartmentCode(), Request.Form["course-number"]);
        newCourse.Save();
        newCourse.AddDepartment(Request.Form["department"]);
        Dictionary<string, object> model = new Dictionary<string, object>{};
        List<Department> allDepartments = Department.GetAll();
        List<Course> allCourses = Course.GetAll();
        model.Add("departments", allDepartments);
        model.Add("courses", allCourses);
        return View["courses.cshtml", model];
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
        Dictionary<string, object> model = new Dictionary<string, object>{};
        List<Student> allStudents = Student.GetAll();
        List<Department> allDepartments = Department.GetAll();
        model.Add("students", allStudents);
        model.Add("departments", allDepartments);
        return View["students.cshtml", model];
      };

      Post["/students"] = _ =>
      {
        Department newDepartment = Department.Find(Request.Form["department"]);
        Student newStudent = new Student(Request.Form["student-name"], Request.Form["enrollment"]);
        newStudent.Save();
        newStudent.AddDepartment(Request.Form["department"]);
        Dictionary<string, object> model = new Dictionary<string, object>{};
        List<Department> allDepartments = Department.GetAll();
        List<Student> allStudents = Student.GetAll();
        model.Add("departments", allDepartments);
        model.Add("students", allStudents);
        return View["students.cshtml", model];
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
