using System.ComponentModel;
using Hasse.Web.Resources;

namespace Hasse.Web.Extensions.Attributes
{
    public class LocalizedDisplayNameAttribute : DisplayNameAttribute
    {
        public LocalizedDisplayNameAttribute(string context, string key)
            : base(FormatMessage(context, key))
        {
        }

        private static string FormatMessage(string context, string key)
        {
            return Translations.ResourceManager.GetString(context + "_" + key) ?? key;
        }
    }
}