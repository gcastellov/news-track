using System;
using System.Threading.Tasks;
using NewsTrack.Browser.Dtos;

namespace NewsTrack.Browser
{
    public interface IBroswer
    {
        Task<ResponseDto> Get(string url);
        Task<string> GetContent(string url);
        Task<ResponseDto> Set(Uri uri, string content);
    }
}