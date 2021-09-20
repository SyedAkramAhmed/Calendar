namespace Calendar.API.Extension
{
    public static class Extenstion
    {
        public static string VerifyIsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value) ? null : value;
        }
    }
}
