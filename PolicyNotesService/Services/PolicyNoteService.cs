using PolicyNotesService.Dtos;
using PolicyNotesService.Models;
using PolicyNotesService.Repositories;

namespace PolicyNotesService.Services;

public class PolicyNoteService : IPolicyNoteService
{
    private readonly IPolicyNoteRepository _repo;
    public PolicyNoteService(IPolicyNoteRepository repo) => _repo = repo;

    public async Task<PolicyNoteDto> AddNoteAsync(CreatePolicyNoteDto dto, CancellationToken ct = default)
    {
        var note = new PolicyNote { PolicyNumber = dto.PolicyNumber, Note = dto.Note };
        var added = await _repo.AddAsync(note, ct);
        return new PolicyNoteDto(added.Id, added.PolicyNumber, added.Note);
    }

    public async Task<List<PolicyNoteDto>> GetAllAsync(CancellationToken ct = default)
    {
        var list = await _repo.GetAllAsync(ct);
        return list.Select(n => new PolicyNoteDto(n.Id, n.PolicyNumber, n.Note)).ToList();
    }

    public async Task<PolicyNoteDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var n = await _repo.GetByIdAsync(id, ct);
        return n == null ? null : new PolicyNoteDto(n.Id, n.PolicyNumber, n.Note);
    }
}
