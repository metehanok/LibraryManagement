using LibraryManagementAPI.Core.Models;
using LibraryManagementAPI.Core.Services;
using LibraryManagementAPI.Data.Repositories;
using LibraryManagementAPI.Service.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAPI.Service.Services
{
    public class BorrowedBookService : IBorrowedBookService
    {
        private readonly LibraryDbContext _context;
        private readonly ILogger<BorrowedBookService> _logger;
        private IBookService _bookService;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Veritabanı nesne örneği</param>
        public BorrowedBookService(ILogger<BorrowedBookService> logger, LibraryDbContext context,IBookService bookService)
            {
            _context = context;
            _logger = logger;
            _bookService = bookService;
        }
        /// <summary>
        /// Ödünç alma işlemini gerçekleştiren metod,Bir üye en fazla 3 adte kitap ödünç alabilir
        /// </summary>
        /// <param name="borrowedBook">Ödünç alınan kitap</param>
        /// <returns>Kitap ödünç alınır.</returns>
        /// <exception cref="Exception">Belirtilen koşulun sağlanmadığı durumda gösterilen hata mesajı</exception>
        public async Task AddBorrowedBookAsync(BorrowedBook borrowedBook)
        {
            var member = await _context.Members.FirstOrDefaultAsync(m=>m.Id==borrowedBook.MemberId);
            var book = await _context.Books.FirstOrDefaultAsync(m=>m.Id==borrowedBook.BookId);

            if (member == null)
            {
                
                throw new InvalidOperationException("Üye bulunamadı.");
            }
            if( book == null)
            {
                throw new InvalidOperationException("Kitap bulunamadı.");
            }
            var borrowedBooksCount=await _context.BorrowedBooks.
                Where(m=>m.MemberId==borrowedBook.MemberId && m.ReturnDate>DateTime.Now).CountAsync();

            if (borrowedBooksCount >= 3)
            {
                throw new Exception("Bir üye aynı anda en fazla 3 kitap ödünç alabilir!");
            }
            if (book.IsBorrowed)
            {
                throw new Exception("Kitap zaten ödünç alınmış.");
            }

            //kitap ödünç alınıyor
            book.IsBorrowed = true;
           // borrowedBook.BorrowDate = DateTime.Now;
            borrowedBook.ReturnDate= DateTime.Now.AddMonths(3);

            var existingBorrowedBook = await _context.BorrowedBooks//satır sonradan eklendi , ödünç alma işlemi yapamzsa bu satır ve if satırını sil
       .FirstOrDefaultAsync(b => b.BookId == borrowedBook.BookId && b.MemberId == borrowedBook.MemberId);
            if (existingBorrowedBook == null)
            {
                await _context.BorrowedBooks.AddAsync(borrowedBook);
            }
           var rowsAffected= await _context.SaveChangesAsync();
            if (rowsAffected == 0)
            {
                throw new Exception("Güncelleme başarısız oldu");
            }
        }
        /// <summary>
        /// Ödünç alınmış kitabın silme işlemini gerçekleştiren metod.Ödünç verilmiş ama iade edilmemişse silme işlemi gerçekleşmez
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Ödünç alınmış kitap kaydı silinir. Kitap ödünç alınmaya müsait.</returns>

        public async Task<bool> DeleteBorrowedBookAsync(int id)
        {
           var borrowedBook=await _context.BorrowedBooks.FirstOrDefaultAsync(x=>x.Id==id);
            if (borrowedBook == null)
                return false;

            // Kitap ödünç verilmiş ama iade edilmemişse silme işlemine izin verilmez
            if (borrowedBook.ReturnDate == null)
                return false;

            var book = await _context.Books.FindAsync(borrowedBook.BookId);
            if(book != null)
            {
                book.IsBorrowed = false;
            }

            _context.BorrowedBooks.Remove(borrowedBook);
            await _context.SaveChangesAsync();
            return true;
        }
        /// <summary>
        /// Ödünç alınmış tüm kitapları listeleyen metod.
        /// </summary>
        /// <returns>Ödünç alınmış tüm kitaplar listelenir.</returns>
        public async Task<IEnumerable<BorrowedBook>> GetAllBorrowedBookAsync()
        {
           return await _context.BorrowedBooks.Include(b=>b.Books).Include(m=>m.Member).ToListAsync();
        }
        /// <summary>
        /// Ödünç alınmış kitapları ID bazlı sorgulamaya yarayan metod.
        /// </summary>
        /// <param name="id">Ödünç alınmış kitaba ait ID</param>
        /// <returns>ID bazlı ödünç alınmış kitapları sorgular.</returns>
        public async Task<BorrowedBook> GetBorrowedBookByIdAsync(int id)
        {
            return await _context.BorrowedBooks
                .Include(b => b.Books)
                .Include(m => m.Member)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        /// <summary>
        /// Ödünç alınmış,alınacak kitaba ait güncelleme işlemini gerçekleştiren metod.Kitap iade edilmişse güncelleme işlemi yapılmaz
        /// </summary>
        /// <param name="borrowedBookId">Ödünç alınmış kitaba ait ID</param>
        /// <param name="returnDate">Ödünç alınmış kitabın teslim tarihi</param>
        /// <returns>Ödünç alınmış,alınacak kitabı günceller.</returns>

        public async Task<bool> UpdateBorrowedBookAsync(int borrowedBookId, BorrowedBook borrowedBook)
        {
            try
            {
                var existingBorrowedBook = await _context.BorrowedBooks
                    .FirstOrDefaultAsync(x => x.Id == borrowedBookId);

                if (existingBorrowedBook == null)
                {
                    _logger.LogWarning($"ID {borrowedBookId} ile ödünç alınan kitap bulunamadı.");
                    return false;
                }

                var book = await _context.Books.FindAsync(borrowedBook.BookId);
                if (book == null)
                {
                    _logger.LogWarning($"ID {borrowedBook.BookId} ile mevcut bir kitap bulunamadı.");
                    return false;
                }

                // Ödünç alma tarihi değiştirilemez
                borrowedBook.BorrowDate = existingBorrowedBook.BorrowDate;

                // Eğer returnDate null ise, 3 ay sonrası atanmalı
                borrowedBook.ReturnDate = DateTime.Now.AddMonths(3);


                if (borrowedBook.ReturnDate < borrowedBook.BorrowDate)
                {
                    throw new Exception("İade tarihi, ödünç alma tarihinden eski olamaz!");
                }
                // Bir üyenin ödünç aldığı kitap sayısını kontrol et
                var borrowedBooksCount = await _context.BorrowedBooks
                    .CountAsync(x => x.MemberId == borrowedBook.MemberId && x.ReturnDate == null);

                if (borrowedBooksCount >= 3)
                {
                    _logger.LogWarning($"Üye ID {borrowedBook.MemberId} zaten 3 kitap ödünç almış. Yeni bir ödünç işlemi yapılamaz.");
                    return false;  // Ya da uygun bir hata mesajı dönebilirsiniz.
                }


                // Verileri güncelle
                existingBorrowedBook.ReturnDate = borrowedBook.ReturnDate;
                existingBorrowedBook.BookId = borrowedBook.BookId;

                _context.BorrowedBooks.Update(existingBorrowedBook);
                int rowAffected = await _context.SaveChangesAsync();

                if (rowAffected == 0)
                {
                    throw new Exception("Veritabanına değişiklik yansıtılamadı, güncelleme başarısız.");
                }

                _logger.LogInformation($"ID {borrowedBookId} için ödünç alınan kitap başarıyla güncellendi.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ödünç alınmış kitap güncelleme hatası.");
                return false;
            }
        }



        /// <summary>
        /// Ödünç alınmış kitaplara ait üyeleri listeleyen metod
        /// </summary>
        /// <param name="memberId">Üye ID.</param>
        /// <returns>Ödünç alınmış kitaba ait üyeleri getirir,listeler</returns>
        public async Task<IEnumerable<BorrowedBook>> GetBooksBorrowedByMemberAsync(int memberId)
        {
            return await _context.BorrowedBooks
                .Where(b => b.MemberId == memberId)
                .Include(b => b.Books)
                .Include(b => b.Member)
                .ToListAsync();
        }

      

       
    }
}
