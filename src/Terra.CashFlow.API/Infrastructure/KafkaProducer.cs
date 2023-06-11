using Confluent.Kafka;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Globalization;
using System.Text;
using Terra.CashFlow.API.Infrastructure.Interfaces;

namespace Terra.CashFlow.API.Infrastructure
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly ProducerConfig _producerConfig;

        public KafkaProducer(ProducerConfig producerConfig)
        {
            _producerConfig = producerConfig;
        }

        public async Task ProduceMessageAsync<TMessage>(string topic, TMessage message, CancellationToken cancellationToken) where TMessage : class
        {
            using var producer = new ProducerBuilder<string, TMessage>(_producerConfig)
                .SetValueSerializer(new KafkaNewtonsoftJsonSerializer<TMessage>())
                .Build();

            var kafkaMessage = new Message<string, TMessage>()
            {
                Key = Guid.NewGuid().ToString(),
                Value = message
            };

            await producer.ProduceAsync(topic, kafkaMessage, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            producer.Flush(TimeSpan.FromSeconds(1));
        }
    }

    public class KafkaNewtonsoftJsonSerializer<T> : ISerializer<T>
    {
        private readonly JsonSerializerSettings _settings;

        public KafkaNewtonsoftJsonSerializer(JsonSerializerSettings settings = null)
        {
            _settings = settings ?? CreateDefaultJsonSerializerSettings();
        }

        public T Deserialize(byte[] data)
        {
            var json = Encoding.ASCII.GetString(data);

            var result = JsonConvert.DeserializeObject<T>(json, _settings);

            return result;
        }

        public byte[] Serialize(T data, SerializationContext context)
        {
            var formatting = Formatting.None;

            var json = JsonConvert.SerializeObject(data, data?.GetType(), formatting, _settings);

            var result = Encoding.ASCII.GetBytes(json);

            return result;
        }

        protected virtual JsonSerializerSettings CreateDefaultJsonSerializerSettings()
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = { new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal } }
            };

            return settings;
        }
    }
}
