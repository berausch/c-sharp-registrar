using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Registrar
{
  public class CourseTest : IDisposable
  {
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

    public void Dispose()
    {
      Course.DeleteAll();
    }
  }
}
