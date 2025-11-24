using PolicyNotesService.Models;

namespace PolicyNotesService.Repositories;

public interface IPolicyNoteRepository
{
    Task<PolicyNote> AddAsync(PolicyNote note, CancellationToken ct = default);
    Task<List<PolicyNote>> GetAllAsync(CancellationToken ct = default);
    Task<PolicyNote?> GetByIdAsync(int id, CancellationToken ct = default);
}
