using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<VinylDb>(opt => opt.UseInMemoryDatabase("VinylList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/vinyls", async (VinylDb db) =>
    await db.Vinyls.ToListAsync());

app.MapGet("/vinyls/{id}", async (int id, VinylDb db) =>
    await db.Vinyls.FindAsync(id)
        is Vinyl vinyl
            ? Results.Ok(vinyl)
            : Results.NotFound());

app.MapPost("/vinyls", async (Vinyl vinyl, VinylDb db) =>
{
  db.Vinyls.Add(vinyl);
  await db.SaveChangesAsync();

  return Results.Created($"/vinyls/{vinyl.Id}", vinyl);
});

app.MapPut("/vinyls/{id}", async (int id, Vinyl inputVinyl, VinylDb db) =>
{
  var vinyl = await db.Vinyls.FindAsync(id);

  if (vinyl is null) return Results.NotFound();

  vinyl.Label = inputVinyl.Label;
  vinyl.ReleaseDate = inputVinyl.ReleaseDate;

  await db.SaveChangesAsync();

  return Results.NoContent();
});

app.MapDelete("/vinyls/{id}", async (int id, VinylDb db) =>
{
  if (await db.Vinyls.FindAsync(id) is Vinyl vinyl)
  {
    db.Vinyls.Remove(vinyl);
    await db.SaveChangesAsync();
    return Results.NoContent();
  }

  return Results.NotFound();
});

app.Run();
