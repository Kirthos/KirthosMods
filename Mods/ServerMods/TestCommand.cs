using Eco.Core.Utils;
using Eco.Gameplay.Players;
using Eco.Gameplay.Systems.Chat;
using Eco.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kirthos.Mods.ServerMods
{
    public class Testcommand : IChatCommandHandler
    {
        [ChatCommand("")]
        public static void test(User user, string newName)
        {
            //string newName = "[FR-Mayor] " + user.Name;
            string previousName = user.Name;
            typeof(User).GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.Name.ContainsCaseInsensitive("Name")).First().SetValue(user, newName);
            ThreadSafeDictionary<string, User> users = (typeof(UserManager).GetFields(BindingFlags.NonPublic | BindingFlags.Static).Where(x => x.Name.ContainsCaseInsensitive("UsersByDisplayName")).First().GetValue(UserManager.Obj) as ThreadSafeDictionary<string, User>);
            users.Remove(previousName);
            users.Add(newName, user);
        }
    }
}
