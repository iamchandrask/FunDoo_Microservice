using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundoo.NotesService.Application.DTOs
{
    public record CreateNoteRequest(
            string Title,
            string Description,
            string Color
        );

}
