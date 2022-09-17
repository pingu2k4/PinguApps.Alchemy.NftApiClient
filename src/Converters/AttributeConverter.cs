using PinguApps.Alchemy.NftApiClient.Responses;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Attribute = PinguApps.Alchemy.NftApiClient.Responses.Attribute;

namespace PinguApps.Alchemy.NftApiClient.Converters
{
    internal sealed class AttributeConverter : JsonConverter<Attribute>
    {
        public override Attribute? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var document = JsonDocument.ParseValue(ref reader);

            var hasValue = document.RootElement.TryGetProperty("value", out var value);

            if (!hasValue)
            {
                return document.Deserialize<Attribute>();
            }

            if (value.ValueKind == JsonValueKind.String)
            {
                return document.Deserialize<StringAttribute>();
            }

            if (value.ValueKind == JsonValueKind.Number)
            {
                return document.Deserialize<DecimalAttribute>();
            }

            return document.Deserialize<Attribute>();
        }

        public override void Write(Utf8JsonWriter writer, Attribute value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
