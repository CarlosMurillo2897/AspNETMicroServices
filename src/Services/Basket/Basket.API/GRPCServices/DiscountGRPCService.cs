using Discount.Grpc.Protos;

namespace Basket.API.GRPCServices
{
    public class DiscountGRPCService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoService;
        public DiscountGRPCService(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
        {
            _discountProtoService = discountProtoService ?? throw new ArgumentNullException(nameof(discountProtoService));
        }

        public async Task<CouponModel> GetDiscount(string productName) => await _discountProtoService
            .GetDiscountAsync(new GetDiscountRequest {
                ProductName = productName,
            });
    }
}
