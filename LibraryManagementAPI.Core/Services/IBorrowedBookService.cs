using LibraryManagementAPI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAPI.Core.Services
{/// <summary>
/// Ödünç alınan kitaba ait IService katmanı.
/// </summary>
   public interface IBorrowedBookService
    {
        /// <summary>
        /// Ödünç alınan kitapları getirir
        /// </summary>
        /// <returns>Liste halinde ödünç alınmış kitapları getirir.</returns>
        Task<IEnumerable<BorrowedBook>> GetAllBorrowedBookAsync();
        /// <summary>
        /// ID'bazlı sorgulama işlemi ile ödünç alınan kitapları getirir.
        /// </summary>
        /// <param name="id">Ödünç alınmış olan kitap için ID.</param>
        /// <returns>Sorgulanan ID'ye göre ödünç alınan kitabı getirir.</returns>
        Task<BorrowedBook> GetBorrowedBookByIdAsync(int id);
        /// <summary>
        /// Ödünç alma işlemini gerçekleştirir
        /// </summary>
        /// <param name="borrowedBook">Ödünç alınacak kitabı belirtir.</param>
        /// <returns>Ödünç alma işlemini tamamlar,ekler.</returns>
        Task AddBorrowedBookAsync(BorrowedBook borrowedBook);
        /// <summary>
        /// Ödünç alınmış kitabı günceller.
        /// </summary>
        /// <param name="borrowedBookId">Ödünç alınmış kitaba ait ID.</param>
        /// <param name="returnDate">Ödünç alınmış kitabın teslim tarihi.</param>
        /// <returns>Ödünç alınmış kitabı belirtilen parametrelere göre günceller</returns>
      //  Task<bool> UpdateBorrowedBookAsync(int borrowedBookId, DateTime? returnDate,Book book);
        Task<bool> UpdateBorrowedBookAsync(int borrowedBookId, BorrowedBook borrowedBook);
        /// <summary>
        /// Ödünç alınmış kitabı silme işlemini yapan metoddur
        /// </summary>
        /// <param name="id">Silinecen olan ödünç alınmış kitaba ait ID.</param>
        /// <returns>Ödünç alınmış kitabı ID'ye göre siler.</returns>
        Task <bool>DeleteBorrowedBookAsync(int id);
      
    }
}
