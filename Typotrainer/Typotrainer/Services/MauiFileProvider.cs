using Typotrainer.Core.Interfaces;

namespace Typotrainer.Services;

public class MauiFileProvider : IFileProvider
{
    public Task<Stream> OpenAppPackageFileAsync(string filename)
    {
        return FileSystem.OpenAppPackageFileAsync(filename);
    }
}