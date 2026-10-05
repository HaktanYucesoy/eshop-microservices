using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Discount.Grpc.Protos;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService(DiscountContext dbContext, ILogger<DiscountContext> logger) : DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = CheckNullCoupon(request.Coupon.Adapt<Coupon>());
           
            dbContext.Coupons.Add(coupon);
            await dbContext.SaveChangesAsync();

            logger.LogInformation("Discount is successfully created. ProductName : {ProductName}", coupon.ProductName);

            var addedCoupon = coupon.Adapt<CouponModel>();

            return addedCoupon;


        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.Coupons
             .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

            var checkedCoupon = CheckNullCoupon(coupon,StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found.");

            dbContext.Coupons.Remove(checkedCoupon);
            await dbContext.SaveChangesAsync();

            logger.LogInformation("Discount is successfully deleted. ProductName : {ProductName}", request.ProductName);

            return new DeleteDiscountResponse { IsSuccess = true };


        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {

            var coupon = CheckNullCoupon(request.Coupon.Adapt<Coupon>());

            dbContext.Coupons.Update(coupon);
            await dbContext.SaveChangesAsync();

            logger.LogInformation("Discount is successfully updated. ProductName : {ProductName}", coupon.ProductName);

            var updatedCoupon= coupon.Adapt<CouponModel>();

            return updatedCoupon;

        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var allCoupon = await dbContext.Coupons.ToListAsync();
            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

            if (coupon == null)
            {
                return new CouponModel()
                {
                    Id = 0,
                    Amount = 0,
                    Description = "No Coupon",
                    ProductName = ""
                };
            }

            logger.LogInformation($"Discount is retrieved for ProductName:{coupon.ProductName},Amount:{coupon.Amount} and Description:{coupon.Description}");
                
            var adaptedCoupon = coupon.Adapt<CouponModel>();

            return adaptedCoupon;

           
        }


        private Coupon CheckNullCoupon(Coupon? coupon,StatusCode code=StatusCode.InvalidArgument,string message= "Invalid request object.")
        {
            if(coupon is null)
                throw new RpcException(new Status(code,message));

            return coupon;
        }
    }
}
