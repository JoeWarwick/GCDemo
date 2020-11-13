using CDWRepository;
using CDWSVCAPI.Helpers;
using CDWSVCAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using static CDWRepository.CDWSVCModel;

namespace CDWSVCAPI.Caching
{
    public class AutoFeedRefreshCache: AutoRefreshCache<Tuple<string, int>, XmlDocument>
    {
        private CDWSVCModel _ctx;
        private IContentCuration _curation;

        public AutoFeedRefreshCache(CDWSVCModel ctx, IContentCuration curation, ILogger logger) 
            : base(interval: TimeSpan.FromMinutes(120), logger) 
        {
            this._ctx = ctx;
            this._curation = curation;
        }
        
        protected override XmlDocument Load(Tuple<string, int> key) 
        {
            var backup = this.GetIfExists(key);
            switch (key.Item1)
            {
                case "Raw":
                    var feedId = key.Item2;                    
                    try
                    {
                        var feed = _ctx.Subscribables.FirstOrDefault(f => f.Id == feedId);
                        if (feed is Feed || (feed.FeedSource != null && !feed.FeedSource.LoadChildren))
                        {
                            var result = LoadFeed(feed);
                            return result;
                        }
                        else if (feed is FeedSet)
                        {
                            var f = (FeedSet)feed;
                            XmlDocument xdoc = null;
                            foreach (var fi in f.Feeds)
                            {
                                if (xdoc == null)
                                {
                                    xdoc = Load(Tuple.Create("Raw", fi.Id));
                                }
                                else
                                {
                                    var xdoc2 = Load(Tuple.Create("Raw", fi.Id));
                                    foreach (XmlNode node in xdoc2.DocumentElement.ChildNodes)
                                    {
                                        if (node.LocalName == "entry")
                                        {
                                            XmlNode imported = xdoc.ImportNode(node, true);
                                            xdoc.DocumentElement.AppendChild(imported);
                                        }
                                        if (node.LocalName == "channel")
                                        {  // remove duplicates
                                            foreach (XmlNode rssNode in node.ChildNodes)
                                            {
                                                var link = rssNode.SelectSingleNode("title");
                                                if (rssNode.LocalName == "item")
                                                {
                                                    var check = xdoc.DocumentElement.FirstChild.SelectSingleNode("item/title[text() = '" + link.InnerText.Replace("'", "&apos;") + "']");
                                                    if (check == null)
                                                    {
                                                        XmlNode imported = xdoc.ImportNode(rssNode, true);
                                                        xdoc.DocumentElement.FirstChild.AppendChild(imported);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            return xdoc;
                        }
                        return null;
                    }
                    catch (Exception ex)
                    {
                        //logger.Warn(ex, "Raw: Old cache used as backup");
                        return backup;
                    }
                case "Subscribable":
                    return TransformResult(Tuple.Create("Subscribable", key.Item2), backup);
                default:
                    var res = LoadEndpoint(key.Item1);
                    var doc = new XmlDocument();
                    doc.LoadXml(res);
                    return doc;
            }            
        }

        private XmlDocument LoadFeed(Subscribable feed)
        {
            var url = feed.Url;
            var doc = this.Get(Tuple.Create("Subscribable", feed.Id));
            return doc;
        }

        private string LoadEndpoint(string key)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                var xmldoc = new XPathDocument(key);
                using (XmlWriter xmlWriter = XmlWriter.Create(sb))
                {
                    xmldoc.CreateNavigator().WriteSubtree(xmlWriter);
                }
            }
            catch (WebException ex)
            {
                //logger.Error(ex, string.Format("LoadEndPoint failed for {0}", key));
                sb.Append("<error>" + ex.Message + "</error>");
            }
            
            return sb.ToString();
        }

        private XmlDocument TransformResult(Tuple<string, int> key, XmlDocument backup)
        {
            string url;
            XmlDocument resp;
            IEnumerable<FeedTransform> transformQueue;
            try
            {
                var feed = _ctx.Subscribables.Include("FeedTransforms").FirstOrDefault(f => f.Id == key.Item2);
                url = feed.Url;
                resp = this.Get(Tuple.Create(url.ToString(), 0));
                transformQueue = feed?.FeedTransforms.FirstOrDefault() == null ? feed.SourceGroup.FeedTransforms : feed.FeedTransforms.Select(sft => sft.FeedTransform);
            }
            catch (Exception ex)
            {
                //logger.Warn(ex, "Subscribable: Old cache used as backup!");
                return backup;
            }

            if (transformQueue != null)
            {
                //apply the cleaning and link checking transforms
                if (resp != null)
                {
                    var xsl = new XslCompiledTransform(enableDebug: false);
                    foreach (var tx in transformQueue)
                    {
                        XsltArgumentList xslArgs = new XsltArgumentList();
                        xslArgs.AddParam("url", "", url);
                        switch (tx.InputFeedType?.TypeName)
                        {
                            case "RSS":
                                xslArgs.AddExtensionObject("ext:RSSProcessing", new RSSProcessing(new AutoImageRefreshCache(_curation, _logger), new AutoMetaRefreshCache(_logger)));
                                break;
                            case "ATOM":
                                xslArgs.AddExtensionObject("ext:AtomProcessing", new AtomProcessing(new AutoImageRefreshCache(_curation, _logger), new AutoMetaRefreshCache(_logger)));
                                break;
                            default:
                                xslArgs.AddExtensionObject("ext:AtomProcessing", new AtomProcessing(new AutoImageRefreshCache(_curation, _logger), new AutoMetaRefreshCache(_logger)));
                                break;
                        }
                        foreach (var p in tx.Params)
                        {
                            xslArgs.AddParam(p.Key, "", p.Value);
                        }
                        xsl.Load($"~{tx.Url}");
                        using (StringReader sr = new StringReader(resp.OuterXml))
                        {
                            using (XmlReader xr = XmlReader.Create(sr))
                            {
                                using (StringWriter sw = new StringWriter())
                                {
                                    try
                                    {
                                        xsl.Transform(xr, xslArgs, sw);
                                        resp.LoadXml(sw.ToString());
                                    }
                                    catch (XsltException ex)
                                    {
                                        resp.LoadXml("<error loc=\"transform\">" + ex.Message + "</error>");
                                        //logger.Error(ex, "Transform step failed", new[] { url, tx.Name });
                                    }
                                    catch (Exception oex)
                                    {
                                        resp.LoadXml("<error loc=\"transform\">" + oex.Message + "</error>");
                                        //logger.Error(oex, "Transform step failed badly", new[] { url, tx.Name });
                                    }
                                    return resp;
                                }
                            }
                        }
                    }
                    // pipeline in the messages, noshows and no messages items
                }
            }
            return resp;
        }
    }
}