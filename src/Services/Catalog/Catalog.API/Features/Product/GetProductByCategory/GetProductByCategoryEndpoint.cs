using Carter;
using Mapster;
using MediatR;

namespace Catalog.API.Features.Product.GetProductByCategory
{

    public record GetProductByCategoryResponse(IEnumerable<Models.Product> Products);
    public class GetProductByCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{category}", async (string category, ISender sender) =>
            {
                var result = await sender.Send(new GetProductByCategoryQuery(category));
                var adaptedResult = result.Adapt<GetProductByCategoryResponse>();
                return Results.Ok(adaptedResult);
            })
                .WithName("GetProductByCategory")
                .Produces(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get Product By Category")
                .WithDescription("Get Product By Category");
        }
    }
}
