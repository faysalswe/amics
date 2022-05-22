using System;
using System.Text;

namespace Amics.Api.utils
{
    public static class Base64EncodeDecode
    {
        public static string Base64StringEncode(this string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            var encodedString = Convert.ToBase64String(bytes);

            return encodedString;
        }
        public static string Base64StringDecode(this string str )
        {
            var bytes = Convert.FromBase64String(str);

            var decodedString = Encoding.UTF8.GetString(bytes);

            return decodedString;
        }
    }
}
