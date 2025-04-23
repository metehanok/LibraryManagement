using FluentValidation;
using LibraryManagementAPI.Service.DTO;

namespace LibraryManagement.WebAPI.Validations
{/// <summary>
 /// Ödünç alınmış kitaba ait DTO yapısını doğrulayan sınıd
 /// </summary>
    public class BorrowedBookUpdateDTOValidator:AbstractValidator<BorrowedBookUpdateDTO>
    {/// <summary>
    /// Doğrulama yapısına ait constructor. Belirli parametrelere göre doğrulama yapar
    /// </summary>
        public BorrowedBookUpdateDTOValidator()
        {
            //RuleFor(x=>x.BookTitle).NotEmpty().MaximumLength(100).WithMessage("Title alanı 100 karakter ya da daha az olmalıdır!");
            RuleFor(x => x.BorrowDate).NotEmpty();
           // RuleFor(x => x.ReturnDate).NotEmpty();//programlanacak devamı için,
        }
    }
}
