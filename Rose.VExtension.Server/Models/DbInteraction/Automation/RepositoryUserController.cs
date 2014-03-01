using System.Collections.Generic;
using System.Linq;

namespace Rose.VExtension.Server.Models.DbInteraction.Automation
{
    public class RepositoryUserController : RepositoryEntityController<User, int>
    {
        public RepositoryUserController(IPluginsRepository repository, PluginsContainer dbContext) : base(repository, dbContext, "Id")
        {
        }

        public IEnumerable<Plugin> GetUserPlugins(int userId)
        {
            var userEntity = DbContext.Users.FirstOrDefault(user => user.Id == userId);
            if (userEntity == null)
                return null;
            return userEntity.Plugins;
        }

        public User GetUserByVkId(string id)
        {
            return DbContext.Users.FirstOrDefault(user => user.VkId == id);
        }

    }
}