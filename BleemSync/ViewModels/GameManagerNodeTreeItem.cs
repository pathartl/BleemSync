using BleemSync.Data.Entities;
using Newtonsoft.Json;
using System;

namespace BleemSync.ViewModels
{
    public class GameManagerNodeTreeItem
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

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
            Text = node.Name;
            Type = Enum.GetName(typeof(GameManagerNodeType), node.Type);
            Parent = node.ParentId == null ? "#" : node.ParentId.ToString();
        }

        public GameManagerNode ToGameManagerNode()
        {
            var node = new GameManagerNode()
            {
                Id = 0,
                Name = Text
            };

            switch (Type)
            {
                default:
                case "Game":
                    node.Type = GameManagerNodeType.Game;
                    break;

                case "Folder":
                    node.Type = GameManagerNodeType.Folder;
                    break;
            }

            return node;
        }
    }
}
