using Newtonsoft.Json;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CyberPupsekBot.Games
{
    [Serializable]
    public class GameModel
    {
        [JsonProperty]
        public List<long>? Ids;
        [JsonProperty]
        public List<string>? UserNames;
        [JsonProperty]
        public long GameId;
        [JsonProperty]
        public Bullet[] Bullets;
        [JsonProperty]
        public int indexBullets = 0;
        [JsonProperty]
        public int generalRounds;
        [JsonProperty]
        public int Round;
        [JsonProperty]
        public int RootMessageId;
        [JsonProperty]
        public string Status;
        [JsonProperty]
        public string TurnUsername;
        [JsonProperty]
        public int indexUserNames = 0;
        [JsonProperty]
        public int AppendCoins;
        [JsonProperty]
        public int RemoveCoins;

        public GameModel()
        {

        }
    }
}
