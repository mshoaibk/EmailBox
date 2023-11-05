using EmailBox_Domain.TableEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailBox_Infrestructure.DataBaseContext
{
    public class EBContexts : DbContext
    {
        private readonly DbContextOptions _options;

        public EBContexts(DbContextOptions<EBContexts> options) : base(options)
        {
            _options = options;
        }
        protected override void OnModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tbl_User>().Property(e => e.Id).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblSignalRConnection>().Property(e => e.ID).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblPrivateEmail>().Property(e => e.PrivateEmailID).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblUserIdentifier>().Property(e => e.UserIdentifierId).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
        }
        public DbSet<Tbl_User> Tbl_User { get; set; }
        public DbSet<TblSignalRConnection> TblSignalRConnection { get; set; }
        public DbSet<TblEmailBox> TblEmailBox { get; set; }
        public DbSet<TblPrivateEmail> TblPrivateEmail { get; set; }
        public DbSet<TblUserIdentifier> TblUserIdentifier { get; set; }
    }
}
