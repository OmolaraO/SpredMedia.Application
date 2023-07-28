using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace SpredMedia.CommonLibrary
{
    public static class UpdateDateTimeContext
    {
        private const string UPDATEDAT = "UpdatedAt";
        private const string CREATEDAT = "CreatedAt";

        public static void AuditPropertiesChange<T>(EntityState state, T obj) where T : class
        {
            PropertyInfo? value;
            switch (state)
            {
                case EntityState.Modified:
                    value = obj.GetType().GetProperty(UPDATEDAT);
                    if (value != null)
                        value.SetValue(obj, DateTime.UtcNow);
                    break;
                case EntityState.Added:
                    value = obj.GetType().GetProperty(CREATEDAT);
                    if (value != null)
                        value.SetValue(obj, DateTime.UtcNow);
                    value = obj.GetType().GetProperty(UPDATEDAT);
                    if (value != null)
                        value.SetValue(obj, DateTime.UtcNow);
                    break;
                default:
                    break;
            }
        }
    }
}
