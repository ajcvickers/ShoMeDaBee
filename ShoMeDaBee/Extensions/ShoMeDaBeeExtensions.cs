using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ShoMeDaBee.Internal;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    public static class ShoMeDaBeeExtensions
    {
        public static DbContextOptionsBuilder UseShoMeDaBe(
            this DbContextOptionsBuilder optionsBuilder,
            string hubUrl,
            TimeSpan? delay = null)
        {
            var extension = optionsBuilder.Options.FindExtension<DaBeeOptionsExtension>()
                            ?? new DaBeeOptionsExtension();

            extension = extension.WithHub(hubUrl, delay);

            ((IDbContextOptionsBuilderInfrastructure) optionsBuilder).AddOrUpdateExtension(extension);

            return optionsBuilder;
        }

        public static DbContextOptionsBuilder<TContext> UseShoMeDaBe<TContext>(
            this DbContextOptionsBuilder<TContext> optionsBuilder,
            string hubUrl,
            TimeSpan? delay = null)
            where TContext : DbContext
            => (DbContextOptionsBuilder<TContext>) UseShoMeDaBe((DbContextOptionsBuilder)optionsBuilder, hubUrl, delay);
    }
}