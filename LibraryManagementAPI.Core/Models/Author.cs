using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LibraryManagementAPI.Core.Models
{/// <summary>
/// Kütüphanede'ki kitaplara ait yazarı temsil eden sınıf yapısı.
/// </summary>
    public class Author
    {
        /// <summary>
        /// Yazara ait ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Yazar ismi
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Yazay soyismi
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// Yazar doğum tarihi
        /// </summary>
        public DateTime BirthDate { get; set; }
        /// <summary>
        /// Yazara ait kitapları belirtir.One to many ilişkisi içerir
        /// </summary>
        /// 
        [JsonIgnore]
        public ICollection<Book> Books { get; set; }

        public static implicit operator Author(List<Author> v)
        {
            throw new NotImplementedException();
        }
    }
}
