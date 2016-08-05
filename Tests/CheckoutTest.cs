using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library
{
  public class CheckoutTest : IDisposable
  {
    public CheckoutTest()
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
    public void T1_Checkout_CreatesACheckoutRecord()
    {
      Patron testPatron = new Patron("Judy");
      testPatron.Save();

      Copy testCopy = new Copy(5);
      testCopy.Save();

      DateTime checkoutDate = new DateTime(2016,08,04);
      DateTime dueDate = new DateTime(2017,01,02);

      Checkout newCheckout = new Checkout(testCopy.GetId(), testPatron.GetId(), checkoutDate, dueDate);
      newCheckout.Save();

      List<Checkout> result = Checkout.GetAll();

      Assert.Equal(newCheckout, result[0]);
    }
  }
}
