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
/// Yazar işlemlerini implement eden service katmanı
/// </summary>
    public class AuthorService : IAuthorService
    {
        private readonly LibraryDbContext _context;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Veritabanı nesne örneği</param>
        public AuthorService(LibraryDbContext context)
        {
            _context = context;
        }/// <summary>
        /// Yazar ekleme işlemini gerçekleştiren metod
        /// </summary>
        /// <param name="author">Eklenecek olan yazar</param>
        /// <returns>Yazar eklenir, kaydedilir.</returns>
        public async Task AddAuthorAsync(Author author)
        {
           await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Yazar silme işlemini gerçekleştiren metod
        /// </summary>
        /// <param name="id">Silinecek olan yazara ait ID.</param>
        /// <returns>Yazar veritabanından silinir.</returns>

        public async Task<Author> DeleteAuthorAsync(int id)
        {
          var author=await _context.Authors.FindAsync(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
            }
            return author;
        }
        /// <summary>
        /// Tüm yazarları listeleyen metod.
        /// </summary>
        /// <returns>Kayıtlı tüm yazarları listeler.</returns>
        public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
        {
            return await _context.Authors.ToListAsync();
        }
        /// <summary>
        /// ID bazlı sorgulama yaparak belirtilen ID'ye göre yazar getiren metod.
        /// </summary>
        /// <param name="id">Sorgulanan yazara ait ID</param>
        /// <returns>Sorgulanan ID'ye göre yazarı getirir.</returns>

        public async Task<Author> GetAuthorByIdAsync(int id)
        {
            return await _context.Authors.FindAsync(id);
        }
        /// <summary>
        /// Yazar güncelleme işlemini yapan metod
        /// </summary>
        /// <param name="author">Güncellenecek olan yazar</param>
        /// <returns>Yazar güncellenir,kaydedilir.</returns>

        public async Task<Author> UpdateAuthorAsync(Author author)
        {
             _context.Authors.Update(author);
            await _context.SaveChangesAsync();
            return author;
        }
    }
}
