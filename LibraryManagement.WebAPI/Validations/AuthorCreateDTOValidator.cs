using FluentValidation;
using LibraryManagementAPI.Service.DTO;

namespace LibraryManagement.WebAPI.Validations
{
    /// <summary>
    /// Yazar DTO yapısına bağlı doğrulama işlemi yapan validator yapısı
    /// </summary>
    public class AuthorCreateDTOValidator:AbstractValidator<AuthorCreateDTO>
    {
        /// <summary>
        /// Belirli parametrelere göre kayıt sırasınd doğrulama adımları içeren constructor yapısı
        /// </summary>
        public AuthorCreateDTOValidator()
        {
            RuleFor(x=>x.Name).NotEmpty().WithMessage("Name is Required");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Surname is Required");
            RuleFor(x => x.BirthDate).NotEmpty().WithMessage("BirtDate is Required");
        }
    }
}
