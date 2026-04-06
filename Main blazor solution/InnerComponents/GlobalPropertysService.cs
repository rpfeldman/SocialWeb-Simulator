namespace LoginWeb.InnerComponents
{
    internal class GlobalPropertysService
    {
        public string ConnectionString { get; private set; }
        public int[] UsernameCharLimits { get; private set;  }
        public int[] PasswordCharLimits { get; private set; }

        public GlobalPropertysService(string conecction, int[] usernameChatLimits, int[] passwordCharLimits)
        {
            ConnectionString = conecction;
            UsernameCharLimits = usernameChatLimits;
            PasswordCharLimits = passwordCharLimits;
        }
    }

    internal enum UserDataBaseImplementation { Test, Dapper, Entity_framework }
}
