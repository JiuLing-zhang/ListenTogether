﻿using SQLite;

namespace MusicPlayerOnline.Repository.Entities
{
    [Table("Music")]
    public class MusicEntity
    {
        [PrimaryKey]
        public string Id { get; set; } = null!;
        public int Platform { get; set; }
        public string PlatformId { get; set; } = null!;
        public string CachePath { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Alias { get; set; } = null!;
        public string Artist { get; set; } = null!;
        public string Album { get; set; } = null!;
        public int Duration { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string PlayUrl { get; set; } = null!;
        public string Lyric { get; set; } = null!;
    }
}