using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAPI.Service.DTO
{/// <summary>
/// Ödünç alınmış kitaba ait güncelleme yapısını temsil eden DTO.
/// </summary>
    public class BorrowedBookUpdateDTO
    {
        /// <summary>
        /// Ödünç alınmış kitaba ait ID.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Ödünç alınmış kitaba ait isim,açıklama
        /// </summary>
        public int BookId { get; set; }
        /// <summary>
        /// Ödünç alınmış kitaba ait üye isim.
        /// </summary>
        public int MemberId { get; set; }
        /// <summary>
        /// Ödünç alma tarihi
        /// </summary>
        public DateTime BorrowDate { get; set; }
        /// <summary>
        /// Teslim etme tarihi
        /// </summary>
        public DateTime? ReturnDate { get; set; }
    }
}
