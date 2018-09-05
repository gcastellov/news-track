using System;
using System.Threading.Tasks;

namespace NewsTrack.Browser
{
    public interface IRequestor
    {
        Task<string> Get(Uri uri);
    }
}