using BleemSync.Data.Abstractions;
using BleemSync.Data.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BleemSync.ViewModels
{
    public class GameManagerNodeTreeItem
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "parent")]
        public string Parent { get; set; }

        public GameManagerNodeTreeItem() { }

        public GameManagerNodeTreeItem(GameManagerNode node)
        {
            Id = node.Id.ToString();
            Name = node.Name;
            Type = Enum.GetName(typeof(GameManagerNodeType), node.Type);
            Parent = node.ParentId == null ? "#" : node.ParentId.ToString();
        }
    }
}
