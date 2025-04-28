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
//sql tan�mlamas�
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))
           .EnableSensitiveDataLogging()   // Hata mesajlar�n� i�erebilir
           .LogTo(Console.WriteLine, LogLevel.Information));  // SQL sorgular�n� konsola yazd�r�r

builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonDateTimeConverter());
});

builder.Services.AddLogging();  // ILogger i�in gerekli yap�land�rma
builder.Logging.AddConsole();  // Konsola log yazd�rmak i�in

//dtomapper� tan�mlad�k
builder.Services.AddAutoMapper(typeof(DTOMapper));

//fluentValidation'� eklendi
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
//fluentValidation'�n varsay�lan davran���n� etkinle�tirmek i�in AddFluentValidation kullan�l�r.
builder.Services.AddFluentValidationAutoValidation(options =>
{
    options.DisableDataAnnotationsValidation = true; // FluentValidation'�n ModelState �zerinden etkisini s�n�rlar
});
builder.Services.AddFluentValidationClientsideAdapters();

//interface ile service �zerinden �al��ma
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
//        Description = "Kitap,Yazar ve �yeleri y�neten ve kullan�ma sunan K�t�phane otomasyonu.",
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
builder.WebHost.UseUrls("http://0.0.0.0:5000");//render i�in 
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();



app.UseAuthorization();


app.MapControllers();

app.Run();
