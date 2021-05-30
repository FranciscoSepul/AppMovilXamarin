using PDRProvBackEnd.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace PDRProvBackEnd.Contexts
{
    public class PDRContext : DbContext
    {
        public PDRContext(DbContextOptions<PDRContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<MessageContact> MessageContacts { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserSupplier> UserSuppliers { get; set; }
        public DbSet<ReasonContact> ReasonContacts { get; set; }
        public DbSet<SupplierCertification> SupplierCertifications { get; set; }
    }
}