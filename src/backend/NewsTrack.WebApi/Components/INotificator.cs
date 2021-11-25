using System.Threading.Tasks;

namespace NewsTrack.WebApi.Components
{
    public interface INotificator
    {
        Task SendEmail(string subject, string content, string recipient);
    }
}