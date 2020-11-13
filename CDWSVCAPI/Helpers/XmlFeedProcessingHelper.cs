using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using TidyManaged;
using CDWSVCAPI.Caching;
using CDWSVCAPI.Models;
using CDWRepository;

namespace CDWSVCAPI.Helpers
{
    public class XmlFeedProcessingHelper
    {
        private static AutoRefreshCache<Tuple<string, string>, List<FeedImage>> _imgCache;
        private static AutoRefreshCache<string, string> _metaCache;
        private static IContentCuration _curation;
        //private static Logger logger = LogManager.GetCurrentClassLogger();
        
        private static Regex flickrPhotos = new Regex(@"photos/[^/]+/[0-9]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private XmlNamespaceManager nsmgr = new XmlNamespaceManager(new NameTable());
        private string cacheSuffix;

        public XmlFeedProcessingHelper(AutoRefreshCache<Tuple<string, string>, List<FeedImage>> imgCache, AutoRefreshCache<string, string> metaCache)
        {
            _imgCache = imgCache;
            _metaCache = metaCache;
        }

        public XPathNavigator GetContent(string content, string type)
        {
            XmlDocument xDoc = new XmlDocument();
            if (!string.IsNullOrEmpty(content) && content.TrimStart().StartsWith("<"))
            {
                content = "<root>" + content.Replace("&", "&amp;") + "</root>";
                try { 
                    xDoc.LoadXml(content);
                }
                catch (XmlException ex)
                {
                    //logger.Warn(ex, "Malformed RSS Content block");
                }
            }
                
            return xDoc.CreateNavigator();

        }

        public XPathNodeIterator GetContentXpath(string content, string xpath)
        {
            XmlDocument xDoc = new XmlDocument();
            if (!string.IsNullOrEmpty(content) && content.TrimStart().StartsWith("<"))
            {
                content = "<root>" + content.Replace("&", "&amp;") + "</root>";
                try
                {
                    xDoc.LoadXml(content);
                }
                catch (XmlException ex)
                {
                    //logger.Warn(ex, "Malformed RSS Content block");
                }
            }
            var nav = xDoc.CreateNavigator();
            if (!string.IsNullOrEmpty(xpath))
            {
                return nav.Select(xpath);
            }
            return nav.Select("root");
        }

        public XPathNavigator GetAsXHTML(string link)
        {
            var xdoc = new XmlDocument();
            using (var wc = new WebClient())
            {
                try
                {
                    //string doc = WebCache.Get("TIDY:" + link);
                    //if (doc == null)
                    //{
                        string pageCode = wc.DownloadString(link);
                        var tidyDoc = Document.FromString(pageCode);
                        tidyDoc.OutputXhtml = true;
                        tidyDoc.CleanAndRepair();
                        var doc = tidyDoc.Save();
                        tidyDoc.Dispose();
                    //    WebCache.Set("TIDY:" + link, doc);
                    //}
                    xdoc.LoadXml(doc);
                    
                }
                catch (Exception ex)
                {
                    xdoc.LoadXml("<error>" + ex.Message + "</error>");
                    //logger.Warn(ex, "HTML Tidy failed for " + link);
                }
            }
            return xdoc.CreateNavigator();
        }

        public string Replace(string content, string set, string rep)
        {
            return content.Replace(set, rep);
        }

        public string Escape(XPathNavigator content)
        {
            return System.Security.SecurityElement.Escape(content.OuterXml);
        }

        public string CheckLink(string link)
        {
            if (string.IsNullOrEmpty(link)) return HttpStatusCode.LengthRequired.ToString();
            Uri url;
            link = HttpUtility.HtmlDecode(link);
            if (!Uri.TryCreate(link, UriKind.Absolute, out url))
            {
                return link;
            }
            using (var client = new StatusClient())
            {
                client.HeadOnly = true;
                try {
                    client.DownloadString(link);
                } catch (WebException ex) {
                    if(ex.Response != null) ex.Response.Dispose();
                    return "Error";
                }
                if (client.Moved) // imgur moved
                    return "Moved";
                var type = client.ResponseHeaders.Get("content-type");
                if (type.ToLowerInvariant().StartsWith("image/") || type.ToLowerInvariant() == "video/mp4")
                {
                    cacheSuffix = url.Scheme + Uri.SchemeDelimiter + url.Host;
                    var noShows = _imgCache.Get(Tuple.Create("NoShowImages", cacheSuffix));
                    if (noShows.Any(fi => fi.Url == link)) return "NOSHOW";
                    _imgCache.Get(Tuple.Create("NoShowMsgs", cacheSuffix));
                    var noMsgs = _imgCache.Get(Tuple.Create("NoShowMsgs", cacheSuffix));
                    if (noMsgs.Any(fi => fi.Url == link)) return "NOMSG";
                    //otherwise
                    return "OK";
                }                    
            }
            return "HTML";
        }
        
        public string MetaOgImageExtract(string url)
        {
            if (string.IsNullOrEmpty(url)) return url;
            
            var res = _metaCache.Get(url);
            return res ?? "NOSHOW";
        }

        public string FlickrOrigExtract(string url, string suffix = "o")
        {
            if (string.IsNullOrEmpty(url)) return "";
            if (!flickrPhotos.IsMatch(url)) return url;
            string res = "";
            //string doc = WebCache.Get("TIDY:" + url + suffix);
            //if (string.IsNullOrEmpty(doc))
            //{
            var path = flickrPhotos.Match(url).Value;
            url = "https://www.flickr.com/" + path + "/sizes/" + suffix;
            string pageCode = "";
            string doc = "";
            using (var wc = new WebClient())
            {
                try
                {
                    pageCode = wc.DownloadString(url);
                    var tidyDoc = Document.FromString(pageCode);
                    tidyDoc.OutputXhtml = true;
                    tidyDoc.PreserveEntities = false;
                    tidyDoc.RemoveComments = true;
                    tidyDoc.FixAttributeUris = true;
                    tidyDoc.EnsureLiteralAttributes = true;
                    tidyDoc.DropProprietaryAttributes = true;
                    //tidyDoc.DropEmptyElements = true;
                    tidyDoc.QuoteAmpersands = true;
                    tidyDoc.QuoteNonBreakingSpaces = false;
                    tidyDoc.IndentBlockElements = AutoBool.Yes;
                    //tidyDoc.IndentWithTabs = false;
                    tidyDoc.Markup = true;
                    tidyDoc.WrapAttributeValues = true;
                    tidyDoc.AttributeSortType = SortStrategy.Alpha;
                    tidyDoc.CleanAndRepair();
                    doc = tidyDoc.Save();
                    tidyDoc.Dispose();
                    //WebCache.Set("TIDY:" + url, doc);
                }
                catch (Exception Ex)
                {
                    res = "ERROR!:" + Ex.Message;
                    //logger.Warn(Ex, "Failure reading flickr html; " + res);
                }
                //}
            }
            var xdoc = new XmlDocument();
            try
            {
                xdoc.LoadXml(doc);
                if (!nsmgr.HasNamespace("h")) nsmgr.AddNamespace("h", "http://www.w3.org/1999/xhtml");
                var urlNode = xdoc.SelectSingleNode("//h:div[@id='allsizes-photo']/h:img/@src", nsmgr);
                if (urlNode != null)
                {
                    url = urlNode.Value;
                }
            }
            catch (Exception ex)
            {
                //logger.DebugException("Xml Parse after Tidy failed", ex);
            }
            
            var sz = url.Substring(url.Length - 5, 1);
            if (sz != suffix && suffix != "h")
                url = FlickrOrigExtract(url, suffix == "o" ? "k" : "h");
            return url;
        }

        public string ExtractMessage(string title)
        {
            string _fileNameRegex = @"([\[\(]OC[\]\)]\s*)?([\w\s\,\-\.']+)";
            Regex rx = new Regex(_fileNameRegex, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.CultureInvariant);
            string message = title;
            Match matches = rx.Match(title);

            if (matches.Success)
            {
                if (matches.Groups.Cast<Group>().Count() > 2)
                {
                    message = (from g in matches.Groups.Cast<Group>() where !g.Equals(matches.Groups.Cast<Group>().Last()) from Capture c in g.Captures select c).Aggregate("", (current, c) => current + (" " + c.Value));
                    message = Regex.Replace(message, "[\\[\\(]OC[\\]\\)]\\s*", "", RegexOptions.IgnoreCase);
                    message = message.Trim();
                }
                else
                {
                    message = matches.Value;
                }
            }
            return message;
        }
    }



    public class AtomProcessing : XmlFeedProcessingHelper
    {
        public AtomProcessing(AutoRefreshCache<Tuple<string, string>, List<FeedImage>> imgCache, AutoRefreshCache<string, string> metaCache) : base(imgCache, metaCache)
        {
        }
    }

    public class RSSProcessing : XmlFeedProcessingHelper
    {
        public RSSProcessing(AutoRefreshCache<Tuple<string, string>, List<FeedImage>> imgCache, AutoRefreshCache<string, string> metaCache) : base(imgCache, metaCache)
        {
        }
    }

    public class StatusClient : WebClient
    {
        private HttpWebResponse _resp = null;

        private HttpStatusCode _status = HttpStatusCode.OK;

        public bool Moved = false;

        public bool HeadOnly { get; set; }

        protected override WebResponse GetWebResponse(WebRequest req)
        {
            try
            {
                _resp = (HttpWebResponse)base.GetWebResponse(req);

                Moved = _resp.StatusDescription.StartsWith("Moved");

                while (_resp.StatusCode == HttpStatusCode.Found)
                {
                    _resp.Close();
                    var location = _resp.Headers["Location"];
                    if (Uri.IsWellFormedUriString(location, UriKind.Relative))
                    {
                        location = $"{(req.AuthenticationLevel == System.Net.Security.AuthenticationLevel.None ? "http" : "https")}://{req.Headers["HOST"]}{location}";
                    }
                    if (Uri.IsWellFormedUriString(location, UriKind.Absolute)) { 
                        req = (HttpWebRequest)WebRequest.Create(location);
                        _resp = (HttpWebResponse)req.GetResponse();
                    }
                    else
                    {
                        _status = HttpStatusCode.NotFound;
                        return _resp;
                    }
                }

                _status = (_resp as HttpWebResponse).StatusCode;

            }
            catch (WebException) {
                _status = HttpStatusCode.NotFound;
            }
            return _resp;
        }

        public HttpStatusCode StatusCode
        {
            get { return _status; }
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest req = base.GetWebRequest(address);
            if (HeadOnly && req.Method == "GET")
            {
                req.Method = "HEAD";
            }
            if (req is HttpWebRequest)
            {
                ((HttpWebRequest)req).AllowAutoRedirect = false;
            }
            return req;
        }
    }
}