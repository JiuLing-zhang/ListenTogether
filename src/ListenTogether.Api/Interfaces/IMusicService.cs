using ListenTogether.Model.Api;
using ListenTogether.Model.Api.Request;
using ListenTogether.Model.Api.Response;

namespace ListenTogether.Api.Interfaces;
public interface IMusicService
{
    Task<Result<MusicResponse>> GetOneAsync(string id);
    Task<Result> AddOrUpdateAsync(MusicRequest music);
}