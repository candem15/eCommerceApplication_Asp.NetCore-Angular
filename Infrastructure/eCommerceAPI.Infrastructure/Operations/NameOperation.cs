using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Infrastructure.Operations
{
    public static class NameOperation
    {
        public static string CharRegulatory(string name)
        {
            string newName = name.ToLower()
                .Replace("\"", "")
                .Replace("/", "")
                .Replace("*", "")
                .Replace("-", "")
                .Replace(" ", "")
                .Replace("#", "")
                .Replace("+", "")
                .Replace(",", "")
                .Replace("<", "")
                .Replace(">", "")
                .Replace("?", "")
                .Replace("=", "")
                .Replace("_", "")
                .Replace("|", "")
                .Replace(".", "-")
                .Replace("(", "")
                .Replace(")", "")
                .Replace("[", "")
                .Replace("]", "")
                .Replace("{", "")
                .Replace("}", "")
                .Replace("$", "")
                .Replace("½", "")
                .Replace("£", "")
                .Replace("ü", "u")
                .Replace("ç", "c")
                .Replace("ş", "s")
                .Replace("ğ", "g")
                .Replace("ı", "i")
                .Replace("â", "a")
                .Replace("ê", "e")
                .Replace("î", "i")
                .Replace("æ", "")
                .Replace("@", "")
                .Replace("^", "");

            return $"{newName}-";
        }
    }
}
