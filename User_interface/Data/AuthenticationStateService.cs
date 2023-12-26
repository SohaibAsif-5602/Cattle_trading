namespace User_interface.Data
{
    // AuthenticationStateService.cs

    public class AuthenticationStateService
    {
        public bool IsAuthenticated { get; private set; }
        public int UserId { get; private set; }

        public void SetAuthenticationState(bool isAuthenticated, int userId)
        {
            IsAuthenticated = isAuthenticated;
            UserId = userId;
        }
    }

}
