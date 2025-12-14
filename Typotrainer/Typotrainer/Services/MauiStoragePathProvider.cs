using Typotrainer.Core.Interfaces;

namespace Typotrainer.Services;

public class MauiStoragePathProvider : IStoragePathProvider
{
    public string GetAppDataDirectory()
    {
        return FileSystem.AppDataDirectory;
    }
}