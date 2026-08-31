using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using FluentValidation;
using Marten;

namespace Catalog.API.Features.Product.Update
{

    public record UpdateProductCommand(Guid Id,string Name, List<string> Category, string Description, string ImageFile, decimal Price)
       : ICommand<UpdateProductResult>;

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty().WithMessage("Product ID is required");
            RuleFor(command => command.Name).NotEmpty().WithMessage("Name is required")
                .Length(2, 150).WithMessage("Name must be between 2 and 150 characters");
            RuleFor(command => command.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }
    public record UpdateProductResult(bool isSuccess);

    public class UpdateProductCommandHandler(IDocumentSession session) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var existProduct=await session.LoadAsync<Models.Product>(request.Id,cancellationToken);

            if (existProduct is null)
                throw new ProductNotFoundException($"That {request.Id} product does not exist");

            existProduct.Name = request.Name;
            existProduct.Price= request.Price;
            existProduct.Category= request.Category;
            existProduct.Description=request.Description;
            existProduct.ImageFile= request.ImageFile;

            session.Update(existProduct);
            await session.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true);
        }
    }
}
