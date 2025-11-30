using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Typotrainer.Services
{
    public class SentenceService
    {
        // Dictionary met per moeilijkheidsgraad een lijst zinnen
        private readonly Dictionary<Difficulty, List<string>> _sentences;

        public SentenceService()
        {
            // Laadt voor elke moeilijkheidsgraad de bijbehorende tekstbestanden
            _sentences = new Dictionary<Difficulty, List<string>>
            {
                { Difficulty.Easy, Loadfile("makkelijk.txt") },
                { Difficulty.Medium, Loadfile("middelmatig.txt") },
                { Difficulty.Hard, Loadfile("moeilijk.txt") }
            };
        }

        private List<string> Loadfile(string fileName)
        {
            // Bestand openen vanuit het apppakket
            using var stream = FileSystem.OpenAppPackageFileAsync(fileName).Result;
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
