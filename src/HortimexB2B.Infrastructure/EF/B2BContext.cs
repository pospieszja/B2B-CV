using HortimexB2B.Core.Domain;
using HortimexB2B.Core.Domain.ShoppingCart;
using Microsoft.EntityFrameworkCore;

namespace HortimexB2B.Infrastructure.EF
{
    public class B2BContext : DbContext
    {
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<TermConsent> TermConsents { get; set; }

        public B2BContext(DbContextOptions<B2BContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Basket>().HasKey(x => new
            {
                x.UserId
            });

            builder.Entity<BasketItem>().HasKey(x => new
            {
                x.UserId,
                x.ProductId,
                x.Unit
            });

            builder.Entity<BasketItem>()
                .Ignore(b => b.ConvertedQuantity)
                .Ignore(b => b.UnitPrice)
                .Ignore(b => b.Vat)
                .Ignore(b => b.UnitName);

            builder.Entity<Notification>().HasKey(x => new
            {
                x.NotificationId
            });

            builder.Entity<TermConsent>().HasKey(x => new
            {
                x.UserName
            });
        }
    }
}