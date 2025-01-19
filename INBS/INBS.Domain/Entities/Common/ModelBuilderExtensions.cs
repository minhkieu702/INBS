using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities.Common
{
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Configures a one-to-many relationship with `DeleteBehavior.Restrict` to prevent cascade delete.
        /// </summary>
        /// <typeparam name="TEntity">The entity type for the relationship.</typeparam>
        /// <typeparam name="TRelatedEntity">The related entity type for the relationship.</typeparam>
        /// <param name="modelBuilder">The instance of ModelBuilder to configure the entity.</param>
        /// <param name="navigationExpression">
        /// An expression specifying the navigation property from the dependent entity to the principal entity.
        /// </param>
        /// <param name="inverseNavigationExpression">
        /// An expression specifying the inverse navigation property from the principal entity to the dependent entity.
        /// If there is no inverse navigation, pass `null`.
        /// </param>
        /// <param name="foreignKeyExpression">
        /// An expression specifying the foreign key property on the dependent entity.
        /// </param>
        public static void ConfigureRestrictOneToMany<TEntity, TRelatedEntity>(
            this ModelBuilder modelBuilder,
            Expression<Func<TEntity, TRelatedEntity?>> navigationExpression,
            Expression<Func<TRelatedEntity, IEnumerable<TEntity>?>> inverseNavigationExpression,
            Expression<Func<TEntity, object?>> foreignKeyExpression)
            where TEntity : class
            where TRelatedEntity : class
        {
            modelBuilder.Entity<TEntity>()
                .HasOne(navigationExpression)
                .WithMany(inverseNavigationExpression)
                .HasForeignKey(foreignKeyExpression)
                .OnDelete(DeleteBehavior.Restrict);
        }

        /// <summary>
        /// Configures a one-to-one relationship with `DeleteBehavior.Restrict` to prevent cascade delete.
        /// </summary>
        /// <typeparam name="TEntity">The entity type for the relationship.</typeparam>
        /// <typeparam name="TRelatedEntity">The related entity type for the relationship.</typeparam>
        /// <param name="modelBuilder">The instance of ModelBuilder to configure the entity.</param>
        /// <param name="navigationExpression">
        /// An expression specifying the navigation property from the dependent entity to the principal entity.
        /// </param>
        /// <param name="inverseNavigationExpression">
        /// An expression specifying the inverse navigation property from the principal entity to the dependent entity.
        /// If there is no inverse navigation, pass `null`.
        /// </param>
        /// <param name="foreignKeyExpression">
        /// An expression specifying the foreign key property on the dependent entity.
        /// </param>
        public static void ConfigureRestrictOneToOne<TEntity, TRelatedEntity>(
            this ModelBuilder modelBuilder,
            Expression<Func<TEntity, TRelatedEntity?>> navigationExpression,
            Expression<Func<TRelatedEntity, TEntity?>> inverseNavigationExpression,
            Expression<Func<TEntity, object?>> foreignKeyExpression)
            where TEntity : class
            where TRelatedEntity : class
        {
            modelBuilder.Entity<TEntity>()
                .HasOne(navigationExpression)
                .WithOne(inverseNavigationExpression)
                .HasForeignKey(foreignKeyExpression)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
