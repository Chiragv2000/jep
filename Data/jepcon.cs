using autolandjepcore.Models;
using Microsoft.EntityFrameworkCore;

namespace autolandjepcore.Data
{
    public partial class jepcon:DbContext
    {
        public jepcon(DbContextOptions<jepcon> options) : base(options)
        {

        }
        public DbSet<jepsonmodel>? SP_xmldoc_bak { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<jepsonmodel>(entity =>
            {
                entity
                    .HasNoKey()                
                    .ToView("SP_xmldoc_bak");



            });

            OnModelCreatingPartial(modelBuilder);
        }


        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
