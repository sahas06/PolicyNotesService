using PolicyNotesService.Dtos;

namespace PolicyNotesService.Services;

public interface IPolicyNoteService
{
    Task<PolicyNoteDto> AddNoteAsync(CreatePolicyNoteDto dto, CancellationToken ct = default);
    Task<List<PolicyNoteDto>> GetAllAsync(CancellationToken ct = default);
    Task<PolicyNoteDto?> GetByIdAsync(int id, CancellationToken ct = default);
}
