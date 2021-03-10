using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eShop.Application.Service
{
    public interface IBusPublisher
    {
        Task PublishAsync<TMessage>(string topic, TMessage message, CancellationToken cancelationToken = default);

        void Publish<TMessage>(string topic, TMessage message, CancellationToken cancelationToken = default);
    }
}
