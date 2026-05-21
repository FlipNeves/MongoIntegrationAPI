using System.ComponentModel;

namespace MongoIntegrationAPI.Domain.Enums
{
    public enum CategoryType
    {
        [Description("Science fiction exploring the future and technology.")]
        SciFi = 1,

        [Description("Magical worlds and epic adventures.")]
        Fantasy = 2,

        [Description("Stories based on real facts and events.")]
        NonFiction = 3,

        [Description("Crime investigations and complex puzzles.")]
        Mystery = 4,

        [Description("Technical and academic books about information technology.")]
        Technology = 5
    }
}
