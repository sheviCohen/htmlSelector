using System.Text.Json;

namespace practiCode_2
{
    internal class HtmlHelper
    {
        private static readonly HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;

        public string[] AllTags { get; }
        public string[] VoidTags { get; }

        private HtmlHelper()
        {
            AllTags = LoadTags("HtmlTags.json");
            VoidTags = LoadTags("HtmlVoidTags.json");
        }

        private string[] LoadTags(string fileName)
        {
            string path = Path.Combine(AppContext.BaseDirectory, fileName);
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<string[]>(json) ?? new string[0];
        }
    }
}
