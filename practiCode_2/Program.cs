using practiCode_2;

TreeTests.Run();

HtmlSerializer s = new HtmlSerializer();
string url = "https://example.com";
string query = "body p";

string html = await s.Load(url);
HtmlElement root = s.BuildTree(s.SplitToLines(html));
HashSet<HtmlElement> results = root.Find(Selector.Parse(query));

Console.WriteLine($"{results.Count} elements match {query}");
//https://github.com/sheviCohen/htmlSelector.git