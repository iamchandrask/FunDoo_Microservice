using Fundoo.LabelService.Application.DTOs;
using MediatR;

namespace Fundoo.LabelService.Application.Features.Labels.Commands.AddLabelToNote;

public record AddLabelToNoteCommand(AddLabelToNoteRequest Request) : IRequest<bool>;