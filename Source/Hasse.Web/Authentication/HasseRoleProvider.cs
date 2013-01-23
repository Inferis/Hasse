using System;
using System.Linq;
using System.Web.Security;
using Hasse.Common;
using Hasse.Models;
using Raven.Client.Linq;

namespace Hasse.Web.Authentication
{
    public class HasseRoleProvider : RoleProvider
    {
        public override bool IsUserInRole(string username, string roleName)
        {
            if (roleName != HasseRoles.Admin) 
                return false;

            using (var session = MvcApplication.Store.OpenSession()) {
                var user = session.Load<User>(username);
                return user != null && user.IsAdmin;
            }
        }

        public override string[] GetRolesForUser(string username)
        {
            using (var session = MvcApplication.Store.OpenSession()) {
                var user = session.Load<User>(username);
                if (user != null && user.IsAdmin)
                    return new[] { HasseRoles.Admin };
            }

            return new string[] {};
        }

        public override void CreateRole(string roleName)
        {
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            return false;
        }

        public override bool RoleExists(string roleName)
        {
            return false;
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
        }

        public override string[] GetUsersInRole(string roleName)
        {
            if (roleName != HasseRoles.Admin)
                return new string[] { };

            using (var session = MvcApplication.Store.OpenSession()) {
                // TODO batch
                return session.Query<User>().Where(x => x.IsAdmin).Select(x => x.Id).ToArray();
            }
        }

        public override string[] GetAllRoles()
        {
            return new [] { HasseRoles.Admin };
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            if (roleName != HasseRoles.Admin)
                return new string[] { };

            using (var session = MvcApplication.Store.OpenSession()) {
                // TODO batch
                return session.Query<User>()
                    .Where(x => x.IsAdmin && x.Id.Contains(usernameToMatch))
                    .Select(x => x.Id)
                    .ToArray();
            }
        }

        public override string ApplicationName { get; set; }
    }
}