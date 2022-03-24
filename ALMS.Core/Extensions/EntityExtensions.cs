using System;
using System.Linq;
using System.Reflection;
using Elsa.NNF.Common.Library;
using Elsa.NNF.Data.ORM;
using Elsa.NNF.Data.ORM.Abilities;

namespace ALMS.Core
{
    public static class EntityExtensions
    {
        public static string GetEntityName(this IEntity entity, bool withCaseSentence = true)
        {
            var attributes = (KeyValueAttribute[])entity.GetType().GetCustomAttributes(typeof(KeyValueAttribute), false);
            var keyValueAttribute = attributes.Where(x => x.Key == "Name").ToArray();
            var entityName = (string)(keyValueAttribute.Length == 1 ? keyValueAttribute[0].Value : entity.GetType().Name);
            return withCaseSentence ? entityName.ToTitleCaseSentence() : entityName;
        }
        public static string GetValueOfKey(this object obj, string key)
        {
            var attributes = (KeyValueAttribute[])obj.GetType().GetCustomAttributes(typeof(KeyValueAttribute), false);
            var keyValueAttribute = attributes.Where(x => x.Key == key).ToArray();
            return (string)(keyValueAttribute.Length == 1 ? keyValueAttribute[0].Value : obj.GetType().Name);
        }
        public static object GetValueOfKey(this PropertyInfo propertyInfo, string key)
        {
            var attributes = (KeyValueAttribute[])propertyInfo.GetCustomAttributes(typeof(KeyValueAttribute), false);
            var keyValueAttribute = attributes.Where(x => x.Key == key).ToArray();
            return (keyValueAttribute.Length == 1 ? keyValueAttribute[0].Value : null);
        }

        #region [ IsEntityBaseEntity ]
        public static bool IsCacheableEntityType(this Type sourceType)
        {
            var attributes = (CacheableAttribute[])sourceType.GetCustomAttributes(typeof(CacheableAttribute), false);
            return attributes != null && attributes.Length > 0;
        }
        private static bool IsTypeA<TTargetType>(this Type sourceType)
        {
            if (sourceType.BaseType == null)
            {
                return false;
            }
            return sourceType.BaseType == typeof(TTargetType) ? true : IsTypeA<TTargetType>(sourceType.BaseType);
        }
        #endregion

        #region [ ClearReferanceObjects ]
        public static void ClearReferanceObjects(this IEntity entity)
        {
            var properties = entity.GetType().GetProperties();
            foreach (var property in properties)
            {
                var isEntityObject = property.PropertyType.IsInheritance(typeof(IEntity));
                if (isEntityObject)
                {
                    var value = property.GetValue(entity) as IEntity;
                    if (value != null)
                    {
                        property.SetValue(entity, null);
                    }
                }
            }
        }
        #endregion

    }
}