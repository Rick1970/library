using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library
{
  public class BookTest : IDisposable
  {
    public BookTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void T1_DatabaseEmptyAtFirst()
    {
      int result = Book.GetAll().Count;

      Assert.Equal(0,result);
    }


    public void Dispose()
    {
      Book.DeleteAll();
    }
  }
}
