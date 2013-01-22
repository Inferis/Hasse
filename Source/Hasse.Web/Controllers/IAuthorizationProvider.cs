using System;
using System.Web.Mvc;
using Hasse.Web.Models;

namespace Hasse.Web.Controllers
{
    public interface IAuthorizationProvider
    {
        ActionResult StartAuthorization(Func<string, string> callbackGenerator);
        string FinishAuthorization();

        string Id { get; }
        AuthModel GetAuthInfo(string accessToken);
    }
}