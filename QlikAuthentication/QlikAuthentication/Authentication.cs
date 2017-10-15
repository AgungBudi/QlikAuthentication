using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.IO;
using System.Text;

namespace QlikAuthentication.QlikAuthentication
{
    public class Authentication
    {
        private const string CONST_TICKET = "ticket";
        private const string CONST_SESSION = "session";

        private TicketRequest _ticketRequest;
        private TicketResponse _ticketResponse;
        private SessionRequest _sessionRequest;
        private SessionResponse _sessionResponse;

        private string _server;
        private string _virtualProxy;
        private string _port;
        private string _certificate;

        private enum RequestType
        {
            Ticket,
            Session
        }

        public Authentication(string server, string virtualProxy, string port, string certificate, string userDirectory, string user, string sessionId = "")
        {
            _ticketRequest = new TicketRequest { UserDirectory = userDirectory, UserId = user };
            _sessionRequest = new SessionRequest { UserDirectory = userDirectory, UserId = user, SessionId = sessionId };
            _ticketResponse = new TicketResponse();
            _sessionResponse = new SessionResponse();

            _certificate = certificate;
            _server = server;
            _port = port;
            _virtualProxy = virtualProxy;
        }

        public TicketResponse GetTicket()
        {
            TicketResponse result = null;
            Stream stream = Execute(RequestType.Ticket);
            if (stream != null)
            {
                result = JsonConvert.DeserializeObject<TicketResponse>(new StreamReader(stream).ReadToEnd());
            }
            return result;
        }

        public SessionResponse GetSession()
        {
            SessionResponse result = null;
            Stream stream = Execute(RequestType.Session);
            if (stream != null)
            {
                result = JsonConvert.DeserializeObject<SessionResponse>(new StreamReader(stream).ReadToEnd());
            }
            return result;
        }

        private Stream Execute(RequestType reqType)
        {            
            string endPoint = string.Empty;
            string jsonBodyString = string.Empty;

            if (reqType == RequestType.Ticket)
            {
                endPoint = CONST_TICKET;
                jsonBodyString = JsonConvert.SerializeObject(_ticketRequest);
            }
            else if (reqType == RequestType.Session)
            {
                endPoint = CONST_SESSION;
                jsonBodyString = JsonConvert.SerializeObject(_sessionRequest);
            }

            Uri url = new Uri(string.Format("{0}:{1}/qps{2}/{3}", _server, _port, string.IsNullOrEmpty(_virtualProxy) ? string.Empty : "/" + _virtualProxy, endPoint));
            X509Certificate2 certificateMachine = GetCertificate();
            string xrfKey = GenerateXrfKey();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "?Xrfkey=" + xrfKey);
            request.Method = "POST";
            request.Accept = "application/json";
            request.Headers.Add("X-Qlik-Xrfkey", xrfKey);
            request.ClientCertificates.Add(certificateMachine);
            byte[] bodyBytes = Encoding.UTF8.GetBytes(jsonBodyString);

            if (!string.IsNullOrEmpty(jsonBodyString))
            {
                request.ContentType = "application/json";
                request.ContentLength = bodyBytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bodyBytes, 0, bodyBytes.Length);
                requestStream.Close();
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            return response.GetResponseStream();
        }

        private X509Certificate2 GetCertificate()
        {
            X509Certificate2 certificateMachine = null;
            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            certificateMachine = store.Certificates.Cast<X509Certificate2>().FirstOrDefault(c => c.FriendlyName == _certificate);
            store.Close();
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            return certificateMachine;
        }

        private string GenerateXrfKey()
        {
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            var chars = new char[16];
            var rd = new Random();

            for (int i = 0; i < 16; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        private Uri CombineUri(string baseUri, string relativeOrAbsoluteUri)
        {
            if (!baseUri.EndsWith("/"))
                baseUri += "/";

            return new Uri(new Uri(baseUri), relativeOrAbsoluteUri);
        }
    }
}