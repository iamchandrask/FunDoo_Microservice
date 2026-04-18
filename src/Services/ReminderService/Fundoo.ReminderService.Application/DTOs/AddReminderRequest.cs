using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundoo.ReminderService.Application.DTOs
{
    public record AddReminderRequest(Guid NoteId, DateTime ReminderTime, string Message);
}
