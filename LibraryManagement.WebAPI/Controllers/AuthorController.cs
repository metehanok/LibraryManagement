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
    /// Yazar işlemleriyle ilgili API metodlarını içeren controller sınıfı.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    
   
    public class AuthorController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IValidator<AuthorCreateDTO> _validator;
        private readonly IValidator<AuthorUpdateDTO> _updateValidator;
        private readonly IAuthorService _authorService;

        /// <summary>
        /// <see cref="AuthorController"/> sınıfının kurucusu
        /// </summary>
        /// <param name="mapper">DTO'lar ile entity'ler arasındaki dönüşümü yapan Mapper nesnesi.</param>
        /// <param name="validator">Yeni yazar kaydı için doğrulama işlemi yapan validator.</param>
        /// <param name="updateValidator">Mevcut yazar kaydını güncelleme için doğrulama işlemi yapan validator.</param>
        /// <param name="authorService">Yazar işlemleri için servis sınıfı.</param>
        public AuthorController(IMapper mapper, IValidator<AuthorCreateDTO> validator, IValidator<AuthorUpdateDTO> updateValidator, IAuthorService authorService)
        {
            _authorService = authorService;
            _mapper = mapper;
            _validator = validator;
            _updateValidator = updateValidator;
        }

        /// <summary>
        /// Tüm yazarları getirir.
        /// </summary>
        /// <returns>Tüm yazarları içeren bir liste döndürür.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllAuthors([FromQuery] AuthorReadDTO authorReadDTO)
        {
            var authors = await _authorService.GetAllAuthorsAsync();
            if (authors == null || !authors.Any())
            {
                return NotFound("Yazar kaydı bulunamadı!");
            }

            var authorsDto = _mapper.Map<List<AuthorReadDTO>>(authors);
            return Ok(authorsDto);
        }

        /// <summary>
        /// Belirtilen ID'ye sahip yazarı getirir.
        /// </summary>
        /// <param name="id">Getirilecek yazarın ID'si.</param>
        /// <returns>Belirtilen ID'ye sahip yazar bulunursa döndürülür.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorsById(int id)
        {
            var author = await _authorService.GetAuthorByIdAsync(id);
            if (author == null)
            {
                return NotFound($"ID'si {id} olan yazar bulunamadı");
            }

            var authorDto = _mapper.Map<AuthorReadDTO>(author);
            return Ok(authorDto);
        }

        /// <summary>
        /// Yeni bir yazar oluşturur.
        /// </summary>
        /// <param name="authorCreateDTO">Oluşturulacak yazar bilgilerini içeren DTO.</param>
        /// <returns>Yazar başarıyla oluşturulursa 201 Created döndürülür.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorCreateDTO authorCreateDTO)
        {
            var validationResult = await _validator.ValidateAsync(authorCreateDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var authorEntity = _mapper.Map<Author>(authorCreateDTO);
            await _authorService.AddAuthorAsync(authorEntity);
            return CreatedAtAction(nameof(GetAuthorsById), new { id = authorEntity.Id }, authorCreateDTO);
        }

        /// <summary>
        /// Var olan bir yazarı günceller.
        /// </summary>
        /// <param name="id">Güncellenecek yazarın ID'si.</param>
        /// <param name="authorUpdateDTO">Güncellenmek istenen yazar bilgilerini içeren DTO.</param>
        /// <returns>Başarıyla güncellenirse 204 No Content döndürülür. Yazar bulunamazsa 404 döndürülür.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthors(int id, [FromBody] AuthorUpdateDTO authorUpdateDTO)
        {
            if (id != authorUpdateDTO.Id)
            {
                return BadRequest("ID'ler eşleşmiyor!");
            }

            var validationResult = await _updateValidator.ValidateAsync(authorUpdateDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var authorEntity = _mapper.Map<Author>(authorUpdateDTO);
            var updatedAuthor = await _authorService.UpdateAuthorAsync(authorEntity);

            if (updatedAuthor == null)
            {
                return NotFound($"ID'si {id} olan yazar bulunamadı.");
            }

            return NoContent(); // başarılı bir güncelleme için 204 No Content döndürülür
        }

        /// <summary>
        /// Belirtilen ID'ye sahip yazarı siler.
        /// </summary>
        /// <param name="id">Silinecek yazarın ID'si.</param>
        /// <returns>Başarıyla silinirse 200 OK döndürülür. Yazar bulunamazsa 404 döndürülür.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthors(int id)
        {
            var result = await _authorService.DeleteAuthorAsync(id);
            if (result == null)
            {
                return NotFound("Silinecek yazar bulunamadı!");
            }

            return Ok(result);
        }
    }
}

