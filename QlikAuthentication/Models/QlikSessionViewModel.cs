using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QlikAuthentication.Models
{
    public class QlikSessionViewModel
    {
        [DisplayName("Server")]
        [Required]
        public string Server { get; set; }

        [DisplayName("Virtual Proxy")]
        public string VirtualProxy { get; set; }

        [DisplayName("Port")]
        [Required]
        public string Port { get; set; }

        [DisplayName("Certificate")]
        [Required]
        public string Certificate { get; set; }

        [DisplayName("User Directory")]
        [Required]
        public string UserDirectory { get; set; }

        [DisplayName("User")]
        [Required]
        public string User { get; set; }

        [DisplayName("Session Id")]
        [Required]
        public string SessionId { get; set; }
    }
}