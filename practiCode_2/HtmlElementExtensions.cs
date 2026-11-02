namespace practiCode_2
{
    internal static class HtmlElementExtensions
    {
        public static HashSet<HtmlElement> Find(this HtmlElement element, Selector selector)
        {
            HashSet<HtmlElement> result = new HashSet<HtmlElement>();
            Find(element, selector, result);
            return result;
        }

        private static void Find(HtmlElement element, Selector selector, HashSet<HtmlElement> result)
        {
            var matches = element.Descendants().Skip(1).Where(selector.IsMatch);

            foreach (HtmlElement match in matches)
            {
                if (selector.Child == null)
                {
                    result.Add(match);
                }
                else
                {
                    Find(match, selector.Child, result);
                }
            }
        }
    }
}
