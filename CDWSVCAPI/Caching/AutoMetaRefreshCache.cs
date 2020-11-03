using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace CDWSVCAPI.Caching
{
    public class AutoMetaRefreshCache : AutoRefreshCache<string, string>
    {
        private static Regex reImageCheck = new Regex(@"rel=""image_src""\s*href=""([^""]*)""", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex ogImageCheck = new Regex(@"property=""og:image""\s*content=""([^""]*)""", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex ogVideoCheck = new Regex(@"property=""og:video""\s*content=""([^""]*)""", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public AutoMetaRefreshCache() : base(interval: TimeSpan.FromMinutes(120)) {}

        protected override string Load(string key)
        {
            using (var wc = new WebClient())
            {
                try
                {
                    HttpWebRequest request = (WebRequest.Create(key) as HttpWebRequest);
                    request.AutomaticDecompression = DecompressionMethods.GZip;
                    HttpWebResponse response = (request.GetResponse() as HttpWebResponse);

                    using (Stream stream = response.GetResponseStream())
                    {
                        int bytesToRead = 8092;
                        byte[] buffer = new byte[bytesToRead];
                        string contents = "";
                        int length = 0;
                        while ((length = stream.Read(buffer, 0, bytesToRead)) > 0)
                        {
                            contents += System.Text.Encoding.UTF8.GetString(buffer, 0, length);
                            Match m = ogVideoCheck.Match(contents);
                            if (!m.Success) m = reImageCheck.Match(contents);
                            if (!m.Success) m = ogImageCheck.Match(contents);
                            if (m.Success)
                                return m.Groups[1].Value.ToString();
                            else if (contents.Contains("</head>"))
                            {
                                // reached end of head-block; no og:image found =[
                                return "no og:image found";
                            }
                        }
                        return "";
                    }
                }
                catch (Exception Ex)
                {
                    logger.Warn(Ex, "Failure reading link header; " + "ERROR!:" + Ex.Message);
                    return "ERROR!:" + Ex.Message;
                }
            }
        }
    }
}