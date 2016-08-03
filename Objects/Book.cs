using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Library
{
  public class Book
  {
    private int _id;
    private string _title;

    public Book (string Title, int Id = 0)
    {
      _id = Id;
      _title = Title;
    }

    public override bool Equals(System.Object otherBook)
    {
      if (!(otherBook is Book))
      {
        return false;
      }
      else
      {
        Book newBook = (Book) otherBook;
        bool idEquality = (this.GetId() == newBook.GetId());
        bool titleEquality = (this.GetTitle() == newBook.GetTitle());
        return (idEquality && titleEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }
    public string GetTitle()
    {
      return _title;
    }
    public void SetTitle(string newTitle)
    {
      _title = newTitle;
    }
    public void Save()
    {
      SqlConnection connection = DB.Connection();
      connection.Open();

      SqlCommand command = new SqlCommand("INSERT INTO books (title) OUTPUT INSERTED.id VALUES (@BookTitle);", connection);

      SqlParameter titleParameter = new SqlParameter();
      titleParameter.ParameterName = "@BookTitle";
      titleParameter.Value = this.GetTitle();
      command.Parameters.Add(titleParameter);

      SqlDataReader reader = command.ExecuteReader();

      while(reader.Read())
      {
        this._id = reader.GetInt32(0);
      }

      if (reader != null)
      {
        reader.Close();
      }
      if (connection != null)
      {
        connection.Close();
      }
    }

    public static List<Book> GetAll()
    {
      List<Book> allBooks = new List<Book>();

      SqlConnection connection = DB.Connection();
      connection.Open();

      SqlCommand command = new SqlCommand("SELECT * FROM books;", connection);
      SqlDataReader reader = command.ExecuteReader();

      while (reader.Read())
      {
        int id = reader.GetInt32(0);
        string title = reader.GetString(1);
        Book book = new Book(title, id);
        allBooks.Add(book);
      }

      if (reader != null)
      {
        reader.Close();
      }
      if (connection != null)
      {
        connection.Close();
      }
      return allBooks;
    }

    public static Book Find(int id)
    {
      SqlConnection connection = DB.Connection();
      connection.Open();

      SqlCommand command = new SqlCommand("SELECT * FROM books WHERE id = @BookId;", connection);
      SqlParameter idParameter = new SqlParameter();
      idParameter.ParameterName = "@BookId";
      idParameter.Value = id.ToString();
      command.Parameters.Add(idParameter);

      SqlDataReader reader = command.ExecuteReader();

      int foundId = 0;
      string foundTitle = null;

      while(reader.Read())
      {
        foundId = reader.GetInt32(0);
        foundTitle = reader.GetString(1);
      }
      Book foundBook = new Book(foundTitle, foundId);

      if (reader != null)
      {
        reader.Close();
      }
      if (connection != null)
      {
        connection.Close();
      }
      return foundBook;
    }
    public void Update(string newTitle)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();


      SqlCommand cmd = new SqlCommand("UPDATE books SET title = @NewTitle OUTPUT INSERTED.title WHERE id = @BookId;", conn);

      SqlParameter newTitleParameter = new SqlParameter();
      newTitleParameter.ParameterName = "@NewTitle";
      newTitleParameter.Value = newTitle;

      SqlParameter bookIdParameter = new SqlParameter();
      bookIdParameter.ParameterName = "@BookId";
      bookIdParameter.Value = this.GetId();

      cmd.Parameters.Add(newTitleParameter);
      cmd.Parameters.Add(bookIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        this._title = rdr.GetString(0);
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

    public static List<Book> SearchTitle(string searchInput)
    {
      SqlConnection connection = DB.Connection();
      connection.Open();

      SqlCommand command = new SqlCommand("SELECT * FROM books WHERE title = @SearchInput;", connection);
      SqlParameter searchParameter = new SqlParameter();
      searchParameter.ParameterName = "@SearchInput";
      searchParameter.Value = searchInput;
      command.Parameters.Add(searchParameter);

      SqlDataReader reader = command.ExecuteReader();

      int foundId = 0;
      string foundTitle = null;
      List<Book> foundBooks = new List<Book> {};

      while(reader.Read())
      {
        foundId = reader.GetInt32(0);
        foundTitle = reader.GetString(1);
        Book foundBook = new Book(foundTitle, foundId);
        foundBooks.Add(foundBook);
      }

      if (reader != null)
      {
        reader.Close();
      }
      if (connection != null)
      {
        connection.Close();
      }
      return foundBooks;
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM books WHERE id = @BookId;", conn);

      SqlParameter bookIdParameter = new SqlParameter();
      bookIdParameter.ParameterName = "@BookId";
      bookIdParameter.Value = this.GetId();

      cmd.Parameters.Add(bookIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection connection = DB.Connection();
      connection.Open();
      SqlCommand command = new SqlCommand("DELETE FROM books;", connection);
      command.ExecuteNonQuery();
      connection.Close();
    }
  }
}
