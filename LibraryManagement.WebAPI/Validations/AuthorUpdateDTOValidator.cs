using FluentValidation;
using LibraryManagementAPI.Service.DTO;

namespace LibraryManagement.WebAPI.Validations
{/// <summary>
/// Yazar güncelleme DTO yapısına bağlı doğrulama işlemi yapan validator
/// </summary>
    public class AuthorUpdateDTOValidator:AbstractValidator<AuthorUpdateDTO>
    {
        /// <summary>
        /// Belirli parametrelere göre güncelleme esnasında doğrulama işlemi yapan validator
        /// </summary>
        public AuthorUpdateDTOValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is Required");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Surname is Required");
            RuleFor(x => x.BirthDate).NotEmpty().WithMessage("BirtDate is Required");
        }
    }
}
