namespace Rose.VExtension.Server.Middleware
{

    /// <summary>
    /// Пдеставляет методы для создания сущности компонента плагина и наоборот
    /// </summary>
    /// <typeparam name="TBase">Тип компонента плагина</typeparam>
    /// <typeparam name="TEntity">Тип сущности компонента плагина</typeparam>
    public interface IPluginComponentMiddleware<TBase, TEntity>
    {
        /// <summary>
        /// При переопределении создает компонент плагина на основе заданнойс сущности
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TBase CreateBase(TEntity entity);

        /// <summary>
        /// Пре переопределении создает сущность на основе заданного компонента
        /// </summary>
        /// <param name="base"></param>
        /// <returns></returns>
        TEntity CreateEntity(TBase @base);
    }
}
