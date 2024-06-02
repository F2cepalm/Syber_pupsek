using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace CyberPupsekBot.Games
{
    internal class ShopInstance
    {
        public static List<ThingModel> things {  get; set; }
        public List<ThingModel> specifiedThings { get; set; }

        internal static void GetShop(Update upd)
        {
            if(things.Count != 0)
            {

            }
        }
    }
}
