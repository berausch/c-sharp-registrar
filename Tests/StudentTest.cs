using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Registrar
{
  public class StudentTest : IDisposable
  {
    public static DateTime DefaultDate = new DateTime(2016, 7, 19);
    public StudentTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Student.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_EqualOverrideTrueForSameDescription()
    {
      //Arrange, Act
      Student firstStudent = new Student("Mow the lawn", DefaultDate);
      Student secondStudent = new Student("Mow the lawn",  DefaultDate);

      //Assert
      Assert.Equal(firstStudent, secondStudent);
    }

    [Fact]
    public void Test_Save()
    {
      //Arrange
      Student testStudent = new Student("Mow the lawn", DefaultDate);
      testStudent.Save();

      //Act
      List<Student> result = Student.GetAll();
      List<Student> testList = new List<Student>{testStudent};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_FindFindsStudentInDatabase()
    {
      //Arrange
      Student testStudent = new Student("Mow the lawn", DefaultDate);
      testStudent.Save();

      //Act
      Student result = Student.Find(testStudent.GetId());

      //Assert
      Assert.Equal(testStudent, result);
    }

    [Fact]
    public void Test_DeleteDeletsStudentFromDatabase()
    {
      Student testStudent = new Student("Mow the lawn", DefaultDate);
      testStudent.Save();

      //Act
      testStudent.Delete();
      List<Student> allStudents = Student.GetAll();

      //Assert
      Assert.Equal(0, allStudents.Count);
    }

    [Fact]
    public void Test_GetCourses_GetsAllOfStudentsCourses()
    {
      //Arrange
      Student testStudent = new Student("Mow the lawn", DefaultDate);
      testStudent.Save();
      Course testCourse = new Course("Lawn Care", "HOR", 100);
      testCourse.Save();
      List<Course> expectedResult = new List<Course> {testCourse};
      //Act
      testStudent.AddCourse(testCourse.GetId());
      List<Course> result = testStudent.GetCourses();

      //Assert
      Assert.Equal(expectedResult, result);

    }

    // [Fact]
    // public void Testing()
    // {
    //   Student testStudent = new Student("Mow the lawn", DefaultDate);
    //   testStudent.Save();
    //   Course testCourse = new Course("Lawn Care", "HOR", 100);
    //   testCourse.Save();
    //
    //   testStudent.AddCourse(testCourse.GetId());
    //
    //   Assert.Equal(0, 0);
    //
    // }


    public void Dispose()
    {
      Student.DeleteAll();
      Course.DeleteAll();
    }
  }
}
