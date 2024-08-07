using Microsoft.EntityFrameworkCore;
using PetsApi.Model;
using static PetsApi.Data.ApDbContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("Petsdb"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/pets", async (AppDbContext db) => await db.Pets.ToListAsync());
app.MapGet("/pets/{id}", async (AppDbContext db, int id) => await db.Pets.FindAsync(id));
app.MapPost("/pets", async (AppDbContext db, Animal pets) =>
{
    object value = await db.Pets.AddAsync(pets);
    await db.SaveChangesAsync();
    return Results.Created($"/pets/{pets.Id}", pets);
});

app.MapPut("/pets/{id}", async (AppDbContext db, Animal updatepets, int id) =>
{
    var pets = await db.Pets.FindAsync(id);
    if (pets is null) return Results.NotFound();
    pets.Nome = updatepets.Nome;
    pets.Idade = updatepets.Idade;
    pets.Cor = updatepets.Cor;
    pets.tipo = updatepets.tipo;
    pets.peso_kg = updatepets.peso_kg;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/pets/{id}", async (AppDbContext db, int id) =>
{
    var pets = await db.Pets.FindAsync(id);
    if (pets is null)
    {
        return Results.NotFound();
    }
    db.Pets.Remove(pets);
    await db.SaveChangesAsync();
    return Results.Ok();
});


app.Run();

