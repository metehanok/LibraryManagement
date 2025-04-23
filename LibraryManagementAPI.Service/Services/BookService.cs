using LibraryManagementAPI.Core.Models;
using LibraryManagementAPI.Core.Services;
using LibraryManagementAPI.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace LibraryManagementAPI.Service.Services
    {/// <summary>
    /// Kitap işlemlerini implement eden service katmanı.
    /// </summary>
        public class BookService : IBookService
        {

            private readonly LibraryDbContext _context;
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="context">Veritabanı nesne örneği</param>
            public BookService(LibraryDbContext context)
            {
                _context = context;
            }/// <summary>
            /// Kitap ekleme işlemini gerçekleştiren metod
            /// </summary>
            /// <param name="book">Eklenece olan kitap</param>
            /// <returns>Kitap eklenir kaydedilir.</returns>
            public async Task AddBookAsync(Book book)
            {
                await _context.Books.AddAsync(book);
                await _context.SaveChangesAsync();
            }
            /// <summary>
            /// Kitap silme işlemini gerçekleştiren metod.Ödünç verilmişse silinmez
            /// </summary>
            /// <param name="id">Silinecen olan kitaba ait ID, silme ıD'ye göre yapılır.</param>
            /// <returns>Kitap silinir</returns>
            public async Task<bool> DeleteBookAsync(int id)
            {
                var book = await _context.Books
               .Include(b => b.BorrowedBooks)
               .FirstOrDefaultAsync(b => b.Id == id);

                if (book == null)
                {
                    return false;
                }

                // Kitap ödünç verilmişse silinemez
                if (book.BorrowedBooks.Any(b => b.ReturnDate == null))
                {
                    return false;
                }

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return true;
            }/// <summary>
            /// Belirtilen parametrelere göre kitap filtreleme işlemini yapan metod
            /// </summary>
            /// <param name="author">Yazar</param>
            /// <param name="startDate">Ödünç alma tarihi</param>
            /// <param name="endDate">Teslim tarihi</param>
            /// <returns>Parametrelere göre filtreleme yapar, istenen filtreyi uygular kitabı getirir.</returns>
                public async Task<IEnumerable<Book>>FilterBookAsync(string author=null,DateTime? startDate=null,DateTime? endDate = null)
                {
                    var query=_context.Books.AsQueryable();
                    if (!string.IsNullOrEmpty(author))
                    {
                        query=query.Where(a=>a.Authors.Name.Contains(author));
                    }
                    if (startDate.HasValue)
                    {
                        query = query.Where(s => s.PublishedYear >= startDate.Value);
                    }
                    if(endDate.HasValue)
                    {
                        query=query.Where(e=>e.PublishedYear <= endDate.Value);
                    }
                    return await query.ToListAsync();
                }
            /// <summary>
            /// Tüm kitapları listeleyen metod.
            /// </summary>
            /// <returns>Kayıtlı tüm kitapları listeler.</returns>
            public async Task<IEnumerable<Book>> GetAllBooksAsync()
            {
                  return await _context.Books.ToListAsync();
            }
            /// <summary>
            /// ID'bazlı sorgulama yaparak belirtilen ID'ye göre kitabı getiren metod
            /// </summary>
            /// <param name="id">Sorgulanan kitaba ait ID.</param>
            /// <returns>ID bazlı kitap getirir.</returns>

            public async Task<Book> GetBookByIdAsync(int id)
            {
                return await _context.Books.Include(b => b.Authors).FirstOrDefaultAsync(b=>b.Id==id);
            }
            /// <summary>
            /// Kitaba ait güncelleme işlemini yapan metod.
            /// </summary>
            /// <param name="book"></param>
            /// <returns>Kitabı günceller.</returns>
            public async Task<Book> UpdateBookAsync(Book book)
            {
                _context.Books.Update(book);
                await _context.SaveChangesAsync();
                return book;
            }
            /// <summary>
            /// Yazara göre kitap getiren metod.
            /// </summary>
            /// <param name="authorId">Yazara ait id</param>
            /// <returns>Yazara ait ID bazlı kitap getirir.</returns>
            public async Task<IEnumerable<Book>> GetBookByAuthorAsync(int authorId)
            {//kitabı yazara göre listeleme
                return await _context.Books
                    .Where(x=>x.AuthorId== authorId)
                    .Include(a=>a.Authors)
                    .ToListAsync();
            }
            /// <summary>
            /// Yayımlanma tarihine göre kitapları listeleyen metod
            /// </summary>
            /// <param name="year">Yayımlanma yılı</param>
            /// <returns>Yayımlanma yılına göre sorgu yapıp, belirtilen yıl,tarihe göre kitap listeler</returns>
            public async Task<IEnumerable<Book>> GetBookByYearAsync(int year)
            {
                return await _context.Books
                    .Where(x=>x.PublishedYear.Year==year)
                    .Include (a=>a.Authors)
                    .ToListAsync() ;
            }
            public async Task<IEnumerable<Book>>GetBookByYearRangeAsync(int startYear, int endYear)
            {
                var book = await _context.Books.Where(b => b.PublishedYear.Year >= startYear && b.PublishedYear.Year <= endYear).ToListAsync();
                return book;
            }
        }
    }
