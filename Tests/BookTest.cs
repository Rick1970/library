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

    [Fact]
    public void T2_Equal_ReturnsTrueIfTitlesAreTheSame()
    {
      Book firstBook = new Book("Harry Potter and the Deathly Hallows");
      Book secondBook = new Book("Harry Potter and the Deathly Hallows");
      Assert.Equal(firstBook, secondBook);
    }

    public void Dispose()
    {
      Book.DeleteAll();
    }

    [Fact]
    public void T3_Save_SavesToDatabase()
    {

      Book testBook = new Book("Harry Potter and the Deathly Hallows");
      testBook.Save();
      List<Book> result =Book.GetAll();
      List<Book> testList = new List<Book>{testBook};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void T4_Save_AssignsIdToObject()
    {
      Book testBook = new Book("Harry Potter and the Deathly Hallows");
      testBook.Save();

      Book savedBook = Book.GetAll()[0];
      int result = savedBook.GetId();
      int testId = testBook.GetId();

      Assert.Equal(testId,result);
    }

    [Fact]
    public void T5_Find_FindsBookInDatabase()
    {
      Book testBook = new Book("Harry Potter and the Deathly Hallows");
      testBook.Save();

      Book foundBook = Book.Find(testBook.GetId());

      Assert.Equal(testBook, foundBook);
    }
    [Fact]
    public void T6_Update_UpdatesBookInDB()
    {
      Book testBook = new Book("Harry Potter and the Deathly Hallows");
      testBook.Save();

      string newTitle = "Freedom";

      testBook.Update(newTitle);

      string resultTitle = testBook.GetTitle();

      Assert.Equal(newTitle, resultTitle);
    }

    [Fact]
    public void T7_Delete_DeletesBookFromDB()
    {
      Book testBook1 = new Book("Harry Potter and the Deathly Hallows");
      testBook1.Save();
      Book testBook2 = new Book("Freedom");
      testBook2.Save();

      testBook1.Delete();

      List<Book> result = Book.GetAll();
      List<Book> testBooks = new List<Book> {testBook2};

      Assert.Equal(testBooks, result);
    }

  }
}
