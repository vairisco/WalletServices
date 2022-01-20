using System;
using System.Runtime.Serialization;

namespace WalletService.Application.Models.GoogleModel
{
    public class GRequestModel
    {
        public string path { get; set; }
        public string secret { get; set; }
        public string response { get; set; }
        //public string remoteip { get; set; }

        public GRequestModel(string res, string _secret, string _path)
        {
            response = res;
            //remoteip = remip;
            secret = _secret;
            path = _path;
            if (String.IsNullOrWhiteSpace(secret) || String.IsNullOrWhiteSpace(path))
            {
                //Invoke logger
                throw new Exception("Invalid 'Secret' or 'Path' properties in appsettings.json. Parent: GoogleRecaptchaV3.");
            }
        }
    }

    //Google's response property naming is 
    //embarrassingly inconsistent, that's why we have to 
    //use DataContract and DataMember attributes,
    //so we can bind the class from properties that have 
    //naming where a C# variable by that name would be
    //against the language specifications... (i.e., '-').
    [DataContract]
    public class GResponseModel
    {
        [DataMember]
        public bool success { get; set; }
        [DataMember]
        public string challenge_ts { get; set; }
        [DataMember]
        public string hostname { get; set; }

        //Could create a child object for 
        //error-codes
        [DataMember(Name = "error-codes")]
        public string[] error_codes { get; set; }
    }
}
