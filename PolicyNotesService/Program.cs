using Microsoft.EntityFrameworkCore;
using PolicyNotesService.Data;
using PolicyNotesService.Dtos;
using PolicyNotesService.Repositories;
using PolicyNotesService.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("PolicyNotesDb"));

builder.Services.AddScoped<IPolicyNoteRepository, PolicyNoteRepository>();
builder.Services.AddScoped<IPolicyNoteService, PolicyNoteService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapPost("/notes", async (IPolicyNoteService svc, CreatePolicyNoteDto dto) =>
{
    var created = await svc.AddNoteAsync(dto);
    return Results.Created($"/notes/{created.Id}", created);
});


app.MapGet("/notes", async (IPolicyNoteService svc) =>
{
    var list = await svc.GetAllAsync();
    return Results.Ok(list);
});


app.MapGet("/notes/{id:int}", async (IPolicyNoteService svc, int id) =>
{
    var note = await svc.GetByIdAsync(id);
    return note is null ? Results.NotFound() : Results.Ok(note);
});

app.Run();

public partial class Program { }
