﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Mod03.Repository;
using Mod03.Models;
using Mod03.Attributes;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Mod03.Controllers
{
    [DebugFilter]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        [FromServices]
        public IContactsRepository ContactsRepo { get; set; }

        [HttpGet]
        public IEnumerable<Contacts> GetAll()
        {
            return ContactsRepo.GetAll();
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var item = ContactsRepo.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Contacts item)
        {
            if (item == null)
            {
                return HttpBadRequest();
            }
            ContactsRepo.Add(item);
            return CreatedAtRoute("GetContacts", new { Controller = "Contacts", id = item.MobilePhone }, item);
        }

        [HttpDelete("{id}", Name = "GetContacts")]
        public void Delete(string id)
        {
            ContactsRepo.Remove(id);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] Contacts item)
        {
            if (item == null)
            {
                return HttpBadRequest();
            }
            var contactObj = ContactsRepo.Find(id);
            if (contactObj == null)
            {
                return HttpNotFound();
            }

            item.MobilePhone = id;
            if (null == item.FirstName)
                item.FirstName = contactObj.FirstName;

            if (null == item.LastName)
                item.LastName = contactObj.LastName;

            if (null == item.Email)
                item.Email = contactObj.Email;

            if (null == item.Company)
                item.Company = contactObj.Company;

            ContactsRepo.Update(item);
            return new NoContentResult();
        }
    }
}
