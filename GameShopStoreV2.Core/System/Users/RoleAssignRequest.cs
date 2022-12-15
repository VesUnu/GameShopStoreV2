using GameShopStoreV2.Core.Common;

namespace GameShopStoreV2.Core.System.Users
{
    public class RoleAssignRequest
    {
        public Guid Id { get; set; }
        public List<SelectItem> Roles { get; set; } = new List<SelectItem>();
    }
}