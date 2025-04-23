using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LibraryManagementAPI.Core.Models
{/// <summary>
/// Kütüphanede'ki üyeleri belirten sınıf yapısı
/// </summary>
    public class Member
    {
        /// <summary>
        /// Üyeye ait ID.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Üye ismi.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Üye soyismi.
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// Üye e-mail.
        /// </summary>
        public  string  Email { get; set; }
        /// <summary>
        /// Üye telefon no.
        /// </summary>
        public string  Phone { get; set; }
        /// <summary>
        /// Üyelik tarihi.
        /// </summary>
        public DateTime MembershipDate { get; set; }//üyelik tarihi
        /// <summary>
        /// Üyenin ödünç aldığı kitaplar
        /// </summary>

        [JsonIgnore]
        public ICollection<BorrowedBook> BorrowedBooks { get; set; }

    }
}
