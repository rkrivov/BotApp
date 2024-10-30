using System.Text;

namespace BotApp.Telegram.Api.Extensions
{
    internal static class StringExtension
    {
        internal static string ToBase64String(this string str)
        {
            var bytes = Encoding.Default.GetBytes(str);
            return Convert.ToBase64String(bytes);
        }
        internal static string ToHexString(this string str)
        {
            var bytes = Encoding.Default.GetBytes(str);
            //var compressed = Global.Compress(bytes);
            return Convert.ToHexString(bytes);
        }

        internal static string ToHashString(this string str)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.Default.GetBytes(str);
                //byte[] compressed = Global.Compress(inputBytes);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes); 
            }
        }

        internal static string FromBase64(this string str)
        {
            var bytes = Convert.FromBase64String(str);
            return Encoding.Default.GetString(bytes);
        }
    }
}
