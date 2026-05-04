using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundoo.ReminderService.Application.DTOs
{
    public record UpdateReminderRequest(DateTime ReminderTime, string Message);
}
