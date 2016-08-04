using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library
{
  public class CopyTest : IDisposable
  {
    public CopyTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
    }
    public void Dispose()
    {
      Patron.DeleteAll();
      Copy.DeleteAll();
      Book.DeleteAll();
    }

    [Fact]
    public void T1_DBEmptyAtFirst()
    {
      int result = Copy.GetAll().Count;

      Assert.Equal(0, result);
    }

    [Fact]
    public void T2_Equal_ReturnsTrueIfCopyIsSame()
    {
      Copy firstCopy = new Copy(1);
      Copy secondCopy = new Copy(1);

      Assert.Equal(firstCopy, secondCopy);
    }

    [Fact]
    public void T3_Save_SavesToDB()
    {
      Copy testCopy = new Copy(1);
      testCopy.Save();

      List<Copy> result = Copy.GetAll();
      List<Copy> testList = new List<Copy>{testCopy};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void T4_Save_AssignsIdToCopy()
    {
      Copy testCopy = new Copy(2);
      testCopy.Save();

      Copy savedCopy = Copy.GetAll()[0];
      int result = savedCopy.GetId();
      int testId = testCopy.GetId();

      Assert.Equal(testId, result);
    }

    [Fact]
    public void T5_Find_FindsCopyInDatabase()
    {
      Copy testCopy = new Copy(1);
      testCopy.Save();

      Copy foundCopy = Copy.Find(testCopy.GetId());

      Assert.Equal(testCopy, foundCopy);
    }

    [Fact]
    public void T6_Update_UpdatesCopyInDB()
    {
      Copy testCopy = new Copy(1);
      testCopy.Save();

      int newBookId = 2;

      testCopy.Update(newBookId);

      int resultBookId = testCopy.GetBookId();

      Assert.Equal(newBookId, resultBookId);
    }
    [Fact]
    public void T7_Delete_DeletesCopyFromDB()
    {
      //Always remember to save to DB (Save())
      Copy testCopy1 = new Copy(1);
      testCopy1.Save();
      Copy testCopy2 = new Copy(2);
      testCopy2.Save();

      testCopy1.Delete();

      List<Copy> result = Copy.GetAll();
      List<Copy> testCopies = new List<Copy> {testCopy2};

      Assert.Equal(testCopies, result);
    }

    [Fact]
    public void T8_Addpatron_AddspatronToCopy()
    {
      Book newBook = new Book("Freedom");
      newBook.Save();

      Patron testPatron = new Patron("Sarah");
      testPatron.Save();


      Copy testCopy = new Copy(1);
      testCopy.Save();

      testCopy.AddPatron(testPatron);
      List<Patron> result = testCopy.GetPatron();
      List<Patron> testList = new List<Patron> {testPatron};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void T9_GetPatrons_ReturnsAllCopyPatrons()
    {
      Patron testPatron1 = new Patron("Sarah");
      testPatron1.Save();
      Patron testPatron2 = new Patron("John");
      testPatron2.Save();


      Copy testCopy = new Copy(5);
      testCopy.Save();

      testCopy.AddPatron(testPatron1);
      List<Patron> result = testCopy.GetPatron();
      List<Patron> testList= new List<Patron>{testPatron1};

      Assert.Equal(testList,result);
    }
    //
    //
    //
    //
    //
    //












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
