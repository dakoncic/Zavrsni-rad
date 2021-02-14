namespace ModelsLayer.DatabaseEntities
{
    public class EditorPickEvent
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string UserId { get; set; }
        public int NumberOfAttenders { get; set; }
        public Event Events { get; set; } //veza s parentom
    }
}
