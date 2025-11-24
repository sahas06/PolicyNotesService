using Microsoft.EntityFrameworkCore;
using PolicyNotesService.Data;
using PolicyNotesService.Models;

namespace PolicyNotesService.Repositories;

public class PolicyNoteRepository : IPolicyNoteRepository
{
    private readonly AppDbContext _db;
    public PolicyNoteRepository(AppDbContext db) => _db = db;

    public async Task<PolicyNote> AddAsync(PolicyNote note, CancellationToken ct = default)
    {
        var e = (await _db.PolicyNotes.AddAsync(note, ct)).Entity;
        await _db.SaveChangesAsync(ct);
        return e;
    }

    public async Task<List<PolicyNote>> GetAllAsync(CancellationToken ct = default) =>
        await _db.PolicyNotes.AsNoTracking().ToListAsync(ct);

    public async Task<PolicyNote?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _db.PolicyNotes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
}
