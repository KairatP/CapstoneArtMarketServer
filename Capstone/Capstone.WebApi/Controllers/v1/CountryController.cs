using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Capstone.WebApi.Controllers.v1
{
    [Authorize]
    [ApiController]
    [Route("/api/v{version:ApiVersion}/country")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    public class CountryController : BaseApiController
    {
    }
}
