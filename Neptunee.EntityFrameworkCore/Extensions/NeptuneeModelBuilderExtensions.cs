using Neptunee.Entities;

namespace Microsoft.EntityFrameworkCore.Metadata;

public static class NeptuneeModelBuilderExtensions
{
    public static void SetPrimaryKeyValueGenerated(this ModelBuilder builder, ValueGenerated valueGenerated)
    {
        foreach (var entityType in builder.GetNeptuneeEntityTypes())
        {
            foreach (var property in entityType.GetProperties().Where(p => p.IsPrimaryKey()))
            {
                property.ValueGenerated = valueGenerated;
            }
        }
    }

    public static IEnumerable<IMutableEntityType> GetNeptuneeEntityTypes(this ModelBuilder builder)
    {
        return builder.Model.GetEntityTypes().Where(e => e.ClrType.IsAssignableTo(typeof(INeptuneeEntity)));
    }

    
}