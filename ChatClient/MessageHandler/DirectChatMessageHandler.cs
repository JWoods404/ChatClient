using ChatProtocol;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ChatClient.MessageHandler
{
    class DirectChatMessageHandler : IMessageHandler
    {
        public void Execute(IMessage message)
        {
            var directChatMessage = message as DirectChatMessage;
            var user = Program.Users.Find(u => u.Id == directChatMessage.ToUserId);
            var username = $"Unbekannt ({directChatMessage.UserId})";
            if (user != null)
                username = user.Username;

            Console.WriteLine($"[Private message from {user} on {DateTime.Now}]: {directChatMessage.Content}");
        }
    }
}
