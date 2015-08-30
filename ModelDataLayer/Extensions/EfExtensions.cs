using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ModelDataLayer.Extensions
{
    public static class EfExtensions
    {
        public static IGuidIdentifiedEntity CreateEntityByType(this DbContext context, string type, bool add = true)
        {
            if (context == null) throw new NullReferenceException();

            var asm = Assembly.GetExecutingAssembly();
            var entityTypeName = asm.GetName().Name + "." + type;
            var result = asm.CreateInstance(entityTypeName, true);

            var newItem = (IGuidIdentifiedEntity)result;
            if (newItem == null) return null;
            if (!add) return newItem;

            var entityType = asm.GetType(entityTypeName, false, true);
            var entitySet = context.Set(entityType);
            newItem.Id = Guid.NewGuid();
            return newItem;
        }
        internal static EntitySetBase GetEntitySet(this ObjectContext context, Type entityType)
        {
            if (context == null) throw new NullReferenceException();
            if (entityType == null) throw new ArgumentNullException("entityType");

            var container = context.MetadataWorkspace.GetEntityContainer(context.DefaultContainerName, DataSpace.CSpace);

            context.MetadataWorkspace.LoadFromAssembly(entityType.Assembly);
            var ospaceType = context.MetadataWorkspace.GetItem<EntityType>(entityType.FullName, DataSpace.OSpace);
            var edmEntityType = context.MetadataWorkspace.GetEdmSpaceType(ospaceType) as EntityType;

            if (edmEntityType == null) return null;

            while (edmEntityType != null && edmEntityType.BaseType != null)
                edmEntityType = edmEntityType.BaseType as EntityType;

            var possibleEntitySets = from es in container.BaseEntitySets
                                     where es.ElementType == edmEntityType
                                     select es;

            return possibleEntitySets.FirstOrDefault();
        }
    }
}
