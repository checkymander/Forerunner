using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using Forerunner.Covenant.Lib;

namespace Forerunner.Covenant.Models
{
    //Shamelessly stolen and modified based on the code in SlackModel.cs
    public class MattermostModel
    {
        private readonly Uri _uri;
        private readonly Encoding _encoding = new UTF8Encoding();

        public MattermostModel(string urlWithAccessToken)
        {
            _uri = new Uri(urlWithAccessToken);
        }
        //Post a message using simple strings
        public void PostMessage(string text, string username = null, string channel = null)
        {
            MattermostPayload payload = new MattermostPayload()
            {
                Channel = channel,
                Username = username,
                Text = text
            };

            PostMessage(payload);
        }
        //Post a message using a Payload object
        public void PostMessage(MattermostPayload payload)
        {
            string payloadJson = JsonConvert.SerializeObject(payload);

            Common.PostCovenant(_uri.ToString(), payloadJson);
        }
    }
    //This class serializes into the Json payload required by Mattermost Incoming WebHooks
    public class MattermostPayload
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
