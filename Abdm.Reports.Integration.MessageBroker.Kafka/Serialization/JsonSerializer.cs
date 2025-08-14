using System.Text;
using System.Text.Json;
using Confluent.Kafka;

namespace Abdm.Integration.MessageBroker.Kafka.Serialization;

public class JsonSerializer<T> : ISerializer<T>, IDeserializer<T>
    where T : class
{
    public byte[] Serialize(T data, SerializationContext context)
    {
        var model = JsonSerializer.Serialize(data);
        return Encoding.UTF8.GetBytes(model);
    }

    public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        return isNull ? null! : JsonSerializer.Deserialize<T>(data)!;
    }
}
