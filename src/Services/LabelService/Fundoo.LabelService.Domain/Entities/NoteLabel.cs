namespace Fundoo.LabelService.Domain.Entities;

public class NoteLabel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid NoteId { get; set; }
    public Guid LabelId { get; set; }
}