using BuildingBlocks.CQRS;
using Marten;
using Marten.Pagination;

namespace Catalog.API.Features.Product.GetProducts
{
    public record GetProductsQuery(int? pageNumber = 1, int? pageSize = 10) : IQuery<GetProductsResult>;
    public record GetProductsResult(IEnumerable<Catalog.API.Models.Product> Products);


    public class GetProductsQueryHandler
        (IDocumentSession session,
        ILogger<GetProductsQueryHandler> logger) : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {

            var products = await session.Query<Models.Product>().ToPagedListAsync(request.pageNumber ?? 1, request.pageSize ?? 10, cancellationToken);

            return new GetProductsResult(products);
        }
    }
}
