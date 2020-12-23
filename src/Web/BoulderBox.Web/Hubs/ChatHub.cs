using System.Threading.Tasks;

using BoulderBox.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BoulderBox.Web.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private const int MessageMinLength = 1;
        private const int MessageMaxLength = 10000;

        public async Task Send(string message)
        {
            var trimmedMessage = message?.Trim();

            if (!this.IsValid(trimmedMessage))
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

        private bool IsValid(string trimmedMessage)
        {
            return trimmedMessage != null
                && trimmedMessage.Length > MessageMinLength
                && trimmedMessage.Length < MessageMaxLength;
        }
    }
}
