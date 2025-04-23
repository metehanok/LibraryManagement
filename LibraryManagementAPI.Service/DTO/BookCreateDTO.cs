using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAPI.Service.DTO
{/// <summary>
/// Kitap ekleme yapısını temsil eden DTO
/// </summary>
    public class BookCreateDTO
    {
        /// <summary>
        /// Eklenecek olam kitabın açıklaması,adı.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Eklenecek olan kitaba ait unique yazar ID.
        /// </summary>
        public int AuthorId { get; set; }
        /// <summary>
        /// Eklenmiş,Eklenecek olan kitabın ödünç alınma durumu
        /// </summary>
        public bool IsBorrowed { get; set; }
        /// <summary>
        /// Eklenecek olan kitabın yayımlanma tarihi
        /// </summary>
        public DateTime PublishedYear { get; set; }
    }
}
