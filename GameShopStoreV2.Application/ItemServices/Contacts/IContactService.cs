using GameShopStoreV2.Core.Common;
using GameShopStoreV2.Core.Items.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Application.ItemServices.Contacts
{
    public interface IContactService
    {
        public Task<ResultApi<bool>> SendContact(ContactSendForRequest request);

        public Task<ResultApi<List<ContactViewModel>>> GetContact();
    }
}
