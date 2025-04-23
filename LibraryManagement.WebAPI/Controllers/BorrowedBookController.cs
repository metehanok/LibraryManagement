using AutoMapper;
using FluentValidation;
using LibraryManagementAPI.Core.Models;
using LibraryManagementAPI.Core.Services;
using LibraryManagementAPI.Data.Repositories;
using LibraryManagementAPI.Service;
using LibraryManagementAPI.Service.DTO;
using LibraryManagementAPI.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.WebAPI.Controllers
{/// <summary>
 /// Ödünç alınmış Kitap işlemleriyle ilgili API metodlarını içeren controller sınıfı.
 /// </summary>
    [Route("api/[controller]")]
    [ApiController]




    public class BorrowedBookController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IValidator<BorrowedBookCreateDTO> _validator;
        private readonly IValidator<BorrowedBookUpdateDTO> _updateValidator;
        private readonly IBorrowedBookService _borrowedBookService;
        private readonly LibraryDbContext _dbContext;
        /// <summary>
        /// <see cref="BorrowedBookController"/> sınıfının kurucusu
        /// </summary>
        /// <param name="mapper">DTO'lar ile entity'ler arasındaki dönüşümü yapan Mapper nesnesi.</param>
        /// <param name="validator">Yeni ödünç kitap kaydı için doğrulama işlemi yapan validator.</param>
        /// <param name="updateValidator">Mevcut ödünç kitap kaydını güncelleme için doğrulama işlemi yapan validator.</param>
        /// <param name="borrowedBookService">Ödünç kitap işlemleri için servis sınıfı.</param>

        public BorrowedBookController(IMapper mapper, IValidator<BorrowedBookCreateDTO> validator, IValidator<BorrowedBookUpdateDTO> updateValidator, IBorrowedBookService borrowedBookService, LibraryDbContext dbContext)
        {
            _borrowedBookService = borrowedBookService;
            _mapper = mapper;
            _validator = validator;
            _updateValidator = updateValidator;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Yeni bir ödünç kitap kaydı oluşturur.
        /// </summary>
        /// <param name="borrowedBookCreateDTO">Yeni ödünç kitap kaydını içeren DTO.</param>
        /// <returns>Başarıyla oluşturulursa ödünç kitap döndürülür.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateBorrowedBook([FromBody] BorrowedBookCreateDTO borrowedBookCreateDTO)
        {
            var validationResult = await _validator.ValidateAsync(borrowedBookCreateDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var borrowedBook = _mapper.Map<BorrowedBook>(borrowedBookCreateDTO);
            try
            {
                await _borrowedBookService.AddBorrowedBookAsync(borrowedBook);
                return Ok(borrowedBook);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Belirtilen ID'ye sahip ödünç kitap kaydını getirir.
        /// </summary>
        /// <param name="id">Getirilecek ödünç kitap kaydının ID'si.</param>
        /// <returns>Ödünç kitap kaydı bulunursa döndürülür.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBorrowedBook(int id)
        {
            var borrowedBook = await _borrowedBookService.GetBorrowedBookByIdAsync(id);
            if (borrowedBook == null)
            {
                return NotFound($"ID'si {id} olan ödünç alma kaydı bulunamadı.");
            }

            var borrowedBookDto = _mapper.Map<BorrowedBookReadDTO>(borrowedBook);
            return Ok(borrowedBookDto);
        }

        /// <summary>
        /// Tüm ödünç alınan kitapları getirir.
        /// </summary>
        /// <returns>Tüm ödünç alınan kitapları içeren bir liste döndürür.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAlllBorrowedBook()
        {
            var borrowedBooks = await _borrowedBookService.GetAllBorrowedBookAsync();
            if (borrowedBooks == null || !borrowedBooks.Any())
            {
                return NotFound("Ödünç alınmış kitap bulunamadı");
            }

            var borrowedBookDtos = _mapper.Map<List<BorrowedBookReadDTO>>(borrowedBooks);
            return Ok(borrowedBookDtos);
        }

        /// <summary>
        /// Var olan bir ödünç kitap kaydını günceller.
        /// </summary>
        /// <param name="id">Güncellenecek ödünç kitap kaydının ID'si.</param>
        /// <param name="borrowedBookUpdateDTO">Güncellenmek istenen ödünç kitap bilgilerini içeren DTO.</param>
        /// <returns>Başarıyla güncellenirse 204 No Content döndürülür. Ödünç kitap kaydı bulunamazsa 404 döndürülür.</returns>
        /*[HttpPut("{id}")]
        public async Task<IActionResult> UpdateBorrowedBook(int id, [FromBody] BorrowedBookUpdateDTO borrowedBookUpdateDTO)
         {
             var borrowedBook = ObjectMapper.Mapper.Map<BorrowedBook>(borrowedBookUpdateDTO);
             if (id != borrowedBookUpdateDTO.Id)
             {
                 return BadRequest("ID'ler eşleşmiyor, güncelleme işlemi tamamlanamadı.");
             }
             else
             {
                 return Ok("Güncelleme tamamlandı");
             }

             var validationResult = await _updateValidator.ValidateAsync(borrowedBookUpdateDTO);
             if (!validationResult.IsValid)
             {
                 return BadRequest(validationResult.Errors);
             }

             borrowedBook = _mapper.Map<BorrowedBook>(borrowedBookUpdateDTO);
             var updateDate=borrowedBookUpdateDTO.ReturnDate??DateTime.Now.AddMonths(3);

             var book = await _dbContext.Books.FindAsync(borrowedBook.BookId);
             if(book == null)
             {
                 return NotFound("Kitap Bulunamadı!");
             }
             var result = await _borrowedBookService.UpdateBorrowedBookAsync(id,updateDate,book);
             if (!result)
             {
                 return NotFound($"ID'si {id} olan ödünç alma kaydı bulunamadı.");
             }

             return Ok("Güncelleme Tamamlandı!");
         }*/

         /// <summary>
         /// Belirtilen ID'ye sahip ödünç kitap kaydını siler.
         /// </summary>
         /// <param name="id">Silinecek ödünç kitap kaydının ID'si.</param>
         /// <returns>Başarıyla silinirse 204 No Content döndürülür. Ödünç kitap kaydı bulunamazsa 404 döndürülür.</returns>
         [HttpDelete("{id}")]
         public async Task<IActionResult> DeleteBorrowedBook(int id)
         {
             var result = await _borrowedBookService.DeleteBorrowedBookAsync(id);
             if (!result)
             {
                 return NotFound($"ID'si {id} olan ödünç alma kaydı bulunamadı.");
             }

             return NoContent();
         }
     
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBorrowedBook(int id, [FromBody] BorrowedBook borrowedBook)
        {
            var result = await _borrowedBookService.UpdateBorrowedBookAsync(id, borrowedBook);

            if (!result)
            {
                return NotFound("Güncelleme başarısız. İlgili kayıt bulunamadı.");
            }

            return Ok("Ödünç alınan kitap başarıyla güncellendi.");
        }

    }
}
