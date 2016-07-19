using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Registrar
{
  public class CourseTest : IDisposable
  {
    public static DateTime DefaultDate = new DateTime(2016, 7, 19);
    public CourseTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Course.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_EqualOverrideTrueForSameDescription()
    {
      //Arrange, Act
      Course firstCourse = new Course("Lawn Care", "HOR", 100);
      Course secondCourse = new Course("Lawn Care", "HOR", 100);

      //Assert
      Assert.Equal(firstCourse, secondCourse);
    }

    [Fact]
    public void Test_Save()
    {
      //Arrange
      Course testCourse = new Course("Lawn Care", "HOR", 100);
      testCourse.Save();

      //Act
      List<Course> result = Course.GetAll();
      List<Course> testList = new List<Course>{testCourse};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_FindFindsCourseInDatabase()
    {
      //Arrange
      Course testCourse = new Course("Lawn Care", "HOR", 100);
      testCourse.Save();

      //Act
      Course result = Course.Find(testCourse.GetId());

      //Assert
      Assert.Equal(testCourse, result);
    }

    [Fact]
    public void Test_DeleteDeletsCourseFromDatabase()
    {
      Course testCourse = new Course("Lawn Care", "HOR", 100);
      testCourse.Save();

      //Act
      testCourse.Delete();
      List<Course> allCourses = Course.GetAll();

      //Assert
      Assert.Equal(0, allCourses.Count);
    }
    [Fact]
    public void Test_GetStudents_GetsAllStudentsEnrolledInCourse()
    {
      //Arrange
      Student testStudent = new Student("Mow the lawn", DefaultDate);
      testStudent.Save();
      Student testStudentTwo = new Student("Lazy Larry", DefaultDate);
      testStudentTwo.Save();
      Course testCourse = new Course("Lawn Care", "HOR", 100);
      testCourse.Save();
      testStudent.AddCourse(testCourse.GetId());
      List<Student> expectedResult = new List<Student> {testStudent};

      //Act
      List<Student> result = testCourse.GetStudents();

      //Assert
      Assert.Equal(expectedResult, result);
    }

    public void Dispose()
    {
      Course.DeleteAll();
      Student.DeleteAll();
    }
  }
}
