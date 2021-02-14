using ModelsLayer.DatabaseEntities;
using System.Collections.Generic;

namespace ModelsLayer.Identity
{
    public class IndexGuestModel
    {
        public List<Event> Top5TrendingEvents { get; set; }
        public List<EditorPickEvent> EditorEvents { get; set; }
    }
}
