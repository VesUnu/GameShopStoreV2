using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Application.CommonServices
{
    public interface IStorageService
    {
        string GetFileUrl(string filename);
        Task SaveFileAsync(Stream mediaBinaryStream, string filename);
        Task DeleteFileAsync(string filename);
    }
}
