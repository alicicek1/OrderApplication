namespace OrderApplication.Core.Extension
{
    public static class ObjectExtension
    {
        public static bool IsNull(this object objectToCall)
        {
            return objectToCall == null || Convert.IsDBNull(objectToCall);
        }
    }
}
