using System;
using System.IO;
using System.Net;
using System.Text;
using Forerunner.Covenant.Models;
using Covenant.API.Models;
using System.Security;
using System.Runtime.InteropServices;
using MoonSharp.Interpreter;
using Covenant.Hub;

namespace Forerunner.Covenant.Lib
{
    public class Common
    {
        public static string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null)
                throw new ArgumentNullException("securePassword");

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
        public static SecureString GetPassword()
        {
            SecureString pwd = new SecureString();

            Console.Write("[!] Enter your password: ");
            do
            {
                ConsoleKeyInfo i = Console.ReadKey(true);
                if (i.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (i.Key == ConsoleKey.Backspace)
                {
                    if (pwd.Length > 0)
                    {
                        pwd.RemoveAt(pwd.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else if (i.KeyChar != '\u0000') // KeyChar == '\u0000' if the key pressed does not correspond to a printable character, e.g. F1, Pause-Break, etc
                {
                    pwd.AppendChar(i.KeyChar);
                    Console.Write("*");
                }
            } while (true);
            Console.WriteLine();
            return pwd;
        }
        public static void TestConnection(string covenantURL)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                Console.WriteLine("[+] Testing connection to Covenant Server.");
                WebRequest rq = WebRequest.Create(covenantURL);
                HttpWebResponse res = (HttpWebResponse)rq.GetResponse();
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine("[+] Connection Established!");
                    res.Close();
                }
                else
                {
                    Console.WriteLine("[!] Covenant Unreachable!");
                    res.Close();
                    Environment.Exit(0);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[!] Covenant Unreachable!\r\n{0}", e.Message);
                Environment.Exit(0);
            }
        }
        public static string GetCovenant(string uri, string token = "")
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            WebRequest req = WebRequest.Create(uri);
            req.ContentType = "application/json";
            req.Proxy = new WebProxy("http://127.0.0.1:8080");
            if (token != "")
            {
                req.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + token);
            }
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            StreamReader sread = new StreamReader(res.GetResponseStream());
            string result = sread.ReadToEnd();
            return result;
        }
        public static string PostCovenant(string uri, string body, string token = "")
        {
            try
            {
                //HTTPClient seems to be giving me SSL errors even if I try to bypass them.
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                WebRequest req = WebRequest.Create(uri);
                req.Method = "POST";
                req.ContentType = "application/json";
                req.Proxy = new WebProxy("http://127.0.0.1:8080");
                if (token != "")
                {
                    req.Headers.Add(HttpRequestHeader.Authorization, "Bearer" + token);
                }
                byte[] data = Encoding.ASCII.GetBytes(body);
                req.ContentLength = data.Length;

                Stream rs = req.GetRequestStream();
                rs.Write(data, 0, data.Length);
                rs.Close();
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                StreamReader sread = new StreamReader(res.GetResponseStream());
                string result = sread.ReadToEnd();
                return result;
            }
            catch
            {
                Console.WriteLine("[-] Error connecting to {0}", uri);
                return "";
            }
        }
        public static string sendSlackNotification(string message, string accessToken, string channel)
        {
            var url = String.Format("https://hooks.slack.com/services/{0}", accessToken);
            var client = new SlackModel(url);
            client.PostMessage(username: "ForerunnerBot",
                        text: message,
                        channel: channel);
            return "Sent";
        }
        public static string WriteToLog(string message)
        {
            try
            {
                var logFile = File.Create("Forerunner.log");
                var logWriter = new StreamWriter(logFile);
                logWriter.WriteLine(message);
                logWriter.Dispose();
                return "Wrote to Log";
            }
            catch
            {
                return "Couldn't Write to Log";
            }
        }
        public static Script GetScript(Grunt g)
        {
            Script script = new Script();
            script.Globals["GruntExec"] = (Func<string, string, string>)GruntHub.GruntExec;
            script.Globals["WriteToLog"] = (Func<string,string>)WriteToLog;
            script.Globals["SendSlackNotification"] = (Func<string, string, string, string>)sendSlackNotification;
            if (g != null)
            {
                script.Globals["gruntName"] = g.Name;
                script.Globals["gruntID"] = g.Id;
                script.Globals["gruntGUID"] = g.Guid;
                script.Globals["gruntListener"] = g.Listener;
            }

            script.Globals["GetGrunts"] = (Func<string>)GruntHub.GetGrunts;
            return script;
        }
        public static void WriteDebug(string message)
        {
            Console.WriteLine("[Forerunner] {0}", message);
        }
    }
}
