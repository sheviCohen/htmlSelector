using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practiCode_2
{
    internal class HtmlElement
    {
        public String? Id { get; set; }
        public string Name { get; set; } = "";
        public List<Attribute> Attributes { get; set; }=new List<Attribute>();
        public List<string> Classes { get; set; }= new List<string>();
        public string InnerHtml { get; set; } = "";
        public HtmlElement? Parent { get; set; }
        public List<HtmlElement> Children { get; set; } = new List<HtmlElement>();
        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                HtmlElement current = queue.Dequeue();
                yield return current;

                foreach (HtmlElement child in current.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }

        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement? current = Parent;
            while (current != null)
            {
                yield return current;
                current = current.Parent;
            }
        }

    }

}


