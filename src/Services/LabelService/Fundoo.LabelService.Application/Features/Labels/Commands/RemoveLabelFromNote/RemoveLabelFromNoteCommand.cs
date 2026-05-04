using Fundoo.LabelService.Application.DTOs;
using MediatR;

namespace Fundoo.LabelService.Application.Features.Labels.Commands.RemoveLabelFromNote;

public record RemoveLabelFromNoteCommand(RemoveLabelFromNoteRequest Request) : IRequest<bool>;