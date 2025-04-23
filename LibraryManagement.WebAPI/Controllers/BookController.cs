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
    /// Kitap işlemleriyle ilgili API metodlarını içeren controller sınıfı.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBookService _bookService;
        private readonly IValidator<BookCreateDTO> _validator;
        private readonly IValidator<BookUpdateDTO> _updateValidator;
        private readonly IBorrowedBookService _borrowedBookService;

        /// <summary>
        /// <see cref="BookController"/> sınıfının kurucusu.
        /// </summary>
        /// <param name="mapper">DTO ve Entity dönüştürmeleri için kullanılan mapper.</param>
        /// <param name="bookService">Kitap servisi.</param>
        /// <param name="validator">Yeni kitap ekleme için kullanılan doğrulayıcı.</param>
        /// <param name="updateValidator">Kitap güncelleme için kullanılan doğrulayıcı.</param>
        /// <param name="borrowedBookService">Ödünç alınan kitapları yönetmek için kullanılan servis.</param>
        public BookController(IMapper mapper, IBookService bookService,
            IValidator<BookCreateDTO> validator, IValidator<BookUpdateDTO> updateValidator, IBorrowedBookService borrowedBookService)
        {
            _bookService = bookService;
            _mapper = mapper;
            _validator = validator;
            _updateValidator = updateValidator;
            _borrowedBookService = borrowedBookService;
        }

        /// <summary>
        /// Yeni bir kitap oluşturur.
        /// </summary>
        /// <param name="bookCreateDTO">Oluşturulacak kitap bilgilerini içeren DTO.</param>
        /// <returns>Başarılı bir şekilde kitap oluşturulursa, kitap bilgisi ile birlikte 201 Created status kodu döner.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] BookCreateDTO bookCreateDTO)
        {
            var validationResult = await _validator.ValidateAsync(bookCreateDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var bookEntity = _mapper.Map<Book>(bookCreateDTO);
            await _bookService.AddBookAsync(bookEntity);
            return CreatedAtAction(nameof(GetBookById), new { id = bookEntity.Id }, bookCreateDTO);
        }

        /// <summary>
        /// Verilen ID'ye sahip kitabın bilgilerini döner.
        /// </summary>
        /// <param name="id">Kitabın ID'si.</param>
        /// <returns>Kitap bulunursa, kitap bilgilerini içeren 200 OK döner; bulunamazsa 404 Not Found döner.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            var bookReadDto = _mapper.Map<BookReadDTO>(book);
            return Ok(bookReadDto);
        }

        /// <summary>
        /// Sistemdeki tüm kitapları döner.
        /// </summary>
        /// <returns>Kitapların listesi döner. Hiç kitap bulunmazsa 404 Not Found döner.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _bookService.GetAllBooksAsync();
            if (books == null)
            {
                return NotFound("Hiç Kitap Bulunamadı!");
            }
            var bookReadDto = _mapper.Map<List<BookReadDTO>>(books);
            return Ok(bookReadDto);
        }

        /// <summary>
        /// Verilen ID'ye sahip kitabı günceller.
        /// </summary>
        /// <param name="id">Güncellenmek istenen kitabın ID'si.</param>
        /// <param name="bookUpdateDto">Güncellenmiş kitap bilgilerini içeren DTO.</param>
        /// <returns>Başarılı bir şekilde güncellenirse, 204 No Content döner; hata durumunda 400 Bad Request veya 404 Not Found döner.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookUpdateDTO bookUpdateDto)
        {
            if (id != bookUpdateDto.Id)
            {
                return BadRequest("ID'ler eşleşmiyor. Lütfen kontrol ediniz.");
            }

            var book = _mapper.Map<Book>(bookUpdateDto);
            var result = await _bookService.UpdateBookAsync(book);

            if (result == null)
            {
                return NotFound($"ID'si {id} olan kitap bulunamadı.");
            }

            return NoContent(); //başarılı bir güncelleme için 204 No Content döndük
        }

        /// <summary>
        /// Verilen ID'ye sahip kitabı siler.
        /// </summary>
        /// <param name="id">Silinmek istenen kitabın ID'si.</param>
        /// <returns>Başarılı bir şekilde silindiyse, 204 No Content döner; silinemeyen bir kitap için 400 Bad Request döner.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);

            if (book == null)
            {
                return NotFound($"ID'si {id} olan kitap bulunamadı ya da kitap ödünç alınmış.");
            }

            // Kitap ödünç alınmış mı kontrol edilir
            var borrowedBook = await _borrowedBookService.GetBorrowedBookByIdAsync(id);
            if (borrowedBook != null)
            {
                return BadRequest("Kitap ödünç alınmış, silinemez.");
            }

            await _bookService.DeleteBookAsync(id);

            return NoContent();
        }
        /// <summary>
        /// Yazarın ID'sine göre kitapları listeleyen metod.
        /// </summary>
        /// <param name="authorId">Yazarın ID'si.</param>
        /// <returns>Yazarına göre kitapların listesi döner.</returns>
        [HttpGet("by-author/{authorId}")]
        public async Task<IActionResult> GetBooksByAuthor(int authorId)
        {
            var books = await _bookService.GetBookByAuthorAsync(authorId);

            if (books == null || !books.Any())
            {
                return NotFound($"Yazar ID'si {authorId} ile ilgili kitap bulunamadı.");
            }

            var booksDto = _mapper.Map<IEnumerable<BookReadDTO>>(books);
            return Ok(booksDto);
        }

        /// <summary>
        /// Yayımlanma yılına göre kitapları listeleyen metod.
        /// </summary>
        /// <param name="year">Yayımlanma yılı.</param>
        /// <returns>Belirtilen yıl bazında kitapların listesi döner.</returns>
        [HttpGet("by-year/{year}")]
        public async Task<IActionResult> GetBooksByYear(int year)
        {
            var books = await _bookService.GetBookByYearAsync(year);

            if (books == null || !books.Any())
            {
                return NotFound($"Yıl {year} ile ilgili kitap bulunamadı.");
            }

            var booksDto = _mapper.Map<IEnumerable<BookReadDTO>>(books);
            return Ok(booksDto);
        }
        /// <summary>
        /// Belirtilen yıl aralığında yayımlanan kitapları getirir.
        /// </summary>
        /// <param name="startYear">Başlangıç yılı.</param>
        /// <param name="endYear">Bitiş yılı.</param>
        /// <returns>Yıl aralığına göre filtrelenmiş kitap listesi.</returns>
        [HttpGet("by-year-range/{startYear}/{endYear}")]
        public async Task<IActionResult> GetBooksByYearRange(int startYear, int endYear)
        {
            var books = await _bookService.GetBookByYearRangeAsync(startYear, endYear);

            if (books == null || !books.Any())
            {
                return NotFound($"Yıllar {startYear}-{endYear} arasında yayımlanan kitap bulunamadı.");
            }

            var booksDto = _mapper.Map<IEnumerable<BookReadDTO>>(books);
            return Ok(booksDto);
        }


    }
}

