using eShop.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eShop.Infrastructure.Projections
{
    public interface ICheckpointProvider
    {
        Task<Checkpoint> GetCheckpoint(string id, CancellationToken cancelationToken = default);

        Task StoreCheckpoint(Checkpoint checkpoint, CancellationToken cancelationToken = default);
    }
}
