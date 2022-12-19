using GameShopStoreV2.Core.Common;
using GameShopStoreV2.Core.Items.Contacts;

namespace GameShopStoreV2.Admin.Services
{
    public interface IContactApiClient
    {
        Task<ResultApi<List<ContactViewModel>>> GetContacts();
    }
}