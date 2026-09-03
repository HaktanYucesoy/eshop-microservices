using Basket.API.Models;
using Carter;
using Mapster;
using MediatR;

namespace Basket.API.Features.Basket.Store
{
    public record StoreBasketRequest(ShoppingCart shoppingCart);
    public record StoreBasketResponse(string UserName);
    public class StoreBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket", async (StoreBasketRequest request, ISender sender) =>
            {
                var command = request.Adapt<StoreBasketCommand>();

                var storeResponse = await sender.Send(command);

                var actualStoreResponse = storeResponse.Adapt<StoreBasketResponse>();

                return Results.Created($"/basket/{actualStoreResponse.UserName}", actualStoreResponse);
            })
                .WithName("StoreBasket")
                .Produces<StoreBasketResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Store Basket")
                .WithDescription("Store Basket");
        }
    }
}
