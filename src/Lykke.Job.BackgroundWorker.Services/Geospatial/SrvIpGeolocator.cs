﻿using System.IO;
using System.Net;
using System.Threading.Tasks;
using Common;
using Lykke.Job.BackgroundWorker.Core.Services.Geospatial;
using Newtonsoft.Json;

namespace Lykke.Job.BackgroundWorker.Services.Geospatial
{
    public class SrvIpGeolocation : ISrvIpGetLocation
    {

        public class TelizeModel : IIpGeolocationData
        {
            [JsonProperty(PropertyName = "country_code")]
            public string CountryCode { get; set; }

            [JsonProperty(PropertyName = "isp")]
            public string Isp { get; set; }

            [JsonProperty(PropertyName = "ip")]
            public string Ip { get; set; }

            [JsonProperty(PropertyName = "city")]
            public string City { get; set; }

            [JsonProperty(PropertyName = "region")]
            public string Region { get; set; }

            [JsonProperty(PropertyName = "longitude")]
            public string Longitude { get; set; }

            [JsonProperty(PropertyName = "latitude")]
            public string Latitude { get; set; }

            public static TelizeModel ParseData(string requestResult)
            {
                var jsonResult = JsonConvert.DeserializeObject<TelizeModel>(requestResult);

                jsonResult.CountryCode = CountryManager.Iso2ToIso3(jsonResult.CountryCode);
                return jsonResult;
            }
        }

        private const string Url = @"http://freegeoip.net/json/";


        public async Task<IIpGeolocationData> GetDataAsync(string ip)
        {
            if (ip.StartsWith("127."))
                ip = "";

            var oWebRequest = (HttpWebRequest)WebRequest.Create(Url + ip);
            oWebRequest.Method = "GET";

            oWebRequest.AllowReadStreamBuffering = true;

            var oWebResponse = await oWebRequest.GetResponseAsync();

            var receiveStream = oWebResponse.GetResponseStream();

            if (receiveStream == null)
                return null;

            var sr = new StreamReader(receiveStream);
            var resultString = await sr.ReadToEndAsync();

            return TelizeModel.ParseData(resultString);
        }




        public IIpGeolocationData GetData(string ip)
        {
            return GetDataAsync(ip).Result;
        }
    }

}