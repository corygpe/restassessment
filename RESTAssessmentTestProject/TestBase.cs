using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

using RESTAssessment;
using RESTAssessment.Models;
using System.Net.Http.Json;

namespace RESTAssessmentTestProject
{
    public class Tests
    {
        private WebApplicationFactory<Program> _app;
        private readonly Contact _testContact = new Contact
        {
            name = new ContactName
            {
                first = "firstName",
                middle = "middleName",
                last = "lastName"
            },
            address = new ContactAddress
            {
                street = "addressStreet",
                city = "addressCity",
                state = "addressState",
                zip = "addressZip"
            },
            phone = new List<ContactPhone> {
                        new ContactPhone
                        {
                            number = "555-555-5555",
                            type = "home"
                        },
                        new ContactPhone
                        {
                            number = "555-555-5555",
                            type = "home"
                        }
                    },
            email = "a@b.com"
        };

        [SetUp]
        public void Setup()
        {
            _app = new WebApplicationFactory<Program>();
        }

        [Test]
        public async Task TestListAllContacts()
        {
            var client = _app.CreateClient();
            var response = await client.GetAsync("/contacts");
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        }

        [Test]
        public async Task TestCreateNewContact()
        {
            var client = _app.CreateClient();
            var response = await client.PostAsJsonAsync<Contact>("/contacts", _testContact);
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        }

        [Test]
        public async Task TestUpdateAContact()
        {
            var client = _app.CreateClient();
            var response = await client.PutAsJsonAsync<Contact>("/contacts/1", _testContact);
            Assert.That(response.StatusCode, Is.AnyOf(System.Net.HttpStatusCode.NoContent, System.Net.HttpStatusCode.OK));
        }

        [Test]
        public async Task TestGetASpecificContact()
        {
            var client = _app.CreateClient();
            var response = await client.GetAsync("/contacts/1");
            Assert.That(response.StatusCode, Is.AnyOf(System.Net.HttpStatusCode.NoContent, System.Net.HttpStatusCode.OK));
        }

        [Test]
        public async Task TestDeleteAContact()
        {
            var client = _app.CreateClient();
            var response = await client.DeleteAsync("/contacts/1");
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        }

        [Test]
        public async Task TestGetACallList()
        {
            var client = _app.CreateClient();
            var response = await client.GetAsync("/contacts/call-list");
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        }
    }
}