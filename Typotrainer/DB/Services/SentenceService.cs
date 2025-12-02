using Domain.Models;
using Domain.Services;

namespace DB.Services
{
    /// <summary>
    /// Service voor het laden en beheren van oefenzinnen
    /// Laadt zinnen uit tekstbestanden per moeilijkheidsgraad
    /// </summary>
    public class SentenceService : ISentenceService
    {
        private readonly Dictionary<Difficulty, List<string>> _sentences;
        private readonly Random _random;

        public SentenceService()
        {
            _sentences = new Dictionary<Difficulty, List<string>>();
            _random = new Random();
            LoadAllSentences();
        }

        public string GetRandomSentence(Difficulty difficulty)
        {
            if (!TryGetSentences(difficulty, out var sentences))
            {
                throw new InvalidOperationException(
                    $"Geen zinnen beschikbaar voor moeilijkheidsgraad: {difficulty}");
            }

            return SelectRandomSentence(sentences);
        }

        public List<string> GetAllSentences(Difficulty difficulty)
        {
            if (!TryGetSentences(difficulty, out var sentences))
            {
                return new List<string>();
            }

            return new List<string>(sentences);
        }

        private void LoadAllSentences()
        {
            LoadSentencesForDifficulty(Difficulty.Easy, "makkelijk.txt");
            LoadSentencesForDifficulty(Difficulty.Medium, "middelmatig.txt");
            LoadSentencesForDifficulty(Difficulty.Hard, "moeilijk.txt");
        }

        private void LoadSentencesForDifficulty(Difficulty difficulty, string fileName)
        {
            try
            {
                var sentences = LoadFromFile(fileName);
                _sentences[difficulty] = sentences;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Fout bij laden van zinnen voor {difficulty}: {ex.Message}", ex);
            }
        }

        private List<string> LoadFromFile(string fileName)
        {
            using var stream = OpenFile(fileName);
            using var reader = new StreamReader(stream);

            var content = reader.ReadToEnd();
            return ParseSentences(content);
        }

        private Stream OpenFile(string fileName)
        {
            try
            {
                return FileSystem.OpenAppPackageFileAsync(fileName).Result;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Kan bestand {fileName} niet openen: {ex.Message}", ex);
            }
        }

        private List<string> ParseSentences(string content)
        {
            return content
                .Split('\n')
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrEmpty(line))
                .ToList();
        }

        private bool TryGetSentences(Difficulty difficulty, out List<string> sentences)
        {
            if (_sentences.TryGetValue(difficulty, out var sentenceList)
                && sentenceList.Count > 0)
            {
                sentences = sentenceList;
                return true;
            }

            sentences = new List<string>();
            return false;
        }

        private string SelectRandomSentence(List<string> sentences)
        {
            int index = _random.Next(sentences.Count);
            return sentences[index];
        }
    }
}