namespace DerbyJson
{
    public class NoteObject
    {
        public string Note { get; set; }
        public string Author { get; set; }
        public string Date { get; set; }
        public NoteObject[] Notes { get; set; }
    }
}