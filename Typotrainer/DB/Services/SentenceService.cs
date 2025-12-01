using Domain.Models;
using Domain.Services;

namespace DB.Services
{
    /// <summary>
    /// Sentence service - loads sentences from text files
    /// Implements ISentenceService from Domain
    /// </summary>
    public class SentenceService : ISentenceService
    {
        private readonly Dictionary<Difficulty, List<string>> _sentences;

        public SentenceService()
        {
            _sentences = new Dictionary<Difficulty, List<string>>
            {
                { Difficulty.Easy, LoadFile("makkelijk.txt") },
                { Difficulty.Medium, LoadFile("middelmatig.txt") },
                { Difficulty.Hard, LoadFile("moeilijk.txt") }
            };
        }

        public string GetRandomSentence(Difficulty difficulty)
        {
            if (_sentences.TryGetValue(difficulty, out var sentences) && sentences.Count > 0)
            {
                var random = new Random();
                return sentences[random.Next(sentences.Count)];
            }

            throw new InvalidOperationException($"Geen zinnen beschikbaar voor {difficulty}");
        }

        public List<string> GetAllSentences(Difficulty difficulty)
        {
            if (_sentences.TryGetValue(difficulty, out var sentences))
            {
                return new List<string>(sentences);
            }

            return new List<string>();
        }

        private List<string> LoadFile(string fileName)
        {
            try
            {
                using var stream = FileSystem.OpenAppPackageFileAsync(fileName).Result;
                using var reader = new StreamReader(stream);

                var content = reader.ReadToEnd();

                return content.Split('\n')
                    .Select(line => line.Trim())
                    .Where(line => !string.IsNullOrEmpty(line))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Kan bestand {fileName} niet laden: {ex.Message}", ex);
            }
        }
    }
}