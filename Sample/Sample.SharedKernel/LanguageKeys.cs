namespace Sample.SharedKernel;

public static class LanguageKeys
{
    public const string Default = "en";
    public static readonly string[] Other = { "ar" };


    public static bool Validate(string language)
    {
        return Default.Equals(language, StringComparison.OrdinalIgnoreCase) ||
               Other.Contains(language, StringComparer.OrdinalIgnoreCase);
    }
}