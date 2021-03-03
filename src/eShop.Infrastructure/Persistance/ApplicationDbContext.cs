using eShop.Application.Persistance;
using eShop.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eShop.Infrastructure.Persistance
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext, ICheckpointProvider
    {
        public DbSet<Product> Products => Set<Product>();

        protected DbSet<Checkpoint> Checkpoints => Set<Checkpoint>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {

        }

        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task<Checkpoint> GetCheckpoint(string id, CancellationToken cancelationToken = default)
        {
            var checkpoint = await Checkpoints.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancelationToken);

            if (checkpoint == null)
            {
                checkpoint = new Checkpoint(id);
            }

            return checkpoint;
        }

        public async Task StoreCheckpoint(Checkpoint checkpoint, CancellationToken cancelationToken = default)
        {
            var existentCheckpoint = await Checkpoints.FirstOrDefaultAsync(e => e.Id == checkpoint.Id, cancelationToken);

            if(existentCheckpoint == null)
            {
                existentCheckpoint = new Checkpoint(checkpoint.Id);
                Checkpoints.Add(existentCheckpoint);
            }

            existentCheckpoint.UpdatePosition(checkpoint.Position ?? throw new Exception("Cannot set null position to checkpoint"));
            await SaveChangesAsync(cancelationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
