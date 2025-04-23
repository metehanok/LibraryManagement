using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAPI.Service.DTO
{/// <summary>
/// Yazarları güncelleyen DTO yapısı
/// </summary>
    public class AuthorUpdateDTO
    {
        /// <summary>
        /// Güncellenecek yazara ait ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Güncellenecek yazara ait isim.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Güncellenecek yazara ait soyisim.
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// Güncellenecek yazara ait doğum tarihi
        /// </summary>
        public DateTime BirthDate { get; set; }
    }
}
