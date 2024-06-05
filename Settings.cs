using CyberPupsekBot.Games;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace CyberPupsekBot.Settings
{
    interface ISettings
    {
        public long ID { get; set; }
        public string Name { get; set; }
    }

    public class ChatSettings : ISettings
    {
        public List<ChatSettings> Settings = new List<ChatSettings>();
        [JsonProperty]
        public long ID { get; set; }
        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public List<long> ChatIds { get; set; }
        [JsonProperty]
        public bool IsGroupMode { get; set; }
        [JsonProperty]
        public bool IsGameUsable { get; set; }
        [JsonProperty]
        public bool IsPremiumUsage { get; set; }
        [JsonProperty]
        public GameModel? GameModel { get; set; } //nullable
        [JsonProperty]
        public int CooldownInSec { get; set; }

        public ChatSettings(long Id, string name)
        {
            ID = Id;
            Name = name;
            ChatIds = new List<long>();
            IsGroupMode = false;
            IsGameUsable = false;
            IsPremiumUsage = false;
            GameModel = null;
            CooldownInSec = 2;
        }

        static void SaveSettings() //write the realization
        {

        }
    }
}
