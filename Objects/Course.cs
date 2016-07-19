using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Registrar
{
  public class Course
  {
    private int _id;
    private string _name;
    private string _department;
    private int _courseNumber;

    public Course(string name, string department, int courseNumber, int id = 0)
    {
      _id = id;
      _name = name;
      _department = department;
      _courseNumber = courseNumber;
    }

    public override bool Equals(System.Object otherCourse)
    {
      if (!(otherCourse is Course))
      {
        return false;
      }
      else
      {
        Course newCourse = (Course) otherCourse;
        bool idEquality = this.GetId() == newCourse.GetId();
        bool nameEquality = this.GetName() == newCourse.GetName();
        bool departmentEquality = this.GetDepartment() == newCourse.GetDepartment();
        bool courseNumberEquality = this.GetCourseNumber() == newCourse.GetCourseNumber();
        return (idEquality && nameEquality && departmentEquality && courseNumberEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }
    public string GetDepartment()
    {
      return _department;
    }
    public void SetDepartment(string newDepartment)
    {
      _department = newDepartment;
    }
    public int GetCourseNumber()
    {
      return _courseNumber;
    }
    public void SetCourseNumber(int newCourseNumber)
    {
      _courseNumber = newCourseNumber;
    }

    public static List<Course> GetAll()
    {
      List<Course> allCourses = new List<Course>{};
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM courses;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int courseId = rdr.GetInt32(0);
        string courseName = rdr.GetString(1);
        string courseDepartment = rdr.GetString(2);
        int courseNumber = rdr.GetInt32(3);
        Course newCourse = new Course(courseName, courseDepartment, courseNumber, courseId);
        allCourses.Add(newCourse);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allCourses;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO courses (name, department, course_number) OUTPUT INSERTED.id VALUES (@courseName, @courseDepartment, @courseNumber);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@courseName";
      nameParameter.Value = this.GetName();

      cmd.Parameters.Add(nameParameter);

      SqlParameter departmentParameter = new SqlParameter();
      departmentParameter.ParameterName = "@courseDepartment";
      departmentParameter.Value = this.GetDepartment();

      cmd.Parameters.Add(departmentParameter);

      SqlParameter courseNumberParameter = new SqlParameter();
      courseNumberParameter.ParameterName = "@courseNumber";
      courseNumberParameter.Value = this.GetCourseNumber();

      cmd.Parameters.Add(courseNumberParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static Course Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM courses WHERE id = @CourseId;", conn);
      SqlParameter courseIdParameter = new SqlParameter();
      courseIdParameter.ParameterName = "@CourseId";
      courseIdParameter.Value = id.ToString();
      cmd.Parameters.Add(courseIdParameter);
      rdr = cmd.ExecuteReader();

      int foundCourseId = 0;
      string foundCourseName = null;
      string foundDepartment = null;
      int foundCourseNumber = 0;
      while(rdr.Read())
      {
        foundCourseId = rdr.GetInt32(0);
        foundCourseName = rdr.GetString(1);
        foundDepartment = rdr.GetString(2);
        foundCourseNumber = rdr.GetInt32(3);
      }
      Course foundCourse = new Course(foundCourseName, foundDepartment, foundCourseNumber, foundCourseId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundCourse;
    }

    public List<Student> GetStudents()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT students.* FROM students JOIN students_courses ON (students.id = students_courses.student_id) JOIN courses ON (students_courses.course_id = courses.id) WHERE courses.id = @courseId;", conn);
      SqlParameter courseIdParameter = new SqlParameter();
      courseIdParameter.ParameterName = "@courseId";
      courseIdParameter.Value = this.GetId().ToString();
      cmd.Parameters.Add(courseIdParameter);

      rdr = cmd.ExecuteReader();
      List<Student> allStudents = new List<Student> {};
      while(rdr.Read())
      {
        int studentId = rdr.GetInt32(0);
        string studentName = rdr.GetString(1);
        DateTime studentEnrollment = rdr.GetDateTime(2);
        Student newStudent = new Student(studentName, studentEnrollment, studentId);
        allStudents.Add(newStudent);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allStudents;
    }

    public void AddDepartment(int departmentId)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO departments_courses (department_id, course_id) VALUES (@departmentId, @courseId);", conn);

      SqlParameter courseIdParameter = new SqlParameter();
      courseIdParameter.ParameterName = "@courseId";
      courseIdParameter.Value = this._id;
      cmd.Parameters.Add(courseIdParameter);

      SqlParameter departmentIdParameter = new SqlParameter();
      departmentIdParameter.ParameterName = "@departmentId";
      departmentIdParameter.Value = departmentId;
      cmd.Parameters.Add(departmentIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public List<Department> GetAssignedDepartment()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT departments.* FROM courses JOIN departments_courses ON (courses.id = departments_courses.course_id) JOIN departments ON (departments_courses.department_id = departments.id) WHERE courses.id = @courseId", conn);

      SqlParameter courseIdParameter = new SqlParameter();
      courseIdParameter.ParameterName = "@courseId";
      courseIdParameter.Value = this.GetId().ToString();

      cmd.Parameters.Add(courseIdParameter);

      rdr = cmd.ExecuteReader();

      List<Department> assignedDepartment = new List<Department> {};
      while(rdr.Read())
      {
        int foundDepartmentId = rdr.GetInt32(0);
        string foundDepartmentName = rdr.GetString(1);
        string foundDepartmentCode = rdr.GetString(2);
        Department foundDepartment = new Department(foundDepartmentName, foundDepartmentCode, foundDepartmentId);
        assignedDepartment.Add(foundDepartment);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return assignedDepartment;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM courses;", conn);
      cmd.ExecuteNonQuery();
    }
    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM courses WHERE id = @CourseId;", conn);

      SqlParameter courseIdParameter = new SqlParameter();
      courseIdParameter.ParameterName = "@CourseId";
      courseIdParameter.Value = this.GetId();

      cmd.Parameters.Add(courseIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
  }
}
