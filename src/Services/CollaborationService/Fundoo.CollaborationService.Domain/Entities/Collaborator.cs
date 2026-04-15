namespace Fundoo.CollaborationService.Domain.Entities;

public class Collaborator
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid NoteId { get; set; }
    public Guid OwnerUserId { get; set; }
    public string CollaboratorEmail { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}