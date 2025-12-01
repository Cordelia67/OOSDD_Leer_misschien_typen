namespace Domain.Models
{
    /// <summary>
    /// Exercise entity - represents a typing exercise attempt
    /// </summary>
    public class Exercise
    {
        public int Id { get; private set; }
        public int UserId { get; set; }
        public string SentenceText { get; set; } = string.Empty;
        public Difficulty DifficultyLevel { get; set; }
        public int ErrorCount { get; set; }
        public int WordsPerMinute { get; set; }
        public double Accuracy { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime CompletedAt { get; private set; } = DateTime.UtcNow;

        public Exercise(int currentUserId, string correctSentence) { }

        public Exercise(int userId, string sentenceText, Difficulty difficulty)
        {
            UserId = userId;
            SentenceText = sentenceText;
            DifficultyLevel = difficulty;
        }

        // Business logic: Calculate accuracy percentage
        public void CalculateAccuracy(int totalCharacters)
        {
            if (totalCharacters == 0)
            {
                Accuracy = 0;
                return;
            }

            int correctCharacters = totalCharacters - ErrorCount;
            Accuracy = (correctCharacters / (double)totalCharacters) * 100;
        }

        // Business logic: Calculate WPM
        public void CalculateWPM(int totalCharacters)
        {
            if (Duration.TotalMinutes == 0)
            {
                WordsPerMinute = 0;
                return;
            }

            // Standard: 5 characters = 1 word
            double words = totalCharacters / 5.0;
            WordsPerMinute = (int)(words / Duration.TotalMinutes);
        }
    }
}