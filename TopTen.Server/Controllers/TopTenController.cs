using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TopTen.Server.HubConfig;
using TopTen.Server.Manager;

namespace TopTen.Server.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class TopTenController : ControllerBase
    {
        private readonly IHubContext<TopTenHub> _hub;
        private readonly GroupManager _group;

        public TopTenController(IHubContext<TopTenHub> hub, GroupManager group)
        {
            _hub = hub;
            _group = group;
        }
    }
}
