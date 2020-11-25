using ChatClient.MessageHandler;
using ClientLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ChatClient
{
    class Program
    {
        public static bool IsApplicationExecuting = true;

        public static List<User> Users = new List<User>();

        public static Client Client;

        static void MessageHandle()
        {
            while (true)
            {
                lock (Client.ReceivedMessages)
                {
                    foreach (var message in Client.ReceivedMessages)
                    {
                        MessageHandlerFactory.GetMessageHandler(message.MessageId).Execute(message);
                    }

                    Client.ReceivedMessages.Clear();
                }
            }
        }

        static void StartMessageHandleThread()
        {
            var messageHandleThreadStart = new ThreadStart(MessageHandle);
            var messageHandleThread = new Thread(messageHandleThreadStart);
            messageHandleThread.Start();
        }
       
        static void PrintUsers()
        {
            var s = "";
            foreach (User user in Users)
            {
                s += $"{user.Username} [Id:{user.Id}], ";
            }
            s = s.TrimEnd(' ');
            s = s.TrimEnd(',');
            Console.WriteLine(s);
        }

        static void ReturnId(string input)
        {
            var s = input.Split(' ');
            var idQuery = from user in Users where user.Username == s[1] select user.Id;
            foreach (int id in idQuery)
            {
                Console.WriteLine("Id: " + id);
            }
        }
         

        static void Main()
        {
            Client = new Client("127.0.0.1", 13000);
            StartMessageHandleThread();

            while (IsApplicationExecuting)
            {
                Console.WriteLine("(l) Login with existing account");
                Console.WriteLine("(r) Register a new account");
                var loginRegisterInput = Console.ReadKey();
                if (loginRegisterInput.Key == ConsoleKey.L)
                {
                    Console.Clear();
                    Console.WriteLine("Login:");
                    Console.Write("Username: ");
                    var username = Console.ReadLine();
                    Console.Write("Password: ");
                    var password = Console.ReadLine();
                    Console.WriteLine("Connecting to server.");
                    Client.Connect(username, password);


                    while (Client.IsConnecting)
                    {

                    }
                    if (Client.IsConnected) Client.CheckSavedMessages(username);
                    while (Client.IsConnected)
                    {
                        Console.WriteLine("Nachricht eingeben:");
                        var input = Console.ReadLine();
                        if (input.StartsWith("/user") && input != "/users")
                        ReturnId(input);
                        else if (input.StartsWith("/message"))
                        {
                            Client.SendDirectMessage(input);
                            continue;
                        }
                        else
                        switch (input)
                        {
                            case "/users":
                                PrintUsers();
                                break;
                            case "/disconnect":
                                Client.Disconnect();
                                break;
                            case "/exit":
                                Client.Disconnect();
                                IsApplicationExecuting = false;
                                break;
                            default:
                                Client.SendChatMessage(input);
                                break;
                        }
                    }
                }
                if (loginRegisterInput.Key == ConsoleKey.R)
                {
                    Console.Clear();

                    Console.WriteLine("Register");

                    Console.WriteLine("Username: ");
                    var username = Console.ReadLine();

                    Console.WriteLine("Password: ");
                    var password = Console.ReadLine();

                    Client.Register(username, password);
                }
            }
        }
    }
}