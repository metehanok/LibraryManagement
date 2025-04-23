using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAPI.Service.DTO
{/// <summary>
/// Ödünç alma durumunu temsil eden DTO yapısı
/// </summary>
    public class BorrowedBookCreateDTO
    {
        /// <summary>
        /// Ödünç alınacak olan kitaba ait ID.
        /// </summary>
        public int BookId { get; set; }
        /// <summary>
        /// Ödünç alınmış kitaba ait üye ID.
        /// </summary>
        public int MemberId { get; set; }
        /// <summary>
        /// Ödünç alma tarihi
        /// </summary>
        public DateTime BorrowDate { get; set; }
        /// <summary>
        /// Teslim Tarihi
        /// </summary>
        public DateTime? ReturnDate { get; set; }
    }
}
