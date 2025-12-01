using Domain.Services;

namespace DB.Services
{
    /// <summary>
    /// Typing service - validates typed text
    /// Implements ITypingService from Domain
    /// </summary>
    public class TypingService : ITypingService
    {
        public bool IsCorrectLetter(string correctText, int index, char typedChar)
        {
            if (string.IsNullOrEmpty(correctText))
                return false;

            if (index < 0 || index >= correctText.Length)
                return false;

            return correctText[index] == typedChar;
        }

        public int CalculateErrors(string correctText, string typedText)
        {
            if (string.IsNullOrEmpty(correctText) || string.IsNullOrEmpty(typedText))
                return 0;

            int errors = 0;
            int minLength = Math.Min(correctText.Length, typedText.Length);

            for (int i = 0; i < minLength; i++)
            {
                if (correctText[i] != typedText[i])
                {
                    errors++;
                }
            }

            return errors;
        }
    }
}