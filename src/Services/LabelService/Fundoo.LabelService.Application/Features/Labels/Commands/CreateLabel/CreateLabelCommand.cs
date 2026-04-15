using Fundoo.LabelService.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundoo.LabelService.Application.Features.Labels.Commands.CreateLabel
{
    public record CreateLabelCommand(Guid UserId, CreateLabelRequest Request) : IRequest<Guid>;
}
