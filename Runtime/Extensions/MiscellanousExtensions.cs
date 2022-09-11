namespace llagache
{
    public static class MiscellanousExtensions
    {
        public static string ToDotString(this float f)
        {
            return f.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}