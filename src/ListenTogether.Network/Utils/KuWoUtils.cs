namespace ListenTogether.Network.Utils;
internal class KuWoUtils
{
    private static readonly Random _random = new Random();
    public static string GetN()
    {
        /*
            var n = function() {
                function t() {
                    return (65536 * (1 + Math.random()) | 0).toString(16).substring(1)
                }
                return t() + t() + t() + t() + t() + t() + t() + t()
            }();
        */
        try
        {
            return $"{GetT()}{GetT()}{GetT()}{GetT()}{GetT()}{GetT()}{GetT()}{GetT()}";
        }
        catch (Exception)
        {

            return "";
        }
    }

    private static string GetT()
    {
        //65536 * (1 + Math.random()) | 0
        var num = _random.Next(65536, 65536 * 2);
        return Convert.ToString(num, 16).Substring(1);
    }

}