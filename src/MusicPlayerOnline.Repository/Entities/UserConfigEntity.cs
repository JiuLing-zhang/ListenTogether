
using SQLite;

namespace MusicPlayerOnline.Repository.Entities
{
    [Table("UserConfig")]
    public class UserConfigEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Column("GeneralSetting")]
        public string GeneralSettingJson { get; set; } = null!;

        [Column("SearchSetting")]
        public string SearchSettingJson { get; set; } = null!;

        [Column("PlaySetting")]
        public string PlaySettingJson { get; set; } = null!;
    }
}
