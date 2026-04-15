using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundoo.UserService.Application.DTOs
{
    public record ResetPasswordRequest(string Token, string NewPassword);
}
