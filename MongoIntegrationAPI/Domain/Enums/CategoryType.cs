using System.ComponentModel;

namespace MongoIntegrationAPI.Domain.Enums
{
    public enum CategoryType
    {
        [Description("Ficção Científica explorando o futuro e a tecnologia.")]
        SciFi = 1,
        
        [Description("Mundos mágicos e aventuras épicas.")]
        Fantasy = 2,
        
        [Description("Histórias baseadas em fatos e eventos reais.")]
        NonFiction = 3,
        
        [Description("Investigação de crimes e quebra-cabeças complexos.")]
        Mystery = 4,
        
        [Description("Livros técnicos e acadêmicos sobre tecnologia na informação.")]
        Technology = 5
    }
}
