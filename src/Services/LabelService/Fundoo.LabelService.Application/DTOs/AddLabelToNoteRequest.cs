using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundoo.LabelService.Application.DTOs
{
    public record AddLabelToNoteRequest(Guid NoteId, Guid LabelId);
}
