using GameShopStoreV2.Core.System.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Application.SystemServices.RoleServices
{
    public interface IRoleService
    {
        Task<List<RoleViewModel>> GetAll();
    }
}
