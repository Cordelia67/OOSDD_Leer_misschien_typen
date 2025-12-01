namespace Domain.Services
{
    /// <summary>
    /// Typing service interface - validates typing input
    /// </summary>
    public interface ITypingService
    {
        bool IsCorrectLetter(string correctText, int index, char typedChar);
        int CalculateErrors(string correctText, string typedText);
    }
}