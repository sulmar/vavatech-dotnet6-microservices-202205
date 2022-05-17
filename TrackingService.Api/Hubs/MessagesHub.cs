using Microsoft.AspNetCore.SignalR;
using TrackingService.Domain;

namespace TrackingService.Api.Hubs
{
    public class MessagesHub : Hub
    {
        private readonly ILogger<MessagesHub> logger;

        public MessagesHub(ILogger<MessagesHub> logger)
        {
            this.logger = logger;
        }

        public override Task OnConnectedAsync()
        {
            // zła praktyka
            // logger.LogInformation($"ConnectionId {Context.ConnectionId}");

            // dobra praktyka
            logger.LogInformation("ConnectionId {ConnectionId}", Context.ConnectionId);

            // Context.User.
            // Groups.AddToGroupAsync(Context.ConnectionId, "GroupA");

            return base.OnConnectedAsync();
        }

        //public async Task Join(string room)
        //{
        //    await Groups.AddToGroupAsync(Context.ConnectionId, room);
        //}

        public async Task SendMessage(Message message)
        {
            // await this.Clients.All.SendAsync("YouHaveGotMessage", message);

            await this.Clients.Others.SendAsync("YouHaveGotMessage", message);

           // await Clients.Groups("GroupA").SendAsync("YouHaveGotMessage", message);
        }

        public async Task Ping()
        {
            await Clients.Caller.SendAsync("Pong");
        }
    }
}
