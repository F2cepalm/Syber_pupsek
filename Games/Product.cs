using Newtonsoft.Json;

namespace CyberPupsekBot.Games
{
    internal struct Product
    {
        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public string Type { get; set; }
        [JsonProperty]
        public string Description { get; set; }
        [JsonProperty]
        public int Price { get; set; }
        [JsonProperty]
        public long Id { get; set; }
        public Product(string name, string type, string desc, int price, long id)
        {
            Name = name;
            Type = type;
            Description = desc;
            Price = price;
            Id = id;
        }
    }
}
