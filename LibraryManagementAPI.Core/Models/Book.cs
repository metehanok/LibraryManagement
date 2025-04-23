using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LibraryManagementAPI.Core.Models
{/// <summary>
/// Kütüphanede ki kitapları belirten sınıf yapısı
/// </summary>
    public class Book
    {
        /// <summary>
        /// Kitaba ait ID.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Kitaba ait açıklama,isim.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Kitabın yazarına ait ID
        /// </summary>
        public int AuthorId { get; set; }
        /// <summary>
        /// Kitabın yayımlanma tarihi,yılı.
        /// </summary>
        public DateTime PublishedYear { get; set; }
        /// <summary>
        /// Kitabın ödünç alınma durumu.
        /// </summary>
        public bool IsBorrowed { get; set; }
        /// <summary>
        /// Kitaba ait yazar.
        /// </summary>
        public Author Authors { get; set; }//one to one
        /// <summary>
        /// Bir kitabın birden çok kez ödünç alınma durumu. One to many ilişkisi
        /// </summary>
        /// 
        [JsonIgnore]//json döngüsünü kırmak için,apide hata veriyor
        public ICollection<BorrowedBook> BorrowedBooks { get; set; }
    }
}
