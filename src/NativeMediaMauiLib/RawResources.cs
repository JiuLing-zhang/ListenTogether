namespace NativeMediaMauiLib
{
    internal class RawResources
    {
        private static byte[] _icon;
        public static byte[] Icon
        {
            get
            {
                if (_icon == null)
                {
                    _icon = LoadIconAsync().Result;
                }
                return _icon;
            }
        }

        private static async Task<byte[]> LoadIconAsync()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("icon.png");
            using var reader = new StreamReader(stream);
            var ms = new MemoryStream();
            stream.CopyTo(ms);
            return ms.ToArray();
        }
    }
}
