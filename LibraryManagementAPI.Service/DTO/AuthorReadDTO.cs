using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAPI.Service.DTO
{/// <summary>
/// Eklenmiş gösteren DTO yapısı
/// </summary>
    public class AuthorReadDTO
    {
        /// <summary>
        /// Eklenmiş yazara ait ID.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Eklenmiş yazara ait Ad-Soyad.
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Eklenmiş yazarın doğum tarihi
        /// </summary>
        public DateTime BirthDay { get; set; }
    }
}
