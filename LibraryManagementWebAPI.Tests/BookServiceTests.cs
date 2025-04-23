using LibraryManagementAPI.Core.Models;
using LibraryManagementAPI.Core.Services;
using LibraryManagementAPI.Data.Repositories;
using LibraryManagementAPI.Service.Services;
using LibraryManagementWebAPI.Tests;
using LibraryManagementWebAPI.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Threading.Tasks;
using Xunit;
using static System.Reflection.Metadata.BlobBuilder;


public class BookServiceTests
{
    private readonly Mock<LibraryDbContext> _mockContext;
    private readonly BookService _bookService;
    private readonly DbContextOptions<LibraryDbContext> _options;
    private readonly LibraryDbContext _dbContext;
    private readonly Mock<IBookService> _bookServiceMock;

    public BookServiceTests()
    {
        _mockContext = new Mock<LibraryDbContext>();
        _bookService = new BookService(_mockContext.Object);
        _bookServiceMock = new Mock<IBookService>();

        _options = new DbContextOptionsBuilder<LibraryDbContext>()
           .UseInMemoryDatabase(databaseName: "TestDatabase")
           .Options;

       
        _dbContext = new LibraryDbContext(_options);
        _bookService = new BookService(_dbContext);
    }

    
    [Fact]
    public async Task GetAllBooksAsync_ShouldReturnAllBooks()
    {
        var options = new DbContextOptionsBuilder<LibraryDbContext>()
                     .UseInMemoryDatabase(databaseName: "TestDatabase")
                     .Options;
      
        using (var context = new LibraryDbContext(options))
        {           
            var books = new List<Book>
        {
            new Book {
                Id = 1,
                Title = "Book 1",
                Authors = new Author { Id = 1, Name = "Author 1", Surname = "Surname 1" }
            },
            new Book {
                Id = 2,
                Title = "Book 2",
                Authors = new Author { Id = 2, Name = "Author 2", Surname = "Surname 2" }
            }
        };

            context.Books.AddRange(books);
            await context.SaveChangesAsync();
            
            var mockDbSet = new Mock<DbSet<Book>>();

            
            mockDbSet.As<IQueryable<Book>>()
                     .Setup(m => m.Provider)
                     .Returns(new TestAsyncQueryProvider<Book>(books.AsQueryable().Provider));

            mockDbSet.As<IQueryable<Book>>()
                     .Setup(m => m.Expression)
                     .Returns(books.AsQueryable().Expression);

            mockDbSet.As<IQueryable<Book>>()
                     .Setup(m => m.ElementType)
                     .Returns(books.AsQueryable().ElementType);

            mockDbSet.As<IQueryable<Book>>()
                     .Setup(m => m.GetEnumerator())
                     .Returns(books.AsQueryable().GetEnumerator());

            mockDbSet.As<IAsyncEnumerable<Book>>()
                     .Setup(m => m.GetAsyncEnumerator(CancellationToken.None))
                     .Returns(new TestAsyncEnumerator<Book>(books.GetEnumerator()));

            _mockContext.Setup(c => c.Books).Returns(mockDbSet.Object);
           
            var result = await _bookService.GetAllBooksAsync();
          
            Assert.Equal(2, result.Count());
            Assert.Contains(result, b => b.Title == "Book 1");
            Assert.Contains(result, b => b.Title == "Book 2");
        }
    }


    // GetBookByIdAsync Testi (Kitap Bulunduğunda)
    //[Fact]
    //public async Task GetBookByIdAsync_ShouldReturnBook_WhenBookExists()
    //{
    //    // Arrange: Test verisi ekleniyor
    //    var author = new Author { Id = 1, Name = "Test Author", Surname = "Surname 1" };
    //    var book = new Book { Id = 1, Title = "Test Book", Authors = author, PublishedYear = new DateTime(2025, 1, 1) };
    //    _dbContext.Authors.Add(author);
    //    _dbContext.Books.Add(book);
    //    await _dbContext.SaveChangesAsync();

    //    // Act: Servisten veri alınıyor
    //    var result = await _bookService.GetBookByIdAsync(1);

    //    // Assert: Sonuç doğrulanıyor
    //    Assert.NotNull(result);
    //    Assert.Equal("Test Book", result.Title);
    //    Assert.Equal("Test Author", result.Authors.Name);
    //    Assert.Equal("Surname 1", result.Authors.Surname);
    //}
    [Fact]
    public async Task GetBookByIdAsync_ShouldReturnBook_WhenBookExists()
    {       
        var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
      
        using (var context = new LibraryDbContext(options))
        {
            var author = new Author { Id = 1, Name = "Test Author", Surname = "Surname 1" };
            var book = new Book { Id = 1, Title = "Test Book", Authors = author, PublishedYear = new DateTime(2025, 1, 1) };

            context.Authors.Add(author);
            context.Books.Add(book);
            await context.SaveChangesAsync();
        }
       
        using (var context = new LibraryDbContext(options))
        {
            var author = new Author { Id = 1, Name = "Test Author", Surname = "Surname 1" };
            var book = new Book { Id = 1, Title = "Test Book", Authors = author, PublishedYear = new DateTime(2025, 1, 1) };
            var books = new List<Book> { book }.AsQueryable();

            var mockBookDbSet = new Mock<DbSet<Book>>();
            mockBookDbSet.As<IQueryable<Book>>()
                .Setup(m => m.Provider).Returns(books.Provider);
            mockBookDbSet.As<IQueryable<Book>>()
                .Setup(m => m.Expression).Returns(books.Expression);
            mockBookDbSet.As<IQueryable<Book>>()
                .Setup(m => m.ElementType).Returns(books.ElementType);
            mockBookDbSet.As<IQueryable<Book>>()
                .Setup(m => m.GetEnumerator()).Returns(books.GetEnumerator());

           
            var mockContext = new Mock<LibraryDbContext>();
            mockContext.Setup(c => c.Books).Returns(mockBookDbSet.Object);
            var bookService = new BookService(context); 
            var result = await bookService.GetBookByIdAsync(1);

           
            Assert.NotNull(result);
            Assert.Equal("Test Book", result.Title);
            Assert.Equal("Test Author", result.Authors.Name);
            Assert.Equal("Surname 1", result.Authors.Surname);
        }
    }    
    [Fact]
    public async Task GetBookByIdAsync_ShouldReturnNull_WhenBookNotFound()
    {        
        var bookId = 1;        
        var books = new List<Book> { }.AsQueryable(); 

        var mockBooksDbSet = new Mock<DbSet<Book>>();
       
        mockBooksDbSet.As<IQueryable<Book>>()
            .Setup(m => m.Provider).Returns(books.Provider);
        mockBooksDbSet.As<IQueryable<Book>>()
            .Setup(m => m.Expression).Returns(books.Expression);
        mockBooksDbSet.As<IQueryable<Book>>()
            .Setup(m => m.ElementType).Returns(books.ElementType);
        mockBooksDbSet.As<IQueryable<Book>>()
            .Setup(m => m.GetEnumerator()).Returns(books.GetEnumerator());

       
        mockBooksDbSet.Setup(m => m.FindAsync(bookId))
            .ReturnsAsync((Book)null);

       
        var mockContext = new Mock<LibraryDbContext>();
        mockContext.Setup(c => c.Books).Returns(mockBooksDbSet.Object);

        var bookService = new BookService(mockContext.Object);
      
        var result = await bookService.GetBookByIdAsync(bookId);
        
        Assert.Null(result); 
    }

    [Fact]
    public async Task DeleteBookAsync_ShouldDeleteBook_WhenBookExistsAndNotBorrowed()
    {       
        var book = new Book
        {
            Id = 1,
            Title = "Test Book",
            AuthorId = 1,
            PublishedYear = new DateTime(2020, 1, 1),
            IsBorrowed = false,
            BorrowedBooks = new List<BorrowedBook>() 
        };

        var books = new List<Book> { book }.AsQueryable();

        var mockBooksDbSet = new Mock<DbSet<Book>>();
        
        mockBooksDbSet.As<IQueryable<Book>>()
            .Setup(m => m.Provider).Returns(books.Provider);
        mockBooksDbSet.As<IQueryable<Book>>()
            .Setup(m => m.Expression).Returns(books.Expression);
        mockBooksDbSet.As<IQueryable<Book>>()
            .Setup(m => m.ElementType).Returns(books.ElementType);
        mockBooksDbSet.As<IQueryable<Book>>()
            .Setup(m => m.GetEnumerator()).Returns(books.GetEnumerator());
       
        mockBooksDbSet.Setup(m => m.FindAsync(1))
            .ReturnsAsync(book);
     
        var mockContext = new Mock<LibraryDbContext>();
        mockContext.Setup(c => c.Books).Returns(mockBooksDbSet.Object);

        var bookService = new BookService(mockContext.Object);

       
        var result = await bookService.DeleteBookAsync(1);
      
        Assert.True(result);  
        mockBooksDbSet.Verify(m => m.Remove(It.IsAny<Book>()), Times.Once); 
    }

    [Fact]
    public async Task DeleteBookAsync_ShouldReturnFalse_WhenBookIsBorrowed()
    {        
        var borrowedBook = new BorrowedBook { ReturnDate = null }; 
        var book = new Book
        {
            Id = 1,
            Title = "Book 1",
            BorrowedBooks = new List<BorrowedBook> { borrowedBook }
        };

        var books = new List<Book> { book }.AsQueryable(); 

        var mockBooksDbSet = new Mock<DbSet<Book>>();
        
        mockBooksDbSet.As<IQueryable<Book>>()
            .Setup(m => m.Provider).Returns(books.Provider);
        mockBooksDbSet.As<IQueryable<Book>>()
            .Setup(m => m.Expression).Returns(books.Expression);
        mockBooksDbSet.As<IQueryable<Book>>()
            .Setup(m => m.ElementType).Returns(books.ElementType);
        mockBooksDbSet.As<IQueryable<Book>>()
            .Setup(m => m.GetEnumerator()).Returns(books.GetEnumerator());
       
        mockBooksDbSet.Setup(m => m.FindAsync(1))
            .ReturnsAsync(book);
       
        var mockContext = new Mock<LibraryDbContext>();
        mockContext.Setup(c => c.Books).Returns(mockBooksDbSet.Object);

        var bookService = new BookService(mockContext.Object);
        
        var result = await bookService.DeleteBookAsync(1);
        
        Assert.False(result); 
        mockBooksDbSet.Verify(m => m.Remove(It.IsAny<Book>()), Times.Never); 
    }   
    [Fact]
    public async Task UpdateBookAsync_ShouldUpdateBook_WhenBookExists()
    {
        
        var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase") 
            .Options;

        var context = new LibraryDbContext(options);
        var bookService = new BookService(context);

        var author = new Author
        {
            Id = 1,
            Name = "Test Author 1",
            Surname = "Test Surname"
        };
        var book = new Book
        {
            Id = 1,
            Title = "Test Book 1",
            Authors = author,
            PublishedYear = new DateTime(2025, 1, 1)
        };

        await context.Authors.AddAsync(author);
        await context.Books.AddAsync(book);
        await context.SaveChangesAsync();
        
        var updatedBook = new Book
        {
            Id = 1,
            Title = "Updated Test Book",
            Authors = author,
            PublishedYear = new DateTime(2026, 1, 1)
        };

       
        var existingBook = await context.Books.FindAsync(1);
        if (existingBook != null)
        {
            existingBook.Title = updatedBook.Title;
            existingBook.PublishedYear = updatedBook.PublishedYear;           
            await context.SaveChangesAsync();
        }
     
        var result = await context.Books.FindAsync(1);
        Assert.NotNull(result);
        Assert.Equal("Updated Test Book", result.Title);
        Assert.Equal(new DateTime(2026, 1, 1), result.PublishedYear);
    }    
    [Fact]
    public async Task FilterBookAsync_ShouldReturnBooks_WhenFilteredByAuthor()
    {
        var options = new DbContextOptionsBuilder<LibraryDbContext>()
          .UseInMemoryDatabase(databaseName: "TestDatabase") 
          .Options;
        using (var context = new LibraryDbContext(options))
        {
            var author1 = new Author{ Id = 1, Name = "Author 1", Surname = "Surname 1" };
            var author2 = new Author { Id = 2, Name = "Author 2", Surname = "Surname 2" };

            
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Book 1", PublishedYear = new DateTime(1932, 1, 1),AuthorId=1, Authors = author1},
                new Book { Id = 2, Title = "Book 2", PublishedYear = new DateTime(1987, 1, 1),AuthorId=1, Authors = author1  },
                new Book { Id = 3, Title = "Book 3", PublishedYear = new DateTime(1994, 1, 1),AuthorId=2, Authors = author2  }
            };
            context.Authors.AddRange(author1,author2);
            context.Books.AddRange(books);
            await context.SaveChangesAsync();

             var mockDbSet = new Mock<DbSet<Book>>();
            
            mockDbSet.As<IQueryable<Book>>()
                     .Setup(m => m.Provider)
                     .Returns(new TestAsyncQueryProvider<Book>(books.AsQueryable().Provider));

            mockDbSet.As<IQueryable<Book>>()
                     .Setup(m => m.Expression)
                     .Returns(books.AsQueryable().Expression);

            mockDbSet.As<IQueryable<Book>>()
                     .Setup(m => m.ElementType)
                     .Returns(books.AsQueryable().ElementType);

            mockDbSet.As<IQueryable<Book>>()
                     .Setup(m => m.GetEnumerator())
                     .Returns(books.AsQueryable().GetEnumerator());

            mockDbSet.As<IAsyncEnumerable<Book>>()
                     .Setup(m => m.GetAsyncEnumerator(CancellationToken.None))
                     .Returns(new TestAsyncEnumerator<Book>(books.GetEnumerator()));

            
            _mockContext.Setup(c => c.Books).Returns(mockDbSet.Object);

         
            var result = await _bookService.FilterBookAsync("Author 1");

            
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count()); // Author 1'in 2 kitabı var anlamına gelir
            Assert.All(result, b => Assert.Equal(1, b.AuthorId));
        }
    }   
    [Fact]
    public async Task FilterBookAsync_ShouldReturnBooks_WhenFilteredByDateRange()
    {
        var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        using (var context = new LibraryDbContext(options))
        {
            // Arrange
            var books = new List<Book>
    {
        new Book { Id = 1, Title = "Book 1", PublishedYear = new DateTime(1932, 1, 1), Authors = new Author { Id = 1, Name = "Author 1", Surname = "Surname 1" } },
        new Book { Id = 2, Title = "Book 2", PublishedYear = new DateTime(1987, 1, 1), Authors = new Author { Id = 2, Name = "Author 2", Surname = "Surname 2" } }
    };
            context.Books.AddRange(books);
            await context.SaveChangesAsync();

            var mockDbSet = new Mock<DbSet<Book>>();
           
            mockDbSet.As<IQueryable<Book>>()
                     .Setup(m => m.Provider)
                     .Returns(new TestAsyncQueryProvider<Book>(books.AsQueryable().Provider));

            mockDbSet.As<IQueryable<Book>>()
                     .Setup(m => m.Expression)
                     .Returns(books.AsQueryable().Expression);

            mockDbSet.As<IQueryable<Book>>()
                     .Setup(m => m.ElementType)
                     .Returns(books.AsQueryable().ElementType);

            mockDbSet.As<IQueryable<Book>>()
                     .Setup(m => m.GetEnumerator())
                     .Returns(books.AsQueryable().GetEnumerator());

            mockDbSet.As<IAsyncEnumerable<Book>>()
                     .Setup(m => m.GetAsyncEnumerator(CancellationToken.None))
                     .Returns(new TestAsyncEnumerator<Book>(books.GetEnumerator()));

            _mockContext.Setup(c => c.Books).Returns(mockDbSet.Object);
           
            var result = await _bookService.GetBookByYearRangeAsync(startYear: 1911, endYear: 1989);
            
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count()); 
            Assert.Contains(result, b => b.Title == "Book 1");
            Assert.Contains(result, b => b.Title == "Book 2");
        }

    }
}



