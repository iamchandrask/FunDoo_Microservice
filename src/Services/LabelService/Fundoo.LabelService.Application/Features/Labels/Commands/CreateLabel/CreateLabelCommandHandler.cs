using Fundoo.LabelService.Application.Contracts;
using Fundoo.LabelService.Domain.Entities;
using MediatR;

namespace Fundoo.LabelService.Application.Features.Labels.Commands.CreateLabel;

public class CreateLabelCommandHandler : IRequestHandler<CreateLabelCommand, Guid>
{
    private readonly ILabelRepository _repository;

    public CreateLabelCommandHandler(ILabelRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateLabelCommand command, CancellationToken cancellationToken)
    {
        var label = new Label
        {
            UserId = command.UserId,
            Name = command.Request.Name
        };

        await _repository.AddLabelAsync(label, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return label.Id;
    }
}