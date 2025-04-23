using LibraryManagementAPI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAPI.Core.Services
{/// <summary>
/// Kütüphane için kitap işlemlerini yöneten IService katmanı.
/// </summary>
    public interface IBookService
    {
        /// <summary>
        /// Tüm kitapları getirir.
        /// </summary>
        /// <returns>Uygun kriterlere göre kitapları listeler.</returns>
        Task<IEnumerable<Book>> GetAllBooksAsync();
        /// <summary>
        /// ID'e göre kitapları listeleyen metoddur.
        /// </summary>
        /// <param name="id">Getirilen kitabın kimliği</param>
        /// <returns>ID bazlı kitapları getirir.ID belirtilir.</returns>
        Task<Book> GetBookByIdAsync(int id);
        /// <summary>
        /// Kitap ekleme işlemini yapan metoddur.
        /// </summary>
        /// <param name="book">Eklenecek olan kitabı belirtir.</param>
        /// <returns>Yeni kitabı ekler.</returns>
        Task AddBookAsync(Book book);
        /// <summary>
        /// Kitap güncelleme işlemini yapan metoddur.
        /// </summary>
        /// <param name="book">Güncellenecek olan kitabı belirtir.</param>
        /// <returns>Kitabı günceller.</returns>
        Task<Book> UpdateBookAsync(Book book);
        /// <summary>
        /// Kitap silme işlemini yapan metoddur.
        /// </summary>
        /// <param name="id">Silinecek olan kitaba ait ID'dir.</param>
        /// <returns>ID'ye göre kitap silme işlemi yapar.</returns>
        Task<bool> DeleteBookAsync(int id);
        /// <summary>
        /// Belirtilen kriterlere göre filtreme yaparak parametrelere uygun sorgu getirir.
        /// </summary>
        /// <param name="author">Kitaba ait yazarı belirtir.</param>
        /// <param name="startDate">Kitabın ödünç alma taridir.</param>
        /// <param name="endDate">Ödünç alınmış kitabın teslim edilme tarihidir.</param>
        /// <returns></returns>
        Task<IEnumerable<Book>> FilterBookAsync(string author = null, DateTime? startDate = null,DateTime? endDate=null);
        Task<IEnumerable<Book>> GetBookByYearAsync(int year);
        Task<IEnumerable<Book>> GetBookByAuthorAsync(int authorId);
        Task<IEnumerable<Book>>GetBookByYearRangeAsync(int startYear, int endYear);
    }
}
