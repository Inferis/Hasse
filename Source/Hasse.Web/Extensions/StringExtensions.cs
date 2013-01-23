using System.Text;

namespace Hasse.Web.Extensions
{
    public static class StringExtensions
    {
        public static string MD5(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return null;

            var x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = Encoding.UTF8.GetBytes(source);
            bs = x.ComputeHash(bs);
            var s = new StringBuilder();
            foreach (var b in bs) {
                s.Append(b.ToString("x2").ToLower());
            }
            return s.ToString();
        }
    }
}