using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAPI.Service.DTO
{/// <summary>
/// Üye ekleme işlemini temsil eden DTO yapısı
/// </summary>
    public class MemberCreateDTO
    {
        /// <summary>
        /// Eklenecek üyeye ait isim.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Eklenecek üyeye ait soyisim
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// Eklenecek üyeye ait e-mail
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Eklenecek üyeye ait telefon no.
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Eklenecek üyeye ait üyelik tarihi
        /// </summary>
        public DateTime MembershipDate { get; set; }//üyelik tarihi
    }
}
