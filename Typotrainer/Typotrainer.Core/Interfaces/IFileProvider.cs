namespace Typotrainer.Core.Interfaces;

public interface IFileProvider
{
    Task<Stream> OpenAppPackageFileAsync(string filename);
}