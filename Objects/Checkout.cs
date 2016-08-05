using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Library
{
  public class Checkout
  {
    private int _id;
    private int _copy_id;
    private int _patron_id;
    private DateTime _checkout_date;
    private DateTime _due_date;

    public Checkout (int CopyId, int PatronId, DateTime CheckoutDate, DateTime DueDate, int Id =0)
    {
      _id = Id;
      _copy_id =CopyId;
      _patron_id = PatronId;
      _checkout_date = CheckoutDate;
      _due_date = DueDate;
    }

    public override bool Equals(System.Object otherCheckout)
    {
      if (!(otherCheckout is Checkout))
      {
        return false;
      }
      else
      {
        Checkout newCheckout = (Checkout) otherCheckout;
        bool idEquality = this.GetId() == newCheckout.GetId();
        return (idEquality);
      }
    }


    //   public void CreateCheckout(Copy newCopy, Patron newPatron, DateTime checkoutDate, DateTime dueDate)
    //   {
    //     SqlConnection conn = DB.Connection();
    //     conn.Open();
    //
    //     SqlCommand cmd = new SqlCommand("INSERT INTO checkouts (patron_id, copy_id, checkout_date, due_date) VALUES (@PatronId, @CopyId, @CheckoutDate, @DueDate);", conn);
    //
    //     SqlParameter patronIdParameter = new SqlParameter();
    //     patronIdParameter.ParameterName = "@PatronId";
    //     patronIdParameter.Value = newPatron.GetId();
    //     cmd.Parameters.Add(patronIdParameter);
    //
    //     SqlParameter copyIdParameter = new SqlParameter();
    //     copyIdParameter.ParameterName = "@CopyId";
    //     copyIdParameter.Value = newCopy.GetId();
    //     cmd.Parameters.Add(copyIdParameter);
    //
    //     SqlParameter checkoutDateParameter = new SqlParameter();
    //     checkoutDateParameter.ParameterName = "@CheckoutDate";
    //     checkoutDateParameter.Value = checkoutDate;
    //     cmd.Parameters.Add(checkoutDateParameter);
    //
    //     SqlParameter dueDateParameter = new SqlParameter();
    //     dueDateParameter.ParameterName = "@DueDate";
    //     dueDateParameter.Value = dueDate;
    //     cmd.Parameters.Add(dueDateParameter);
    //
    //     cmd.ExecuteNonQuery();
    //
    //     if (conn != null)
    //     {
    //       conn.Close();
    //     }
    //   }
    // //
    public int GetCopyId()
    {
      return _copy_id;
    }
    public int GetPatronId()
    {
      return _patron_id;
    }
    public string GetCheckoutDate()
    {
      return _checkout_date.ToString("MM/dd/yyyy");
    }
    public string GetDueDate()
    {
      return _due_date.ToString("MM/dd/yyyy");
    }
    public int GetId()
    {
      return _id;
    }

    public static List<Checkout> GetAll()
    {
      List<Checkout> allCheckouts = new List<Checkout>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM checkouts;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        int id = rdr.GetInt32(0);
        int copyId = rdr.GetInt32(1);
        int patronId = rdr.GetInt32(2);
        DateTime checkoutDate = rdr.GetDateTime(3);
        DateTime dueDate = rdr.GetDateTime(4);

        Checkout newCheckout = new Checkout(copyId, patronId, checkoutDate, dueDate, id);
        allCheckouts.Add(newCheckout);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allCheckouts;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO checkouts (patron_id, copy_id, checkout_date, due_date) OUTPUT INSERTED.id VALUES (@PatronId, @CopyId, @CheckoutDate, @DueDate);", conn);

      SqlParameter patronIdParameter = new SqlParameter();
      patronIdParameter.ParameterName = "@PatronId";
      patronIdParameter.Value = this.GetPatronId();

      SqlParameter copyIdParameter = new SqlParameter();
      copyIdParameter.ParameterName = "@CopyId";
      copyIdParameter.Value = this.GetCopyId();

      SqlParameter checkoutDateParameter = new SqlParameter();
      checkoutDateParameter.ParameterName = "@CheckoutDate";
      checkoutDateParameter.Value = this.GetCheckoutDate();

      SqlParameter dueDateParameter = new SqlParameter();
      dueDateParameter.ParameterName = "@DueDate";
      dueDateParameter.Value = this.GetDueDate();

      cmd.Parameters.Add(patronIdParameter);
      cmd.Parameters.Add(copyIdParameter);
      cmd.Parameters.Add(checkoutDateParameter);
      cmd.Parameters.Add(dueDateParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }
    // //
    // public static Patron Find(int id)
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("SELECT * FROM patrons WHERE id = @PatronId;", conn);
    //   SqlParameter patronIdParameter = new SqlParameter();
    //   patronIdParameter.ParameterName = "@PatronId";
    //   patronIdParameter.Value = id.ToString();
    //
    //   cmd.Parameters.Add(patronIdParameter);
    //
    //   SqlDataReader rdr = cmd.ExecuteReader();
    //
    //   int foundPatronId = 0;
    //   string foundPatronName = null;
    //
    //   while (rdr.Read())
    //   {
    //     foundPatronId = rdr.GetInt32(0);
    //     foundPatronName = rdr.GetString(1);
    //   }
    //   Patron foundPatron = new Patron(foundPatronName, foundPatronId);
    //
    //   if (rdr != null)
    //   {
    //     rdr.Close();
    //   }
    //   if (conn != null)
    //   {
    //     conn.Close();
    //   }
    //   return foundPatron;
    // }
    //
    // public void Update(string newName)
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //
    //   SqlCommand cmd = new SqlCommand("UPDATE patrons SET name = @NewName OUTPUT INSERTED.name WHERE id = @PatronId;", conn);
    //
    //   SqlParameter newNameParameter = new SqlParameter();
    //   newNameParameter.ParameterName = "@NewName";
    //   newNameParameter.Value = newName;
    //
    //   SqlParameter patronIdParameter = new SqlParameter();
    //   patronIdParameter.ParameterName = "@PatronId";
    //   patronIdParameter.Value = this.GetId();
    //
    //   cmd.Parameters.Add(newNameParameter);
    //   cmd.Parameters.Add(patronIdParameter);
    //
    //   SqlDataReader rdr = cmd.ExecuteReader();
    //
    //   while (rdr.Read())
    //   {
    //     this._name = rdr.GetString(0);
    //   }
    //
    //   if (rdr != null)
    //   {
    //     rdr.Close();
    //   }
    //   if (conn != null)
    //   {
    //     conn.Close();
    //   }
    // }
    //
    // public void AddCopy(Copy newCopy)
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("INSERT INTO checkouts (patron_id, copy_id) VALUES (@PatronId, @CopyId);", conn);
    //
    //   SqlParameter patronIdParameter = new SqlParameter();
    //   patronIdParameter.ParameterName = "@PatronId";
    //   patronIdParameter.Value = this.GetId();
    //   cmd.Parameters.Add(patronIdParameter);
    //
    //   SqlParameter copyIdParameter = new SqlParameter();
    //   copyIdParameter.ParameterName = "@CopyId";
    //   copyIdParameter.Value = newCopy.GetId();
    //   cmd.Parameters.Add(copyIdParameter);
    //
    //   cmd.ExecuteNonQuery();
    //
    //   if (conn != null)
    //   {
    //     conn.Close();
    //   }
    // }
    //
    // public List<Copy> GetCopies()
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("SELECT copies.* FROM patrons JOIN checkouts ON (patrons.id = checkouts.patron_id) JOIN copies ON (checkouts.copy_id = copies.id) WHERE patrons.id = @PatronId",conn);
    //
    //   SqlParameter patronIdParameter = new SqlParameter();
    //   patronIdParameter.ParameterName= "@PatronId";
    //   patronIdParameter.Value=this.GetId();
    //   cmd.Parameters.Add(patronIdParameter);
    //
    //   SqlDataReader rdr = cmd.ExecuteReader();
    //   List<Copy> copies = new List<Copy> {};
    //
    //   while(rdr.Read())
    //   {
    //     int copyId = rdr.GetInt32(0);
    //     int copyBookId = rdr.GetInt32(1);
    //     Copy newCopy = new Copy(copyBookId, copyId);
    //     copies.Add(newCopy);
    //   }
    //
    //   if(rdr !=null)
    //   {
    //     rdr.Close();
    //   }
    //
    //   if (conn != null)
    //   {
    //     conn.Close();
    //   }
    //   return copies;
    // }
    //
    // public void Delete()
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("DELETE FROM patrons WHERE id = @PatronId;", conn);
    //
    //   SqlParameter patronIdParameter = new SqlParameter();
    //   patronIdParameter.ParameterName = "@PatronId";
    //   patronIdParameter.Value = this.GetId();
    //
    //   cmd.Parameters.Add(patronIdParameter);
    //
    //   cmd.ExecuteNonQuery();
    //
    //   if (conn != null)
    //   {
    //     conn.Close();
    //   }
    // }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM checkouts;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
