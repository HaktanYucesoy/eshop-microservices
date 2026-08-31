using Carter;
using Mapster;
using MediatR;

namespace Catalog.API.Features.Product.GetProductById
{
    public record GetProductByIdResponse(Models.Product Product);
    public class GetProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetProductByIdQuery(id));

                var adaptedResult = result.Adapt<GetProductByIdResponse>();

                return Results.Ok(adaptedResult);
            }).WithName("GetProductById")
                .Produces(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get Product By Id")
                .WithDescription("Get Product By Id");
        }
    }
}
