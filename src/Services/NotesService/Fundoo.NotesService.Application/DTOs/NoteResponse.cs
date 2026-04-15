using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundoo.NotesService.Application.DTOs
{
    public record NoteResponse(
            Guid Id,
            Guid UserId,
            string Title,
            string Description,
            string Color,
            bool IsPinned,
            bool IsArchived,
            bool IsTrashed,
            DateTime CreatedAt,
            DateTime UpdatedAt
        );
}
