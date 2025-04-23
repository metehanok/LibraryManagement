using LibraryManagementAPI.Core.Models;
using LibraryManagementAPI.Core.Services;
using LibraryManagementAPI.Data.Repositories;
using LibraryManagementAPI.Service.Services;
using LibraryManagementWebAPI.Tests;
using LibraryManagementWebAPI.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Threading.Tasks;
using Xunit;
using static System.Reflection.Metadata.BlobBuilder;

namespace LibraryManagementAPI.Tests.Unit
{
    public class AuthorServiceTests
    {
        private readonly AuthorService _authorService;
        private readonly DbContextOptions<LibraryDbContext> _dbContextOptions;
        private readonly Mock<LibraryDbContext> _mockContext;
        private readonly Mock<DbSet<Author>> _mockAuthorsDbSet;
        private readonly Mock<IAuthorService> _mockAuthorService;
        

        public AuthorServiceTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<LibraryDbContext>()
               .UseInMemoryDatabase(databaseName: "TestLibraryDb")
               .Options;
            
            _mockAuthorsDbSet = new Mock<DbSet<Author>>();
            
            _mockContext = new Mock<LibraryDbContext>(_dbContextOptions);
           
            _mockContext.Setup(c => c.Authors).Returns(_mockAuthorsDbSet.Object);
                     
            _authorService = new AuthorService(_mockContext.Object);

        }

        [Fact]
        public async Task AddAuthorAsync_ShouldAddAuthorSuccessfully()
        {
            
            var author = new Author { Id = 1, Name = "Author 1", Surname = "Surname 1" };

            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new LibraryDbContext(_dbContextOptions))
            {
               
                var mockAuthorsDbSet = new Mock<DbSet<Author>>();
                mockAuthorsDbSet.As<IQueryable<Author>>().Setup(m => m.Provider).Returns((new List<Author>()).AsQueryable().Provider);
                mockAuthorsDbSet.As<IQueryable<Author>>().Setup(m => m.Expression).Returns((new List<Author>()).AsQueryable().Expression);
                mockAuthorsDbSet.As<IQueryable<Author>>().Setup(m => m.ElementType).Returns((new List<Author>()).AsQueryable().ElementType);
                mockAuthorsDbSet.As<IQueryable<Author>>().Setup(m => m.GetEnumerator()).Returns((new List<Author>()).AsQueryable().GetEnumerator());

                var mockContext = new Mock<LibraryDbContext>(options);
                mockContext.Setup(c => c.Authors).Returns(mockAuthorsDbSet.Object);

                var authorService = new AuthorService(mockContext.Object);
               
                await authorService.AddAuthorAsync(author);
               
                mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            }
        }

        [Fact]
       public async Task GetAllAuthorsAsync_ShouldReturnAllAuthors()
        {
            
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                 .Options;

            using (var context = new LibraryDbContext(options))
            {                                         
               
                var mockAuthorsDbSet = new Mock<DbSet<Author>>();
                var authors = new List<Author>
        {
            new Author { Id = 1, Name = "Author 1", Surname = "Surname 1", BirthDate = new DateTime(1945, 3, 4) },
            new Author { Id = 2, Name = "Author 2", Surname = "Surname 2", BirthDate = new DateTime(1922, 5, 9) }
        };
                context.Authors.AddRange(authors.AsEnumerable());
                await context.SaveChangesAsync();

                
                mockAuthorsDbSet.As<IQueryable<Author>>()
                    .Setup(m => m.Provider).Returns(authors.AsQueryable().Provider);
                mockAuthorsDbSet.As<IQueryable<Author>>()
                    .Setup(m => m.Expression).Returns(authors.AsQueryable().Expression);
                mockAuthorsDbSet.As<IQueryable<Author>>()
                    .Setup(m => m.ElementType).Returns(authors.AsQueryable().ElementType);
                mockAuthorsDbSet.As<IQueryable<Author>>()
                    .Setup(m => m.GetEnumerator()).Returns(authors.AsQueryable().GetEnumerator());
               
                _mockContext.Setup(c => c.Authors).Returns(mockAuthorsDbSet.Object);
                
                var authorService = new AuthorService(context);
                
                var result = await authorService.GetAllAuthorsAsync();
               
                Assert.NotNull(result);
                Assert.Equal(authors.Count, result.ToList().Count);
                Assert.Contains(result.ToList(), a => a.Name == "Author 1" && a.Surname == "Surname 1");
                Assert.Contains(result.ToList(), a => a.Name == "Author 2" && a.Surname == "Surname 2");
            }
        }
        [Fact]
        public async Task GetAuthorByIdAsync_ShouldReturnAuthor_WhenAuthorExists()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                            .UseInMemoryDatabase(databaseName: "Test Database")
                            .Options;
            
            using (var context = new LibraryDbContext(options))
            {
                var author = new Author { Id = 1, Name = "Author 1", Surname = "Surname 1" };
                var mockAuthorsDbSet = new Mock<DbSet<Author>>();
                mockAuthorsDbSet.As<IQueryable<Author>>()
                   .Setup(m => m.Provider).Returns(new List<Author>().AsQueryable().Provider);
                mockAuthorsDbSet.As<IQueryable<Author>>()
                    .Setup(m => m.Expression).Returns(new List<Author>().AsQueryable().Expression);
                mockAuthorsDbSet.As<IQueryable<Author>>()
                    .Setup(m => m.ElementType).Returns(new List<Author>().AsQueryable().ElementType);
                mockAuthorsDbSet.As<IQueryable<Author>>()
                    .Setup(m => m.GetEnumerator()).Returns(new List<Author>().AsQueryable().GetEnumerator());
                _mockContext.Setup(c => c.Authors).Returns(mockAuthorsDbSet.Object);

                context.Authors.Add(author);
                await context.SaveChangesAsync();
            }           
            using (var context = new LibraryDbContext(options))
            {
               
                var authorService = new AuthorService(context);

               
                var result = await authorService.GetAuthorByIdAsync(1);

                
                Assert.NotNull(result);  
                Assert.Equal("Author 1", result.Name);
                Assert.Equal("Surname 1", result.Surname);
            }
        }       
        [Fact]
        public async Task GetAuthorByIdAsync_ShouldReturnNull_WhenAuthorNotFound()
        {
           
            var authors = new List<Author> {
        new Author { Id = 1, Name = "Author 1" },
        new Author { Id = 2, Name = "Author 2" }
    }.AsQueryable();

            var mockAuthorDbSet = new Mock<DbSet<Author>>();
            mockAuthorDbSet.As<IQueryable<Author>>()
                .Setup(m => m.Provider).Returns(authors.Provider);
            mockAuthorDbSet.As<IQueryable<Author>>()
                .Setup(m => m.Expression).Returns(authors.Expression);
            mockAuthorDbSet.As<IQueryable<Author>>()
                .Setup(m => m.ElementType).Returns(authors.ElementType);
            mockAuthorDbSet.As<IQueryable<Author>>()
                .Setup(m => m.GetEnumerator()).Returns(authors.GetEnumerator());

            // Mock DbContext
            var mockContext = new Mock<LibraryDbContext>();
            mockContext.Setup(c => c.Authors).Returns(mockAuthorDbSet.Object);

            var authorService = new AuthorService(mockContext.Object);
            
            var result = await authorService.GetAuthorByIdAsync(999);
            
            Assert.Null(result);
        }


        [Fact]
        public async Task DeleteAuthorAsync_ShouldDeleteAuthorSuccessfully()
        {            
            var author = new Author { Id = 1, Name = "Author 1", Surname = "Surname 1" };

            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new LibraryDbContext(options))
            {               
                var mockAuthorsDbSet = new Mock<DbSet<Author>>();
                mockAuthorsDbSet.As<IQueryable<Author>>().Setup(m => m.Provider).Returns((new List<Author> { author }).AsQueryable().Provider);
                mockAuthorsDbSet.As<IQueryable<Author>>().Setup(m => m.Expression).Returns((new List<Author> { author }).AsQueryable().Expression);
                mockAuthorsDbSet.As<IQueryable<Author>>().Setup(m => m.ElementType).Returns((new List<Author> { author }).AsQueryable().ElementType);
                mockAuthorsDbSet.As<IQueryable<Author>>().Setup(m => m.GetEnumerator()).Returns((new List<Author> { author }).AsQueryable().GetEnumerator());

                var mockContext = new Mock<LibraryDbContext>(options);
                mockContext.Setup(c => c.Authors).Returns(mockAuthorsDbSet.Object);

                var authorService = new AuthorService(mockContext.Object);
               
                await authorService.DeleteAuthorAsync(1);
                
                var deletedAuthor = await authorService.GetAuthorByIdAsync(1);
                Assert.Null(deletedAuthor);
            }
        }       
        [Fact]
        public async Task DeleteAuthorAsync_ShouldReturnNull_WhenAuthorNotFound()
        {
           
            var authors = new List<Author> {
        new Author { Id = 1, Name = "Author 1" },
        new Author { Id = 2, Name = "Author 2" }
    }.AsQueryable();

            var mockAuthorDbSet = new Mock<DbSet<Author>>();
            mockAuthorDbSet.As<IQueryable<Author>>()
                .Setup(m => m.Provider).Returns(authors.Provider);
            mockAuthorDbSet.As<IQueryable<Author>>()
                .Setup(m => m.Expression).Returns(authors.Expression);
            mockAuthorDbSet.As<IQueryable<Author>>()
                .Setup(m => m.ElementType).Returns(authors.ElementType);
            mockAuthorDbSet.As<IQueryable<Author>>()
                .Setup(m => m.GetEnumerator()).Returns(authors.GetEnumerator());

            
            var mockContext = new Mock<LibraryDbContext>();
            mockContext.Setup(c => c.Authors).Returns(mockAuthorDbSet.Object);

            var authorService = new AuthorService(mockContext.Object);

            
            var deletedAuthor = await authorService.DeleteAuthorAsync(999);
            
            Assert.Null(deletedAuthor);
        }


        [Fact]
        public async Task UpdateAuthorAsync_ShouldUpdateAuthor_WhenAuthorExists()
        {           
            var author = new Author { Id = 1, Name = "Author 1", Surname = "Surname 1" };
           
            var mockAuthorsDbSet = new Mock<DbSet<Author>>();
            mockAuthorsDbSet.As<IQueryable<Author>>().Setup(m => m.Provider).Returns((new List<Author> { author }).AsQueryable().Provider);
            mockAuthorsDbSet.As<IQueryable<Author>>().Setup(m => m.Expression).Returns((new List<Author> { author }).AsQueryable().Expression);
            mockAuthorsDbSet.As<IQueryable<Author>>().Setup(m => m.ElementType).Returns((new List<Author> { author }).AsQueryable().ElementType);
            mockAuthorsDbSet.As<IQueryable<Author>>().Setup(m => m.GetEnumerator()).Returns((new List<Author> { author }).AsQueryable().GetEnumerator());

            _mockContext.Setup(x => x.Authors).Returns(mockAuthorsDbSet.Object);
           
            var updatedAuthor = new Author { Id = 1, Name = "Updated Author Name", Surname = "Updated Author Surname" };
           
            var result = await _authorService.UpdateAuthorAsync(updatedAuthor);
            
            Assert.NotNull(result);
            Assert.Equal("Updated Author Name", result.Name);
            Assert.Equal("Updated Author Surname", result.Surname);
        }
    }
}
