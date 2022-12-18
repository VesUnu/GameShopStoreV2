using GameShopStoreV2.Core.Common;
using GameShopStoreV2.Core.Items.Contacts;
using GameShopStoreV2.Data.EF;
using GameShopStoreV2.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Application.ItemServices.Contacts
{
    public class ContactService : IContactService
    {
        private readonly GameShopStoreVTwoDBContext _context;

        public ContactService(GameShopStoreVTwoDBContext context)
        {
            _context = context;
        }

        public async Task<ResultApi<List<ContactViewModel>>> GetContact()
        {
            var contacts = await _context.Contacts.Select(x => new ContactViewModel()
            {
                Email = x.Email,
                Title = x.Title,
                Content = x.Content,
                Receiveddate = x.DateReceived
            }).ToListAsync();
            if (contacts == null)
            {
                return new ErrorResultApi<List<ContactViewModel>>("There are no comments!");
            }
            else return new SuccessResultApi<List<ContactViewModel>>(contacts);
        }

        public async Task<ResultApi<bool>> SendContact(ContactSendForRequest request)
        {
            if (request == null)
            {
                return new ErrorResultApi<bool>("Contact send command has failed!");
            }
            var newContact = new Contact()
            {
                Email = request.Email,
                Title = request.Title,
                Content = request.Content,
                DateReceived = DateTime.Now,
            };
            _context.Contacts.Add(newContact);
            await _context.SaveChangesAsync();
            return new SuccessResultApi<bool>();
        }
    }
}
