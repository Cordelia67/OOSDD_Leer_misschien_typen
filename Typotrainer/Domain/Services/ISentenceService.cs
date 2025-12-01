using Domain.Models;

namespace Domain.Services
{
    /// <summary>
    /// Sentence service interface - provides sentences for exercises
    /// </summary>
    public interface ISentenceService
    {
        string GetRandomSentence(Difficulty difficulty);
        List<string> GetAllSentences(Difficulty difficulty);
        //string GetRandomSentence(global::Difficulty easy);
    }
}