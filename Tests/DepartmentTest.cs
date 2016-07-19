using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Registrar
{
  public class DepartmentTest : IDisposable
  {
    public static DateTime DefaultDate = new DateTime(2016, 7, 19);
    public DepartmentTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Department.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_EqualOverrideTrueForSameDescription()
    {
      //Arrange, Act
      Department firstDepartment = new Department("Math", "MTH");
      Department secondDepartment = new Department("Math", "MTH");

      //Assert
      Assert.Equal(firstDepartment, secondDepartment);
    }

    [Fact]
    public void Test_Save()
    {
      //Arrange
      Department testDepartment = new Department("Math", "MTH");
      testDepartment.Save();

      //Act
      List<Department> result = Department.GetAll();
      List<Department> testList = new List<Department>{testDepartment};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_FindFindsDepartmentInDatabase()
    {
      //Arrange
      Department testDepartment = new Department("Math", "MTH");
      testDepartment.Save();

      //Act
      Department result = Department.Find(testDepartment.GetId());

      //Assert
      Assert.Equal(testDepartment, result);
    }

    [Fact]
    public void Test_DeleteDeletesDepartmentFromDatabase()
    {
      Department testDepartment = new Department("Math", "MTH");
      testDepartment.Save();

      //Act
      testDepartment.Delete();
      List<Department> allDepartments = Department.GetAll();

      //Assert
      Assert.Equal(0, allDepartments.Count);
    }

    [Fact]
    public void Test_GetCourses_GetsAllOfDepartmentsCourses()
    {
      //Arrange
      Department testDepartment = new Department("Math", "MTH");
      testDepartment.Save();
      Course testCourse = new Course("Calculus", "MTH", 200);
      testCourse.Save();
      List<Course> expectedResult = new List<Course> {testCourse};
      //Act
      testCourse.AddDepartment(testDepartment.GetId());
      List<Course> result = testDepartment.GetCourses();

      //Assert
      Assert.Equal(expectedResult, result);

    }
    public void Dispose()
    {
      Department.DeleteAll();
      Course.DeleteAll();
    }
  }
}
