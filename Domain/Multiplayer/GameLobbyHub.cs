using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Vue.Domain.Multiplayer
{
    public interface IGameLobbyHub 
    {
        Task SendMessage(Guid gameId);
        Task EndTurn(Guid gameId);
    }
    
    public class GameLobbyHub : Hub, IGameLobbyHub
    {
        IHubContext<GameLobbyHub> _hubContext;

        public GameLobbyHub(IHubContext<GameLobbyHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendMessage(Guid gameId)
        {
            await _hubContext.Clients.All.SendAsync("NewGame", gameId.ToString());
        }

        public async Task EndTurn(Guid gameId)
        {
            await _hubContext.Clients.All.SendAsync("EndTurn", gameId.ToString());
        }

        public Task Subscribe(Guid gameId)
        {
            var connectionID = Context.ConnectionId;
            var game = GameManager.GetGame(gameId);
            game.SetConnectionId(connectionID);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var connectionID = Context.ConnectionId;
            var disconnectedGameId = GameManager.Disconnected(connectionID);
            if (disconnectedGameId.HasValue)
            {
                PlayerLeft(disconnectedGameId.Value);
            }
            return base.OnDisconnectedAsync(exception);
        }

        public async Task PlayerLeft(Guid gameId)
        {
            await _hubContext.Clients.All.SendAsync("PlayerLeft", gameId.ToString());
        }

        public async Task SendChatMessage(string user, string message)
        {
            var text = $"{user} : {message}";
            ChatManager.AddMessage(text);
            await Clients.All.SendAsync("ReceiveMessage", text);
        }
    }
}