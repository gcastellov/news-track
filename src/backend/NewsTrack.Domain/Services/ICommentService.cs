using NewsTrack.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace NewsTrack.Domain.Services
{
    public interface ICommentService
    {
        Task Save(Comment comment);
    }
}