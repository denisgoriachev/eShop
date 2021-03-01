using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Domain
{
    public interface IAggregateStore
    {
        Task<TAggregate> Load<TAggregate>(Guid id) where TAggregate : Aggregate;

        Task Save<TAggregate>(TAggregate aggregate) where TAggregate : Aggregate;

        Task<bool> Exists<TAggregate>(Guid id) where TAggregate : Aggregate;
    }
}
