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
