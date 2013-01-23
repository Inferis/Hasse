using System;
using System.Web.Mvc;
using Hasse.Common;

namespace Hasse.Web.Authorization
{
    public interface IAuthorizationProvider
    {
        ActionResult StartAuthorization(Func<string, string> callbackGenerator);
        Tuple<string, DateTime> FinishAuthorization();

        string Id { get; }
        ExternalAuthenticationInfo GetAuthenticationInfo(string accessToken);
    }
}