using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kirthos.Mods.TransportPipe
{
    public class Filter
    {
        public List<Type> acceptItemList;
        public List<Type> refusedItemList;

        public Filter()
        {
            acceptItemList = new List<Type>();
            refusedItemList = new List<Type>();
        }

        public bool CanAccept(Type itemType)
        {
            bool whitelist = true;
            bool blacklist = true;
            if (acceptItemList != null && acceptItemList.Count > 0 && acceptItemList.Contains(itemType) == false)
                whitelist = false;
            if (refusedItemList != null && refusedItemList.Count > 0 && refusedItemList.Contains(itemType))
                blacklist = false;
            return whitelist && blacklist;  
        }
    }
}
