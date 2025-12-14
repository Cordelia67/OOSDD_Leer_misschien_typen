using Typotrainer.Core.Models;

namespace Typotrainer.Core.Services;

public class StatsStorageService
{
    private readonly string _filePath =
        Path.Combine(FileSystem.AppDataDirectory, "typing_stats.txt");
   // ^ staat in C:\Users\<jouw naam>\AppData\Local\Packages\com.companyname.voorbeeldmainpage_9zz4h110yvjzm\LocalState
    public async Task SaveAsync(ExerciseSessionResult result)
    {
        await File.AppendAllTextAsync(_filePath, result + Environment.NewLine);
    }

    public async Task<List<ExerciseSessionResult>> LoadAsync()
    {
        if (!File.Exists(_filePath))
            return new();

        var lines = await File.ReadAllLinesAsync(_filePath);
        return lines
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(ExerciseSessionResult.FromString)
            .ToList();
    }
}
