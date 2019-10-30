using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using MoonSharp.Interpreter;
using Forerunner;
using System.Linq;
using Covenant.API.Models;
using Covenant.API;
using Forerunner.Covenant.Lib;

#pragma warning disable CS4014
namespace Covenant.Hub
{
    public static class EventHub
    {
        public async static Task<HubConnection> Connect(string CovenantURL, string AuthenticationToken)
        {
            Console.WriteLine("[+] Connecting to EventHub");
            var _connection = new HubConnectionBuilder()
           .WithUrl(CovenantURL + "/eventHub", options =>
           {
               options.AccessTokenProvider = () => { return Task.FromResult(AuthenticationToken); };
               options.HttpMessageHandlerFactory = inner =>
               {
                   var HttpClientHandler = (HttpClientHandler)inner;
                   HttpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                   return HttpClientHandler;
               };             
           }).Build();

            _connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await _connection.StartAsync();
            };
            try
            {
                await _connection.StartAsync();
                Console.WriteLine("[+] Connected to EventHub");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("[Forerunner] InnerListener SignalRConnection Exception: " + e.Message + Environment.NewLine + e.StackTrace);
                Environment.Exit(0);
            }
            //Receive Event fires when a new grunt has been activated.
            _connection.On<object>("ReceiveEvent", param => {
                DoReceiveEvent(param.ToString());
            });
            _connection.Closed += (exception) =>
            {
                Console.WriteLine("[!] Connection Closed");
                return Task.CompletedTask;
            };
            Console.WriteLine("[+] Joining Group");
            await _connection.InvokeAsync("JoinGroup", "*");

            Console.WriteLine("[+] Joined EventHub group with ID:{0}", _connection.ConnectionId);

            return _connection;
        }
        public static async void DoReceiveEvent(string message)
        {
            try
            {
                //Parse GruntID From Message
                JObject o = JObject.Parse(message);
                string gruntRegex = "Grunt: [0-9a-fA-F]{10}";
                RegexOptions options = RegexOptions.Multiline;
                string gruntName = Regex.Match(o["messageHeader"].ToString(), gruntRegex, options).ToString().Replace("Grunt: ", "");
                Grunt grunt = Program.covenantConnection.ApiGruntsByNameGet(gruntName);
                //Execute Function
                string scriptCode = File.ReadAllText("Forerunner.lua");
                Script script = Common.GetScript(grunt);
                script.DoString(scriptCode);
                //Because this call is not awaited, execution of the current method continues before the call is completed
                Task.Run(() => { script.Call(script.Globals["OnGruntInitial"], gruntName); });
            }
            catch (Exception e)
            {
                Console.WriteLine("[Forerunner] An Error Occured: {0}", e.Message);
            }
        }
    }
}
