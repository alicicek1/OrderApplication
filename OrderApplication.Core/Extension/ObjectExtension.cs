namespace OrderApplication.Core.Extension
{
    public static class ObjectExtension
    {
        public static bool IsNull(this object objectToCall)
        {
            return objectToCall == null || Convert.IsDBNull(objectToCall);
        }
        public static T ConvertTo<T>(this object obj)
        {
            try
            {
                return ((T)obj);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
    }
}
