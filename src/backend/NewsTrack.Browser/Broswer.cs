using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using cloudscribe.HtmlAgilityPack;
using NewsTrack.Browser.Dtos;

namespace NewsTrack.Browser
{
    public class Broswer : IBroswer
    {
        private readonly IRequestor _requestor;

        public Broswer(IRequestor requestor)
        {
            _requestor = requestor;
        }

        public async Task<ResponseDto> Get(string url)
        {
            var uri = new Uri(url);
            var content = await _requestor.Get(uri);
            return Set(uri, content);
        }

        public async Task<string> GetContent(string url)
        {
            var uri = new Uri(url);
            var content = await _requestor.Get(uri);
            if (content != null)
            {
                var document = new HtmlDocument();
                document.LoadHtml(content);
                var codes = new[] { "h1", "h2", "h3", "h4", "h5", "h6", "span", "p", "div" };
                var elements = GetElements(document, codes).ToList();
                var stringBuilder = new StringBuilder();
                elements.ForEach(e => stringBuilder.AppendLine(e));
                return stringBuilder.ToString();
            }

            return null;
        }

        private static ResponseDto Set(Uri uri, string content)
        {
            var response = new ResponseDto(uri);
            if (content != null)
            {
                var document = new HtmlDocument();
                document.LoadHtml(content);
                response.Titles = GetTitles(document);
                response.Paragraphs = GetParagraphs(document);
                var rawPictures = GetRawPictures(document);
                response.Pictures = GetPictures(uri, rawPictures.ToArray());
            }

            return response;
        }

        private static IEnumerable<Uri> GetPictures(Uri baseUri, string[] rawPictures)
        {
            var pictures = new List<Uri>();
            if (rawPictures != null && rawPictures.Any())
            {
                foreach (var htmlPicture in rawPictures)
                {
                    if (Uri.IsWellFormedUriString(htmlPicture, UriKind.Absolute))
                    {
                        pictures.Add(new Uri(htmlPicture));
                    }
                    else
                    {
                        var uri = new Uri(baseUri, htmlPicture);
                        pictures.Add(uri);
                    }
                }
            }

            return pictures;
        }

        private static IEnumerable<string> GetTitles(HtmlDocument document)
        {
            var codes = new[] {"h1", "h2", "h3", "h4", "h5", "h6"};
            return GetElements(document, codes);
        }

        private static IEnumerable<string> GetParagraphs(HtmlDocument document)
        {
            var codes = new[] { "p", "span" };
            return GetElements(document, codes, 300);
        }

        private static IEnumerable<string> GetRawPictures(HtmlDocument document)
        {
            var codes = new[] {"img"};
            return GetElements(document, codes, "src", "alt");
        }

        private static IEnumerable<string> GetElements(HtmlDocument document, IEnumerable<string> codes, int? minLenght = null)
        {
            var elements = new HashSet<string>();

            foreach (var code in codes)
            {
                var nodes = document.DocumentNode.SelectNodes("//" + code);
                if (nodes != null && nodes.Any())
                {
                    foreach (var htmlNode in nodes)
                    {
                        if (htmlNode.InnerText != null)
                        {
                            if (minLenght != null && htmlNode.InnerText.Length > minLenght || minLenght == null)
                            {
                                elements.Add(WebUtility.HtmlDecode(htmlNode.InnerText));
                            }
                        }
                    }
                }
            }

            return elements;
        }

        private static IEnumerable<string> GetElements(
            HtmlDocument document, 
            IEnumerable<string> codes, 
            string attribute, 
            params string[] conditionalAttributes)
        {
            var elements = new HashSet<string>();

            foreach (var code in codes)
            {
                var nodes = document.DocumentNode.SelectNodes("//" + code);
                if (nodes != null && nodes.Any())
                {
                    foreach (var htmlNode in nodes)
                    {
                        if (htmlNode.Attributes.Contains(attribute))
                        {
                            if (conditionalAttributes != null && !conditionalAttributes.All(a => htmlNode.Attributes.Contains(a)))
                            {
                                continue;
                            }

                            elements.Add(WebUtility.HtmlDecode(htmlNode.Attributes[attribute].Value));
                        }
                    }
                }
            }

            return elements;
        }
    }
}
