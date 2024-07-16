using MongoDB.Driver.GeoJsonObjectModel;

namespace MongoIntegrationAPI.Data.Collections
{
    public class Infectado
    {
        public Infectado(DateTime birthday, string sex, double latitude, double longitude) 
        {
            Birthday = birthday;
            Sex = sex;
            Localization = new GeoJson2DGeographicCoordinates(latitude, longitude);
        }

        public DateTime Birthday { get; set; }
        public string Sex { get; set; }
        public GeoJson2DGeographicCoordinates? Localization { get; set; }
    }
}
