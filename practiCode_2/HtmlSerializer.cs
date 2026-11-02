using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace practiCode_2
{
    internal class HtmlSerializer
    {
        public async Task<string> Load(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var html = await response.Content.ReadAsStringAsync();
            return html;
        }

        public List<string> SplitToLines(string html)
        {
            var cleanHtml = new Regex("\\s+").Replace(html, " ");
            cleanHtml = new Regex("<!--.*?-->").Replace(cleanHtml, "");
            cleanHtml = new Regex("(<(script|style)\\b[^>]*>).*?(</\\2>)").Replace(cleanHtml, "$1$3");

            return new Regex("(<.*?>)").Split(cleanHtml)
                .Select(line => line.Trim())
                .Where(line => line.Length > 0)
                .ToList();
        }
        public HtmlElement BuildTree (List<string> lines)
        {
            HtmlElement root = new HtmlElement();
            HtmlElement current = root;

            foreach (string line in lines)
            {
                string content = line.Trim('<', '>').TrimEnd('/').Trim();
                string tagName = content.Split(' ')[0].ToLower();
                string rest = content.Substring(tagName.Length);

                if (line.StartsWith("<!"))
                {
                    continue;
                }
                else if (tagName == "/html")
                {
                    break;
                }
                else if (tagName.StartsWith("/"))
                {
                    current = current.Parent ?? current;
                }
                else if (line.StartsWith("<") && HtmlHelper.Instance.AllTags.Contains(tagName))
                {
                    HtmlElement newHtmlElement= new HtmlElement();
                    newHtmlElement.Name=tagName;
                    newHtmlElement.Parent=current;
                    current.Children.Add(newHtmlElement);


                    var matches = new Regex("([^\\s=]+)(?:\\s*=\\s*(\"[^\"]*\"|'[^']*'|[^\\s\"']+))?").Matches(rest);
                    foreach (Match match in matches)
                    {
                        string attributeName = match.Groups[1].Value;
                        string attributeValue = match.Groups[2].Value.Trim('"', '\'');
                        newHtmlElement.Attributes.Add(new Attribute { Name = attributeName, Value = attributeValue });
                    }
                    Attribute? idAttribute = newHtmlElement.Attributes.FirstOrDefault(a => a.Name == "id");
                    if (idAttribute != null)
                    {
                        newHtmlElement.Id = idAttribute.Value;
                    }

                    Attribute? classAttribute = newHtmlElement.Attributes.FirstOrDefault(a => a.Name == "class");
                    if (classAttribute != null)
                    {
                        newHtmlElement.Classes.AddRange(classAttribute.Value.Split(' '));
                    }

                    if (!line.EndsWith("/>") && !HtmlHelper.Instance.VoidTags.Contains(tagName))
                    {
                        current = newHtmlElement;
                    }

                }


                else
                {
                    current.InnerHtml += line;
                }

            }





            return root;
            
        }
    }
}
