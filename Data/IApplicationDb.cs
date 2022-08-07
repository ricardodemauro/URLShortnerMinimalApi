using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URLShortnerMinimalApi.Models;

namespace URLShortnerMinimalApi.Data
{
    public interface IApplicationDb
    {
        Task<ShortUrl> GetByChunckId(string chunckId, CancellationToken cancellationToken = default);

        Task<ShortUrl> Create(ShortUrl data);

        Task<List<ShortUrlNeat>> GetAll(CancellationToken cancellationToken = default);
    }
}
