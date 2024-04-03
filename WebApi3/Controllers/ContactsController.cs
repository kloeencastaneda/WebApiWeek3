using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi3.Controllers
{
    public class ContactsController : ApiController
    {
        public IHttpActionResult GetAllContacts()
        {
            IList<Contact> contacts = null;

            using (var ctx = new WebApi2Entities())
            {
               contacts = ctx.Contacts.ToList<Contact>();
                           
            }

            if (contacts.Count == 0)
            {
                return NotFound();
            }

            return Ok(contacts);
        }
        public IHttpActionResult GetContactById(int id)
        {
            Contact contacts = null;

            using (var ctx = new WebApi2Entities())
            {
                contacts = ctx.Contacts.Where(s => s.id==id).FirstOrDefault();

            }

            return Ok(contacts);
        }

        //Get action methods of the previous section
        public IHttpActionResult PostNewStudent(Contact contacts)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new WebApi2Entities())
            {
                ctx.Contacts.Add(contacts);
                ctx.SaveChanges();
            }

            return Ok();
        }

        public IHttpActionResult Put(Contact contacts)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new WebApi2Entities())
            {
                var existingContact = ctx.Contacts.Where(s => s.id == contacts.id)
                                                        .FirstOrDefault<Contact>();

                if (existingContact != null)
                {
                    existingContact.FirstName = contacts.FirstName;
                    existingContact.LastName = contacts.LastName;
                    existingContact.Email = contacts.Email;
                    existingContact.Phone = contacts.Phone;

                    ctx.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok();
        }
        public IHttpActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid contact id");

            using (var ctx = new WebApi2Entities())
            {
                var contacts = ctx.Contacts
                    .Where(s => s.id == id)
                    .FirstOrDefault();

                ctx.Entry(contacts).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
    }
}
