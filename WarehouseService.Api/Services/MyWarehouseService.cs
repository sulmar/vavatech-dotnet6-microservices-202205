using Bogus;
using Grpc.Core;

namespace WarehouseService.Api.Services
{
    public class MyWarehouseService : WarehouseService.WarehouseServiceBase
    {
        private readonly ILogger<MyWarehouseService> logger;

        public MyWarehouseService(ILogger<MyWarehouseService> logger)
        {
            this.logger = logger;
        }

        public override Task<GetWarehouseStateResponse> GetWarehouseState(GetWarehouseStateRequest request, ServerCallContext context)
        {
            logger.LogInformation("{WarehouseId} {ProductId}", request.WarehouseId, request.ProductId);

            var response = new GetWarehouseStateResponse
            {
                IsAvailable = true,
                Quantity = 100
            };

            return Task.FromResult(response);
        }

        public override async Task<IncrementWarehouseStateResponse> IncrementWarehouseState(IAsyncStreamReader<IncrementWarehouseStateRequest> requestStream, ServerCallContext context)
        {
            await foreach (var request in requestStream.ReadAllAsync())
            {
                logger.LogInformation("{ProductId} {Quantity}", request.ProductId, request.Quantity);

                //await Task.Delay(500);
            }

            var response = new IncrementWarehouseStateResponse { IsConfirmed = true };

            return response;
        }
        public override async Task SubscribeWarehouseState(SubscribeWarehouseStateRequest request, IServerStreamWriter<GetWarehouseStateResponse> responseStream, ServerCallContext context)
        {            
            var responses = new Faker<GetWarehouseStateResponse>()
                .RuleFor(p => p.Quantity, f => f.Random.Int(0, 100))
                .RuleFor(p => p.IsAvailable, (f, x) => x.Quantity > 0)
                .GenerateForever();

            foreach (var response in responses.Where(r=>r.Quantity > request.LimitQuantity))
            {
                await responseStream.WriteAsync(response);
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }
    }
}
