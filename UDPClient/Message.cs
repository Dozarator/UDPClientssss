using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace udpClient
{
    public class Message
    {
        public string Command { get; set; }
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
        public string IpTo { get; set; }
        public string IpFrom { get; set; }

        public Message(string command, string text, string ipTo, string ipFrom)
        {
            this.Command = command;
            this.Text = text;
            this.IpFrom = ipFrom;
            this.IpTo = ipTo;
            this.DateTime = DateTime.Now;
        }
        public string ToJSON()
        {
            return JsonSerializer.Serialize(this);

        }
        public static Message FromJSON(string json)
        {
            return JsonSerializer.Deserialize<Message>(json);
        }
    }
}