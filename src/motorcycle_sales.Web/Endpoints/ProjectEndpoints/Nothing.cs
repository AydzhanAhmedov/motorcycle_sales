using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace motorcycle_sales.Web.Endpoints.ProjectEndpoints;

public class Nothing : BaseAsyncEndpoint.WithoutRequest.WithoutResponse
{
    [HttpGet("/Nothing")]
    [SwaggerOperation(
      Summary = "Do nothing",
      Description = "Do just nothing",
      OperationId = "Nothing",
      Tags = new[] { "ProjectEndpoints" })
  ]
    public override Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
