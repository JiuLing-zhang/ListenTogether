using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;

namespace MusicPlayerOnline.Api.Interfaces
{
    public interface IMusicService
    {
        Task<Result<MusicDto>> GetOneAsync(string id);
        Task<Result> AddOrUpdateAsync(Music music);
        Task<Result> UpdateCacheAsync(string id, string cachePath);
    }
}
