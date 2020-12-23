using System.Threading.Tasks;

using BoulderBox.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BoulderBox.Web.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public async Task Send(string message)
        {
            var trimmedMessage = message?.Trim();

            if (trimmedMessage == null
                || trimmedMessage.Length == 0
                || trimmedMessage.Length > 10000)
            {
                return;
            }

            await this.Clients.Others.SendAsync(
                "NewMessage",
                new Message()
                {
                    User = this.Context.User.Identity.Name,
                    Text = trimmedMessage,
                });
        }
    }
}
