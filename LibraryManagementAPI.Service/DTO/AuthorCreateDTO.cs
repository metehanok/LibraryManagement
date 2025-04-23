using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAPI.Service.DTO
{/// <summary>
/// Yazar ekleme işlemini soyutlayan DTO sınıfı
/// </summary>
    public class AuthorCreateDTO
    {
        /// <summary>
        /// Eklenecek yazar ismi.
        /// </summary>        
        public string Name { get; set; }
        /// <summary>
        /// Eklenecek yazar soyismi.
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// Eklenecek yazarın doğum tarihi.
        /// </summary>
        public DateTime BirthDate { get; set; }
    }
}
