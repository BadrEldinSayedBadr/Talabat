using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.APIs.Helper;
using Talabat.Core.Interfaces;
using Talabat.Core.Services;
using Talabat.Repository.Repositories;
using Talabat.Service;

namespace Talabat.APIs.Extensions
{
    public static class ApplicationServicesExtention
    {
        //ExtenTion Method
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {

            //Services


            Services.AddScoped<IOrderService, OrderService>();

            Services.AddScoped<IPaymentService, PaymentService>();

            Services.AddScoped<IUnitOfWork, UnitOfWork>();


            //Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            Services.AddAutoMapper(typeof(MappingProfile));

            Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count > 0)
                                                         .SelectMany(P => P.Value.Errors)
                                                         .Select(P => P.ErrorMessage)
                                                         .ToArray();

                    var validationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(validationErrorResponse);
                };
            });

            Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));


            return Services;
        }
    }
}
