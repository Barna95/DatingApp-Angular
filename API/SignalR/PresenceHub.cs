using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class PresenceHub : Hub
    {
        //signalR uses websockets protocol instead of http protocol
        [Authorize]
        public override async Task OnConnectedAsync()
        {
            //clients object from SignalR allows us to use methods on our clients
            //Specify to who we send the message example: user, other, etc.
            //name of the method that should run
            //Context is the Hub-s context and gives us access to our user claims principal so we can use their methods too
            //everyone will get this information that the user that logged in is online here
            await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUsername());
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.Others.SendAsync("UserIsOffline", Context.User.GetUsername());

            await base.OnDisconnectedAsync(exception);
        }
    }
}
