using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAPI.Service.DTO
{/// <summary>
/// Kitap güncelleme yapısını temsil eden DTO
/// </summary>
    public class BookUpdateDTO
    {/// <summary>
    /// Güncellenecek olan kitaba ait ID.
    /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Güncellenecek kitaba ait isim,açıklama
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Güncellenecek kitaba ait yazar ID.
        /// </summary>
        public int AuthorId { get; set; }
        /// <summary>
        /// Güncellenecek kitabın ödünç alınma durumu.
        /// </summary>
        public bool IsBorrowed { get; set; }
    }
}
