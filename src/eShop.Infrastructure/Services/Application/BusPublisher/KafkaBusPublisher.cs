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
        private readonly IProducer<Null, byte[]> _jsonProducer;

        public KafkaBusPublisher(KafkaClientHandle clientHandle, IDateTimeService dateTimeService)
        {
            _clientHandle = clientHandle;
            _dateTimeService = dateTimeService;

            _jsonProducer = new DependentProducerBuilder<Null, byte[]>(_clientHandle.Handle)
                .Build();
        }

        public Task PublishAsync<TMessage>(string topic, TMessage message, CancellationToken cancelationToken = default)
        {
            return _jsonProducer.ProduceAsync(topic,
                new Message<Null, byte[]>()
                {
                    Value = JsonSerializer.SerializeToUtf8Bytes(message, typeof(TMessage)),
                    Timestamp = new Timestamp(_dateTimeService.Now)
                }, cancelationToken);
        }

        public void Publish<TMessage>(string topic, TMessage message)
        {
            _jsonProducer.Produce(topic, 
                new Message<Null, byte[]>() 
                { 
                    Value = JsonSerializer.SerializeToUtf8Bytes(message, typeof(TMessage)),
                    Timestamp = new Timestamp(_dateTimeService.Now)
                });
        }
    }
}
