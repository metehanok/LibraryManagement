using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LibraryManagementAPI.Core.Models
{/// <summary>
/// Ödünç alınmış kitabı temsil eden sınıf yapısı
/// </summary>
    public class BorrowedBook
    {
        /// <summary>
        /// Ödünç alma işlemine ait ID.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Ödünç alınmış,alınacak kitaba ait ID.
        /// </summary>
        public int BookId { get; set; }
        /// <summary>
        /// Kitabı ödünç alan üyeye ait ID.
        /// </summary>
        public int MemberId { get; set; }
        /// <summary>
        /// Ödünç alma tarihi.
        /// </summary>
        public DateTime BorrowDate { get; set; }
        /// <summary>
        /// Teslim etme tarihi
        /// </summary>
        public DateTime? ReturnDate { get; set; }
        /// <summary>
        /// Ödünç alınan kitap
        /// </summary>

        [JsonIgnore]
        public Book Books { get; set; }
        /// <summary>
        /// Kitabı ödünç alan üye
        /// </summary>
        public Member Member { get; set; }
    }
}
