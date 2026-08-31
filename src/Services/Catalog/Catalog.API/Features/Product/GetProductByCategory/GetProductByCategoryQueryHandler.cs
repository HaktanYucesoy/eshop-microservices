using BuildingBlocks.CQRS;
using Marten;

namespace Catalog.API.Features.Product.GetProductByCategory
{
    public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;

    public record GetProductByCategoryResult(IEnumerable<Models.Product> Products);
    public class GetProductByCategoryQueryHandler(IDocumentSession session) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
    {
        public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
        {
            var products = await session.Query<Models.Product>()
                .Where(x => x.Category.Contains(request.Category))
                .ToListAsync();

            return new GetProductByCategoryResult(products);
        }
    }
}
