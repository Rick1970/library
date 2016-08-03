using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library
{
  public class LibraryTest : IDisposable
  {
    public LibraryTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
    }
    public void Dispose()
    {
      Author.DeleteAll();
      Book.DeleteAll();
    }

    [Fact]
    public void T1_DBEmptyAtFirst()
    {
      int result = Author.GetAll().Count;

      Assert.Equal(0, result);
    }

    [Fact]
    public void T2_Equal_ReturnsTrueIfAuthorIsSame()
    {
      Author firstAuthor = new Author("Rowling");
      Author secondAuthor = new Author("Rowling");

      Assert.Equal(firstAuthor, secondAuthor);
    }

    [Fact]
    public void T3_Save_SavesToDB()
    {
      Author testAuthor = new Author("Rowling");
      testAuthor.Save();

      List<Author> result = Author.GetAll();
      List<Author> testList = new List<Author>{testAuthor};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void T4_Save_AssignsIdToAuthor()
    {
      Author testAuthor = new Author("Rowling");
      testAuthor.Save();

      Author savedAuthor = Author.GetAll()[0];
      int result = savedAuthor.GetId();
      int testId = testAuthor.GetId();

      Assert.Equal(testId, result);
    }

    [Fact]
    public void T5_Find_FindsAuthorInDatabase()
    {
      Author testAuthor = new Author("Rowling");
      testAuthor.Save();

      Author foundAuthor = Author.Find(testAuthor.GetId());

      Assert.Equal(testAuthor, foundAuthor);
    }

    [Fact]
    public void T6_Update_UpdatesAuthorInDB()
    {
      Author testAuthor = new Author("Rowling");
      testAuthor.Save();

      string newName = "Franzen";

      testAuthor.Update(newName);

      string resultName = testAuthor.GetName();

      Assert.Equal(newName, resultName);
    }
    [Fact]
    public void T7_Delete_DeletesAuthorFromDB()
    {
      //Always remember to save to DB (Save())
      Author testAuthor1 = new Author("Rowling");
      testAuthor1.Save();
      Author testAuthor2 = new Author("Franzen");
      testAuthor2.Save();

      testAuthor1.Delete();

      List<Author> result = Author.GetAll();
      List<Author> testAuthors = new List<Author> {testAuthor2};

      Assert.Equal(testAuthors, result);
    }

    [Fact]
    public void T8_AddBook_AddsBookToAuthor()
    {
      Book testBook = new Book("Freedom");
      testBook.Save();

      Author testAuthor = new Author("Franzen");
      testAuthor.Save();

      testAuthor.AddBook(testBook);
      List<Book> result = testAuthor.GetBooks();
      List<Book> testList = new List<Book> {testBook};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void T9_GetBooks_ReturnsAllAuthorBooks()
    {
      Author testAuthor = new Author("Franzen");
      testAuthor.Save();

      Book testBook1 = new Book("Harry Potter");
      testBook1.Save();

      Book testBook2 = new Book("Freedom");
      testBook2.Save();

      testAuthor.AddBook(testBook1);
      List<Book> result = testAuthor.GetBooks();
      List<Book> testList= new List<Book>{testBook1};

      Assert.Equal(testList,result);
    }


    //
    // [Fact]
    // public void T8_GetClients_RetrievesAllClientsOfStylist()
    // {
    //   Stylist testStylist = new Stylist("Jake", "Shears", "L.5 Master");
    //   testStylist.Save();
    //
    //   Client testClient1 = new Client("Shaggy", "Dew", testStylist.GetId());
    //   testClient1.Save();
    //   Client testClient2 = new Client("Lange", "Ponyta", testStylist.GetId());
    //   testClient2.Save();
    //
    //   List<Client> testClients = new List<Client> {testClient1, testClient2};
    //   List<Client> result = testStylist.GetClients();
    //
    //   Assert.Equal(testClients, result);
    // }
    //
    // [Fact]
    // public void T9_DeleteStylistClients_DeletesClientIfNoStylist()
    // {
    //   Stylist testStylist = new Stylist("Clementine", "Clips", "L.4 Specialist");
    //   testStylist.Save();
    //
    //   Client testClient = new Client("Shaggy", "Dew", testStylist.GetId());
    //   testClient.Save();
    //
    //   testStylist.DeleteStylistClients();
    //   testStylist.Delete();
    //
    //   List<Client> result = Client.GetAll();
    //   int resultCount = result.Count;
    //   List<Client> testClients = new List<Client> {};
    //   int testCount = testClients.Count;
    //
    //   Assert.Equal(testCount, resultCount);
    // }
  }
}
