using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace ProBoost.Helpers
{
    public class Validation
    {
        public static void IsRquired (string propertyName, string value)
        {
            if (string.IsNullOrEmpty(value.Trim()) || string.IsNullOrWhiteSpace(value.Trim()))
                throw new ApplicationException($"{propertyName} est requis");
        }

        public static void IsEmail(string propertyName, string value)
        {
            var email = new MailAddress(value);
        }
    }
}
