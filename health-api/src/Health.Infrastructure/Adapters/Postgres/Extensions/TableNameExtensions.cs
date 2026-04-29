using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Health.Infrastructure.Adapters.Postgres.Extensions;

public static class PostgreNamingExtensions
{
    public static void ApplySnakeCaseNaming(this ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // 1. Rename Table
            var tableName = entity.GetTableName();
            if (!string.IsNullOrEmpty(tableName))
                entity.SetTableName(tableName.ToSnakeCase());

            // 2. Rename Columns
            foreach (var property in entity.GetProperties())
            {
                // Usamos o StoreObjectIdentifier para garantir que pegamos o nome correto da coluna na tabela
                var identifier = StoreObjectIdentifier.Table(entity.GetTableName()!, entity.GetSchema());
                var currentColumnName = property.GetColumnName(identifier);

                if (!string.IsNullOrEmpty(currentColumnName))
                    property.SetColumnName(currentColumnName.ToSnakeCase());
            }

            // 3. Rename Keys (Primary & Alternate)
            foreach (var key in entity.GetKeys())
            {
                var keyName = key.GetName();
                if (!string.IsNullOrEmpty(keyName))
                    key.SetName(keyName.ToSnakeCase());
            }

            // 4. Rename Foreign Keys
            foreach (var foreignKey in entity.GetForeignKeys())
            {
                var constraintName = foreignKey.GetConstraintName();
                if (!string.IsNullOrEmpty(constraintName))
                    foreignKey.SetConstraintName(constraintName.ToSnakeCase());
            }

            // 5. Rename Indexes
            foreach (var index in entity.GetIndexes())
            {
                var indexName = index.GetDatabaseName();
                if (!string.IsNullOrEmpty(indexName))
                    index.SetDatabaseName(indexName.ToSnakeCase());
            }
        }
    }

    private static string ToSnakeCase(this string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        return Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }
}