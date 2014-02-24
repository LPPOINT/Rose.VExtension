using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
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

    public interface IRepositoryEntityCRUDController<TEntityType, TEntityIDType> : IRepositoryEntityController where TEntityType : class where TEntityIDType : class
    {

        IEnumerable<TEntityType> All { get; }

        TEntityType GetEntity(TEntityIDType id);
        void AddEntity(TEntityType entity);
        void RemoveEntity(TEntityIDType id);

    }

    public class RepositoryEntityController<TEntityType, TEntityIDType> : IRepositoryEntityCRUDController<TEntityType, TEntityIDType> where TEntityType : class where TEntityIDType : class
    {
        public RepositoryEntityController(IPluginsRepository repository, PluginsContainer dbContext, string idProperty)
        {
            IdProperty = idProperty;
            DbContext = dbContext;
            Repository = repository;
        }



        public IPluginsRepository Repository { get; private set; }
        public PluginsContainer DbContext { get; set; }
        public string IdProperty { get; private set; }

        public IEnumerable<TEntityType> All { get { return DbContext.Set<TEntityType>().ToList(); } }

        public TEntityType GetEntity(TEntityIDType id)
        {

            var set = DbContext.Set<TEntityType>();
            var res = set.FirstOrDefault(new Func<TEntityType, bool>(type => type.GetType()
                .GetProperties()
                .Any(
                    info =>
                        info.Name == IdProperty &&
                        info.PropertyType ==
                        typeof (TEntityIDType) &&
                        ((TEntityIDType)
                            info.GetValue(type)) == id)));
            return res;
        }

        public void AddEntity(TEntityType entity)
        {
            DbContext.Set<TEntityType>().Add(entity);
        }

        public void RemoveEntity(TEntityIDType id)
        {
            var entity = GetEntity(id);
            if (entity != null)
                DbContext.Set<TEntityType>().Remove(entity);
        }
    }

}
