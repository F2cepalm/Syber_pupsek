using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberPupsekBot.Games
{

    [Serializable]
    public class Bullet
    {
        [JsonProperty]
        public bool IsReal;
        public Bullet(bool status)
        {
            IsReal = status;
        }

        public static Bullet[] CreateBulletPack(int generalCount, int realCount)
        {
            Bullet[] bullets = new Bullet[generalCount];
            for (int i = 0; i < generalCount; i++)
            {
                bullets[i] = new Bullet(false);
            }
            for (int i = 0; i < realCount; i++)
            {
                bullets[i] = new Bullet(true);
            }
            Random rnd = new Random();
            bullets = bullets.OrderBy(x => rnd.Next()).ToArray();
            bullets = bullets.OrderBy(x => rnd.Next()).ToArray();
            return bullets;
        }
    }
}
