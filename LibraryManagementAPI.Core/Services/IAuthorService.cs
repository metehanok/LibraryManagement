using LibraryManagementAPI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAPI.Core.Services
{/// <summary>
/// Yazarlara ait işlemleri yöneten IServis katmanı
/// </summary>
    public interface IAuthorService
    {/// <summary>
    /// Tüm yazarları getiren metoddur.
    /// </summary>
    /// <returns>Tüm yazarları liste halinde getirir.</returns>
        Task<IEnumerable<Author>> GetAllAuthorsAsync();
        /// <summary>
        /// ID bazlı yazar döndüren metoddur.
        /// </summary>
        /// <param name="id">Yazar için belirtilen ID.</param>
        /// <returns>Kaydedilen yazarın ID'sine göre istenen yazarı getirir.</returns>
        Task<Author> GetAuthorByIdAsync(int id);
        /// <summary>
        /// Yazar ekleme işlemini yapan metoddur.
        /// </summary>
        /// <param name="author">Eklenecek olan yazarı belirtir.</param>
        /// <returns>Yazar ekler.</returns>
        Task AddAuthorAsync(Author author);
        /// <summary>
        /// Yazar güncelleme işlemini yapan metoddur.
        /// </summary>
        /// <param name="author">Güncellenecek olan yazarı belirtir.</param>
        /// <returns>Yazarı günceller</returns>
        Task<Author> UpdateAuthorAsync(Author author);
        /// <summary>
        /// Yazar silme işlemini yapan metoddur.
        /// </summary>
        /// <param name="id">Silinecek olan yazarı belirtir</param>
        /// <returns>Yazarı siler</returns>
        Task<Author> DeleteAuthorAsync(int id);
    }
}
