using System.Runtime.Versioning;
using Typotrainer.Core.Interfaces;

namespace Typotrainer.Services;

public class MauiFileProvider : IFileProvider
{
    [SupportedOSPlatform("windows")]
    public Task<Stream> OpenAppPackageFileAsync(string filename)
    {
        return FileSystem.OpenAppPackageFileAsync(filename);
    }
}