namespace practiCode_2
{
    internal static class TreeTests
    {
        private const string SampleHtml = @"
<!doctype html>
<html lang=en>
<body>
  <!-- <div id=""commented""></div> -->
  <div id=""main"" class=""box big"" data-x='1'>
    Hello
    <br/>
    <img src=""a.png"">
    <p>World</p>
  </div>
  <p class=note>div</p>
  <input disabled>
  <script>if (a<b) { x = ""</div>""; }</script>
  <footer></footer>
</body>
</html>
<div id=""after""></div>";

        private static int passed = 0;
        private static int failed = 0;

        public static void Run()
        {
            HtmlSerializer serializer = new HtmlSerializer();
            HtmlElement root = serializer.BuildTree(serializer.SplitToLines(SampleHtml));

            HtmlElement Html() => root.Children[0];
            HtmlElement Body() => Html().Children[0];
            HtmlElement Div() => Body().Children[0];
            HtmlElement InnerP() => Div().Children[2];
            HtmlElement NoteP() => Body().Children[1];
            HtmlElement Input() => Body().Children[2];
            HtmlElement Script() => Body().Children[3];

            Check("doctype and content after </html> are skipped", "1", () => root.Children.Count);
            Check("first child of root is html", "html", () => Html().Name);
            Check("html knows its parent", "True", () => Html().Parent == root);
            Check("attribute without quotes", "lang=en", () => AttributeText(Html(), "lang"));
            Check("comment and closing tags keep the right children in body", "div,p,input,script,footer", () => ChildNames(Body()));
            Check("Id comes from the attributes", "main", () => Div().Id);
            Check("class is split to Classes", "box,big", () => string.Join(",", Div().Classes));
            Check("attribute with single quotes", "data-x=1", () => AttributeText(Div(), "data-x"));
            Check("text goes to InnerHtml", "Hello", () => Div().InnerHtml);
            Check("br and img do not swallow what comes after them", "br,img,p", () => ChildNames(Div()));
            Check("closing tag goes back to the parent", "div", () => InnerP().Parent?.Name);
            Check("text of an inner element", "World", () => InnerP().InnerHtml);
            Check("the word div as text is not a tag", "div", () => NoteP().InnerHtml);
            Check("element with text has no children", "0", () => NoteP().Children.Count);
            Check("class without quotes", "note", () => string.Join(",", NoteP().Classes));
            Check("attribute without a value", "disabled=", () => AttributeText(Input(), "disabled"));
            Check("script body is not part of the tree", "0", () => Script().Children.Count);

            Console.WriteLine($"Passed: {passed}, Failed: {failed}");
        }

        private static string ChildNames(HtmlElement element)
        {
            return string.Join(",", element.Children.Select(child => child.Name));
        }

        private static string AttributeText(HtmlElement element, string name)
        {
            Attribute attribute = element.Attributes.First(a => a.Name == name);
            return $"{attribute.Name}={attribute.Value}";
        }

        private static void Check(string title, string expected, Func<object?> getActual)
        {
            string actual;
            try
            {
                actual = getActual()?.ToString() ?? "null";
            }
            catch (Exception ex)
            {
                actual = ex.GetType().Name;
            }

            if (actual == expected)
            {
                passed++;
                Console.WriteLine($"OK   {title}");
            }
            else
            {
                failed++;
                Console.WriteLine($"FAIL {title} | expected: {expected} | actual: {actual}");
            }
        }
    }
}
