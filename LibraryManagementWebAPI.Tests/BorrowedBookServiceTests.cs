using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using LibraryManagementAPI.Service;
using LibraryManagementAPI.Data;
using LibraryManagementAPI.Core.Models;
using Microsoft.EntityFrameworkCore;
using LibraryManagementAPI.Data.Repositories;
using LibraryManagementAPI.Service.Services;
using LibraryManagementAPI.Core.Services;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Collections;
using Moq.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using LibraryManagementWebAPI.Tests.Helpers;
using FluentAssertions;
using LibraryManagementWebAPI.Tests;
using Microsoft.EntityFrameworkCore.ChangeTracking;

public class BorrowedBookServiceTests
{
    private readonly LibraryDbContext _context;
    private readonly Mock<LibraryDbContext> _mockDbContext;
    private readonly BorrowedBookService _borrowedBookService;
    private readonly Mock<IBookService> _mockBookService;
    private readonly Mock<ILogger<BorrowedBookService>> _mockLogger;
    private readonly Mock<DbSet<Member>> _mockMemberDbSet;
    private readonly Mock<DbSet<Book>> _mockBookDbSet;
    private readonly Mock<DbSet<BorrowedBook>> _mockBorrowedBookDbSet;
    public BorrowedBookServiceTests()
    {   // Mock nesnelerini başlat
        // In-Memory Database kullanımı
        var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .EnableSensitiveDataLogging()// Test için geçici bir veritabanı oluşturma komutu
            .Options;

        _context = new LibraryDbContext(options);

        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

       
        _context.Members.Add(new Member
        {
            Id = 7332,
            Name = "Member1",
            Email = "member1@example.com",
            Phone = "123-456-7890",
            Surname = "Surname1"
        });
        _context.Books.Add(new Book { Id = 1, Title = "Book1" });
        _context.SaveChanges();

        //Mock objelerinin başlatılması
        _mockLogger = new Mock<ILogger<BorrowedBookService>>();
        _mockBookService = new Mock<IBookService>();
        _mockDbContext = new Mock<LibraryDbContext>();

       
        _borrowedBookService = new BorrowedBookService(
            _mockLogger.Object,
            _context,
            _mockBookService.Object
        //_mockDbContext.Object
        );
    }

    [Fact]
    public async Task BorrowedBooks_ShouldFail_WhenBookIsAlreadyBorrowed()
    {
        var book = new Book { Id = 1, IsBorrowed = true }; // Kitap zaten ödünç alınmış
        var borrowedBooks = new List<BorrowedBook>
        {
            new BorrowedBook { BookId = 1, MemberId = 1, BorrowDate = DateTime.Now }
        }.AsQueryable();

        var mockDbSet = new Mock<DbSet<BorrowedBook>>();
        mockDbSet.As<IQueryable<BorrowedBook>>().Setup(m => m.Provider).Returns(borrowedBooks.Provider);
        mockDbSet.As<IQueryable<BorrowedBook>>().Setup(m => m.Expression).Returns(borrowedBooks.Expression);
        mockDbSet.As<IQueryable<BorrowedBook>>().Setup(m => m.ElementType).Returns(borrowedBooks.ElementType);
        mockDbSet.As<IQueryable<BorrowedBook>>().Setup(m => m.GetEnumerator()).Returns(borrowedBooks.GetEnumerator());
        mockDbSet.As<IAsyncEnumerable<BorrowedBook>>().Setup(x => x.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<BorrowedBook>(borrowedBooks.GetEnumerator()));

        mockDbSet.As<IQueryable<BorrowedBook>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<BorrowedBook>(borrowedBooks.Provider));

        _mockDbContext.Setup(m => m.BorrowedBooks).Returns(mockDbSet.Object);
        _mockDbContext.Setup(m => m.Books.FindAsync(1)).ReturnsAsync(book);

        var result = await _borrowedBookService.UpdateBorrowedBookAsync(borrowedBooks.First().BookId, borrowedBooks.First());

        Assert.False(result);
    }
    [Fact]
    //public async Task AddBorrowedBookAsync_ShouldThrowException_WhenMemberIsNotFound()
    //{

    //    var options = new DbContextOptionsBuilder<LibraryDbContext>()
    //        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) //Yeni GUID ile her test için yeni DB ismi oluşturma
    //        .Options;

    //    using (var context = new LibraryDbContext(options))
    //    {
    //        var mockContext = new Mock<LibraryDbContext>(options);

    //        var nonExistentMemberId = 999; 
    //        var nonExistentBookId = 888;
    //        var borrowedBook = new BorrowedBook { MemberId = nonExistentMemberId, BookId = nonExistentBookId };

    //        var mockMembers = new Mock<DbSet<Member>>();
    //        var mockBooks = new Mock<DbSet<Book>>();

    //        mockContext.Setup(c => c.Members).Returns(mockMembers.Object);
    //        mockContext.Setup(c => c.Books).Returns(mockBooks.Object);

    //        var exception = await Assert.ThrowsAsync<System.InvalidOperationException>(() =>
    //            _borrowedBookService.AddBorrowedBookAsync(borrowedBook));

    //        Assert.Equal("Üye bulunamadı.", exception.Message);
    //    }
    //}
    public async Task AddBorrowedBookAsync_ShouldThrowException_WhenMemberIsNotFound()
    {       
        var mockContext = new Mock<LibraryDbContext>();
              
        var members = new List<Member>().AsQueryable();
        var books = new List<Book>().AsQueryable();
       
        var mockMembers = new Mock<DbSet<Member>>();
        mockMembers.As<IQueryable<Member>>().Setup(m => m.Provider).Returns(members.Provider);
        mockMembers.As<IQueryable<Member>>().Setup(m => m.Expression).Returns(members.Expression);
        mockMembers.As<IQueryable<Member>>().Setup(m => m.ElementType).Returns(members.ElementType);
        mockMembers.As<IQueryable<Member>>().Setup(m => m.GetEnumerator()).Returns(members.GetEnumerator());

        var mockBooks = new Mock<DbSet<Book>>();
        mockBooks.As<IQueryable<Book>>().Setup(m => m.Provider).Returns(books.Provider);
        mockBooks.As<IQueryable<Book>>().Setup(m => m.Expression).Returns(books.Expression);
        mockBooks.As<IQueryable<Book>>().Setup(m => m.ElementType).Returns(books.ElementType);
        mockBooks.As<IQueryable<Book>>().Setup(m => m.GetEnumerator()).Returns(books.GetEnumerator());

        mockContext.Setup(c => c.Members).Returns(mockMembers.Object);
        mockContext.Setup(c => c.Books).Returns(mockBooks.Object);
       
        var borrowedBook = new BorrowedBook { MemberId = 999, BookId = 888 };
       
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _borrowedBookService.AddBorrowedBookAsync(borrowedBook));

        Assert.Equal("Üye bulunamadı.", exception.Message);
    }

    [Fact]  
    public async Task AddBorrowedBookAsync_ShouldThrowException_WhenBookIsNotFound()
    {
       
        var mockLogger = new Mock<ILogger<BorrowedBookService>>();
        var mockBookService = new Mock<IBookService>();
        var mockContext = new Mock<LibraryDbContext>();

        var nonExistentMemberId = 111;
        var nonExistentBookId = 222;
        var borrowedBook = new BorrowedBook { MemberId = nonExistentMemberId, BookId = nonExistentBookId };
        
        var members = new List<Member> { new Member { Id = nonExistentMemberId, Name = "Test Üye" } }.AsQueryable();
        var books = new List<Book>().AsQueryable(); 

        
        var mockMembers = new Mock<DbSet<Member>>();
        mockMembers.As<IQueryable<Member>>().Setup(m => m.Provider).Returns(members.Provider);
        mockMembers.As<IQueryable<Member>>().Setup(m => m.Expression).Returns(members.Expression);
        mockMembers.As<IQueryable<Member>>().Setup(m => m.ElementType).Returns(members.ElementType);
        mockMembers.As<IQueryable<Member>>().Setup(m => m.GetEnumerator()).Returns(members.GetEnumerator());

       
        var mockBooks = new Mock<DbSet<Book>>();
        mockBooks.As<IQueryable<Book>>().Setup(m => m.Provider).Returns(books.Provider);
        mockBooks.As<IQueryable<Book>>().Setup(m => m.Expression).Returns(books.Expression);
        mockBooks.As<IQueryable<Book>>().Setup(m => m.ElementType).Returns(books.ElementType);
        mockBooks.As<IQueryable<Book>>().Setup(m => m.GetEnumerator()).Returns(books.GetEnumerator());

        
        mockBooks.Setup(m => m.FindAsync(It.IsAny<int>()))
                 .ReturnsAsync((Book)null);
       
        mockContext.Setup(c => c.Members).Returns(mockMembers.Object);
        mockContext.Setup(c => c.Books).Returns(mockBooks.Object);
       
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _borrowedBookService.AddBorrowedBookAsync(borrowedBook));

        Assert.Equal("Üye bulunamadı.", exception.Message);
    }



    [Fact]
    public async Task AddBorrowedBookAsync_ShouldThrowException_WhenMemberHasThreeBooks()
    {
        var borrowedBook = new BorrowedBook { MemberId = 1, BookId = 1 };
        var member = new Member { Id = 1, BorrowedBooks = new List<BorrowedBook> { new BorrowedBook(), new BorrowedBook(), new BorrowedBook() } }; // 3 kitap ödünç almış
        var book = new Book { Id = 1, IsBorrowed = false };
       
        _mockDbContext.Setup(x => x.Members.FindAsync(It.IsAny<int>())).ReturnsAsync(member); 
        _mockDbContext.Setup(x => x.Books.FindAsync(It.IsAny<int>())).ReturnsAsync(book); 
       
        var borrowedBooks = new List<BorrowedBook>
    {
        new BorrowedBook { MemberId = 1 },
        new BorrowedBook { MemberId = 2 },
        new BorrowedBook { MemberId = 3 }
    }.AsQueryable();

        var mockDbSet = new Mock<DbSet<BorrowedBook>>();
        mockDbSet.As<IQueryable<BorrowedBook>>()
                 .Setup(m => m.Provider)
                 .Returns(borrowedBooks.Provider);
        mockDbSet.As<IQueryable<BorrowedBook>>()
                 .Setup(m => m.Expression)
                 .Returns(borrowedBooks.Expression);
        mockDbSet.As<IQueryable<BorrowedBook>>()
                 .Setup(m => m.ElementType)
                 .Returns(borrowedBooks.ElementType);
        mockDbSet.As<IQueryable<BorrowedBook>>()
                 .Setup(m => m.GetEnumerator())
                 .Returns(borrowedBooks.GetEnumerator());
        
        _mockDbContext.Setup(x => x.BorrowedBooks).Returns(mockDbSet.Object);
        
        await Assert.ThrowsAsync<InvalidOperationException>(() => _borrowedBookService.AddBorrowedBookAsync(borrowedBook));
    }


    [Fact]
    public async Task AddBorrowedBookAsync_ShouldThrowException_WhenBookIsAlreadyBorrowed()
    {
        var borrowedBook = new BorrowedBook { MemberId = 1, BookId = 1 };
        var book = new Book { Id = 1, IsBorrowed = true };

        
        var books = new List<BorrowedBook> { borrowedBook }.AsQueryable();
        var mockBorrowedBooksDbSet = new Mock<DbSet<BorrowedBook>>();
        mockBorrowedBooksDbSet.As<IQueryable<BorrowedBook>>().Setup(m => m.Provider).Returns(books.Provider);
        mockBorrowedBooksDbSet.As<IQueryable<BorrowedBook>>().Setup(m => m.Expression).Returns(books.Expression);
        mockBorrowedBooksDbSet.As<IQueryable<BorrowedBook>>().Setup(m => m.ElementType).Returns(books.ElementType);
        mockBorrowedBooksDbSet.As<IQueryable<BorrowedBook>>().Setup(m => m.GetEnumerator()).Returns(books.GetEnumerator());

        _mockDbContext.Setup(x => x.BorrowedBooks).Returns(mockBorrowedBooksDbSet.Object);
       
        _mockDbContext.Setup(x => x.Members.FindAsync(1)).ReturnsAsync(new Member());
      
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _borrowedBookService.AddBorrowedBookAsync(borrowedBook));

        Assert.Equal("Üye bulunamadı.", exception.Message);
    }
    [Fact]
    public async Task AddBorrowedBookAsync_ShouldAddBookSuccessfully()
    {
        var options = new DbContextOptionsBuilder<LibraryDbContext>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
           .Options;

        using (var context = new LibraryDbContext(options))
        {
           
            var mockLogger = new Mock<ILogger<BorrowedBookService>>();
            var mockBookService = new Mock<IBookService>();

            
            var borrowedBookService = new BorrowedBookService(mockLogger.Object, context, mockBookService.Object);

           
            context.BorrowedBooks.RemoveRange(context.BorrowedBooks);
            context.Books.RemoveRange(context.Books);
            context.Members.RemoveRange(context.Members);
            await context.SaveChangesAsync();

            
            var member = new Member { Id = 77, Name = "Member7", Surname = "Surname7", Email = "member7@hotmail.com", Phone = "05341552452" };
            var book = new Book { Id = 44, Title = "Book22", AuthorId = 77, PublishedYear = new DateTime(1912, 1, 1), IsBorrowed = false };

            context.Members.Add(member);
            context.Books.Add(book);
            await context.SaveChangesAsync();

            
            var checkMember = await context.Members.FindAsync(77);
            Assert.NotNull(checkMember); 

            
            var borrowedBook = new BorrowedBook { MemberId = 77, BookId = 44, BorrowDate = DateTime.Now };
            await borrowedBookService.AddBorrowedBookAsync(borrowedBook);

           
            var addedBook = await context.Books.FindAsync(44);
            Assert.NotNull(addedBook); 
            Assert.True(addedBook.IsBorrowed);

            var addedBorrowedBook = await context.BorrowedBooks
                .FirstOrDefaultAsync(b => b.BookId == 44 && b.MemberId == 77);
            Assert.NotNull(addedBorrowedBook); 
        }
    }


}





