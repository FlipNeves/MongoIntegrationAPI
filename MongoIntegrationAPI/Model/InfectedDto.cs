using MongoDB.Driver.GeoJsonObjectModel;

namespace MongoIntegrationAPI.Model
{
    public class InfectedDto
    {
        public DateTime Birthday { get; set; }
        public string Sex { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}