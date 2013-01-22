using System;
using System.Web.Mvc;
using Hasse.Web.Models;

namespace Hasse.Web.Authorization
{
    public interface IAuthorizationProvider
    {
        ActionResult StartAuthorization(Func<string, string> callbackGenerator);
        Tuple<string, DateTime> FinishAuthorization();

        string Id { get; }
        AuthModel GetAuthInfo(string accessToken);
    }
}