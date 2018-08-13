using Asphalt.Api.Event;
using Asphalt.Api.Event.PlayerEvents;
using Eco.Gameplay.Interactions;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Shared.Items;
using Eco.Shared.Localization;
using Eco.Shared.Math;
using Eco.World;
using Kirthos.Mods.TransportPipe.Components;
using Kirthos.Mods.TransportPipe.Eco;
using Kirthos.Mods.TransportPipe.Items;
using Kirthos.Mods.TransportPipe.Objects;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 04/12/2018
 */

namespace Kirthos.Mods.TransportPipe
{
    public class TransportPipeEventListener
    {
        public TransportPipeEventListener() { }

        /*
        [RegisterEvent()]
        public void onObjectReady(PlayerInteract rpc)
        {
            BSONValue bsonresult = null;
            InteractionContext context = null;

            if (rpc.bson.TryGetValue("args", out bsonresult))
            {
                if (bsonresult.ObjectValue != null)
                {
                    if (bsonresult.ObjectValue.TryGetValue("0", out bsonresult))
                    {
                        if (bsonresult.ObjectValue != null)
                        {
                            InteractionInfo info = new InteractionInfo();
                            info.FromBson(bsonresult.ObjectValue);
                            context = InteractionExtensions.MakeContext(info, rpc.client.Observer as Player);
                        }
                    }
                }
            }
            if (context != null)
                ContextOnInterraction(context);
        }
        */

        [EventHandler]
        public void OnPlayerInteract(PlayerInteractEvent evt)
        {
            if (evt.Context != null)
                evt.SetCancelled(ContextOnInterraction(evt.Context));
        }

        public bool ContextOnInterraction(InteractionContext context)
        {
            if (context.Method == InteractionMethod.Left)
            {
                if (context.HasBlock && context.Block is BaseTransportPipeBlock && context.SelectedItem is HammerItem)
                {
                    Item itemToAdd = Item.Create(BlockItem.CreatingItem(context.Block.GetType()).Type);
                    if (itemToAdd != null && context.Player.User.Inventory.TryAddItem(itemToAdd))
                    {
                        StringBuilder message = new StringBuilder();
                        message.AppendLocString("You received");
                        message.Append(" ");
                        message.Append(Item.Get(itemToAdd.TypeID).UILinkAndNumber(1));
                        context.Player.SendTemporaryMessage(message.ToStringAlreadyLocalized());
                        World.DeleteBlock(context.BlockPosition.Value);
                        TransportPipeInfo info = null;
                        Vector3i pos = Utils.MakeWorldMod(context.BlockPosition.Value);
                        if (TransportPipeManager.pipesInfo.TryGetValue(pos, out info))
                        {
                            info.beltLinker.RemoveConveyor(info);
                            TransportPipeManager.pipesInfo.TryRemove(pos, out info);
                            info.beltLinker.RebuildLinker();
                        }
                    }
                }
                else if (context.HasTarget && context.Target is ConnectorObject && context.SelectedItem is HammerItem)
                {
                    TransportPipeInfo info = null;
                    Vector3i pos = Utils.MakeWorldMod((context.Target as ConnectorObject).Position3i);
                    if (TransportPipeManager.pipesInfo.TryGetValue(pos, out info))
                    {
                        Item itemToAdd = null;
                        if (info.type == PIPETYPE.Electric)
                            itemToAdd = Item.Create(typeof(ElectricTransportPipeItem));
                        else if (info.type == PIPETYPE.Wooden)
                            itemToAdd = Item.Create(typeof(WoodenTransportPipeItem));
                        if (context.Player.User.Inventory.TryAddItems(itemToAdd.Type, 1))
                        {
                            StringBuilder message = new StringBuilder();
                            message.AppendLocString("You received");
                            message.Append(" ");
                            message.Append(Item.Get(itemToAdd.TypeID).UILinkAndNumber(1));
                            context.Player.SendTemporaryMessage(message.ToStringAlreadyLocalized());
                            info.beltLinker.RemoveConnector(context.Target as ConnectorObject);
                            info.beltLinker.RemoveConveyor(info);
                            TransportPipeManager.pipesInfo.TryRemove((context.Target as ConnectorObject).Position3i, out info);
                            info.beltLinker.RebuildLinker();
                            (context.Target as ConnectorObject).Destroy();
                        }
                    }
                    else
                    {
                        return false;
                    }
                    return true;
                }
                else if (context.HasTarget && context.Target is WorldObject && (context.Target as WorldObject).GetComponent<AutomaticCraftingComponent>() != null && context.SelectedItem is HammerItem)
                {
                    (context.Target as WorldObject).GetComponent<AutomaticCraftingComponent>().RemoveComponent();
                }
            }
            else if (context.Method == InteractionMethod.Right)
            {
                if (context.HasBlock && context.SelectedItem != null && context.SelectedItem is HammerItem && context.CarriedItem != null && context.CarriedItem is BlockItem<BaseTransportPipeBlock>)
                {
                    TransportPipeInfo info = new TransportPipeInfo();
                    info.beltLinker = null;
                    if (context.CarriedItem is ElectricTransportPipeItem)
                        info.type = PIPETYPE.Electric;
                    else if (context.CarriedItem is WoodenTransportPipeItem)
                        info.type = PIPETYPE.Wooden;
                    Vector3i pos = Utils.MakeWorldMod(context.BlockPosition.Value);
                    List<TransportPipeInfo> neighbourgs = Utils.GetNeighbourgPipesInfo(pos);
                    if (neighbourgs.Count > 0)
                    {
                        foreach (TransportPipeInfo neighbourg in neighbourgs)
                        {
                            if (info.beltLinker == null)
                                info.beltLinker = neighbourg.beltLinker;
                            else if (neighbourg.beltLinker != null && info.beltLinker != neighbourg.beltLinker)
                                info.beltLinker.AbsorbLinker(neighbourg.beltLinker);
                        }
                        if (info.beltLinker == null)
                        {
                            info.beltLinker = new TransportPipeLinker();
                        }
                        info.beltLinker.AddConveyor(info);
                    }
                    else
                    {
                        info.beltLinker = new TransportPipeLinker();
                        info.beltLinker.AddConveyor(info);
                    }
                    info.pos = pos;
                    TransportPipeManager.pipesInfo[pos] = info;
                }
            }
            return false;
        }
    }
}
