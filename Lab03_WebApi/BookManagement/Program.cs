using BookManagement.Features.Books;
using BookManagement.Persistence;
using BookManagement.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<BookManagementContext>(options => options.UseSqlite("Data Source=bookmanagement.db"));
builder.Services.AddScoped<AddBookHandler>();
builder.Services.AddScoped<GetAllBooksHandler>();
builder.Services.AddScoped<DeleteBookHandler>();
builder.Services.AddScoped<UpdateBookHandler>();
builder.Services.AddValidatorsFromAssemblyContaining<AddBookValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateBookValidator>();

var app = builder.Build();

//Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BookManagementContext>();
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/books", async (AddBookRequest request, AddBookHandler handler) =>
    await handler.Handle(request));
app.MapGet("/books", async ([AsParameters] GetAllBooksRequest request, GetAllBooksHandler handler) =>
    await handler.Handle(request));
app.MapDelete("/books/{id:guid}", async (Guid id, DeleteBookHandler handler) =>
    await handler.Handle(new DeleteBookRequest(id)));
app.MapPatch( "/books/{id:guid}", async (Guid id, UpdateBookHandler handler) =>
    await handler.Handle(new UpdateBookRequest { Id = id }));

app.Run();
