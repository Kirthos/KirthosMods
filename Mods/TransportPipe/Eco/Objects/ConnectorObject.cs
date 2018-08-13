using Eco.Core.Utils;
using Eco.Gameplay.Components;
using Eco.Gameplay.Components.Auth;
using Eco.Gameplay.Interactions;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Players;
using Eco.Shared.Localization;
using Eco.Shared.Math;
using Eco.Shared.Serialization;
using Eco.Shared.Services;
using Eco.Shared.Utils;
using Eco.World;
using Kirthos.Mods.TransportPipe.Eco;
using Kirthos.Mods.TransportPipe.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 04/12/2018
 */

namespace Kirthos.Mods.TransportPipe.Objects
{
    [Serialized]
    public enum CONNECTOR_MODE
    {
        Input,
        Output,
        InOut,
        None
    }

    [Serialized]
    [RequireComponent(typeof(CustomTextComponent))]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(OnOffComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(PowerGridComponent))]
    [RequireComponent(typeof(PowerConsumptionComponent))]
    public class ConnectorObject : WorldObject
    {
        static ConnectorObject()
        {
            WorldObject.AddOccupancy<ConnectorObject>(new List<BlockOccupancy>()
            {
                new BlockOccupancy(new Vector3i(0, 0, 0), typeof(BeltSlotBlock)),
            });
        }
        public override string FriendlyName { get { return "Pipe connector"; } }

        [Serialized] public TransportPipeInfo info;
        [Serialized] public CONNECTOR_MODE mode;

        public WorldObject Input;
        public WorldObject Output;
        public Inventory InvInput;
        public Inventory InvOutput;
        public Filter filter = new Filter();

        private string lastText;
        private bool callUpdate;
        public bool doInit;
        private Player lastPlayer;
        private double timeStampMillis;
        private CustomTextComponent textComp;
        private bool askDestroy = false;
        private PIPETYPE previousType;

        public override InteractResult OnActInteract(InteractionContext context)
        {
            
            lastPlayer = context.Player;
            if (lastText == null)
            {
                GetComponent<CustomTextComponent>().SetText(lastPlayer, "All");
                lastText = "All";
            }
            return base.OnActInteract(context);
        }

        public void ProcessConfig(InteractionContext context)
        {
            if (mode == CONNECTOR_MODE.Input)
            {
                context.Player.SendTemporaryMessage($"Set to output");
                mode = CONNECTOR_MODE.Output;
            }
            else if (mode == CONNECTOR_MODE.Output)
            {
                context.Player.SendTemporaryMessage($"Set to input/output");
                mode = CONNECTOR_MODE.InOut;
            }
            else if (mode == CONNECTOR_MODE.InOut)
            {
                context.Player.SendTemporaryMessage($"Set to none");
                mode = CONNECTOR_MODE.None;
                CleanDestroy();
                return;
            }
            else
            {
                context.Player.SendTemporaryMessage($"Set to input");
                mode = CONNECTOR_MODE.Input;
            }
            callUpdate = true;
        }

        protected override void Initialize()
        {
            timeStampMillis = (DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
            doInit = true;
            GetComponent<PropertyAuthComponent>().Initialize();
            textComp = GetComponent<CustomTextComponent>();
            GetComponent<PowerConsumptionComponent>().Initialize(0);
            GetComponent<PowerGridComponent>().Initialize(10, new ElectricPower());
        }

        public override void Destroy()
        {
            if (askDestroy) return;
            askDestroy = true;
            base.Destroy();
        }


        /// <summary>
        /// Remove the connector and place the pipe at the same place
        /// It remove the connector from the linker lists
        /// </summary>
        public void CleanDestroy()
        {
            // in case we ask to destroy a connector without info
            if (info != null)
                info.beltLinker.RemoveConnector(this);
            Destroy();
            World.SetBlock(info.type == PIPETYPE.Electric ? typeof(ElectricTransportPipeBlock) : typeof(WoodenTransportPipeBlock), Position3i, null);
        }
    

        private void Init()
        {
            doInit = false;

            if (info == null)
            {
                info = new TransportPipeInfo();
                info.beltLinker = null;
            }

            List<TransportPipeInfo> neighbourgs = Utils.GetNeighbourgPipesInfo(this.Position3i);
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
            }
            else
            {
                info.beltLinker = new TransportPipeLinker();
            }
            if (TransportPipeManager.pipesInfo.ContainsKey(Utils.MakeWorldMod(Position3i)) == false)
            {
                TransportPipeManager.pipesInfo.TryAdd(Utils.MakeWorldMod(Position3i), info);
            }
            Utils.RecreateLinker(Utils.MakeWorldMod(Position3i), info.beltLinker);
            InitInputOutput();
        }

        public void InitInputOutput()
        {
            info.beltLinker.RemoveConnector(this);
            if (TransportPipeManager.pipesInfo.ContainsKey(Utils.MakeWorldMod(Position3i)))
            {
                info = TransportPipeManager.pipesInfo[Utils.MakeWorldMod(Position3i)];
            }
            if (mode == CONNECTOR_MODE.Input)
            {
                WorldObject obj = Utils.SearchForConnectedStorageObject(Utils.MakeWorldMod(Position3i));
                if (obj != null)
                {
                    Input = obj;
                    InvInput = Utils.SearchInventoryFromObject(obj);
                    info.beltLinker.AddConnector(true, this);
                    SetAnimatedState("SetModeInput", true);
                }
                if (Input == null || InvInput == null)
                {
                    mode = CONNECTOR_MODE.None;
                }
            }
            if (mode == CONNECTOR_MODE.Output)
            {
                WorldObject obj = Utils.SearchForConnectedStorageObject(Utils.MakeWorldMod(Position3i));
                if (obj != null)
                {
                    Output = obj;
                    InvOutput = Utils.SearchInventoryFromObject(obj);
                    info.beltLinker.AddConnector(false, this);
                    SetAnimatedState("SetModeOutput", true);
                }
                if (Output == null || InvOutput == null)
                {
                    mode = CONNECTOR_MODE.None;
                }
            }
            if (mode == CONNECTOR_MODE.InOut)
            {
                WorldObject obj = Utils.SearchForConnectedStorageObject(Utils.MakeWorldMod(Position3i));
                if (obj != null)
                {
                    Input = obj;
                    Output = obj;
                    InvOutput = Utils.SearchInventoryFromObject(obj);
                    InvInput = InvOutput;
                    info.beltLinker.AddConnector(false, this);
                    info.beltLinker.AddConnector(true, this);
                    SetAnimatedState("SetModeIOput", true);
                }
                if (Input == null || InvOutput == null)
                {
                    mode = CONNECTOR_MODE.None;
                }
            }
            // If it's in an unknow state - remove the connector
            if (mode == CONNECTOR_MODE.None)
            {
                CleanDestroy();
                return;
            }
            if (info != null && info.beltLinker != null)
                info.beltLinker.UpdateElectricityType();
            if (info != null)
            {
                SetAnimatedState("SetWood", info.type == PIPETYPE.Wooden);
                previousType = info.type;
            }
        }

        public override void Tick()
        {
            if (doInit)
                Init();
            if (callUpdate)
            {
                InitInputOutput();
                callUpdate = false;
            }
            base.Tick();
            if (timeStampMillis + 500f < (DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds)
            {
                if (info != null && info.type != previousType)
                {
                    SetAnimatedState("SetWood", info.type == PIPETYPE.Wooden);
                    previousType = info.type;
                }
                timeStampMillis = (DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
                // If text change
                if (textComp.Text != lastText)
                {
                    string goodTextName = "";

                    // Make a copy of the filter in case we need to revert the filter
                    Filter copy = new Filter();
                    copy.acceptItemList.AddRange(filter.acceptItemList);
                    copy.refusedItemList.AddRange(filter.refusedItemList);

                    filter.acceptItemList.Clear();
                    filter.refusedItemList.Clear();
                    string newText = textComp.Text;
                    // In case the current text is null set the previous text
                    if (lastPlayer != null && newText == null)
                    {
                        textComp.SetText(lastPlayer, lastText);
                        newText = lastText;
                    }
                    foreach (string itemName in newText.Split(','))
                    {
                        string blacklistItem = null;
                        if (itemName.Length > 0 && itemName[0] == '!')
                            blacklistItem = itemName.Remove(0, 1);
                        if (blacklistItem != null)
                        {
                            var blackItems = typeof(Item).CreatableTypes().Where(x => x.Name.ContainsCaseInsensitive(blacklistItem));
                            // Search for the items
                            if (blackItems.Count() > 1)
                            {
                                var lessItemsBlack = blackItems.Where(i => i.Name.Remove(i.Name.Length - "Item".Length).CompareCaseInsensitive(blacklistItem) == 0);
                                if (lessItemsBlack.Any())
                                    blackItems = lessItemsBlack;
                            }
                            //If the item is found
                            if (blackItems.Count() == 1)
                            {
                                filter.refusedItemList.Add(blackItems.First());
                                if (goodTextName != "") goodTextName += ",";
                                goodTextName += "!" + blackItems.First().ToString().Split('.').Last().Remove(blackItems.First().ToString().Split('.').Last().Length - "Item".Length);
                                continue;
                            }
                        }
                        string testedItem = itemName;
                        if (blacklistItem != null)
                            testedItem = testedItem.Remove(0, 1);
                        var items = typeof(Item).CreatableTypes().Where(x => x.Name.ContainsCaseInsensitive(testedItem));
                        // Search for the items
                        if (items.Count() > 1)
                        {
                            var lessItems = items.Where(i => i.Name.Remove(i.Name.Length - "Item".Length).CompareCaseInsensitive(testedItem) == 0);
                            if (lessItems.Any())
                                items = lessItems;
                        }
                        //If the item is found
                        if (items.Count() == 1)
                        {
                            filter.acceptItemList.Add(items.First());
                            if (goodTextName != "") goodTextName += ",";
                            goodTextName += items.First().ToString().Split('.').Last().Remove(items.First().ToString().Split('.').Last().Length - "Item".Length);
                            continue;
                        }
                        // If no item found and if it's not All
                        if (lastPlayer != null && !itemName.Equals("All", StringComparison.InvariantCultureIgnoreCase))
                        {
                            // show error message to the player
                            if (items.Count() == 0)
                                lastPlayer.SendTemporaryMessage($"Couldn't find any items matching {testedItem}", ChatCategory.Error);
                            else
                                lastPlayer.SendTemporaryMessage($"Multiple items found matching, use more specific string: {string.Join(", ", items.Select(x => x.Name))}", ChatCategory.Error);

                            // Back to the last working filter
                            filter.acceptItemList.Clear();
                            filter.refusedItemList.Clear();
                            filter.acceptItemList.AddRange(copy.acceptItemList);
                            filter.refusedItemList.AddRange(copy.refusedItemList);
                            // Back to the previous text
                            textComp.SetText(lastPlayer, lastText);
                            newText = lastText;
                            goodTextName = "";
                            break;

                        }
                    }
                    // If good text exist and if player exist change text
                    if (goodTextName.IsEmpty() == false && lastPlayer != null)
                    {
                        textComp.SetText(lastPlayer, goodTextName);
                        newText = goodTextName;
                    }
                    // Changing the lastText to the next text. That permit to check when text changes
                    lastText = newText;
                }
            }
            if (info != null && info.beltLinker != null)
                info.beltLinker.testUpdate(this);
        }
    }
}
