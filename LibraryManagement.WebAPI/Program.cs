using FluentValidation;
using FluentValidation.AspNetCore;
using LibraryManagement.WebAPI;
using LibraryManagementAPI.Core.Services;
using LibraryManagementAPI.Data.Repositories;
using LibraryManagementAPI.Service;
using LibraryManagementAPI.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//sql tanýmlamasý
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))
           .EnableSensitiveDataLogging()   // Hata mesajlarýný içerebilir
           .LogTo(Console.WriteLine, LogLevel.Information));  // SQL sorgularýný konsola yazdýrýr

builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonDateTimeConverter());
});

builder.Services.AddLogging();  // ILogger için gerekli yapýlandýrma
builder.Logging.AddConsole();  // Konsola log yazdýrmak için

//dtomapperý tanýmladýk
builder.Services.AddAutoMapper(typeof(DTOMapper));

//fluentValidation'ý eklendi
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
//fluentValidation'ýn varsayýlan davranýþýný etkinleþtirmek için AddFluentValidation kullanýlýr.
builder.Services.AddFluentValidationAutoValidation(options =>
{
    options.DisableDataAnnotationsValidation = true; // FluentValidation'ýn ModelState üzerinden etkisini sýnýrlar
});
builder.Services.AddFluentValidationClientsideAdapters();

//interface ile service üzerinden çalýþma
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBorrowedBookService, BorrowedBookService>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(//c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Title = "Library Management WebAPI",
//        Version = "v1",
//        Description = "Kitap,Yazar ve Üyeleri yöneten ve kullanýma sunan Kütüphane otomasyonu.",
//        TermsOfService = new Uri("https://example.com.terms"),
//        Contact = new OpenApiContact
//        {
//            Name = "Metehan Ok",
//            Email = "metet.ok@outlook.com",
//            Url = new Uri("https://www.linkedin.com/in/metehanok/"),
//        },
//        License = new OpenApiLicense
//        {
//            Name = "API License",
//            Url = new Uri("https://example.com/license"),
//        }
//    });
//    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
//    c.IncludeXmlComments(xmlPath);
//}
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(//c =>
    //{
    //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library Management WebAPI v1");
    //    c.RoutePrefix = string.Empty;
    //}
    );
}
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});
app.UseCors(builder =>
    builder.WithOrigins("https://librarymanagement-59gn.onrender.com")
           .AllowAnyHeader()
          .AllowAnyMethod());
builder.WebHost.UseUrls("http://0.0.0.0:5000");//render için 
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();



app.UseAuthorization();


app.MapControllers();

app.Run();
