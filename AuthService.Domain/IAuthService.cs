namespace AuthService.Domain
{
    public interface IAuthService
    {
        //User Authorize(string username, string password);

        //Task<User> AuthorizeAsync(string username, string password);

        //bool TryAuthorize(string username, string password, out User user);

        Task<(bool isValid, User? user)> TryAuthorizeAsync(string username, string password);
    }
}