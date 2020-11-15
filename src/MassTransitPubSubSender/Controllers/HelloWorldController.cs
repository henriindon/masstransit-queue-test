using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using MassTransitPubSubSender;

namespace MassTransitWebAppPubSub.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloWorldController : ControllerBase
    {
        readonly ISendEndpointProvider _sendEndpointProvider;
        public HelloWorldController(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpGet]
        public async Task<ActionResult> Hello(string userName)
        {
            try
            {
                var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:anothertest"));
                if (String.IsNullOrEmpty(userName))
                    await sendEndpoint.Send<HelloWorldModel>(new { UserName = userName, HelloText = "hello world"});
                else
                    await sendEndpoint.Send<HelloWorldModel>(new { UserName = userName, HelloText = "hello " + userName.ToLower() });
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

            return Ok();
        }
    }
}
