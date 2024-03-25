using Presentation.Contracts;

namespace Presentation.Services
{
    public class ModifiersMappingHelper : IModifiersMappingHelper
    {
        private readonly Dictionary<string, string> WordMappings = new Dictionary<string, string>
        {
            { "public", "+" },
            { "internal", "<<internal>>" },
            { "protected internal", "# <<internal>>" },
            { "protected", "#" },
            { "private", "-" },
            { "abstract", "{abstract}" },
            { "static", "{static}" },
            { "virtual", "<<virtual>>" },
            { "override", "<<override>>" },
            { "new", "<<new>>" },
            { "readonly", "<<readonly>>" },
            { "event", "<<event>>" }
        };

        public IEnumerable<string> MapWords(IEnumerable<string> words)
        {
            var mappedWords = words.Select(MapWord);
            if (ShouldPrependHyphen(mappedWords))
            {
                return PrependHyphen(mappedWords);
            }
            return mappedWords;
        }

        private bool ShouldPrependHyphen(IEnumerable<string> words)
        {
            return !words.Any(
                w =>
                    w.StartsWith("+")
                    || w.StartsWith("#")
                    || w.StartsWith("<<internal>>")
                    || w.StartsWith("-")
            );
        }

        private IEnumerable<string> PrependHyphen(IEnumerable<string> words)
        {
            return new List<string> { "-" }.Concat(words);
        }

        private string MapWord(string word)
        {
            return WordMappings.TryGetValue(word, out var mappedWord) ? mappedWord : word;
        }
    }
}
