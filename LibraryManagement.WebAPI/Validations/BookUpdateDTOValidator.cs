using FluentValidation;
using LibraryManagementAPI.Service.DTO;

namespace LibraryManagement.WebAPI.Validations
{
    /// <summary>
    /// Kitap güncelleme DTO yapısına bağlı doğrulama işlemi yapan validator
    /// </summary>
    public class BookUpdateDTOValidator:AbstractValidator<BookUpdateDTO>
    {
        /// <summary>
        ///  Belirli parametrelere göre güncelleme esnasında doğrulama işlemi yapan validator
        /// </summary>
        public BookUpdateDTOValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
            
        }
    }
}
