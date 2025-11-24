using Microsoft.EntityFrameworkCore;
using PolicyNotesService.Models;
using System.Collections.Generic;

namespace PolicyNotesService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

    public DbSet<PolicyNote> PolicyNotes { get; set; } = null!;
}
