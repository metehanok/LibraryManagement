using LibraryManagementAPI.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAPI.Core.Services
{/// <summary>
/// Üye işlemlerini yöneten IService katmanı
/// </summary>
    public interface IMemberService
    {
        /// <summary>
        /// Tüm üyeleri getiren metod
        /// </summary>
        /// <returns>Liste halinde tüm üyeleri getirir.</returns>
        Task<IEnumerable<Member>> GetAllMembersAsync();
        /// <summary>
        /// ID bazlı üyeleri getiren metod.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Sorgulanan ID'e göre üye getirir.</returns>
        Task<Member> GetMemberByIdAsync(int id);
        /// <summary>
        /// Üye ekleme işlemini gerçekleştiren metod.
        /// </summary>
        /// <param name="member">Eklenecek olan üyeyi belirtir.</param>
        /// <returns>Üye eklenir.</returns>
        Task AddMemberAsync(Member member);
        /// <summary>
        /// Üye güncelleme işlemini gerçekleştiren metod.
        /// </summary>
        /// <param name="member">Güncellenecek olan üyeyi belirtir.</param>
        /// <returns>Üye güncellenir.</returns>
        Task <Member>UpdateMemberAsync(Member member);
        /// <summary>
        /// Üye silme işlemini gerçekleştiren metod.
        /// </summary>
        /// <param name="id">Silinecek olan üyeye ait ID.</param>
        /// <returns>Üye silinir.</returns>
        Task <Member>DeleteMemberAsync(int id);
    }
}
