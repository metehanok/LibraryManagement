using FluentValidation;
using LibraryManagementAPI.Service.DTO;

namespace LibraryManagement.WebAPI.Validations
{
    /// <summary>
    /// Kitap oluşturma DTO yapısına bağlı doğrulama işlemi yapan validator
    /// </summary>
    public class BookCreateDTOValidator:AbstractValidator<BookCreateDTO>
    {
        /// <summary>
        ///  Belirli parametrelere göre kitap kayıt esnasında doğrulama işlemi yapan validator
        /// </summary>
        public BookCreateDTOValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
            RuleFor(x=>x.PublishedYear).NotEmpty().WithMessage("Published year is required").LessThanOrEqualTo(DateTime.Now); 
        }
    }
}
