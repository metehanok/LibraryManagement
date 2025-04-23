using FluentValidation;
using LibraryManagementAPI.Service.DTO;

namespace LibraryManagement.WebAPI.Validations
{
    /// <summary>
    /// Ödünç Kitap oluşturma DTO yapısına bağlı doğrulama işlemi yapan validator
    /// </summary>
    public class BorrowedBookCreateDTOValidator:AbstractValidator<BorrowedBookCreateDTO>
    {
        /// <summary>
        /// Belirli parametrelere göre ödünç kitap kayıt esnasında doğrulama işlemi yapan validator
        /// </summary>
        public BorrowedBookCreateDTOValidator()
        {
            RuleFor(x=>x.BookId).NotEmpty();
            RuleFor(x=>x.MemberId).NotEmpty();
            RuleFor(x=>x.BorrowDate).NotEmpty().LessThanOrEqualTo(DateTime.Now);
        }
    }
}
