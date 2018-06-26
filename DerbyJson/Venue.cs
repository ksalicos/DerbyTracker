namespace DerbyJson
{
    public class Venue
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Url { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string OtherAddr { get; set; }
        public string Phone { get; set; }
        public string Pob { get; set; }
        public string PostCode { get; set; }
        public string Street { get; set; }
        public NoteObject[] Notes { get; set; }
        public string[] Uuid { get; set; }
        public Logo Logo { get; set; }
    }
}