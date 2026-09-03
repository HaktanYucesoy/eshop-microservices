using Basket.API.Data;
using Basket.API.Exceptions;
using Basket.API.Models;
using BuildingBlocks.CQRS;

namespace Basket.API.Features.Basket.GetBasket
{

    public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;
    public record GetBasketResult(ShoppingCart shoppingCart);
    public class GetBasketHandler(IBasketRepository repository) : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        public async Task<GetBasketResult> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
            var result = await repository.GetBasket(request.UserName, cancellationToken);

            return result != null ? new GetBasketResult(result) : throw new BasketNotFoundException(request.UserName);
        }
    }
}
