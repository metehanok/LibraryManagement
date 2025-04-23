using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAPI.Service.DTO
{/// <summary>
/// Ödünç alınmış kitapları temsil edip listeleyen DTO yapısı
/// </summary>
    public class BorrowedBookReadDTO
    {
        /// <summary>
        /// Ödünç alınmış kitaba ait ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Ödünç alınmış kitaba it açıklama,isim
        /// </summary>
        public string BookTitle { get; set; }
        /// <summary>
        /// Ödünç alınmış kitaba ait üye ismi.
        /// </summary>
        public string MemberName { get; set; }
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
