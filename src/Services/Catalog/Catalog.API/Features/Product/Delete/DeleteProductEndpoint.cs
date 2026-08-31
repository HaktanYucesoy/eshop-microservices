using Carter;
using Mapster;
using MediatR;

namespace Catalog.API.Features.Product.Delete
{

    public record DeleteProductResponse(bool isSuccess);
    public class DeleteProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id}",async(Guid id,ISender sender) =>
            {

                var deleteResponse = await sender.Send(new DeleteProductCommand(id));
                var adaptedDeleteResponse = deleteResponse.Adapt<DeleteProductResponse>();
                return Results.Ok(adaptedDeleteResponse);
            })
                .WithName("DeleteProduct")
                .Produces<DeleteProductResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithDescription("Delete a single product")
                .WithSummary("Delete product");
        }
    }
}
