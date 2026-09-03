using Basket.API.Models;
using Carter;
using Mapster;
using MediatR;

namespace Basket.API.Features.Basket.GetBasket
{
    public record GetBasketResponse(ShoppingCart shoppingCart);
    public class GetBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
             app.MapGet("/basket/{username}",async(string username,ISender sender) =>
             {
                 var response = await sender.Send(new GetBasketQuery(username));

                 var actualResponse = response.Adapt<GetBasketResponse>();

                 return Results.Ok(actualResponse);
             })
                .WithName("GetBasket")
                .Produces<GetBasketResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get Basket By Username")
                .WithDescription("Get Basket By Username");
        }
    }
}
