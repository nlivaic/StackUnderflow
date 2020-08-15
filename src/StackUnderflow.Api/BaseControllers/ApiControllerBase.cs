using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackUnderflow.Api.Helpers;

namespace StackUnderflow.Api.BaseControllers
{
    public abstract class ApiControllerBase : ControllerBase
    {
        protected new UnprocessableEntityObjectResult UnprocessableEntity()
        {
            var validationProblemDetails = ValidationProblemDetailsFactory.Create(ControllerContext);
            validationProblemDetails.Status = StatusCodes.Status422UnprocessableEntity;
            return UnprocessableEntity(validationProblemDetails);
        }
    }
}
