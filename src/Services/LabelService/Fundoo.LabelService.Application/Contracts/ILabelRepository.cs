using Fundoo.LabelService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundoo.LabelService.Application.Contracts
{
    public interface ILabelRepository
    {
        Task AddLabelAsync(Label label, CancellationToken cancellationToken = default);
        Task<Label?> GetLabelByIdAsync(Guid labelId, CancellationToken cancellationToken = default);
        Task AddNoteLabelAsync(NoteLabel noteLabel, CancellationToken cancellationToken = default);
        Task<NoteLabel?> GetNoteLabelAsync(Guid noteId, Guid labelId, CancellationToken cancellationToken = default);
        Task RemoveNoteLabelAsync(NoteLabel noteLabel, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
