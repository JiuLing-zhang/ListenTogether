using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace MusicPlayerOnline.Data.Entities;

[Table("EnvironmentConfig")]
internal class EnvironmentConfigEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    

    [Column("PlayerSetting")]
    public string PlayerSettingJson { get; set; } = null!;
}