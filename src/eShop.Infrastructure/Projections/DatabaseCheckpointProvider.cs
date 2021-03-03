using eShop.Application.Persistance;
using eShop.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eShop.Infrastructure.Projections
{
    public class DatabaseCheckpointProvider : ICheckpointProvider
    {
        private readonly ApplicationDbContext _context;

        public DatabaseCheckpointProvider(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Checkpoint> GetCheckpoint(string id, CancellationToken cancelationToken = default)
        {
            var checkpoint = await _context.Checkpoints.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancelationToken);

            if (checkpoint == null)
            {
                checkpoint = new Checkpoint(id);
            }

            return checkpoint;
        }

        public async Task StoreCheckpoint(Checkpoint checkpoint, CancellationToken cancelationToken = default)
        {
            var existentCheckpoint = await _context.Checkpoints.FirstOrDefaultAsync(e => e.Id == checkpoint.Id, cancelationToken);

            if (existentCheckpoint == null)
            {
                existentCheckpoint = new Checkpoint(checkpoint.Id);
                _context.Checkpoints.Add(existentCheckpoint);
            }

            existentCheckpoint.UpdatePosition(checkpoint.Position ?? throw new Exception("Cannot set null position to checkpoint"));
            await _context.SaveChangesAsync(cancelationToken);
        }
    }
}
