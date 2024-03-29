﻿namespace ListenTogether.Services;
public class MusicResultService
{
    private readonly MusicPlayerService _musicPlayerService;
    private readonly IPlaylistService _playlistService;
    private readonly IMyFavoriteService _myFavoriteService;
    private readonly IMusicService _musicService;
    private readonly MusicNetPlatform _musicNetPlatform;
    public MusicResultService(IPlaylistService playlistService, MusicPlayerService musicPlayerService, IMyFavoriteService myFavoriteService, IMusicService musicService, MusicNetPlatform musicNetPlatform
        )
    {
        _playlistService = playlistService;
        _musicPlayerService = musicPlayerService;
        _myFavoriteService = myFavoriteService;
        _musicService = musicService;
        _musicNetPlatform = musicNetPlatform;
    }

    public async Task PlayAllAsync(List<LocalMusic> musics)
    {
        musics = await UpdateMusicDetail(musics);
        await AddToPlaylistAsync(musics);
        await PlayMusicAsync(musics.First());
    }

    public async Task PlayAsync(LocalMusic music)
    {
        music = await UpdateMusicDetail(music);
        await AddToPlaylistAsync(music);
        await PlayMusicAsync(music);
    }

    private async Task AddToPlaylistAsync(LocalMusic music)
    {
        var playlist = new Playlist()
        {
            Id = music.Id,
            IdOnPlatform = music.IdOnPlatform,
            Platform = music.Platform,
            Name = music.Name,
            Artist = music.Artist,
            Album = music.Album,
            ImageUrl = music.ImageUrl,
            ExtendDataJson = music.ExtendDataJson,
            EditTime = DateTime.Now
        };
        await _playlistService.AddOrUpdateAsync(playlist);
    }

    private async Task AddToPlaylistAsync(List<LocalMusic> musics)
    {
        var playlists = musics.Select(
            x => new Playlist()
            {
                Id = x.Id,
                IdOnPlatform = x.IdOnPlatform,
                Platform = x.Platform,
                Name = x.Name,
                Artist = x.Artist,
                Album = x.Album,
                ImageUrl = x.ImageUrl,
                ExtendDataJson = x.ExtendDataJson,
                EditTime = DateTime.Now
            }).ToList();

        await _playlistService.AddOrUpdateAsync(playlists);
    }

    private async Task PlayMusicAsync(LocalMusic music)
    {
        await _musicPlayerService.PlayAsync(music.Id);
    }

    /// <summary>
    /// 更新歌曲信息
    /// </summary>
    private async Task<LocalMusic> UpdateMusicDetail(LocalMusic music)
    {
        var myMusic = music;
        if (myMusic.ImageUrl.IsEmpty() && myMusic.Platform == Model.Enums.PlatformEnum.KuGou)
        {
            myMusic.ImageUrl = await _musicNetPlatform.GetImageUrlAsync((NetMusicLib.Enums.PlatformEnum)myMusic.Platform, myMusic.IdOnPlatform, myMusic.ExtendDataJson);
        }
        return myMusic;
    }

    /// <summary>
    /// 更新歌曲信息
    /// </summary>
    private async Task<List<LocalMusic>> UpdateMusicDetail(List<LocalMusic> musics)
    {
        var myMusics = musics;
        for (int i = 0; i < myMusics.Count; i++)
        {
            myMusics[i] = await UpdateMusicDetail(myMusics[i]);
        }
        return myMusics;
    }
}