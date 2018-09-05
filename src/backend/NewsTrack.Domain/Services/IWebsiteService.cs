using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsTrack.Domain.Services
{
    public interface IWebsiteService
    {
        Task Save(IEnumerable<Uri> uris);
    }
}