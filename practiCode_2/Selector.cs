using System.Text.RegularExpressions;

namespace practiCode_2
{
    internal class Selector
    {
        public string? TagName { get; set; }
        public string? Id { get; set; }
        public List<string> Classes { get; set; } = new List<string>();
        public Selector? Parent { get; set; }
        public Selector? Child { get; set; }

        public static Selector Parse(string query)
        {
            string[] levels = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            Selector root = new Selector();
            Selector current = root;

            for (int i = 0; i < levels.Length; i++)
            {
                string[] parts = new Regex("(?=[#.])").Split(levels[i]);
                foreach (string part in parts)
                {
                    if (part.StartsWith("#"))
                    {
                        current.Id = part.Substring(1);
                    }
                    else if (part.StartsWith("."))
                    {
                        current.Classes.Add(part.Substring(1));
                    }
                    else if (HtmlHelper.Instance.AllTags.Contains(part))
                    {
                        current.TagName = part;
                    }
                }

                if (i < levels.Length - 1)
                {
                    Selector child = new Selector();
                    child.Parent = current;
                    current.Child = child;
                    current = child;
                }
            }

            return root;
        }

        public bool IsMatch(HtmlElement element)
        {
            return (TagName == null || element.Name == TagName)
                && (Id == null || element.Id == Id)
                && Classes.All(c => element.Classes.Contains(c));
        }
    }
}
