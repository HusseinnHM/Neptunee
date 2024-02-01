using Microsoft.AspNetCore.Http;
using Neptunee.Extensions;

namespace Neptunee.IO.Files.Options;

public class NeptuneeFileOptions
{
    internal string BasePath { get; private set; } = string.Empty;

    internal Func<IFormFile, string> FileNameProvider { get; private set; }
        = file => NeptuneeFile.GuidWithExtensionName(file.FileName);

    internal Func<string, IFormFile, string> DirUploadPathProvider { get; private set; }
        = (basePath, _) =>
        {
            var dir = $"{DateTimeOffset.UtcNow.Year}-{DateTimeOffset.UtcNow.Month}";
            var phyDir = Path.Combine(basePath, dir);
            if (!Directory.Exists(phyDir))
            {
                Directory.CreateDirectory(phyDir);
            }

            return dir;
        };

    public NeptuneeFileOptions SetBasePath(string path)
    {
        BasePath = path;
        return this;
    }

    public NeptuneeFileOptions SetFileNameProvider(Func<IFormFile, string> fileNameProvider)
    {
        FileNameProvider = fileNameProvider;
        return this;
    }

    public NeptuneeFileOptions SetDirUploadPathProvider(Func<string, IFormFile, string> dirUploadPathProvider)
    {
        DirUploadPathProvider = dirUploadPathProvider;
        return this;
    }
}