using MusicPlayerOnline.Api.Models;
using MusicPlayerOnline.Model.Api;
using MusicPlayerOnline.Model.Api.Request;
using MusicPlayerOnline.Model.Api.Response;

namespace MusicPlayerOnline.Api.Interfaces
{
    public interface IMusicService
    {
        Task<Result<MusicResponse>> GetOneAsync(string id);
        Task<Result> AddOrUpdateAsync(MusicRequest music);
        Task<Result> UpdateCacheAsync(string id, string cachePath);
    }
}
