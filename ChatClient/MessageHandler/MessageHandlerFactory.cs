namespace ChatClient.MessageHandler
{
    public static class MessageHandlerFactory
    {
        public static IMessageHandler GetMessageHandler(int messageId)
        {
            switch (messageId)
            {
                case 1:
                    return new ChatMessageHandler();
                case 4:
                    return new ConnectResponseMessageHandler();
                case 5:
                    return new UserCountMessageHandler();
                case 7:
                    return new UserListResponseMessageHandler();
                case 9:
                    return new UserRegisterResponseMessageHandler();
                case 10:
                    return new DirectChatMessageHandler();
                case 11:
                    return new CheckPrivateMessagesHandler();
            }

            return null;
        }
    }
}