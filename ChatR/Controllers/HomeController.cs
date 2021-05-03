using ChatR.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ChatR.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHubContext<Hubs.ChatHub> _hubContext;

        public HomeController(ILogger<HomeController> logger, IHubContext<Hubs.ChatHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> GreetAll()
        {
            await _hubContext.Clients.All.SendAsync("GreetAll");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GreetVIPs()
        {
            await _hubContext.Clients.Group(Helpers.ClientHandler.VIP_GROUP).SendAsync("GreetVIPs");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] string message)
        {
            await _hubContext.Clients.Group(Helpers.ClientHandler.VIP_GROUP).SendAsync("BroadcastMessage", "Server", message, true); ;
            return RedirectToAction("Index");
        }
    }
}
