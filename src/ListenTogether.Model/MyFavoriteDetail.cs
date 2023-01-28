using ListenTogether.Model.Enums;

namespace ListenTogether.Model
{
    public class MyFavoriteDetail
    {
        public int Id { get; set; }
        public int MyFavoriteId { get; set; }
        public LocalMusic Music { get; set; } = null!;
    }
}
