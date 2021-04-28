using ChatR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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

        public async Task<IActionResult> Clear()
        {
            await _hubContext.Clients.All.SendAsync("ClearMessages");
            return RedirectToRoute(new { action = "Index", controller = "Home", area = "" });
        }

        public async Task<IActionResult> GreetVIPs()
        {
            await _hubContext.Clients.Group(Helpers.ClientHandler.VIP_GROUP).SendAsync("HelloFromServer");
            return RedirectToRoute(new { action = "Index", controller = "Home", area = "" });
        }
    }
}
