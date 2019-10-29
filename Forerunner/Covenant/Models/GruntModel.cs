using System;
using System.Collections.Generic;
using System.Text;

namespace Forerunner.Covenant.Models
{
    public class GruntModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string originalServerGuid { get; set; }
        public string guid { get; set; }
        public List<object> children { get; set; }
        public int implantTemplateId { get; set; }
        public object implantTemplate { get; set; }
        public bool validateCert { get; set; }
        public bool useCertPinning { get; set; }
        public string smbPipeName { get; set; }
        public int listenerId { get; set; }
        public object listener { get; set; }
        public string note { get; set; }
        public int delay { get; set; }
        public int jitterPercent { get; set; }
        public int connectAttempts { get; set; }
        public DateTime killDate { get; set; }
        public int dotNetFrameworkVersion { get; set; }
        public int status { get; set; }
        public int integrity { get; set; }
        public string process { get; set; }
        public string userDomainName { get; set; }
        public string userName { get; set; }
        public string ipAddress { get; set; }
        public string hostname { get; set; }
        public string operatingSystem { get; set; }
        public string gruntSharedSecretPassword { get; set; }
        public string gruntRSAPublicKey { get; set; }
        public string gruntNegotiatedSessionKey { get; set; }
        public string gruntChallenge { get; set; }
        public DateTime activationTime { get; set; }
        public DateTime lastCheckIn { get; set; }
        public string powerShellImport { get; set; }
        public List<object> gruntCommands { get; set; }
    }
}
