using Basket.API.Data;
using Basket.API.Models;
using BuildingBlocks.CQRS;
using Discount.Grpc.Protos;
using FluentValidation;

namespace Basket.API.Features.Basket.Store
{
    public record StoreBasketCommand(ShoppingCart shoppingCart) : ICommand<StoreBasketResult>;

    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.shoppingCart).NotNull().WithMessage("Cart can not be null");
            RuleFor(x => x.shoppingCart.UserName).NotEmpty().WithMessage("Username is required");
        }
    }
    public record StoreBasketResult(string UserName);

    public class StoreBasketCommandHandler(IBasketRepository repository,DiscountProtoService.DiscountProtoServiceClient discountProto) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand request, CancellationToken cancellationToken)
        {

            await DeductDiscount(request.shoppingCart, cancellationToken);

            var addedResponse = await repository.StoreBasket(request.shoppingCart, cancellationToken);

            return new StoreBasketResult(addedResponse.UserName);
        }

        private async Task DeductDiscount(ShoppingCart shoppingCart,CancellationToken cancellationToken)
        {
            foreach(var item in shoppingCart.Items)
            {
                var coupon=discountProto.GetDiscount(new GetDiscountRequest() { ProductName= item.ProductName });
                item.Price-=coupon.Amount;
            }
        }
    }
}
