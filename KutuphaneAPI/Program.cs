using KutuphaneCore.Entities;
using KutuphaneDataAccess;
using KutuphaneDataAccess.Repository;
using KutuphaneService.Interfaces;
using KutuphaneService.MapProfile;
using KutuphaneService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new MapProfile()));
builder.Services.AddScoped<IGenericRepository<Author>, Repository<Author>>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
//builder.Services.AddScoped<IBookService, BookService>();
//builder.Services.AddScoped<ICategoryService, CategoryService>();
//builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
