using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using System.Web.UI.HtmlControls;
using WebGrease.Css.Extensions;

namespace Rose.VExtension.Server.Models.DbInteraction.Automation
{
    public interface IRepositoryEntityController
    {
        IPluginsRepository Repository { get; }
        PluginsContainer DbContext { get; }
        string IdProperty { get; }
    }

    public interface IRepositoryEntityCRUDController<TEntityType, TEntityIDType> : IRepositoryEntityController where TEntityType : class
    {

        IEnumerable<TEntityType> All { get; }

        TEntityType GetEntity(TEntityIDType id);
        void AddEntity(TEntityType entity);
        void RemoveEntity(TEntityIDType id);
        void Clear();

    }
    public class RepositoryEntityController<TEntityType, TEntityIDType> : IRepositoryEntityCRUDController<TEntityType, TEntityIDType> where TEntityType : class 
    {
        public RepositoryEntityController(IPluginsRepository repository, PluginsContainer dbContext, string idProperty)
        {
            IdProperty = idProperty;
            DbContext = dbContext;
            Repository = repository;
        }

        public DbSet<TEntityType> Set
        {
            get { return DbContext.Set<TEntityType>(); }
        }

        public PropertyInfo GetEntityIdPropertyInfo(TEntityIDType id)
        {
            var entity = GetEntity(id);
            if(entity == null)
                return null;
            return entity.GetType().GetProperties().FirstOrDefault(info => info.Name == IdProperty);
        }

        public TEntityIDType GetEntityId(TEntityType entity)
        {
          
            var prop = entity.GetType().GetProperties().FirstOrDefault(info => info.Name == IdProperty);
            if (prop == null)
                return default(TEntityIDType);
            return (TEntityIDType) prop.GetValue(entity);
        }

        public IPluginsRepository Repository { get; private set; }
        public PluginsContainer DbContext { get; set; }
        public string IdProperty { get; private set; }

        public IEnumerable<TEntityType> All { get { return DbContext.Set<TEntityType>().ToList(); } }

        public virtual TEntityType GetEntity(TEntityIDType id)
        {

            var set = DbContext.Set<TEntityType>();
            foreach (var entity in set.Local)
            {
                var entityType = entity.GetType();
                var targetType = typeof (TEntityType);
                if (true)
                {
                    var props = entityType.GetProperties();
                    var idProperty = props.FirstOrDefault(info => info.Name == IdProperty);
                    if (idProperty != null)
                    {
                        var idRes = (TEntityIDType) idProperty.GetValue(entity);
                        if (idRes.Equals(id))
                            return entity;
                    }
                }
            }
            return null;
        }

        public virtual void AddEntity(TEntityType entity)
        {
            DbContext.Set<TEntityType>().Add(entity);
            DbContext.SaveChanges();
        }

        public virtual void RemoveEntity(TEntityIDType id)
        {
            var entity = GetEntity(id);
            if (entity != null)
            {
                DbContext.Set<TEntityType>().Remove(entity);
                DbContext.SaveChanges();
            }
        }

        public virtual void Clear()
        {
            var set = DbContext.Set<TEntityType>();

            var setContent = set.Local;

            foreach (var entity in setContent)
            {
                set.Remove(entity);
            }

            DbContext.SaveChanges();
        }
    }

    public class RepositoryPluginComponentEntityController<TEntityType, TEntityIDType> :
        RepositoryEntityController<TEntityType, TEntityIDType> where TEntityType : class 
    {

        public string PluginIdProperty { get; private set; }

        public RepositoryPluginComponentEntityController(IPluginsRepository repository, PluginsContainer dbContext, string idProperty, string pluginIdProperty) : base(repository, dbContext, idProperty)
        {
            PluginIdProperty = pluginIdProperty;
        }

        public IEnumerable<TEntityType> GetEntitiesByPluginId(string pluginId)
        {

            var set = DbContext.Set<TEntityType>();
            var res = new List<TEntityType>();
            foreach (var entity in set.Local)
            {
                var entityType = entity.GetType();
                var targetType = typeof(TEntityType);
                if (true)
                {
                    var props = entityType.GetProperties();
                    var idProperty = props.FirstOrDefault(info => info.Name == PluginIdProperty);
                    if (idProperty != null)
                    {
                        var idRes = idProperty.GetValue(entity).ToString();
                        if (idRes.Equals(pluginId))
                             res.Add(entity);
                    }
                }
            }
            return res;
        }

        public void RemoveEntitiesByPluginId(string pluginId)
        {
            var entity = GetEntitiesByPluginId(pluginId);
            foreach (var entityType in entity)
            {
                RemoveEntity(GetEntityId(entityType));
            }
        }



    }

}
