using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Registrar
{
  public class Student
  {
    private int _id;
    private string _name;
    private DateTime _enrollment;

    public Student(string name, DateTime enrollment, int id = 0)
    {
      _id  = id;
      _name = name;
      _enrollment = enrollment;
    }

    public override bool Equals(System.Object otherStudent)
    {
      if (!(otherStudent is Student))
      {
        return false;
      }
      else
      {
        Student newStudent = (Student) otherStudent;
        bool idEquality = this.GetId() == newStudent.GetId();
        bool nameEquality = this.GetName() == newStudent.GetName();
        bool enrollmentEquality = this.GetEnrollment() == newStudent.GetEnrollment();
        return (idEquality && nameEquality && enrollmentEquality);
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
    public DateTime GetEnrollment()
    {
      return _enrollment;
    }
    public void SetEnrollment(DateTime newEnrollment)
    {
      _enrollment = newEnrollment;
    }

    public static List<Student> GetAll()
    {
      List<Student> allStudents = new List<Student>{};
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM students;", conn);
      rdr = cmd.ExecuteReader();

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

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO students (name, enrollment) OUTPUT INSERTED.id VALUES (@studentName, @studentEnrollment);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@studentName";
      nameParameter.Value = this.GetName();

      cmd.Parameters.Add(nameParameter);

      SqlParameter enrollmentParameter = new SqlParameter();
      enrollmentParameter.ParameterName = "@studentEnrollment";
      enrollmentParameter.Value = this.GetEnrollment();

      cmd.Parameters.Add(enrollmentParameter);

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

    public static Student Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM students WHERE id = @StudentId;", conn);
      SqlParameter studentIdParameter = new SqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = id.ToString();
      cmd.Parameters.Add(studentIdParameter);
      rdr = cmd.ExecuteReader();

      int foundStudentId = 0;
      string foundStudentName = null;
      DateTime foundEnrollment = DateTime.Now;
      while(rdr.Read())
      {
        foundStudentId = rdr.GetInt32(0);
        foundStudentName = rdr.GetString(1);
        foundEnrollment = rdr.GetDateTime(2);
      }
      Student foundStudent = new Student(foundStudentName, foundEnrollment, foundStudentId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundStudent;
    }

    public void EditStudent(string newName, DateTime newEnrollment, int newDepartmentId)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE students SET name = @name, enrollment = @enrollment WHERE id = @studentId;", conn);
      SqlParameter studentIdParameter = new SqlParameter();
      studentIdParameter.ParameterName = "@studentId";
      studentIdParameter.Value = this.GetId();

      cmd.Parameters.Add(studentIdParameter);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@name";
      nameParameter.Value = newName;

      cmd.Parameters.Add(nameParameter);

      SqlParameter enrollmentParameter = new SqlParameter();
      enrollmentParameter.ParameterName = "@enrollment";
      enrollmentParameter.Value = newEnrollment;

      cmd.Parameters.Add(enrollmentParameter);
      cmd.ExecuteNonQuery();

      SqlCommand cmd2 = new SqlCommand("UPDATE students_departments SET department_id = @departmentId WHERE student_id = @studentId;", conn);

      SqlParameter studentIdParameter2 = new SqlParameter();
      studentIdParameter2.ParameterName = "@studentId";
      studentIdParameter2.Value = this.GetId();

      cmd2.Parameters.Add(studentIdParameter2);

      SqlParameter departmentIdParameter = new SqlParameter();
      departmentIdParameter.ParameterName = "@departmentId";
      departmentIdParameter.Value = newDepartmentId;

      cmd2.Parameters.Add(departmentIdParameter);

      cmd2.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public void AddCourse(int courseId)
    {

      if (!(this.GetCourses().Contains(Course.Find(courseId))))
      {
        SqlConnection conn = DB.Connection();
        conn.Open();

        SqlCommand cmd = new SqlCommand("INSERT INTO students_courses (student_id, course_id) VALUES (@studentId, @courseId);", conn);

        SqlParameter studentIdParameter = new SqlParameter();
        studentIdParameter.ParameterName = "@studentId";
        studentIdParameter.Value = this._id;
        cmd.Parameters.Add(studentIdParameter);

        SqlParameter courseIdParameter = new SqlParameter();
        courseIdParameter.ParameterName = "@courseId";
        courseIdParameter.Value = courseId;
        cmd.Parameters.Add(courseIdParameter);

        cmd.ExecuteNonQuery();

        if (conn != null)
        {
          conn.Close();
        }
      }
    }

    public void AddDepartment(int departmentId)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO students_departments (student_id, department_id) VALUES (@studentId, @departmentId);", conn);

      SqlParameter studentIdParameter = new SqlParameter();
      studentIdParameter.ParameterName = "@studentId";
      studentIdParameter.Value = this._id;
      cmd.Parameters.Add(studentIdParameter);

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

      SqlCommand cmd = new SqlCommand("SELECT departments.* FROM students JOIN students_departments ON (students.id = students_departments.student_id) JOIN departments ON (students_departments.department_id = departments.id) WHERE students.id = @studentId", conn);

      SqlParameter studentIdParameter = new SqlParameter();
      studentIdParameter.ParameterName = "@studentId";
      studentIdParameter.Value = this.GetId().ToString();

      cmd.Parameters.Add(studentIdParameter);

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

    public List<Course> GetCourses()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT courses.* FROM students JOIN students_courses ON (students.id = students_courses.student_id) JOIN courses ON (students_courses.course_id = courses.id) WHERE students.id = @studentId;", conn);

      SqlParameter studentIdParameter = new SqlParameter();
      studentIdParameter.ParameterName = "@studentId";
      studentIdParameter.Value = this.GetId().ToString();

      cmd.Parameters.Add(studentIdParameter);

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

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM students; DELETE FROM students_courses", conn);
      cmd.ExecuteNonQuery();
    }
    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM students WHERE id = @StudentId; DELETE FROM students_courses WHERE student_id = @StudentId;", conn);

      SqlParameter studentIdParameter = new SqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = this.GetId();

      cmd.Parameters.Add(studentIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

  }
}
