// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using QuickType;
//
//    var sensorData = SensorData.FromJson(jsonString);

using System.Text.Json.Serialization;

namespace Temperature.Models
{
    public class Measurement{
        [JsonPropertyName("id")]
        public int Id { get; set; }
         [JsonPropertyName("name")]
          public string Name { get; set; }
           [JsonPropertyName("value")]
            public double Value { get; set; }
    }
}
