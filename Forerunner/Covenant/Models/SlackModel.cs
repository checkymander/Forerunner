using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using Forerunner.Covenant.Lib;

namespace Forerunner.Covenant.Models
{
    //Shamelessly stolen and modified based on the code from @jogleasonjr https://gist.github.com/jogleasonjr/7121367
    public class SlackModel
    {
        private readonly Uri _uri;
        private readonly Encoding _encoding = new UTF8Encoding();

        public SlackModel(string urlWithAccessToken)
        {
            _uri = new Uri(urlWithAccessToken);
        }
        //Post a message using simple strings
        public void PostMessage(string text, string username = null, string channel = null)
        {
            Payload payload = new Payload()
            {
                Channel = channel,
                Username = username,
                Text = text
            };

            PostMessage(payload);
        }
        //Post a message using a Payload object
        public void PostMessage(Payload payload)
        {
            string payloadJson = JsonConvert.SerializeObject(payload);

            Common.PostCovenant(_uri.ToString(), payloadJson);
        }
    }
    //This class serializes into the Json payload required by Slack Incoming WebHooks
    public class Payload
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
