using System.Globalization;

namespace Djm.OGame.Web.Api.Services.Utils
{
    public class Utils
    {
        public static string ToStringInvariant(int n)
        {
            return n.ToString("D", CultureInfo.InvariantCulture);
        }
    }
}