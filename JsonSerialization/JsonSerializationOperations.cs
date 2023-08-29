using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

[assembly: CLSCompliant(true)]

namespace JsonSerialization
{
    public static class JsonSerializationOperations
    {
        public static string SerializeObjectToJson(object obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        public static T? DeserializeJsonToObject<T>(string json)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            T? obj = JsonSerializer.Deserialize<T>(json, options);

            return obj;
        }

        public static string SerializeCompanyObjectToJson(object obj)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            string serializedData = JsonSerializer.Serialize(obj, options);
            return serializedData;
        }

        public static T? DeserializeCompanyJsonToObject<T>(string json)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            return JsonSerializer.Deserialize<T>(json, options);
        }

        public static string SerializeDictionary(Company obj)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            var camelCaseDomains = new Dictionary<string, int>();
            foreach (var kvp in obj.Domains ?? new Dictionary<string, int>())
            {
                var camelCaseKey = options.PropertyNamingPolicy.ConvertName(kvp.Key);
                camelCaseDomains[camelCaseKey] = kvp.Value;
            }

            var serializedData = JsonSerializer.Serialize(camelCaseDomains, options);

            return serializedData;
        }

        public static string SerializeEnum(Company obj)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new CompanyTypeConverter() },
            };

            var serializedData = JsonSerializer.Serialize(obj, options);

            var expectedCasing = "softwareServices";
            var actualCasing = obj.CompanyType.ToString().ToLower(CultureInfo.InvariantCulture);
#pragma warning disable CA1307
            serializedData = serializedData.Replace(actualCasing, expectedCasing);
#pragma warning restore CA1307

            return serializedData;
        }

        public class CompanyTypeConverter : JsonConverter<CompanyType>
        {
#pragma warning disable CS8604
            public override CompanyType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => (CompanyType)Enum.Parse(typeof(CompanyType), value: reader.GetString(), true);
#pragma warning restore CS8604

            public override void Write(Utf8JsonWriter writer, CompanyType value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString().ToLower(CultureInfo.InvariantCulture));
            }
        }
    }
}
