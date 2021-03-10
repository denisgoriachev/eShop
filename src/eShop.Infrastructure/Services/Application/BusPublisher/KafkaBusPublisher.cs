using Confluent.Kafka;
using eShop.Application.Service;
using eShop.Common;
using eShop.Infrastructure.Services.Application.BusPublisher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace eShop.Infrastructure.Services.Application
{
    public class KafkaBusPublisher : IBusPublisher
    {
        private readonly KafkaClientHandle _clientHandle;
        private readonly IDateTimeService _dateTimeService;
        private readonly IProducer<Null, string> _jsonProducer;

        public KafkaBusPublisher(KafkaClientHandle clientHandle, IDateTimeService dateTimeService)
        {
            _clientHandle = clientHandle;
            _dateTimeService = dateTimeService;

            _jsonProducer = new DependentProducerBuilder<Null, string>(_clientHandle.Handle).Build();
        }

        public Task PublishAsync<TMessage>(string topic, TMessage message, CancellationToken cancelationToken = default)
        {
            return _jsonProducer.ProduceAsync(topic,
                new Message<Null, string>()
                {
                    Value = JsonSerializer.Serialize(message, typeof(TMessage)),
                    Timestamp = new Timestamp(_dateTimeService.Now)
                });
        }

        public void Publish<TMessage>(string topic, TMessage message, CancellationToken cancelationToken)
        {
            _jsonProducer.Produce(topic, 
                new Message<Null, string>() 
                { 
                    Value = JsonSerializer.Serialize(message, typeof(TMessage)),
                    Timestamp = new Timestamp(_dateTimeService.Now)
                });
        }
    }
}
