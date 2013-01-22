using DotNetOpenAuth.OAuth2;
using Hasse.Web.Models;

namespace Hasse.Web.Controllers
{
    public interface IAuthorizationProvider
    {
        WebServerClient GetOAuthClient();
        string Id { get; }
        string[] Scope { get; }
        AuthModel GetAuthInfo(string accessToken);
    }
}