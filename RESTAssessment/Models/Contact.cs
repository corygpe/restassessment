namespace RESTAssessment.Models
{
    public class Contact
    {
        public long id { get; set; }
        public ContactName name { get; set; }
        public ContactAddress address { get; set; }
        public List<ContactPhone> phone { get; set; }
        public string email { get; set; }
    }
}