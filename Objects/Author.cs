using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Library
{
  public class Author
  {
    private int _id;
    private string _name;

    public Author (string Name, int Id = 0)
    {
      _id = Id;
      _name = Name;
    }

    public override bool Equals(System.Object otherAuthor)
    {
      if (!(otherAuthor is Author))
      {
        return false;
      }
      else
      {
        Author newAuthor = (Author) otherAuthor;
        bool idEquality = this.GetId() == newAuthor.GetId();
        bool NameEquality = this.GetName() == newAuthor.GetName();

        return (idEquality && NameEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }

    public static List<Author> GetAll()
    {
      List<Author> allAuthors = new List<Author>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM authors;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        int authorId = rdr.GetInt32(0);
        string authorName = rdr.GetString(1);

        Author newAuthor = new Author(authorName, authorId);
        allAuthors.Add(newAuthor);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allAuthors;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO authors (name) OUTPUT INSERTED.id VALUES (@AuthorName);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@AuthorName";
      nameParameter.Value = this.GetName();

      cmd.Parameters.Add(nameParameter);

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
    public static Author Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM authors WHERE id = @AuthorId;", conn);
      SqlParameter authorIdParameter = new SqlParameter();
      authorIdParameter.ParameterName = "@AuthorId";
      authorIdParameter.Value = id.ToString();

      cmd.Parameters.Add(authorIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      int foundAuthorId = 0;
      string foundAuthorName = null;

      while (rdr.Read())
      {
        foundAuthorId = rdr.GetInt32(0);
        foundAuthorName = rdr.GetString(1);
      }
      Author foundAuthor = new Author(foundAuthorName, foundAuthorId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundAuthor;
    }

    // public void Update(string newFirst, string newLast, int newStylistId)
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   //Discovered how to output all (*) will allow you to select which column by index in Reader below
    //   SqlCommand cmd = new SqlCommand("UPDATE clients SET first_name = @NewFirst, last_name = @NewLast, stylist_id = @NewStylistId OUTPUT INSERTED.* WHERE id = @ClientId;", conn);
    //
    //   SqlParameter newFirstParameter = new SqlParameter();
    //   newFirstParameter.ParameterName = "@NewFirst";
    //   newFirstParameter.Value = newFirst;
    //
    //   SqlParameter newLastParameter = new SqlParameter();
    //   newLastParameter.ParameterName = "@NewLast";
    //   newLastParameter.Value = newLast;
    //
    //   SqlParameter newStylistIdParameter = new SqlParameter();
    //   newStylistIdParameter.ParameterName = "@NewStylistId";
    //   newStylistIdParameter.Value = newStylistId;
    //
    //   SqlParameter clientIdParameter = new SqlParameter();
    //   clientIdParameter.ParameterName = "@ClientId";
    //   clientIdParameter.Value = this.GetId();
    //
    //   cmd.Parameters.Add(newFirstParameter);
    //   cmd.Parameters.Add(newLastParameter);
    //   cmd.Parameters.Add(newStylistIdParameter);
    //   cmd.Parameters.Add(clientIdParameter);
    //
    //   SqlDataReader rdr = cmd.ExecuteReader();
    //
    //   while (rdr.Read())
    //   {
    //     this._name = rdr.GetString(1);
    //     this._lastName = rdr.GetString(2);
    //     this._stylistId = rdr.GetInt32(3);
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
    // public void Delete()
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("DELETE FROM clients WHERE id = @ClientId;", conn);
    //
    //   SqlParameter clientIdParameter = new SqlParameter();
    //   clientIdParameter.ParameterName = "@ClientId";
    //   clientIdParameter.Value = this.GetId();
    //
    //   cmd.Parameters.Add(clientIdParameter);
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
      SqlCommand cmd = new SqlCommand("DELETE FROM authors;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
