using System.Linq;
using HtmlAgilityPack;

namespace ReverseMarkdown.Converters
{
    public class Strong : ConverterBase
    {
        public Strong(Converter converter) : base(converter)
        {
            var elements = new [] { "strong", "b" };
            
            foreach (var element in elements)
            {
                Converter.Register(element, this);
            }
        }

        public override string Convert(HtmlNode node)
        {
            var content = TreatChildren(node);
            if (string.IsNullOrEmpty(content) || AlreadyBold(node))
            {
                return content;
            }
            else
            {
                var spaceSuffix = (node.NextSibling?.Name == "strong" || node.NextSibling?.Name == "b" || node.NextSibling?.Name == "i" || node.NextSibling?.Name == "em")
                    ? " "
                    : "";

                var nbsp = System.Convert.ToChar(160).ToString();
                var whiteSpaceAtFront = content.StartsWith(nbsp) || content.StartsWith(" ") ? " " : string.Empty;
                var whiteSpaceAtBack = content.EndsWith(nbsp) || content.EndsWith(" ") ? " " : string.Empty;

                return $"{whiteSpaceAtFront}*{content.Trim()}*{whiteSpaceAtBack}{spaceSuffix}";
            }
        }

        private static bool AlreadyBold(HtmlNode node)
        {
            return node.Ancestors("strong").Any() || node.Ancestors("b").Any();
        }
    }
}
