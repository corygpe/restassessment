using LiteDB;
using Microsoft.AspNetCore.Mvc;

using RESTAssessment.Models;

namespace RESTAssessment.Controllers
{
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ContactsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //  List all contacts
        [Route("contacts")]
        [HttpGet]
        public IEnumerable<Contact> Get()
        {
            using (LiteDatabase db = new LiteDatabase(_configuration.GetValue<string>("DBPath")))
            {
                var col = db.GetCollection<Contact>("contacts", BsonAutoId.Int64);
                IEnumerable<Contact> ret = col.FindAll();
                return ret.ToArray();
            }
        }

        //  Create a new contact
        [Route("contacts")]
        [HttpPost]
        public Contact Post([FromBody] Contact contact)
        {
            using (LiteDatabase db = new LiteDatabase(_configuration.GetValue<string>("DBPath")))
            {
                var col = db.GetCollection<Contact>("contacts", BsonAutoId.Int64);
                BsonValue id = col.Insert(contact);
                return col.FindById(id);
            }
        }

        //  Update a contact
        [Route("contacts/{id}")]
        [HttpPut]
        public Contact Put(long id, [FromBody] Contact contact)
        {
            using (LiteDatabase db = new LiteDatabase(_configuration.GetValue<string>("DBPath")))
            {
                var col = db.GetCollection<Contact>("contacts", BsonAutoId.Int64);
                if (col.Update(id, contact))
                {
                    return col.FindById(id);
                }
                return null;
            }
        }

        //  Get a specific contact
        [Route("contacts/{id}")]
        [HttpGet]
        public Contact Get(long id)
        {
            using (LiteDatabase db = new LiteDatabase(_configuration.GetValue<string>("DBPath")))
            {
                var col = db.GetCollection<Contact>("contacts", BsonAutoId.Int64);
                return col.FindById(id);
            }
        }

        //  Delete a contact
        [Route("contacts/{id}")]
        [HttpDelete]
        public bool Delete(long id)
        {
            using (LiteDatabase db = new LiteDatabase(_configuration.GetValue<string>("DBPath")))
            {
                var col = db.GetCollection<Contact>("contacts", BsonAutoId.Int64);
                return col.Delete(id);
            }
        }

        //  Get a call list
        [Route("contacts/call-list")]
        [HttpGet]
        public IEnumerable<CallListElement> GetCallList()
        {
            using (LiteDatabase db = new LiteDatabase(_configuration.GetValue<string>("DBPath")))
            {
                var col = db.GetCollection<Contact>("contacts", BsonAutoId.Int64);
                IEnumerable<Contact> contacts = col.FindAll().Where(x =>
                    x.phone.Where(y => y.type.ToUpper() == "HOME").ToArray().Length > 0
                );
                IEnumerable<CallListElement> ret = from x in contacts select new CallListElement
                {
                    name = x.name,
                    phone = x.phone.Where(y => y.type.ToUpper() == "HOME").ToArray()[0].number
                };
                return ret.ToArray().OrderBy(x => x.name.first).OrderBy(x => x.name.last);
            }
        }
    }
}