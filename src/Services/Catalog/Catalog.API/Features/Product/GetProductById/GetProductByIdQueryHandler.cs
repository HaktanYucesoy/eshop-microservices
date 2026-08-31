using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Marten;

namespace Catalog.API.Features.Product.GetProductById
{
    public record GetProductByIdQuery(Guid id) : IQuery<GetProductByIdResult>;

    public record GetProductByIdResult(Models.Product Product);
    public class GetProductByIdQueryHandler(IDocumentSession session) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var data = await session.LoadAsync<Models.Product>(request.id);

            if (data is null)
                throw new ProductNotFoundException($"That {request.id} product does not exist");


            return new GetProductByIdResult(data);
        }
    }
}
