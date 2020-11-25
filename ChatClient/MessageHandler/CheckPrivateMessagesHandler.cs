using ChatProtocol;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ChatClient.MessageHandler
{
    public class CheckPrivateMessagesHandler : IMessageHandler
    {
        public void Execute(IMessage message)
        {
            var privateMessage = message as CheckPrivateMessages;
            var user = Program.Users.Find(u => u.Username == privateMessage.Username);
            var username = $"Unbekannt ({privateMessage.Username})";
            if (user != null)
                username = user.Username;

            Console.WriteLine($"[Private message from {privateMessage.Username} on {privateMessage.SavedMessage.Now}]: {privateMessage.SavedMessage.Content}");
        }
    }
}
