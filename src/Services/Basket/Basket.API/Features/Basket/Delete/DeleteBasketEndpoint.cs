using Carter;
using Mapster;
using MediatR;

namespace Basket.API.Features.Basket.Delete
{
    public record DeleteBasketRespone(bool isSuccess);
    public class DeleteBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket/{username}", async (string username, ISender sender) =>
            {
                var deletedResponse = await sender.Send(new DeleteBasketCommand(username));

                var actualDeletedResponse = deletedResponse.Adapt<DeleteBasketRespone>();

                return Results.Ok(actualDeletedResponse);
            })
                .WithName("DeleteBasket")
                .Produces<DeleteBasketRespone>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Delete Basket")
                .WithDescription("Delete Basket");
        }
    }

    
    
}
