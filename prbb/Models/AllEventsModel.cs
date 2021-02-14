using ModelsLayer.DatabaseEntities;
using System.Collections.Generic;

namespace ModelsLayer.Identity
{
    public class AllEventsModel
    {
        public List<Event> AllEvents { get; set; }
        public List<EditorPickEvent> AllEditorEvents { get; set; }
    }
}
