using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crud.DTOs;

namespace Crud.Data
{
    public class ChatHub : Hub
    {
        public async Task CreateConnection(DTOs.ConversacionDto.ConnectionChat connection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, connection.Id);
        }

    }
}
