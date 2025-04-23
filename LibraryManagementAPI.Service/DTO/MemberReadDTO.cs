using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAPI.Service.DTO
{/// <summary>
/// Eklenmiş üyeleri listeleme yapısını temsil eden DTO
/// </summary>
    public class MemberReadDTO
    {
        /// <summary>
        /// Eklenmiş olan üyeye ait ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Eklenmiş üyeye ait isim-soyisim
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Eklenmiş üyeye ait e-mail.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Eklenmiş üyeye ait telefon no.
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Eklenmiş üyeye ait üyelik tarihi.
        /// </summary>
        public DateTime MembershipDate { get; set; }
    }
}
