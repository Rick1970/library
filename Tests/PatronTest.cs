using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library
{
  public class PatronTest : IDisposable
  {
    public PatronTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
    }
    public void Dispose()
    {
      Patron.DeleteAll();
      Copy.DeleteAll();
      Book.DeleteAll();
      Checkout.DeleteAll();
    }

    [Fact]
    public void T1_DBEmptyAtFirst()
    {
      int result = Patron.GetAll().Count;

      Assert.Equal(0, result);
    }

    [Fact]
    public void T2_Equal_ReturnsTrueIfPatronIsSame()
    {
      Patron firstPatron = new Patron("Judy");
      Patron secondPatron = new Patron("Judy");

      Assert.Equal(firstPatron, secondPatron);
    }

    [Fact]
    public void T3_Save_SavesToDB()
    {
      Patron testPatron = new Patron("Judy");
      testPatron.Save();

      List<Patron> result = Patron.GetAll();
      List<Patron> testList = new List<Patron>{testPatron};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void T4_Save_AssignsIdToPatron()
    {
      Patron testPatron = new Patron("Judy");
      testPatron.Save();

      Patron savedPatron = Patron.GetAll()[0];
      int result = savedPatron.GetId();
      int testId = testPatron.GetId();

      Assert.Equal(testId, result);
    }

    [Fact]
    public void T5_Find_FindsPatronInDatabase()
    {
      Patron testPatron = new Patron("Judy");
      testPatron.Save();

      Patron foundPatron = Patron.Find(testPatron.GetId());

      Assert.Equal(testPatron, foundPatron);
    }

    [Fact]
    public void T6_Update_UpdatesPatronInDB()
    {
      Patron testPatron = new Patron("Judy");
      testPatron.Save();

      string newName = "Barb";

      testPatron.Update(newName);

      string resultName = testPatron.GetName();

      Assert.Equal(newName, resultName);
    }
    [Fact]
    public void T7_Delete_DeletesPatronFromDB()
    {
      //Always remember to save to DB (Save())
      Patron testPatron1 = new Patron("Judy");
      testPatron1.Save();
      Patron testPatron2 = new Patron("Barb");
      testPatron2.Save();

      testPatron1.Delete();

      List<Patron> result = Patron.GetAll();
      List<Patron> testPatrons = new List<Patron> {testPatron2};

      Assert.Equal(testPatrons, result);
    }

    [Fact]
    public void T8_AddCopy_AddsCopyToPatron()
    {
      Book newBook = new Book("Freedom");
      newBook.Save();

      Copy testCopy = new Copy(newBook.GetId());
      testCopy.Save();


      Patron testPatron = new Patron("Barb");
      testPatron.Save();

      testPatron.AddCopy(testCopy);
      List<Copy> result = testPatron.GetCopies();
      List<Copy> testList = new List<Copy> {testCopy};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void T9_GetCopys_ReturnsAllPatronCopys()
    {
      Patron testPatron = new Patron("Barb");
      testPatron.Save();

      Copy testCopy1 = new Copy(5);
      testCopy1.Save();

      Copy testCopy2 = new Copy(6);
      testCopy2.Save();

      testPatron.AddCopy(testCopy1);
      List<Copy> result = testPatron.GetCopies();
      List<Copy> testList= new List<Copy>{testCopy1};

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
