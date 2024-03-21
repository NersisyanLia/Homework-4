using System;
using System.Collections.Generic;

class Author
{
    public string Name { get; set; }
    public string Biography { get; set; }
}

class User
{
    public string Name { get; set; }
    public string Email { get; set; }
}

class Book
{
    public string Title { get; set; }
    public Author Author { get; set; }
    public string Category { get; set; }
    public bool IsAvailable { get; set; } = true;
    public double? Rating { get; set; } 

    public void RentBook()
    {
        if (IsAvailable)
        {
            IsAvailable = false;
            Console.WriteLine($"Book '{Title}' rented successfully.");
        }
        else
        {
            Console.WriteLine($"Book '{Title}' is not available for rent.");
        }
    }

    public void ReturnBook(double? rating = null)
    {
        IsAvailable = true;
        if (rating != null)
        {
            Rating = rating;
            Console.WriteLine($"Book '{Title}' returned successfully. Thank you for rating.");
        }
        else
        {
            Console.WriteLine($"Book '{Title}' returned successfully.");
        }
    }
}

class Library
{
    private List<Book> books = new List<Book>();
    private List<User> users = new List<User>();
    private double balance = 0;

    public void AddBook(Book book) => books.Add(book);

    public void RemoveBook(string title)
    {
        Book bookToRemove = books.Find(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (bookToRemove != null)
        {
            books.Remove(bookToRemove);
            Console.WriteLine($"Removed: {title}");
        }
        else
        {
            Console.WriteLine($"Not found: {title}");
        }
    }

    public void ListAllBooks()
    {
        Console.WriteLine("Books in the Library:");
        foreach (var book in books)
        {
            Console.WriteLine($"Title: {book.Title}, Author: {book.Author.Name}, Category: {book.Category}, Available: {book.IsAvailable}");
        }
    }

    public void RentBook(string title, string userEmail)
    {
        Book bookToRent = books.Find(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        User user = users.Find(u => u.Email.Equals(userEmail, StringComparison.OrdinalIgnoreCase));
        if (bookToRent != null && user != null)
        {
            bookToRent.RentBook();
            Console.WriteLine($"Book '{title}' rented by {user.Name}");
        }
        else
        {
            Console.WriteLine("Book or user not found.");
        }
    }

    public void ReturnBook(string title, double? rating = null)
    {
        Book bookToReturn = books.Find(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (bookToReturn != null)
        {
            bookToReturn.ReturnBook(rating);
            if (rating != null)
            {
                UpdateLibraryBalance((double)rating);
            }
        }
        else
        {
            Console.WriteLine("Book not found.");
        }
    }

    public void AddUser(User user) => users.Add(user);

    public void RemoveUser(string userEmail)
    {
        User userToRemove = users.Find(u => u.Email.Equals(userEmail, StringComparison.OrdinalIgnoreCase));
        if (userToRemove != null)
        {
            users.Remove(userToRemove);
            Console.WriteLine($"Removed user: {userEmail}");
        }
        else
        {
            Console.WriteLine($"User not found: {userEmail}");
        }
    }

    public void UpdateLibraryBalance(double amount)
    {
        balance += amount;
        Console.WriteLine($"Library balance updated. Current balance: {balance}");
    }
}

class Program
{
    static void Main()
    {
        Library library = new Library();

        while (true)
        {
            Console.WriteLine("Menu\n1. Add Book\n2. Remove Book\n3. List All Books\n4. Rent Book\n5. Return Book\n6. Add User\n7. Remove User\n8. Exit");
            Console.Write("Enter choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    {
                        Book newBook = new Book
                        {
                            Title = GetUserInput("Title"),
                            Author = new Author { Name = GetUserInput("Author Name") }, 
                            Category = GetUserInput("Category")
                        };
                        library.AddBook(newBook);
                        break;
                    }
                case "2":
                    {
                        string titleToRemove = GetUserInput("Enter title to remove");
                        library.RemoveBook(titleToRemove);
                        break;
                    }
                case "3":
                    {
                        library.ListAllBooks();
                        break;
                    }
                case "4":
                    {
                        string titleToRent = GetUserInput("Enter title to rent");
                        string userEmail = GetUserInput("Enter user email");
                        library.RentBook(titleToRent, userEmail);
                        break;
                    }
                case "5":
                    {
                        string titleToReturn = GetUserInput("Enter title to return");
                        double? rating = null;
                        string ratingInput = GetUserInput("Enter rating (optional, press Enter to skip)");
                        if (!string.IsNullOrEmpty(ratingInput))
                        {
                            if (double.TryParse(ratingInput, out double userRating))
                            {
                                rating = userRating;
                            }
                            else
                            {
                                Console.WriteLine("Invalid rating. Skipping...");
                            }
                        }
                        library.ReturnBook(titleToReturn, rating);
                        break;
                    }
                case "6":
                    {
                        User newUser = new User
                        {
                            Name = GetUserInput("Enter user name"),
                            Email = GetUserInput("Enter user email")
                        };
                        library.AddUser(newUser);
                        break;
                    }
                case "7":
                    {
                        string userEmailToRemove = GetUserInput("Enter user email to remove");
                        library.RemoveUser(userEmailToRemove);
                        break;
                    }
                case "8":
                    {
                        Environment.Exit(0);
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                    }
            }
        }
    }

    static string GetUserInput(string prompt)
    {
        Console.Write($"{prompt}: ");
        return Console.ReadLine();
    }
}
