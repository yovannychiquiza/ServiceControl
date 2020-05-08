using System.Text.RegularExpressions;
using Abp.Extensions;

namespace ServiceControl.Validation
{
    public static class ValidationHelper
    {
        public const string EmailRegex = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        public static bool IsEmail(string value)
        {
            if (value.IsNullOrEmpty())
            {
                return false;
            }

            var regex = new Regex(EmailRegex);
            return regex.IsMatch(value);
        }

        public static string ToCapital(string value)
        {
            string newValue = value;
            if (!value.IsNullOrEmpty())
            {
                newValue = value.Substring(0, 1).ToUpper();
                if (value.Length > 1)
                    newValue = newValue + value.Substring(1, value.Length - 1).ToLower();
            }
            return newValue;
        }
        public static string ToLower(string value)
        {
            return !value.IsNullOrEmpty() ? value.ToLower(): value;
        }
        public static string ToUpper(string value)
        {
            return !value.IsNullOrEmpty() ? value.ToUpper() : value;
        }
    }
}
