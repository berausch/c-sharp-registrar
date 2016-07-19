using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Registrar
{
  public class Department
  {
    private int _id;
    private string _name;
    private string _departmentCode;

    public Department(string name, string departmentCode, int id=0)
    {
      _id = id;
      _name = name;
      _departmentCode = departmentCode;
    }

    public string GetName()
    {
      return _name;
    }
    public int GetId()
    {
      return _id;
    }
    public string GetDepartmentCode()
    {
      return _departmentCode;
    }

    public override bool Equals(System.Object otherDepartment)
    {
      if (!(otherDepartment is Department))
      {
        return false;
      }
      else
      {
        Department newDepartment = (Department) otherDepartment;
        bool idEquality = this.GetId() == newDepartment.GetId();
        bool nameEquality = this.GetName() == newDepartment.GetName();
        bool departmentCodeEquality = this.GetDepartmentCode() == newDepartment.GetDepartmentCode();
        return (idEquality && nameEquality && departmentCodeEquality);
      }
    }

    public static List<Department> GetAll()
    {
      List<Department> allDepartments = new List<Department>{};
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM departments;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int departmentId = rdr.GetInt32(0);
        string departmentName = rdr.GetString(1);
        string departmentCode = rdr.GetString(2);
        Department newDepartment = new Department(departmentName, departmentCode, departmentId);
        allDepartments.Add(newDepartment);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allDepartments;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO departments (name, department_code) OUTPUT INSERTED.id VALUES (@departmentName, @departmentCode);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@departmentName";
      nameParameter.Value = this.GetName();

      cmd.Parameters.Add(nameParameter);

      SqlParameter departmentCodeParameter = new SqlParameter();
      departmentCodeParameter.ParameterName = "@departmentCode";
      departmentCodeParameter.Value = this.GetDepartmentCode();

      cmd.Parameters.Add(departmentCodeParameter);

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

    public static Department Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM departments WHERE id = @DepartmentId;", conn);
      SqlParameter departmentIdParameter = new SqlParameter();
      departmentIdParameter.ParameterName = "@DepartmentId";
      departmentIdParameter.Value = id.ToString();
      cmd.Parameters.Add(departmentIdParameter);
      rdr = cmd.ExecuteReader();

      int foundDepartmentId = 0;
      string foundDepartmentName = null;
      string foundDepartmentCode = null;
      while(rdr.Read())
      {
        foundDepartmentId = rdr.GetInt32(0);
        foundDepartmentName = rdr.GetString(1);
        foundDepartmentCode = rdr.GetString(2);
      }
      Department foundDepartment = new Department(foundDepartmentName, foundDepartmentCode, foundDepartmentId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundDepartment;
    }

    public List<Course> GetCourses()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT courses.* FROM departments JOIN departments_courses ON (departments.id = departments_courses.department_id) JOIN courses ON (departments_courses.course_id = courses.id) WHERE departments.id = @departmentId;", conn);

      SqlParameter departmentIdParameter = new SqlParameter();
      departmentIdParameter.ParameterName = "@departmentId";
      departmentIdParameter.Value = this.GetId().ToString();

      cmd.Parameters.Add(departmentIdParameter);

      rdr = cmd.ExecuteReader();

      List<Course> allCourses = new List<Course>{};

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

    public List<Student> GetStudents()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT students.* FROM students JOIN students_departments ON (students.id = students_departments.student_id) JOIN departments ON (students_departments.department_id = departments.id) WHERE departments.id = @departmentId;", conn);
      SqlParameter departmentIdParameter = new SqlParameter();
      departmentIdParameter.ParameterName = "@departmentId";
      departmentIdParameter.Value = this.GetId().ToString();
      cmd.Parameters.Add(departmentIdParameter);

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



    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM departments WHERE id = @DepartmentId;", conn);

      SqlParameter departmentIdParameter = new SqlParameter();
      departmentIdParameter.ParameterName = "@DepartmentId";
      departmentIdParameter.Value = this.GetId();

      cmd.Parameters.Add(departmentIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM departments;", conn);
      cmd.ExecuteNonQuery();
    }
  }
}
