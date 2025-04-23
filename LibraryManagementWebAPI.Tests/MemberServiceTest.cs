using LibraryManagementAPI.Core.Models;
using LibraryManagementAPI.Core.Services;
using LibraryManagementAPI.Data.Repositories;
using LibraryManagementAPI.Service.Services;
using LibraryManagementWebAPI.Tests;
using LibraryManagementWebAPI.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
    public class MemberServiceTest
    {
        private readonly MemberService _memberService;
        private readonly DbContextOptions<LibraryDbContext> _dbContextOptions;
        private readonly Mock<LibraryDbContext> _mockContext;
        private readonly Mock<DbSet<Member>> _mockMemberDbSet;
        private readonly Mock<IMemberService> _mockMemberService;

        public MemberServiceTest()
        {
            _dbContextOptions = new DbContextOptionsBuilder<LibraryDbContext>()
               .UseInMemoryDatabase(databaseName: "TestLibraryDb")
               .Options;
            _mockMemberDbSet = new Mock<DbSet<Member>>();
            _mockContext = new Mock<LibraryDbContext>(_dbContextOptions);

            _memberService = new MemberService(_mockContext.Object);

        }
        [Fact]
        public async Task AddMember_ShouldAddMemberSuccesfully()
        {
            var member = new Member { Id = 1, Name = "NameMember1", Surname = "SurnameMember1", Email = "member1@hotmail.com", Phone = "532-234-45-45", MembershipDate = new DateTime(2019, 1, 1) };
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
               .UseInMemoryDatabase(databaseName: "TestDatabase")
               .Options;
            using (var context = new LibraryDbContext(_dbContextOptions))
            {
                var mockMemberDbSet = new Mock<DbSet<Member>>();
                var members = new List<Member> { member }.AsQueryable();
                mockMemberDbSet.As<IQueryable<Member>>().Setup(m => m.Provider).Returns((new List<Member>()).AsQueryable().Provider);
                mockMemberDbSet.As<IQueryable<Member>>().Setup(m => m.Expression).Returns((new List<Member>()).AsQueryable().Expression);
                mockMemberDbSet.As<IQueryable<Member>>().Setup(m => m.ElementType).Returns((new List<Member>()).AsQueryable().ElementType);
                mockMemberDbSet.As<IQueryable<Member>>().Setup(m => m.GetEnumerator()).Returns((new List<Member>()).AsQueryable().GetEnumerator());


                _mockContext.Setup(c => c.Members).Returns(mockMemberDbSet.Object);

                var memberService = new MemberService(_mockContext.Object);
                await memberService.AddMemberAsync(member);
                mockMemberDbSet.Verify(m => m.AddAsync(It.IsAny<Member>(), default), Times.Once);
            }
        }

        [Fact]


        public async Task GetAllMembers_ShouldReturnAllMembers()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: "Test Database") // 🔥 Her test için farklı isim
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                var mockMemberDbSet = new Mock<DbSet<Member>>();

                var members = new List<Member>
                {
                    new Member { Id = 2, Name = "NameMember 2", Surname = "SurnameMember 2", Email = "member2@hotmail.com", Phone = "533-274-15-45", MembershipDate = new DateTime(2020, 1, 1) },
                    new Member { Id = 3, Name = "NameMember 3", Surname = "SurnameMember 3", Email = "member3@hotmail.com", Phone = "531-254-47-45", MembershipDate = new DateTime(2011, 1, 1) },
                };
                context.Members.AddRange(members.AsEnumerable());
                await context.SaveChangesAsync();

                mockMemberDbSet.As<IQueryable<Member>>().Setup(m => m.Provider).Returns(members.AsQueryable().Provider);
                mockMemberDbSet.As<IQueryable<Member>>().Setup(m => m.Expression).Returns(members.AsQueryable().Expression);
                mockMemberDbSet.As<IQueryable<Member>>().Setup(m => m.ElementType).Returns(members.AsQueryable().ElementType);
                mockMemberDbSet.As<IQueryable<Member>>().Setup(m => m.GetEnumerator()).Returns(members.AsQueryable().GetEnumerator());

                _mockContext.Setup(c => c.Members).Returns(mockMemberDbSet.Object);
                var memberService = new MemberService(context);
                var result = await memberService.GetAllMembersAsync();
                Assert.NotNull(result);
                Assert.Equal(2, result.Count());
            }
        }
        [Fact]
        public async Task GetMemberByIdAsync_ShouldReturnMember_WhenMemberExists()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                            .UseInMemoryDatabase(databaseName: "Test Database")
                            .Options;
            using (var context = new LibraryDbContext(options))
            {
                var member = new Member { Id = 5, Name = "Member 5", Surname = "Surname Member 5", Email = "member1@hotmail.com", Phone = "532-234-45-45", MembershipDate = new DateTime(2019, 1, 1) };
                var mockMembersDbSet = new Mock<DbSet<Member>>();
                var members = new List<Member> { member }.AsQueryable();

                mockMembersDbSet.As<IQueryable<Member>>()
                    .Setup(m => m.Provider).Returns(members.Provider);
                mockMembersDbSet.As<IQueryable<Member>>()
                    .Setup(m => m.Expression).Returns(members.AsQueryable().Expression);
                mockMembersDbSet.As<IQueryable<Member>>()
                    .Setup(m => m.ElementType).Returns(members.AsQueryable().ElementType);
                mockMembersDbSet.As<IQueryable<Member>>()
                    .Setup(m => m.GetEnumerator()).Returns(members.AsQueryable().GetEnumerator());

                // Mock DbContext'e Members DbSet'ini ayarla
                _mockContext.Setup(c => c.Members).Returns(mockMembersDbSet.Object);

                context.Members.Add(member);
                await context.SaveChangesAsync();
            }
            using (var context = new LibraryDbContext(options))
            {
                var memberService = new MemberService(context);

               
                var result = await memberService.GetMemberByIdAsync(5);

               
                Assert.NotNull(result);
                Assert.Equal("Member 5", result.Name);
                Assert.Equal("Surname Member 5", result.Surname);
            }

        }
        public async Task UpdateMemberAsync_ShouldUpdateMember_WhenMemberExists()
        {

            var member = new Member { Id = 9, Name = "Member 5", Surname = "Surname Member 5", Email = "member1@hotmail.com", Phone = "532-234-45-45", MembershipDate = new DateTime(2019, 1, 1) };
            var members = new List<Member>() { member }.AsQueryable();
            var mockMembersDbSet = new Mock<DbSet<Member>>();

            mockMembersDbSet.As<IQueryable<Member>>()
                   .Setup(m => m.Provider).Returns(members.Provider);
            mockMembersDbSet.As<IQueryable<Member>>()
                .Setup(m => m.Expression).Returns(members.Expression);
            mockMembersDbSet.As<IQueryable<Member>>()
                .Setup(m => m.ElementType).Returns(members.ElementType);
            mockMembersDbSet.As<IQueryable<Member>>()
                .Setup(m => m.GetEnumerator()).Returns(members.GetEnumerator());

            // Mock DbContext'e Members DbSet'ini ayarla
            _mockContext.Setup(c => c.Members).Returns(mockMembersDbSet.Object);

            //var updatedmember = new Member { Id = 5, Name = "Updated Member 5", Surname = "Updated Surname Member 5", Email = "member1@hotmail.com", Phone = "532-234-45-45", MembershipDate = new DateTime(2019, 1, 1) };
            member.Name = "Updated Name";
            member.Surname = "Updated Surname";

            var result = await _memberService.UpdateMemberAsync(member);
            Assert.Null(result);
            Assert.Equal("Updated Member 5", result.Name);
            Assert.Equal("Updated Surname Member 5", result.Surname);
        }



        //[Fact]
        //public async Task DeleteMemberAsync_ShouldDeleteMember()
        //{
        //    var options = new DbContextOptionsBuilder<LibraryDbContext>()
        //        .UseInMemoryDatabase(databaseName: "TestDatabase")
        //        .Options;

        //    using (var context = new LibraryDbContext(options))
        //    {
        //        // Arrange: Test verisini veritabanına ekliyoruz
        //        var member = new Member
        //        {
        //            Id = 9,
        //            Name = "Member 5",
        //            Surname = "Surname Member 5",
        //            Email = "member1@hotmail.com",
        //            Phone = "532-234-45-45",
        //            MembershipDate = new DateTime(2019, 1, 1)
        //        };

        //        await context.Members.AddAsync(member);
        //        await context.SaveChangesAsync(); // Veriyi kaydediyoruz

        //        var memberService = new MemberService(context);

        //        // Act: Üye silme işlemi
        //        var deletedMember = await memberService.DeleteMemberAsync(9);

        //        // Assert: Üye gerçekten silindi mi?
        //        Assert.NotNull(deletedMember); // Silinen üye null olmamalı
        //        Assert.Equal(9, deletedMember.Id); // Doğru üyenin silindiğinden emin ol

        //        var memberInDb = await context.Members.FindAsync(9);
        //        Assert.Null(memberInDb); // Veritabanında artık olmamalı
        //    }
        //}
        [Fact]
        public async Task DeleteMemberAsync_ShouldDeleteMember()
        {
            
            var member = new Member
            {
                Id = 9,
                Name = "Member 5",
                Surname = "Surname Member 5",
                Email = "member1@hotmail.com",
                Phone = "532-234-45-45",
                MembershipDate = new DateTime(2019, 1, 1)
            };

            var members = new List<Member> { member };

           
            var mockMembersDbSet = new Mock<DbSet<Member>>();

            var queryableMembers = members.AsQueryable();

            mockMembersDbSet.As<IQueryable<Member>>()
                .Setup(m => m.Provider).Returns(queryableMembers.Provider);
            mockMembersDbSet.As<IQueryable<Member>>()
                .Setup(m => m.Expression).Returns(queryableMembers.Expression);
            mockMembersDbSet.As<IQueryable<Member>>()
                .Setup(m => m.ElementType).Returns(queryableMembers.ElementType);
            mockMembersDbSet.As<IQueryable<Member>>()
                .Setup(m => m.GetEnumerator()).Returns(() => queryableMembers.GetEnumerator());

           
            mockMembersDbSet.As<IAsyncEnumerable<Member>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<Member>(queryableMembers.GetEnumerator()));

            mockMembersDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] ids) => members.FirstOrDefault(m => m.Id == (int)ids[0]));

            mockMembersDbSet.Setup(m => m.Remove(It.IsAny<Member>()))
                .Callback<Member>(m => members.Remove(m));

           
            var mockContext = new Mock<LibraryDbContext>();
            mockContext.Setup(c => c.Members).Returns(mockMembersDbSet.Object);

            var memberService = new MemberService(mockContext.Object);

            
            var deletedMember = await memberService.DeleteMemberAsync(9);

           
            Assert.NotNull(deletedMember);
            Assert.Equal(9, deletedMember.Id);

            var memberDb = members.FirstOrDefault(m => m.Id == 9);
            Assert.Null(memberDb); 
        }

       
    }
}

