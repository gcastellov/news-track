using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsTrack.Domain.Entities;

namespace NewsTrack.Domain.Repositories
{
    public interface IDraftRepository
    {
        Task Save(Draft draft);
        Task<IEnumerable<Draft>> GetLatest(int page, int count);
        Task<IEnumerable<Draft>> GetMostViewed(int page, int count);
        Task<IEnumerable<Draft>> GetMostFucked(int page, int count);
        Task<IEnumerable<Draft>> Get(string website, string pattern, IEnumerable<string> tags, int page, int count);
        Task<IEnumerable<Draft>> Get(IEnumerable<string> tags);
        Task<IEnumerable<Draft>> GetLatest(int take);
        Task<long> Count();
        Task<long> Count(string website, string pattern, IEnumerable<string> tags);
        Task<IEnumerable<string>> GetTags();
        Task<IEnumerable<string>> GetTags(Guid id);
        Task<IDictionary<string, long>> GetTagsStats();        
        Task<IEnumerable<Draft>> Search(string pattern);
        Task<long> AddViews(Guid id);
        Task<long> AddFuck(Guid id);
        Task<Draft> Get(Guid id);
        Task SetRelationship(Guid id, long count);
        Task<IDictionary<string, long>> GetWebsiteStats(int take);
        Task<IEnumerable<Draft>> GetMostViewed(int take);
        Task<IEnumerable<Draft>> GetMostFucking(int take);
    }
}