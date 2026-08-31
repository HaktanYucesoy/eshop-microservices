using Carter;
using Mapster;
using MediatR;

namespace Catalog.API.Features.Product.GetProducts
{
    public record GetProductsRequest(int? pageNumber = 1, int? pageSize = 10);
    public record GetProductsRespose(IEnumerable<Models.Product> Products);
    public class GetProductsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async ([AsParameters] GetProductsRequest request, ISender sender) =>
            {
                var query = request.Adapt<GetProductsQuery>();

                var result = await sender.Send(query);

                var adaptedResult = result.Adapt<GetProductsRespose>();

                return Results.Ok(adaptedResult);
            })
                .WithName("Get Products")
                .Produces<GetProductsRespose>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get Products")
                .WithDescription("Get Products");


        }
    }
}
