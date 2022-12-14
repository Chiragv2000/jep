using autolandjepcore.Models;
using Microsoft.EntityFrameworkCore;

namespace autolandjepcore.Data
{
    public partial class jepconautoland:DbContext
    {
        public jepconautoland(DbContextOptions<jepconautoland> options) : base(options)
        {

        }
        public DbSet<jepsonmodel>? SP_xmldoc_autoland { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<jepsonmodel>(entity =>
            {
                entity
                    .HasNoKey()                
                    .ToView("SP_xmldoc_autoland");



            });

            OnModelCreatingPartial(modelBuilder);
        }


        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
