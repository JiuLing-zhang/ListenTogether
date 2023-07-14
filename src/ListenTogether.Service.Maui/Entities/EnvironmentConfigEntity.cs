﻿using SQLite;

namespace ListenTogether.Data.Maui.Entities;

[Table("EnvironmentConfig")]
internal class EnvironmentConfigEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }


    [Column("PlayerSetting")]
    public string PlayerSettingJson { get; set; } = null!;

    [Column("GeneralSetting")]
    public string GeneralSettingJson { get; set; } = null!;

    [Column("SearchSetting")]
    public string SearchSettingJson { get; set; } = null!;

    [Column("PlaySetting")]
    public string PlaySettingJson { get; set; } = null!;
}