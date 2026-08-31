using Carter;
using Mapster;
using MediatR;

namespace Catalog.API.Features.Product.Update
{
    public record UpdateProductRequest(Guid id, string Name, List<string> Category, string Description, string ImageFile, decimal Price);
    public record UpdateProductResponse(bool isSuccess);
    public class UpdateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products",async(UpdateProductRequest request,ISender sender) =>
            {
                var adaptedRequest = request.Adapt<UpdateProductCommand>();
                var response = await sender.Send(adaptedRequest);
                var adaptedResponse = response.Adapt<UpdateProductResponse>();
                return Results.Ok(adaptedResponse);
            })
                .WithName("UpdateProduct")
                .Produces<UpdateProductResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithDescription("Update a single product")
                .WithSummary("Update product");
        }
    }
}
