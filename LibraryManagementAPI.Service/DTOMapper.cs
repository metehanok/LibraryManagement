using AutoMapper;
using LibraryManagementAPI.Core.Models;
using LibraryManagementAPI.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAPI.Service
{
    public class DTOMapper:Profile
    {
        public DTOMapper()
        {
            CreateMap<Book, BookCreateDTO>().ReverseMap();
            CreateMap<Book, BookUpdateDTO>().ReverseMap();
            CreateMap<Book, BookReadDTO>().ReverseMap();

            CreateMap<Author, AuthorCreateDTO>().ReverseMap(); ;
            CreateMap<Author, AuthorUpdateDTO>().ReverseMap(); ;
            CreateMap<Author, AuthorReadDTO>()
             .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.Name} {src.Surname}"))
             .ForMember(dest => dest.BirthDay, opt => opt.MapFrom(src => src.BirthDate)).ReverseMap(); ;

            CreateMap<Member, MemberCreateDTO>().ReverseMap(); ;
            CreateMap<Member, MemberUpdateDTO>().ReverseMap(); ;
            CreateMap<Member, MemberReadDTO>().ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.Name} {src.Surname}"))
             .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email)).ReverseMap(); ;

            CreateMap<BorrowedBook, BorrowedBookCreateDTO>().ReverseMap(); ;
            CreateMap<BorrowedBook, BorrowedBookUpdateDTO>().ReverseMap() ;
            CreateMap<BorrowedBook, BorrowedBookReadDTO>().ReverseMap(); ;
        }
    }
}
