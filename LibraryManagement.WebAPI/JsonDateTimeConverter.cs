using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LibraryManagement.WebAPI
{
    public class JsonDateTimeConverter : JsonConverter<DateTime>
    {
        private readonly string[] _dateFormats = new string[]
        {
        "yyyy-MM-dd HH:mm:ss.ffffffZ",  // Örneğin: 2025-01-30 10:57:07.3938768Z
        "yyyy-MM-ddTHH:mm:ss.fffZ",      // Örneğin: 2025-02-17T12:11:07.640Z
        "yyyy-MM-ddTHH:mm:ss.ffffffZ"    // Alternatif format: 2025-01-30T10:57:07.3938768Z
        };

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dateString = reader.GetString();

            // Birden fazla format ile kontrol edilerek dönüştürme işlemi yapılır.
            foreach (var format in _dateFormats)
            {
                if (DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime result))
                {
                    return result;
                }
            }

            // Eğer hiçbir formatla eşleşmediyse, hata döndürebiliriz
            throw new FormatException($"Tarih '{dateString}' belirlenen formatta değil.");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // Tarihi belirtilen formata dönüştürüp yazıyoruz
            writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
        }
    }

}


