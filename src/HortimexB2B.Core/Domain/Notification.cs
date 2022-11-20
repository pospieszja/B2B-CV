using System.Collections.Generic;

namespace HortimexB2B.Core.Domain
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string Content { get; set; }
        public string Language { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }

        public void ReplaceTagWithValue(Dictionary<string, string> tags)
        {
            foreach (var tag in tags)
            {
                Content = Content.Replace("[" + tag.Key + "]", tag.Value);
            }
        }
    }
}
