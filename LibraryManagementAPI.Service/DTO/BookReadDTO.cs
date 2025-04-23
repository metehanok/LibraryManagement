using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAPI.Service.DTO
{/// <summary>
/// Eklenmiş kitapları listelemeye yarayan DTO yapısı
/// </summary>
    public class BookReadDTO
    {
        /// <summary>
        /// Eklenmiş kitaba ait ID.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Eklenmiş kitaba ait açıklama,isim
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Eklenmiş kitaba ait unique yazar ID.
        /// </summary>
        public int AuthorId { get; set; }
        /// <summary>
        /// Eklenmiş kitabın yayımlanma tarihi.
        /// </summary>
        public DateTime PublishedYear { get; set; }
        /// <summary>
        /// Eklenmiş kitabın ödünç alma durumunu listeler.
        /// </summary>
        public bool IsBorrowed { get; set; }
    }
}
