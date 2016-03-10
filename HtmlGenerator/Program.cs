using RazorEngine;
using RazorEngine.Templating;
using System;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;

namespace HtmlGenerator
{
    public class Program
    {
        static int Main(string[] args)
        {
            if (AppDomain.CurrentDomain.IsDefaultAppDomain())
            {
                // RazorEngine cannot clean up from the default appdomain...
                Console.WriteLine("Switching to secound AppDomain, for RazorEngine...");
                AppDomainSetup adSetup = new AppDomainSetup();
                adSetup.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                var current = AppDomain.CurrentDomain;
                // You only need to add strongnames when your appdomain is not a full trust environment.
                var strongNames = new StrongName[0];

                var domain = AppDomain.CreateDomain(
                    "MyMainDomain", null,
                    current.SetupInformation, new PermissionSet(PermissionState.Unrestricted),
                    strongNames);
                
                var exitCode = domain.ExecuteAssembly(Assembly.GetExecutingAssembly().Location);
                // RazorEngine will cleanup. 
                AppDomain.Unload(domain);

                return exitCode;
            }
            var model = new UserModel()
            {
                Email = "navdev@test.com",
                Name = "Zoney Zing",
                IsPremiumUser = false
            };
            var indexTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "Templates",
                "Index.cshtml");
            var partialTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
               "Templates",
               "Partial.cshtml");
            var layoutTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
               "Templates",
               "layout.cshtml");
            Engine.Razor.AddTemplate("layout", File.ReadAllText(layoutTemplate));
            Engine.Razor.Compile(File.ReadAllText(partialTemplate), "Partial");
            Engine.Razor.Compile(File.ReadAllText(indexTemplate), "Index");

            var result = Engine.Razor.Run("Index", typeof(UserModel), model);
            Console.WriteLine(result);
            Console.ReadLine();
            return 0;
        }
    }
}
