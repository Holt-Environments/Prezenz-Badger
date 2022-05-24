/**
 *  HoltEnvironments::ScanToClassConverter
 *  
 *  Developer: Anthony Mesa
 * 
 *  The purpose of this class is to provide an easy-to-edit class that 
 *  takes care of converting the barcode that is scanned into the 
 */

using System.Collections.Generic;
using System.IO;
using System.Net;
using HoltEvironments;

namespace HoltEnvironments
{
    public interface IBadgerTranscoder
    {
        ApiResponse GetBadgeUserData(string scan_code);
    }

    class ScanToClassConverter : IBadgerTranscoder
    {
        public ApiResponse GetBadgeUserData(string barcode_data)
        {
            const string api_url = "https://developer.experientswap.com/APIv1/LeadInfo";

            Dictionary<string, string> args = new Dictionary<string, string>(){
                { "apikey", "CGyb.atRBhkCuNQMOZfGJvd5XHDekiySeLtc4J5Zg5YwP0SWCfD77QYzmcBdfg__" },
                { "actcode", "0596562060941861" },
                { "badgeid", "22558" },
                { "barcode", barcode_data }
            };

            ApiResponse classy_intuiface_trigger_response = MakeRequest("GET", api_url, args);

            return classy_intuiface_trigger_response;
        }

        static public ApiResponse MakeGetRequest(string url, Dictionary<string, string> args)
        {
            string api_url = url;

            if (args.Count > 0)
            {
                api_url += "?";
            }

            foreach (var i in args)
            {
                api_url += i.Key + "=" + i.Value + "&";
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(api_url);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return ApiResponse.FromJson(reader.ReadToEnd());
            }
        }

        static public ApiResponse MakePostRequest(string url, Dictionary<string, string> args)
        {
            return new ApiResponse();
        }

        static public ApiResponse MakeRequest(string method, string url, Dictionary<string, string> args)
        {
            switch (method)
            {
                case "GET":
                    return MakeGetRequest(url, args);
                case "POST":
                    return MakePostRequest(url, args);
                default:
                    return null;
            }
        }
    }
}
