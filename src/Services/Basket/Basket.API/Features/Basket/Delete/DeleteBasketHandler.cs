using Basket.API.Data;
using BuildingBlocks.CQRS;
using FluentValidation;

namespace Basket.API.Features.Basket.Delete
{
    public record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketResult>;

    public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
    {
        public DeleteBasketCommandValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required");
        }
    }
    public record DeleteBasketResult(bool isSuccess);
    public class DeleteBasketHandler(IBasketRepository repository) : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
    {
        public async Task<DeleteBasketResult> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
        {
            var deletedResponse=await repository.DeleteBasket(request.UserName, cancellationToken);

            return new DeleteBasketResult(deletedResponse);
        }
    }
}
