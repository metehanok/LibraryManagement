using LibraryManagementAPI.Core.Models;
using LibraryManagementAPI.Core.Services;
using LibraryManagementAPI.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAPI.Service.Services
{
    public class MemberService : IMemberService
    {
        private readonly LibraryDbContext _context;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Veritabanı nesne örneği</param>
        public MemberService(LibraryDbContext context)
        {
            _context = context;
        }/// <summary>
        /// Üye ekleme işlemini gerçekleştiren metod.
        /// </summary>
        /// <param name="member">Eklenecek olan üye</param>
        /// <returns>Üye eklenir,kaydedilir</returns>
        public async Task AddMemberAsync(Member member)
        {
            await _context.Members.AddAsync(member);   
            _context.SaveChanges();
        }
        /// <summary>
        /// Üye silme işlemini gerçekleştiren metod.
        /// </summary>
        /// <param name="id">Silinecek üyeye ait ID.</param>
        /// <returns>Üye silinir, kayıt veritabanından kaldırılır.</returns>

        public async Task<Member> DeleteMemberAsync(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member != null)
            {
                _context.Members.Remove(member);

                await _context.SaveChangesAsync();

            }
            return member;

        }
        /// <summary>
        /// Tüm üyeleri listeleyen metod
        /// </summary>
        /// <returns>Kayıtlı üyeleri listeler.</returns>

        public async Task<IEnumerable<Member>> GetAllMembersAsync()
        {
            return await _context.Members.ToListAsync();
        }
        /// <summary>
        /// Kayıtlı üyeleri ID bazlı sorgulama yaparak getiren metod.
        /// </summary>
        /// <param name="id">Kayıtlı üye ID.</param>
        /// <returns>Kayıtlı üye ID'e göre sorgu yapar, ID bazlı sonuç getirir.</returns>

        public async Task<Member> GetMemberByIdAsync(int id)
        {
            return await _context.Members.FindAsync(id);
        }
        /// <summary>
        /// Üye güncelleme işlemini gerçekleştiren metod
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public async Task<Member> UpdateMemberAsync(Member member)
        {
           _context.Members.Update(member);
            await _context.SaveChangesAsync();
            return member;
        }
        /// <summary>
        /// Kayıtlı üyeleri üyelik tarihine göre sorgu yaparak sonucu getiren metod
        /// </summary>
        /// <param name="year">Üyelik tarihi</param>
        /// <returns>Üyeye ait Yıl-tarih bazlı sorgulama yapar.</returns>
        public async Task<IEnumerable<Member>> GetMemberByMemberShipDate(DateTime year)
        {//değiştirilebilir,kullanılmayabilir
            return await _context.Members
                .Where(x=>x.MembershipDate==year)
                .ToListAsync();
        }

     
    }
}
