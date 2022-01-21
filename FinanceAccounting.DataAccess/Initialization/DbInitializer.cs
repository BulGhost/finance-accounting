using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceAccounting.DataAccess.DbContext;
using FinanceAccounting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace FinanceAccounting.DataAccess.Initialization
{
    public static class DbInitializer
    {
        public static async Task Initialize(BookkeepingDbContext context)
        {
            await DropAndCreateDatabase(context);
            await SeedData(context);
        }

        public static void ClearAndReseedDatabase(BookkeepingDbContext context)
        {
            ClearData(context);
            SeedData(context).RunSynchronously();
        }

        internal static async Task DropAndCreateDatabase(BookkeepingDbContext context)
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.MigrateAsync();
        }

        internal static void ClearData(BookkeepingDbContext context)
        {
            string[] entities =
            {
                typeof(Operation).FullName,
                typeof(Category).FullName,
                typeof(User).FullName
            };

            foreach (string entityName in entities)
            {
                IEntityType entity = context.Model.FindEntityType(entityName);
                string tableName = entity.GetTableName();
                string schemaName = entity.GetSchema();
                string fullyQualifiedTableName = schemaName == null ? tableName : $"{schemaName}.{tableName}";
                context.Database.ExecuteSqlRaw($"DELETE FROM {fullyQualifiedTableName}");
                context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT (\'{fullyQualifiedTableName}\', RESEED, 1);");
            }
        }

        internal static async Task SeedData(BookkeepingDbContext context)
        {
            try
            {
                await ProcessInsert(context, context.Users, TestData.Users);
                await ProcessInsert(context, context.Categories, TestData.Categories);
                await ProcessInsert(context, context.Operations, TestData.Operations);
            }
            catch (Exception)
            {
                await context.DisposeAsync();
                throw;
            }
        }

        private static async Task ProcessInsert<TEntity>(BookkeepingDbContext context, DbSet<TEntity> table,
            List<TEntity> records) where TEntity : class
        {
            if (await table.AnyAsync()) return;

            IExecutionStrategy strategy = context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync();
                try
                {
                    IEntityType metaData = context.Model.FindEntityType(typeof(TEntity).FullName!);
                    string schemaName = metaData.GetSchema() ?? "dbo";
                    string tableName = metaData.GetTableName();
                    await context.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {schemaName}.{tableName} ON");
                    await table.AddRangeAsync(records);
                    await context.SaveChangesAsync();
                    await context.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {schemaName}.{tableName} OFF");
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }
    }
}
