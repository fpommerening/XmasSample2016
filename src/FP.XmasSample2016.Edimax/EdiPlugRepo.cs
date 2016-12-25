using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Linq;

namespace FP.XmasSample2016.Edimax
{
    public class EdiPlugRepo
    {
        public EdiPlugRepo(Uri targetUrl)
        {
            UserName = "admin";
            Passwort = "1234";
            TargetUrl = targetUrl;
        }

        public EdiPlugRepo(Uri targetUrl, string userName, string passwort)
        {
            UserName = userName;
            Passwort = passwort;
            TargetUrl = targetUrl;
        }

        public string UserName { get; }

        public string Passwort { get; }

        public Uri TargetUrl { get; }

        public async Task<bool> GetPowerState()
        {
            var request = CreateRequest("get", new XElement("Device.System.Power.State"));
            var result = await SendMessage(request);
            var valueElement = result.Descendants().First(x => x.Name.LocalName == "Device.System.Power.State");
            return valueElement.Value == "ON";
        }

        public Task SetPowerState(bool value)
        {
            var request = CreateRequest("setup", new XElement("Device.System.Power.State", value ? "ON" : "OFF"));
            return SendMessage(request);
        }

        public async Task<decimal> NowPowerWatt()
        {
            var request = CreateRequest("get", new XElement("NOW_POWER"));
            var result = await SendMessage(request);
            var valueElement = result.Descendants().First(x => x.Name.LocalName == "Device.System.Power.NowPower");
            return decimal.Parse(valueElement.Value, CultureInfo.InvariantCulture);
        }

        public async Task<decimal> NowPowerAmp()
        {
            var request = CreateRequest("get", new XElement("NOW_POWER"));
            var result = await SendMessage(request);
            var valueElement = result.Descendants().First(x => x.Name.LocalName == "Device.System.Power.NowCurrent");
            return decimal.Parse(valueElement.Value, CultureInfo.InvariantCulture);
        }

        public Task<XDocument> NowPower()
        {
            var request = CreateRequest("get", new XElement("NOW_POWER"));
            return SendMessage(request);
        }

        public Task<XDocument> SystemInfo()
        {
            var request = CreateRequest("get", new XElement("SYSTEM_INFO"));
            return SendMessage(request);
        }

        private XDocument CreateRequest(string command, XElement childElement)
        {
            return new XDocument(new XElement("SMARTPLUG", new XAttribute("id", "edimax"), new XElement("CMD", new XAttribute("id", command), childElement)));
        }

        private async Task<XDocument> SendMessage(XDocument xmlMessageToSend)
        {
            byte[] xmlMessageAsByteArray = System.Text.Encoding.UTF8.GetBytes(xmlMessageToSend.ToString());

            using (var header = new HttpClientHandler())
            {
                header.Credentials = new NetworkCredential(UserName, Passwort);

                using (var client = new HttpClient(header))
                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new ByteArrayContent(xmlMessageAsByteArray));
                    var resul = await client.PostAsync(TargetUrl, content);
                    var str = await resul.Content.ReadAsStringAsync();
                    return XDocument.Parse(str);
                }
            }
        }
    }


}
