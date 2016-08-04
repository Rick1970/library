using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;
using System;

namespace Library
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        List<Book> booksByTitle = Book.SearchByTitle("VOID-SEARCH-TO-PREVENT-DATA");
        List<Book> booksByAuthor = Book.SearchByAuthor("VOID-SEARCH-TO-PREVENT-DATA");
        model.Add("booksByTitle", booksByTitle);
        model.Add("booksByAuthor", booksByAuthor);
        return View["index.cshtml", model];
      };

      Get["/librarian"] = _ => {
        return View["librarian.cshtml"];
      };

      Post["/success"] = _ => {
        Book newBook = new Book(Request.Form["title-input"]);
        newBook.Save();

        Author newAuthor = new Author(Request.Form["author-input"]);
        newAuthor.Save();

        newBook.AddAuthor(newAuthor);

        Dictionary<string, object> model = new Dictionary<string, object>();
        model.Add("addedBook", newBook);
        model.Add("addedAuthor", newAuthor);

        return View["success.cshtml", model];
      };

      Post["/results"] = _ => {
        string input = Request.Form["search-input"];
        Dictionary<string, object> model = new Dictionary<string, object>();
        List<Book> booksByTitle = Book.SearchByTitle(input.ToString());
        List<Book> booksByAuthor = Book.SearchByAuthor(input.ToString());
        model.Add("booksByTitle", booksByTitle);
        model.Add("booksByAuthor", booksByAuthor);
        return View["index.cshtml", model];
      };
    }
  }
}
