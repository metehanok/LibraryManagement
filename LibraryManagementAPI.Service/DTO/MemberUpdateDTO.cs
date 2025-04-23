using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAPI.Service.DTO
{/// <summary>
/// ÜYe güncelleme yapısını temsil eden DTO yapısı
/// </summary>
    public class MemberUpdateDTO
    {
        /// <summary>
        /// Güncellenecek olan üyeye ait ID.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Güncellenecek olan üyeye ait isim.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Güncellenecek olan üyeye ait soyisim.
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// Güncellenecek olan üyeye ait e-mail.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Güncellenecek olan üyeye ait telefon no.
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Güncellenecek olan üyeye ait üyelik tarihi.
        /// </summary>
        public DateTime MembershipDate { get; set; }
    }
}
