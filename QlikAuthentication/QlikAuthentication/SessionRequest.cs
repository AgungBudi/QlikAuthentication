using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QlikAuthentication.QlikAuthentication
{
    [Serializable]
    public class SessionRequest
    {
        public string UserDirectory {get ; set;}
        public string UserId { get; set; }
        public List<Dictionary<string, string>> Attributes { get; set; }
        public string SessionId { get; set; }
    }
}