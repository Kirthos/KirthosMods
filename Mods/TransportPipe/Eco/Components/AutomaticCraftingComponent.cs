using Eco.Core.Utils;
using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Players;
using Eco.Shared.Serialization;
using Eco.Shared.Utils;
using Kirthos.Mods.TransportPipe.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kirthos.Mods.TransportPipe.Components
{
    [Serialized]
    public class AutomaticCraftingComponent : CraftingComponent
    {
        private TransportPipeLinker linker;

        private Dictionary<WorkOrder, int> itemWanted = new Dictionary<WorkOrder, int>();

        public override void Tick()
        {
            linker = (this.Parent as PrinterObject).entries.Last().GetLinker();
            if (linker != null)
            {
                // Check for existing work order in the dictionnary
                foreach (WorkOrder order in this.WorkOrders)
                {
                    if (itemWanted.ContainsKey(order) == false)
                    {
                        itemWanted[order] = order.OriginalQuantity;
                    }
                }

                // try adding resources to any in progress orders
                foreach (WorkOrder order in this.WorkOrders)
                    order.TryToContributeItems(this.Parent.OwnerUser, this.linker.GetAllInputInventory(this.Parent.OwnerUser));

                // try adding any finished products, if space allows
                foreach (WorkOrder workOrder in this.WorkOrders.Where(wo => wo.UngivenCraftedQuantity > 0))
                    workOrder.TryToGiveItems(this.linker.GetAllOutputInventory(this.Parent.OwnerUser, workOrder.Product.Type));
            }

            base.Tick();

            // ===================================================================================================
            // ============== ALL THIS CODE IS FOR WHEN I TRIED TO ADD INFINITE CRAFT=============================
            // ===================================================================================================
            /*
            if (this.Parent.Enabled)
                this.ProcessWorkOrders(WorldObjectManager.TickDeltaTime);

            //Reset workOrder depending on qty
            try
            {
                List<WorkOrder> toRemove = new List<WorkOrder>();
                foreach (WorkOrder order in this.WorkOrders)
                {
                    int qty = itemWanted[order];
                    // if order is finish
                    if (order.UncraftedQuantity <= 0 && order.UngivenCraftedQuantity <= 0)
                    {
                        foreach (ItemStack item in linker.GetAllOutputInventory(this.Parent.OwnerUser).Stacks)
                        {
                            if (item.Empty == false && item.Item.Type == order.Product.Type)
                            {
                                qty -= item.Quantity;
                                if (qty <= 0)
                                    break;
                            }
                        }
                        // Reset craft
                        if (qty > 0)
                        {
                            Console.WriteLine(qty);
                            typeof(WorkOrder).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.Name.ContainsCaseInsensitive("UncraftedQuantity")).First().Invoke(order, new object[] { qty });
                            typeof(WorkOrder).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.Name.ContainsCaseInsensitive("UngivenCraftedQuantity")).First().Invoke(order, new object[] { qty });
                            typeof(WorkOrder).GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.Name.Contains("addedIngredients")).First().SetValue(order, new ThreadSafeList<ItemStack>());
                            order.Progress = 0;
                        }
                    }
                }
            }
            catch(Exception)
            {

            }
            */
        }

        public void RemoveComponent()
        {
            int ordersLength = this.WorkOrders.Count;
            for (int i = 0; i < ordersLength; i++)
            {
                WorkOrder wo = this.WorkOrders.First();
                this.WorkOrders.Remove(wo);
                var destInventory = this.linker.GetAllInputInventory(this.Parent.OwnerUser);
                var result = this.TryCancel(wo, destInventory, this.Parent.OwnerUser);
                if (!result)
                {
                    // Failed - add the work order back in
                    this.WorkOrders.Insert(0, wo);
                    break;
                }
            }
        }

        private Result TryCancel(WorkOrder workOrder, Inventory destinationInventory, User user)
        {
            return destinationInventory.TryModify(changeSet =>
            {
                // refund the contributed items, rounded down
                foreach (ItemStack stack in workOrder.NeededIngredients)
                {
                    int numMissing = workOrder.MissingIngredients
                        .Where(missingStack => missingStack.Item.TypeID == stack.Item.TypeID)
                        .Sum(missingStack => missingStack.Quantity);
                    int numAdded = stack.Quantity - numMissing;

                    int givenQuantity = workOrder.OriginalQuantity - workOrder.UncraftedQuantity - workOrder.UngivenCraftedQuantity;
                    int numUsed = (int)Math.Ceiling(stack.Quantity * (givenQuantity / (float)workOrder.OriginalQuantity));
                    changeSet.AddItems(stack.Item.Type, numAdded - numUsed);
                }
            }, user);
        }

    }
}
