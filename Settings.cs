using CyberPupsekBot.Games;
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
        public long ID { get; set; }
        public string Name { get; set; }
        public List<long> ChatIds { get; set; }
        public bool IsGroupMode { get; set; }
        public bool IsGameUsable { get; set; }
        public bool IsPremiumUsage { get; set; }
        public GameModel? GameModel { get; set; } //nullable
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
    }
}
