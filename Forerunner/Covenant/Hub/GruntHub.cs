using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.SignalR.Client;
using System.IO;
using MoonSharp.Interpreter;
using Forerunner;
using Covenant.API.Models;
using Covenant.API;
using Newtonsoft.Json;
using Forerunner.Covenant.Lib;

namespace Covenant.Hub
{
#pragma warning disable CS4014
    public static class GruntHub
    {
        public async static Task<HubConnection> Connect(string CovenantURL, string AuthenticationToken)
        {
            Console.WriteLine("[+] Connecting to GruntHub");
            var _connection = new HubConnectionBuilder()
           .WithUrl(CovenantURL + "/gruntHub", options =>
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
                Console.WriteLine("[+] Connected to GruntHub");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("InnerListener SignalRConnection Exception: " + e.Message + Environment.NewLine + e.StackTrace);
                Environment.Exit(0);
            }

            //ReceiveCommandEvent fires when a new task has finished
            _connection.On<object, object>("ReceiveCommandEvent", (command, taskingEvent) => {
                DoReceiveCommandEvent(command.ToString(), taskingEvent.ToString());
            });
            //ReceiveGrunt fires when requesting a list of grunts
            _connection.On<object, object>("ReceiveGrunt", (guid, name) => {
                Console.WriteLine("[Grunt List]: {0} - {1}", guid, name);
            });


            _connection.Closed += (exception) =>
            {
                Console.WriteLine("[!] Connection Closed");
                return Task.CompletedTask;
            };
            Console.WriteLine("[+] Joining Group");
            await _connection.InvokeAsync("JoinGroup", "*");

            Console.WriteLine("[+] Joined GruntHub group with ID:{0}", _connection.ConnectionId);
            return _connection;   
        }
        //Launch a job without waiting for output
        public static async void LaunchJob(string gruntName, string command)
        {
            Grunt g = Program.covenantConnection.ApiGruntsByNameGet(gruntName);
            await Program.covenantConnection.ApiGruntsByIdInteractPostAsync((int)g.Id, command);
        }
        public static string GruntExec(string gruntName, string command)
        {
            Grunt g = Program.covenantConnection.ApiGruntsByNameGet(gruntName);
            GruntCommand res = Program.covenantConnection.ApiGruntsByIdInteractPost((int)g.Id, command);
            Program.tasks.Add(res);

            int index = Program.tasks.FindIndex(c => c.Id == res.Id);
            DateTime timeoutTime = DateTime.UtcNow.AddMinutes(60);     
            while (String.IsNullOrEmpty(Program.tasks[index].CommandOutput.Output))
            {
                if(DateTime.Compare(DateTime.UtcNow,timeoutTime) > 0)
                {
                    return "[Forerunner] Task Failed: No Response";
                }
            }
            return Program.tasks[index].CommandOutput.Output;             
        }
        public static string GetGrunts()
        {
            Program.gruntHC.InvokeAsync("GetGrunts");
            return "";
        }
        public static void GetCommandOutput()
        {
            var logPath = Directory.GetCurrentDirectory();
            var logFile = File.Create(logPath);
        }
        public static async void DoReceiveCommandEvent(string command, string taskingEvent)
        {
            try
            {
                JObject o = JObject.Parse(taskingEvent);
                JObject o2 = JObject.Parse(command);

                // Because this call is not awaited, execution of the current method continues before the call is completed
                Task.Run(() =>
                {
                    GruntCommand comm = JsonConvert.DeserializeObject<GruntCommand>(command);
                    if (o["messageHeader"].ToString().Contains("completed") && !o["messageBody"].ToString().Contains("Grunts.CommandOutput"))
                    {
                        try
                        {
                            Program.tasks[Program.tasks.FindIndex(c => c.Id == comm.Id)] = comm;
                        }
                        catch
                        {
                            Console.WriteLine("[Forerunner] Returned tasks didn't originate from us. Handling via GlobalFunc");
                            string scriptCode = File.ReadAllText("Forerunner.lua");
                            Script script = Common.GetScript(comm.Grunt, comm);
                            script.DoString(scriptCode);
                            Task.Run(() => { script.Call(script.Globals["OnGlobalOutput"], comm.CommandOutput.Output ?? ""); });
                        }
                    }
                });
            }
            catch (Exception e)
            {
                Console.WriteLine("[Forerunner] An error occured: {0}", e.Message);
            }
        }
    }
}
