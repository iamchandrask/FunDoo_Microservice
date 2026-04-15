using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundoo.CollaborationService.Application.DTOs
{
    public record RemoveCollaboratorRequest(Guid NoteId, string CollaboratorEmail);
}
