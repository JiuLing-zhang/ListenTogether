using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ListenTogether.Api.Entities
{
    [Table("UserConfig")]
    public class UserConfigEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserBaseId { get; set; }

        [Column("GeneralSetting")]
        public string GeneralSettingJson { get; set; } = null!;

        [Column("SearchSetting")]
        public string SearchSettingJson { get; set; } = null!;

        [Column("PlaySetting")]
        public string PlaySettingJson { get; set; } = null!;
    }
}
