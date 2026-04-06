namespace LoginWeb.Components
{
    internal class LoginSecurityService
    {
        private bool _IsAuthorized;
        public LoginSecurityService()
        {
            _IsAuthorized = false;
        }
        public bool IsAuthorized { get { return _IsAuthorized; } }
        public void AuthorizeLogin()
        {
            _IsAuthorized = true;
            return;
        }
        public void UnauthorizeLogin() 
        {
            _IsAuthorized = false;
            return;
        }
    }
} 
