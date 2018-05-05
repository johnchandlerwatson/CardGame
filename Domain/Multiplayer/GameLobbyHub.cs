using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Vue.Domain.Multiplayer
{
    public interface IGameLobbyHub 
    {
        Task SendMessage(Guid gameId);
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
    }
}