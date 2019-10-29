using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forerunner.Covenant.Lib;
using Microsoft.AspNetCore.SignalR.Client;
using Covenant.API;
using Covenant.API.Models;
using Microsoft.Rest;
using Covenant.Hub;
using System.Net.Http;
using System.Net;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using System.Security;

namespace Forerunner
{
    class Program
    {
        public static HubConnection gruntHC = null;
        public static HubConnection eventHC = null;
        public static List<GruntCommand> tasks = new List<GruntCommand>();
        public static CovenantAPI covenantConnection = null;
        public static CovenantUser curUser = null;
        static async Task Main(string[] args)
        {
            if (args.Length < 1)
            {
                displayHelp();
                Environment.Exit(0);
            }

            HttpClientHandler clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, errors) =>
                {
                    return true;
                }
            };
            TokenCredentials creds = null;
            string accessToken = "";
            string covenantURL = args[0];
            switch (args.Length)
            {
                case 0:
                    displayHelp();
                    break;
                case 1:
                    covenantURL = args[0];
                    Common.TestConnection(covenantURL);
                    Console.WriteLine("[!] No Access Token Provided. Getting Token.");
                    Console.Write("[!] Enter your Username: ");

                    string username = Console.ReadLine();
                    SecureString password = Common.GetPassword();

                    covenantConnection = new CovenantAPI(new Uri(covenantURL), new BasicAuthenticationCredentials { UserName = "", Password = "" }, clientHandler);
                    CovenantUserLoginResult result = covenantConnection.ApiUsersLoginPost(new CovenantUserLogin { UserName = username, Password = Common.ConvertToUnsecureString(password)});
                    if (result.Success ?? default)
                    {
                        Console.WriteLine("[+] Access Token Received!\r\n[+] Using Token: {0}", result.CovenantToken);
                        accessToken = result.CovenantToken;

                        creds = new TokenCredentials(accessToken);

                        //CovenantAPI _client = new CovenantAPI(
                        covenantConnection = new CovenantAPI(
                            new Uri(covenantURL),
                            creds,
                            clientHandler
                        );
                        curUser = covenantConnection.ApiUsersCurrentGet();
                    }
                    else
                    {
                        Console.WriteLine("[!] Failed to Connect to Covenant");
                    }
                    break;
                case 2:
                    covenantURL = args[0];
                    accessToken = args[1];
                    Common.TestConnection(covenantURL);
                    Console.WriteLine("[+] Access Token Provided. Verifying validity.");
                    creds = new TokenCredentials(accessToken);
                    covenantConnection = new CovenantAPI(
                        new Uri(covenantURL),
                        creds,
                        clientHandler
                    );
                    Console.WriteLine("[+] Access Token is Valid!");
                    break;
                default:
                    displayHelp();
                    break;
            }
            eventHC = await EventHub.Connect(covenantURL, accessToken);
            gruntHC = await GruntHub.Connect(covenantURL, accessToken);
            while (true) { };
        }
        static void displayHelp()
        {
            Console.WriteLine("[!] Missing Required Parameters! \r\n\r\nUsage: Forerunner.exe [CovenantURL]\r\n\r\nForerunner.exe [CovenantURL] [AccessToken] ");
        }
    }
}
