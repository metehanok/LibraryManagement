using FluentValidation;
using LibraryManagementAPI.Service.DTO;

namespace LibraryManagement.WebAPI.Validations
{
    /// <summary>
    ///  Üye oluşturma DTO yapısına bağlı doğrulama işlemi yapan validator
    /// </summary>
    public class MemberCreateDTOValidator:AbstractValidator<MemberCreateDTO>
    {
        /// <summary>
        /// Belirli parametrelere göre üye kayıt esnasında doğrulama işlemi yapan validator
        /// </summary>
        public MemberCreateDTOValidator()
        {
            RuleFor(x=>x.Name).NotEmpty().WithMessage("Name is Required");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Surname is Required");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email is Required");
            RuleFor(x => x.Phone).NotEmpty().MaximumLength(11).WithMessage("Phone is Required");
            RuleFor(x=>x.MembershipDate).NotEmpty().LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Membership Date is Required");
            
        }
    }
}
