using Typotrainer.Core.Interfaces;
using Typotrainer.Core.Models;

namespace Typotrainer.Core.Services
{ 
    public class SentenceService
    {
        private readonly IFileProvider _fileProvider;
        private readonly Dictionary<Difficulty, List<string>> _sentences;

        public SentenceService(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;

            // Laadt voor elke moeilijkheidsgraad de bijbehorende tekstbestanden
            _sentences = new Dictionary<Difficulty, List<string>>
            {
                { Difficulty.Easy, LoadFile("makkelijk.txt") },
                { Difficulty.Medium, LoadFile("middelmatig.txt") },
                { Difficulty.Hard, LoadFile("moeilijk.txt") }
            };
        }

        private List<string> LoadFile(string fileName)
        {
            // Bestand openen via de IFileProvider interface
            using var stream = _fileProvider.OpenAppPackageFileAsync(fileName).Result;
            using var reader = new StreamReader(stream);

            // Hele inhoud inlezen
            string content = reader.ReadToEnd();

            // Regels opsplitsen, trimmen en lege regels verwijderen
            return content.Split("\n")
                          .Select(line => line.Trim())
                          .Where(line => !string.IsNullOrEmpty(line))
                          .ToList();
        }

        public string GetRandomSentence(Difficulty difficulty)
        {
            // Checken of er zinnen zijn voor de gekozen moeilijkheid
            if (_sentences.TryGetValue(difficulty, out var sentences) && sentences.Count > 0)
            {
                // Willekeurige zin kiezen
                Random random = new Random();
                int index = random.Next(sentences.Count);
                return sentences[index];
            }

            // Foutmelding als er geen zinnen zijn
            throw new InvalidOperationException("Geen zin beschikbaar voor de geselecteerde moeilijkheid");
        }
    }
}