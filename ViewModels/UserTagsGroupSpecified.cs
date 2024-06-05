using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberPupsekBot.Types
{
    internal class UserTagsGroupSpecified
    {
        [JsonProperty]
        internal List<string> UserTags {  get; set; }
        [JsonProperty]
        internal List<long> ActiveUserTags { get; set; }

        static internal Dictionary<long, UserTagsGroupSpecified> Read()
        {
            string link = "Saves.json";
            string fileText = System.IO.File.ReadAllText(link);
            Dictionary<long, UserTagsGroupSpecified> list = JsonConvert.DeserializeObject<Dictionary<long, UserTagsGroupSpecified>>(fileText);
            if (list != null)
            {
                return list;
            }
            else { return new Dictionary<long, UserTagsGroupSpecified>(); }
        }

        static internal void Write(Dictionary<long, UserTagsGroupSpecified> dlu)
        {
            string link = "Saves.json";
            string list = JsonConvert.SerializeObject(dlu);
            File.WriteAllText(link, list);
        }
    }
}
