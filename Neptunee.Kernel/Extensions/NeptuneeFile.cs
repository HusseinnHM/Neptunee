namespace Neptunee.Extensions;

public static class NeptuneeFile
{
    public static string GuidAndDateWithExtensionName(string fileName)
    {
        return $"{Guid.NewGuid():N}_{DateTimeOffset.UtcNow.DateTime.ToString("yyyy-MM-dd-hh-mm-ss").Replace("-", string.Empty)}{Path.GetExtension(fileName)}";
    }

    public static string GuidWithExtensionName(string fileName)
    {
        return $"{Guid.NewGuid():N}{Path.GetExtension(fileName)}";
    }

    public static string DateWithExtensionName(string fileName)
    {
        return $"{DateTimeOffset.UtcNow.DateTime.ToString("yyyy-MM-dd-hh-mm-ss").Replace("-", string.Empty)}{Path.GetExtension(fileName)}";
    }
}