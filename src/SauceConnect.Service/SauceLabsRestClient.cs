using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace SauceConnect.Service
{
    public interface ISauceLabsRestClient
    {
        IList<string> GetExistingTunnels();
    }

    public class SauceLabsRestClient : ISauceLabsRestClient
    {
        private readonly string _userName;
        private readonly WebClient _webClient;

        public SauceLabsRestClient(string userName, string accessKey)
        {
            _userName = userName;
            _webClient = new WebClient();
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(userName + ":" + accessKey));
            _webClient.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            _webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
        }

        public IList<string> GetExistingTunnels()
        {
            var url = string.Format("https://saucelabs.com/rest/v1/{0}/tunnels", _userName);
            var tunnelIds = _webClient.DownloadString(url);

            var tunnelList = JsonConvert.DeserializeObject<IList<string>>(tunnelIds);

            return tunnelList;
        }
    }

    public class SauceTunnel
    {
        public SauceTunnel(string tunnelId)
        {
            TunnelId = tunnelId;
        }

        public string TunnelId { get; set; }
    }

    public class SauceTunnelList
    {
        public SauceTunnel Tunnel { get; set; }
    }
}
