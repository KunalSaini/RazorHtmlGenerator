using System;
using System.IO;

namespace HtmlGenerator
{
    public class UserModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsPremiumUser { get; set; }
        public string AddText(string path)
        {
            var t = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplate", "PartialAirline.cshtml"));
            return t;
        }
    }
}
