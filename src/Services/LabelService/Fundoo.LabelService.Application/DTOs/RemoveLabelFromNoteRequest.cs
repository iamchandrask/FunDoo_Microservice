using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundoo.LabelService.Application.DTOs
{
    public record RemoveLabelFromNoteRequest(Guid NoteId, Guid LabelId);
}
