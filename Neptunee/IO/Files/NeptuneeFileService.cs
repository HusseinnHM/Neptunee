using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Neptunee.IO.Files.Options;

namespace Neptunee.IO.Files;

public class NeptuneeFileService : INeptuneeFileService
{
    private readonly NeptuneeFileOptions _options;

    public NeptuneeFileService(IOptions<NeptuneeFileOptions> options)
    {
        _options = options.Value;
    }

    public virtual async Task<string?> UploadAsync(IFormFile? file, string uploadPath)
    {
        if (file is null)
        {
            return null;
        }

        await PhysicalUpload(file, uploadPath);
        return uploadPath;
    }

    public virtual async Task<string?> UploadAsync(IFormFile? file, Func<IFormFile, string> uploadPath)
    {
        if (file is null)
        {
            return null;
        }

        return await UploadAsync(file, uploadPath(file));
    }

    public virtual async Task<string?> UploadAsync(IFormFile? file)
    {
        if (file is null)
        {
            return null;
        }

        var path = Path.Combine(_options.DirUploadPathProvider(_options.BasePath, file), _options.FileNameProvider(file));
        await PhysicalUpload(file, path);
        return path;
    }

    public virtual async Task<IEnumerable<string>> UploadAsync(List<IFormFile> files, string dirPath, Func<IFormFile, string> fileName)
    {
        var list = new List<string?>();
        foreach (var file in files)
        {
            list.Add(await UploadAsync(file, Path.Combine(dirPath, fileName(file))));
        }

        return list!;
    }

    public virtual async Task<IEnumerable<string>> UploadAsync(List<IFormFile> files)
    {
        return await UploadAsync(files, _options.DirUploadPathProvider(_options.BasePath, null!), _options.FileNameProvider);
    }

    public virtual async Task<string?> ModifyAsync(string? original, bool forceDelete, Func<Task<string?>> upload)
    {
        var newPath = await upload();

        if (forceDelete || newPath is not null)
        {
            Delete(original);
            original = null;
        }

        return newPath ?? original;
    }

    public virtual async Task ModifyAsync(List<string> original, IEnumerable<string> deletes, Task<IEnumerable<string>> uploads)
    {
        foreach (var deletePath in deletes)
        {
            Delete(deletePath);
            original.Remove(deletePath);
        }

        original.AddRange(await uploads);
    }


    public virtual void Delete(string? path)
    {
        if (path == null)
        {
            return;
        }

        PhysicalDelete(path);
    }

    public virtual void Delete(List<string> path)
    {
        foreach (var p in path)
        {
            Delete(p);
        }
    }

    public virtual string CreateDirectoryIfNotExists(string path)
    {
        var phyPath = Path.Combine(_options.BasePath, path);
        if (Directory.Exists(phyPath))
        {
            return path;
        }

        Directory.CreateDirectory(phyPath);
        return path;
    }

    public virtual void DeleteDirectoryIfExists(string path)
    {
        var phyPath = Path.Combine(_options.BasePath, path);
        if (Directory.Exists(phyPath))
        {
            Directory.Delete(phyPath);
        }
    }

    protected virtual async Task PhysicalUpload(IFormFile file, string path)
    {
        var fs = new FileStream(Path.Combine(_options.BasePath, path), FileMode.Create);
        await file.CopyToAsync(fs);
        await fs.DisposeAsync();
        fs.Close();
    }

    protected virtual void PhysicalDelete(string path)
    {
        var phyPath = Path.Combine(_options.BasePath, path);
        if (File.Exists(phyPath))
        {
            File.Delete(phyPath);
        }
    }
}