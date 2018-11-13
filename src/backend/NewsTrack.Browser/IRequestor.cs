using System;
using System.Threading.Tasks;

namespace NewsTrack.Browser
{
    public interface IRequestor : IDisposable
    {
        Task<string> Get(Uri uri);
    }
}