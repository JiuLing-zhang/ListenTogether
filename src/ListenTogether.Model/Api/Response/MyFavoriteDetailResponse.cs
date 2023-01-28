namespace ListenTogether.Model.Api.Response
{
    /// <summary>
    /// 收藏的明细
    /// </summary>
    public class MyFavoriteDetailResponse
    {
        public int Id { get; set; }
        public int MyFavoriteId { get; set; }
        public LocalMusic Music { get; set; } = null!;
    }
}
