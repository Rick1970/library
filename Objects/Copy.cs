using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Library
{
  public class Copy
  {
    private int _id;
    private int _book_id;

    public Copy (int BookId, int Id = 0)
    {
      _id = Id;
      _book_id = BookId;
    }

    public override bool Equals(System.Object otherCopy)
    {
      if (!(otherCopy is Copy))
      {
        return false;
      }
      else
      {
        Copy newCopy = (Copy) otherCopy;
        bool idEquality = this.GetId() == newCopy.GetId();
        bool BookIdEquality = this.GetBookId() == newCopy.GetBookId();

        return (idEquality && BookIdEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }

    public int GetBookId()
    {
      return _book_id;
    }
    public void SetBookId(int newBookId)
    {
      _book_id = newBookId;
    }

    public static List<Copy> GetAll()
    {
      List<Copy> allcopies = new List<Copy>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM copies;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        int copyId = rdr.GetInt32(0);
        int copyBookId = rdr.GetInt32(1);

        Copy newCopy = new Copy(copyBookId, copyId);
        allcopies.Add(newCopy);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allcopies;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO copies (book_id) OUTPUT INSERTED.id VALUES (@CopyBookId);", conn);

      SqlParameter bookIdParameter = new SqlParameter();
      bookIdParameter.ParameterName = "@CopyBookId";
      bookIdParameter.Value = this.GetBookId();

      cmd.Parameters.Add(bookIdParameter);

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
    //
    public static Copy Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM copies WHERE id = @CopyId;", conn);
      SqlParameter copyIdParameter = new SqlParameter();
      copyIdParameter.ParameterName = "@CopyId";
      copyIdParameter.Value = id.ToString();

      cmd.Parameters.Add(copyIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      int foundCopyId = 0;
      int foundCopyBookId = 0;

      while (rdr.Read())
      {
        foundCopyId = rdr.GetInt32(0);
        foundCopyBookId = rdr.GetInt32(1);
      }
      Copy foundCopy = new Copy(foundCopyBookId, foundCopyId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundCopy;
    }

    public void Update(int newBookId)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();


      SqlCommand cmd = new SqlCommand("UPDATE copies SET book_id = @NewBookId OUTPUT INSERTED.book_id WHERE id = @CopyId;", conn);

      SqlParameter newBookIdParameter = new SqlParameter();
      newBookIdParameter.ParameterName = "@NewBookId";
      newBookIdParameter.Value = newBookId;

      SqlParameter copyIdParameter = new SqlParameter();
      copyIdParameter.ParameterName = "@CopyId";
      copyIdParameter.Value = this.GetId();

      cmd.Parameters.Add(newBookIdParameter);
      cmd.Parameters.Add(copyIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        this._book_id = rdr.GetInt32(0);
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

    public void AddPatron(Patron newPatron)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO checkouts (patron_id, copy_id) VALUES (@PatronId, @CopyId);", conn);

      SqlParameter patronIdParameter = new SqlParameter();
      patronIdParameter.ParameterName = "@PatronId";
      patronIdParameter.Value = newPatron.GetId();
      cmd.Parameters.Add(patronIdParameter);

      SqlParameter copyIdParameter = new SqlParameter();
      copyIdParameter.ParameterName = "@CopyId";
      copyIdParameter.Value = this.GetId();
      cmd.Parameters.Add(copyIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public List<Patron> GetPatron()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT patrons.* FROM patrons JOIN checkouts ON (patrons.id = checkouts.patron_id) JOIN copies ON (checkouts.copy_id = copies.id) WHERE copies.id = @CopyId",conn);

      SqlParameter copyIdParameter = new SqlParameter();
      copyIdParameter.ParameterName= "@CopyId";
      copyIdParameter.Value=this.GetId();
      cmd.Parameters.Add(copyIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();
      List<Patron> patrons = new List<Patron> {};

      while(rdr.Read())
      {
        int patronId = rdr.GetInt32(0);
        string patronName = rdr.GetString(1);
        Patron newPatron = new Patron(patronName, patronId);
        patrons.Add(newPatron);
      }

      if(rdr !=null)
      {
        rdr.Close();
      }

      if (conn != null)
      {
        conn.Close();
      }
      return patrons;
    }



    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM copies WHERE id = @CopyId;", conn);

      SqlParameter copyIdParameter = new SqlParameter();
      copyIdParameter.ParameterName = "@CopyId";
      copyIdParameter.Value = this.GetId();

      cmd.Parameters.Add(copyIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM copies;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
