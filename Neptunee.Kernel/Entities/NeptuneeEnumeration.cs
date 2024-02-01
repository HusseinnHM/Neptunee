using System.Reflection;

namespace Neptunee.Entities;

public abstract record NeptuneeEnumeration<TEnum>(int Value, string Name) where TEnum : NeptuneeEnumeration<TEnum>
{
    private static readonly Lazy<Dictionary<int, TEnum>> EnumerationsDictionary = new(() => GetAllEnumerationOptions().ToDictionary(item => item.Value));

    protected NeptuneeEnumeration() : this(default, string.Empty)
    {
    }

    public static IReadOnlyCollection<TEnum> List => EnumerationsDictionary.Value.Values.ToList();

    public int Value { get; private set; } = Value;

    public string Name { get; private set; } = Name;

    public static TEnum FromValue(int value) => EnumerationsDictionary.Value.TryGetValue(value, out var enumeration)
        ? enumeration!
        : default!;

    public static bool ContainsValue(int value) => EnumerationsDictionary.Value.ContainsKey(value);


    private static IEnumerable<TEnum> GetAllEnumerationOptions()
    {
        Type enumType = typeof(TEnum);

        IEnumerable<Type> enumerationTypes = Assembly
            .GetAssembly(enumType)!
            .GetTypes()
            .Where(type => enumType.IsAssignableFrom(type));

        var enumerations = new List<TEnum>();

        foreach (Type enumerationType in enumerationTypes)
        {
            List<TEnum> enumerationTypeOptions = GetFieldsOfType<TEnum>(enumerationType);

            enumerations.AddRange(enumerationTypeOptions);
        }

        return enumerations;
    }


    private static List<TFieldType> GetFieldsOfType<TFieldType>(Type type) =>
        type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fieldInfo => type.IsAssignableFrom(fieldInfo.FieldType))
            .Select(fieldInfo => (TFieldType)fieldInfo.GetValue(null)!)
            .ToList();
}