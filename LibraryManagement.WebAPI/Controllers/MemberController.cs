using AutoMapper;
using FluentValidation;
using LibraryManagementAPI.Core.Models;
using LibraryManagementAPI.Core.Services;
using LibraryManagementAPI.Service.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.WebAPI.Controllers
{
    /// <summary>
    /// Üye işlemlerini yönetmek için kullanılan controller sınıfı.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _memberService;
        private readonly IMapper _mapper;
        private readonly IValidator<MemberCreateDTO> _createValidator;
        private readonly IValidator<MemberUpdateDTO> _updateValidator;

        /// <summary>
        /// <see cref="MemberController"/> sınıfının kurucusu.
        /// </summary>
        /// <param name="memberService">Üye servisi.</param>
        /// <param name="mapper">DTO ve Entity dönüştürmeleri için kullanılan mapper.</param>
        /// <param name="createValidator">Yeni üye eklemek için kullanılan doğrulayıcı.</param>
        /// <param name="updateValidator">Üye güncelleme işlemi için kullanılan doğrulayıcı.</param>
        public MemberController(IMemberService memberService, IMapper mapper,
                                IValidator<MemberCreateDTO> createValidator,
                                IValidator<MemberUpdateDTO> updateValidator)
        {
            _memberService = memberService;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        /// <summary>
        /// Yeni bir üye oluşturur.
        /// </summary>
        /// <param name="memberCreateDto">Oluşturulacak üye bilgilerini içeren DTO.</param>
        /// <returns>Başarılı bir şekilde üye oluşturulursa, üye bilgisi ile birlikte 201 Created status kodu döner.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateMember([FromBody] MemberCreateDTO memberCreateDto)
        {
            var validationResult = await _createValidator.ValidateAsync(memberCreateDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var memberEntity = _mapper.Map<Member>(memberCreateDto);
            await _memberService.AddMemberAsync(memberEntity);
            return CreatedAtAction(nameof(GetMemberById), new { id = memberEntity.Id }, memberEntity);
        }

        /// <summary>
        /// Verilen ID'ye sahip üye bilgilerini döner.
        /// </summary>
        /// <param name="id">Üyenin ID'si.</param>
        /// <returns>Üye bulunursa, üye bilgilerini içeren 200 OK döner; bulunamazsa 404 Not Found döner.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMemberById(int id)
        {
            var member = await _memberService.GetMemberByIdAsync(id);
            if (member == null)
            {
                return NotFound($"ID'si {id} olan üye bulunamadı.");
            }

            var memberReadDto = _mapper.Map<MemberReadDTO>(member);
            return Ok(memberReadDto);
        }

        /// <summary>
        /// Sistemdeki tüm üyeleri döner.
        /// </summary>
        /// <returns>Üyelerin listesi döner.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllMembers()
        {
            var members = await _memberService.GetAllMembersAsync();
            var memberReadDtos = _mapper.Map<List<MemberReadDTO>>(members);
            return Ok(memberReadDtos);
        }

        /// <summary>
        /// Verilen ID'ye sahip üyenin bilgilerini günceller.
        /// </summary>
        /// <param name="id">Güncellenmek istenen üyenin ID'si.</param>
        /// <param name="memberUpdateDto">Güncellenmiş üye bilgilerini içeren DTO.</param>
        /// <returns>Başarılı bir şekilde güncellenirse, 204 No Content döner; hata durumunda 400 Bad Request veya 404 Not Found döner.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMember(int id, [FromBody] MemberUpdateDTO memberUpdateDto)
        {
            if (id != memberUpdateDto.Id)
            {
                return BadRequest("ID'ler eşleşmiyor.");
            }

            var validationResult = await _updateValidator.ValidateAsync(memberUpdateDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var memberEntity = _mapper.Map<Member>(memberUpdateDto);
            var updatedMember = await _memberService.UpdateMemberAsync(memberEntity);

            if (updatedMember == null)
            {
                return NotFound($"ID'si {id} olan üye bulunamadı.");
            }

            return NoContent();
        }

        /// <summary>
        /// Verilen ID'ye sahip üyeyi siler.
        /// </summary>
        /// <param name="id">Silinmek istenen üyenin ID'si.</param>
        /// <returns>Başarılı bir şekilde silindiyse, 204 No Content döner; silinemeyen bir üye için 400 Bad Request döner.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var result = await _memberService.DeleteMemberAsync(id);
            if (result != null)
            {
                return BadRequest($"ID'si {id} olan üye silinemiyor.");
            }

            return NoContent();
        }
    }
}
