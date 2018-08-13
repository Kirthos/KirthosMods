using Eco.Gameplay.Components.Auth;
using System.Collections.Generic;
using System;
using Eco.Gameplay.Players;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Items;
using Eco.Gameplay.Components;
using Kirthos.Mods.TransportPipe.Objects;
using Eco.Core.Utils;
using System.Reflection;
using System.Linq;
using Eco.Shared.Utils;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 04/12/2018
 */

namespace Kirthos.Mods.TransportPipe
{
    public class TransportPipeLinker
    {
        public float ELECTRIC_PIPE_POWER_NEED = TransportPipePlugin.Conf.Get<float>("ElectricPipePowerNeed");
        public float MECHANICS_PIPE_POWER_NEED = TransportPipePlugin.Conf.Get<float>("WoodenPipePowerNeed");
        
        public float ELECTRIC_PIPE_TIME = TransportPipePlugin.Conf.Get<float>("ElectricPipeTimeMove");
        public float MECHANICS_PIPE_TIME = TransportPipePlugin.Conf.Get<float>("WoodenPipeTimeMove");

        private ThreadSafeList<TransportPipeInfo> conveyors;
        private ThreadSafeList<ConnectorObject> inputConveyors;
        private ThreadSafeList<ConnectorObject> outputConveyors;
        private int id;
        private float time;
        private PIPETYPE linkerPipeType;
        public TransportPipeLinker()
        {
            conveyors = new ThreadSafeList<TransportPipeInfo>();
            inputConveyors = new ThreadSafeList<ConnectorObject>();
            outputConveyors = new ThreadSafeList<ConnectorObject>();
            id = TransportPipeManager.idLinker;
            TransportPipeManager.idLinker++;
            linkerPipeType = PIPETYPE.Electric;
        }

        private void update()
        {
            if (inputConveyors.Count > 0 && outputConveyors.Count > 0)
            {
                UpdateElectricityCost();
                foreach (ConnectorObject convIn in inputConveyors)
                {
                    if (WorldObjectManager.GetFromID(convIn.Input.ObjectID) == null)
                    {
                        convIn.CleanDestroy();
                        break;
                    }
                    if (convIn.Operating == false)
                    {
                        continue;
                    }
                    PropertyAuthComponent convInAuth = convIn.Input.GetComponent<PropertyAuthComponent>();
                    // If the owner of the connector input can interract with the stockpile
                    if ((convIn.OwnerUser == null && convIn.Input.OwnerUser == null) || (convIn.OwnerUser != null && convIn.Input.AuthorizedToInteract(convIn.OwnerUser).Success))
                    {
                        foreach (ConnectorObject convOut in outputConveyors)
                        {
                            if (WorldObjectManager.GetFromID(convOut.Output.ObjectID) == null)
                            {
                                convOut.CleanDestroy();
                                break;
                            }
                            if (convOut.Operating == false)
                            {
                                continue;
                            }
                            PropertyAuthComponent convOutAuth = convOut.Output.GetComponent<PropertyAuthComponent>();
                            // If the owner of the input can interract with the owner of the output
                            if ((convIn.OwnerUser == null && convOut.OwnerUser == null) || (convIn.OwnerUser != null && convOut.AuthorizedToInteract(convIn.OwnerUser).Success))
                            {
                                // If the owner of the connector output can interract with the stockpile
                                if ((convOut.OwnerUser == null && convOut.Output.OwnerUser == null) || (convOut.OwnerUser != null && convOut.Output.AuthorizedToInteract(convOut.OwnerUser).Success))
                                {
                                    if (Utils.MoveItemFromToInventory(convIn.InvInput, convOut.InvOutput, 1, convIn.filter, convOut.filter))
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }

        public void testUpdate(ConnectorObject ticker)
        {
            if (inputConveyors.Count > 0)
            {
                if (inputConveyors[0].ID == ticker.ID)
                {
                    time += WorldObjectManager.TickDeltaTime;
                    if (time >= (linkerPipeType == PIPETYPE.Electric ? ELECTRIC_PIPE_TIME : MECHANICS_PIPE_TIME))
                    {
                        time = 0;
                        update();
                    }
                }
            }
        }

        public void UpdateInfo(TransportPipeInfo info)
        {
            if (info.type == PIPETYPE.Wooden)
            {
                if (linkerPipeType != PIPETYPE.Wooden)
                {
                    linkerPipeType = PIPETYPE.Wooden;
                    UpdateElectricityType();
                }
            }
            else if (info.type == PIPETYPE.Electric)
            {
                if (linkerPipeType == PIPETYPE.Wooden)
                {
                    linkerPipeType = PIPETYPE.Electric;
                    foreach (TransportPipeInfo conv in conveyors)
                    {
                        if (conv.type == PIPETYPE.Wooden)
                        {
                            linkerPipeType = PIPETYPE.Wooden;
                            break;
                        }
                    }
                    UpdateElectricityType();
                }
            }
            UpdateElectricityCost();
        }

        public void UpdateElectricityType()
        {
            List<PowerGridComponent> grids = new List<PowerGridComponent>();
            foreach (ConnectorObject connector in inputConveyors)
            {
                PowerGridComponent grid = connector.GetComponent<PowerGridComponent>();
                if (grids.Contains(grid) == false)
                    grids.Add(grid);

                typeof(PowerGridComponent).GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.Name.ContainsCaseInsensitive("EnergyType")).First().SetValue(grid, linkerPipeType == PIPETYPE.Electric ? new ElectricPower() as IPowerEnergyType : new MechanicalPower() as IPowerEnergyType);
            }
            foreach (ConnectorObject connector in outputConveyors)
            {
                PowerGridComponent grid = connector.GetComponent<PowerGridComponent>();
                if (grids.Contains(grid) == false)
                    grids.Add(grid);
                typeof(PowerGridComponent).GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.Name.ContainsCaseInsensitive("EnergyType")).First().SetValue(grid, linkerPipeType == PIPETYPE.Electric ? new ElectricPower() as IPowerEnergyType : new MechanicalPower() as IPowerEnergyType);

            }
            foreach (var grid in grids)
            {
                if (grid.PowerGrid != null)
                {
                    grid.PowerGrid.Disconnect(grid);
                    grid.PowerGrid = null;
                }
            }
            foreach(var grid in grids)
            { 
                grid.Relink();
            }
        }

        public void AbsorbLinker(TransportPipeLinker linker)
        {
            foreach(TransportPipeInfo con in linker.conveyors)
            {
                con.beltLinker = this;
                AddConveyor(con);
            }
            foreach (ConnectorObject con in linker.inputConveyors)
            {
                AddConnector(true, con);
            }
            foreach (ConnectorObject con in linker.outputConveyors)
            {
                AddConnector(false, con);
            }
            linker = null;
        }

        public void RebuildLinker()
        {
            foreach (TransportPipeInfo con in conveyors)
            {
                con.beltLinker = null;
            }
            TransportPipeInfo toRebuild = null;
            while (true)
            {
                toRebuild = null;
                foreach (TransportPipeInfo p in conveyors)
                {
                    if (p.beltLinker == null)
                    {
                        p.beltLinker = new TransportPipeLinker();
                        toRebuild = p;
                        break;
                    }
                }
                if (toRebuild != null)
                {
                    RecursifRebuild(toRebuild);
                    toRebuild.beltLinker.UpdateElectricityCost();
                }
                else
                {
                    break;
                }
            }
            List<ConnectorObject> copie = new List<ConnectorObject>();

            foreach (ConnectorObject pipe in inputConveyors)
            {
                copie.Add(pipe);
            }
            foreach (ConnectorObject pipe in outputConveyors)
            {
                copie.Add(pipe);
            }
            inputConveyors.Clear();
            outputConveyors.Clear();
            foreach (ConnectorObject pipe in copie)
            {
                pipe.InitInputOutput();
            }
        }

        private void RecursifRebuild(TransportPipeInfo info)
        {
            info.beltLinker.AddConveyor(info);
            List<TransportPipeInfo> neighbourgs = Utils.GetNeighbourgPipesInfo(info.pos);
            foreach (TransportPipeInfo neighbourg in neighbourgs)
            {
                if (neighbourg.beltLinker == null)
                {
                    neighbourg.beltLinker = info.beltLinker;
                    RecursifRebuild(neighbourg);
                }
            }
        }

        private int GetPipeType(PIPETYPE type)
        {
            int counter = 0;
            foreach (TransportPipeInfo p in conveyors)
            {
                if (p.type == type)
                    counter++;
            }
            return counter;
        }

        public override string ToString()
        {
            return id + "(" + linkerPipeType + "): " + conveyors.Count + " conv(" + GetPipeType(PIPETYPE.Wooden) + ":" + GetPipeType(PIPETYPE.Electric) +"), " + inputConveyors.Count + " input, " + outputConveyors.Count + " output." ;
        }

        public ConnectorObject GetFirstValidOutput(WorldObject inputObject, Type transportItemType)
        {
            // User is the owner of the input object (I.E hopper)
            User user = inputObject.OwnerUser;
            foreach (ConnectorObject conv in outputConveyors)
            {
                if (WorldObjectManager.GetFromID(conv.Output.ObjectID) == null)
                {
                    conv.CleanDestroy();
                    break;
                }
                if (conv.Operating == false)
                {
                    continue;
                }
                // If the owner of the input object is authorized to interract with the conv
                if (Utils.IsAuthorizedToInteract(conv, user))
                {
                    // If the owner of the connector can interract with the output
                    if (Utils.IsAuthorizedToInteract(conv.Output, conv.OwnerUser))
                    {
                        if (conv.filter.CanAccept(transportItemType) && Utils.VerifyInventoryPlace(conv.InvOutput, transportItemType))
                            return conv;
                    }
                }
            }
            return null;
        }

        /*
        public ConnectorObject AddToFirstOutput(WorldObject inputObject, Type transportItemType, int quantity)
        {
            // User is the owner of the input object (I.E hopper)
            User user = inputObject.OwnerUser;
            foreach (ConnectorObject conv in outputConveyors)
            {
                if (WorldObjectManager.GetFromID(conv.Output.ObjectID) == null)
                {
                    conv.CleanDestroy();
                    break;
                }
                if (conv.Operating == false)
                {
                    continue;
                }
                // If the owner of the input object is authorized to interract with the conv
                if (Utils.IsAuthorizedToInteract(conv, user))
                {
                    // If the owner of the connector can interract with the output
                    if (Utils.IsAuthorizedToInteract(conv.Output, conv.OwnerUser))
                    {
                        if (conv.itemFiltered != null && conv.itemFiltered.Count > 0 && conv.itemFiltered.Contains(transportItemType) == false)
                            continue;
                        if (conv.InvOutput.TryAddItems(transportItemType, quantity))
                            return conv;
                    }
                }
            }
            return null;
        }
        */

        public InventoryCollection GetAllInputInventory(User user)
        {
            List<Inventory> invList = new List<Inventory>();
            foreach (ConnectorObject conv in inputConveyors)
            {
                // If the connected input don't exist, destroy the connector
                if (WorldObjectManager.GetFromID(conv.Input.ObjectID) == null)
                {
                    conv.CleanDestroy();
                    break;
                }
                if (conv.Operating == false)
                {
                    continue;
                }
                // If the owner of the input object is authorized to interract with the conv
                if (Utils.IsAuthorizedToInteract(conv, user))
                {
                    // If the owner of the connector can interract with the input
                    if (Utils.IsAuthorizedToInteract(conv.Input, conv.OwnerUser))
                    {
                        invList.Add(conv.InvInput);
                    }
                }
            }
            return new InventoryCollection(invList);
        }

        public InventoryCollection GetAllOutputInventory(User user, Type itemToPlace = null)
        {
            List<Inventory> invList = new List<Inventory>();
            foreach (ConnectorObject conv in outputConveyors)
            {
                // If the connected output don't exist, destroy the connector
                if (WorldObjectManager.GetFromID(conv.Output.ObjectID) == null)
                {
                    conv.CleanDestroy();
                    break;
                }
                if (conv.Operating == false)
                {
                    continue;
                }
                // If the owner of the input object is authorized to interract with the conv
                if (Utils.IsAuthorizedToInteract(conv, user))
                {
                    // If the owner of the connector can interract with the output
                    if (Utils.IsAuthorizedToInteract(conv.Output, conv.OwnerUser))
                    {
                        if (itemToPlace != null && conv.filter.CanAccept(itemToPlace) == false)
                            continue;
                        invList.Add(conv.InvOutput);
                    }
                }
            }
            return new InventoryCollection(invList);
        }

        public Inventory SearchInventoryContains(User user, Type itemType)
        {
            foreach (ConnectorObject conv in inputConveyors)
            {
                // If the connected input don't exist, destroy the connector
                if (WorldObjectManager.GetFromID(conv.Input.ObjectID) == null)
                {
                    conv.CleanDestroy();
                    break;
                }
                if (conv.Operating == false)
                {
                    continue;
                }
                // If the owner of the input object is authorized to interract with the conv
                if (Utils.IsAuthorizedToInteract(conv, user))
                {
                    // If the owner of the connector can interract with the output
                    if (Utils.IsAuthorizedToInteract(conv.Input, conv.OwnerUser))
                    {
                        foreach (var item in conv.InvInput.Stacks)
                        {
                            if (item.Quantity > 0)
                            {
                                if (item.Item.Type == itemType)
                                    return conv.InvInput;
                            }
                        }

                    }
                }
            }
            return null;
        }

        public List<KeyValuePair<Type, int>> GetAllItemsInput(WorldObject objectAsk)
        {
            List<KeyValuePair<Type, int>> result = new List<KeyValuePair<Type, int>>();
            // User is the owner of the object that ask for the items
            User user = objectAsk.OwnerUser;
            List<Inventory> checkedInventory = new List<Inventory>();
            foreach (ConnectorObject conv in inputConveyors)
            {
                // If the connected input don't exist, destroy the connector
                if (WorldObjectManager.GetFromID(conv.Input.ObjectID) == null)
                {
                    conv.CleanDestroy();
                    break;
                }
                if (conv.Operating == false)
                {
                    continue;
                }
                // If the owner of the input object is authorized to interract with the conv
                if (Utils.IsAuthorizedToInteract(conv, user))
                {
                    // If the owner of the connector can interract with the output
                    if (Utils.IsAuthorizedToInteract(conv.Input, conv.OwnerUser))
                    {
                        if (conv.InvInput != null && checkedInventory.Contains(conv.InvInput) == false)
                        {
                            checkedInventory.Add(conv.InvInput);
                            foreach(var item in conv.InvInput.Stacks)
                            {
                                if (item.Quantity > 0)
                                {
                                    int qty = item.Quantity;
                                    int i = 0;
                                    foreach (var kvp in result)
                                    {
                                        if (kvp.Key == item.Item.Type)
                                        {
                                            qty += kvp.Value;
                                            break;
                                        }
                                        i++;
                                    }
                                    if (qty != item.Quantity)
                                        result.RemoveAt(i);
                                    result.Add(new KeyValuePair<Type, int>(item.Item.Type, qty));
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        public void UpdateElectricityCost()
        {
            float nbConnectorActive = 0;
            foreach (ConnectorObject connector in inputConveyors)
            {
                if (connector.GetComponent<OnOffComponent>().On)
                    nbConnectorActive++;
            }
            foreach (ConnectorObject connector in outputConveyors)
            {
                if (connector.GetComponent<OnOffComponent>().On)
                    nbConnectorActive++;
            }
            float totalEnergyNeed = 0;
            if (linkerPipeType == PIPETYPE.Electric)
                totalEnergyNeed = ELECTRIC_PIPE_POWER_NEED * conveyors.Count;
            else
                totalEnergyNeed = MECHANICS_PIPE_POWER_NEED * conveyors.Count;
            float energyPerConnectorActive = 0;
            if (nbConnectorActive > 0)
                energyPerConnectorActive = totalEnergyNeed / nbConnectorActive;
            foreach (ConnectorObject connector in inputConveyors)
            {
                connector.GetComponent<PowerConsumptionComponent>().OverridePowerConsumption(energyPerConnectorActive);
            }
            foreach (ConnectorObject connector in outputConveyors)
            {
                connector.GetComponent<PowerConsumptionComponent>().OverridePowerConsumption(energyPerConnectorActive);
            }
        }

        public void AddConveyor(TransportPipeInfo info)
        {
            if (info.type == PIPETYPE.Wooden)
            {
                if (linkerPipeType != PIPETYPE.Wooden)
                {
                    linkerPipeType = PIPETYPE.Wooden;
                    UpdateElectricityType();
                }
            }
            conveyors.Add(info);
            UpdateElectricityCost();
        }

        public void RemoveConveyor(TransportPipeInfo info)
        {
            conveyors.Remove(info);
            bool electric = true;
            foreach (TransportPipeInfo i in conveyors)
            {
                if (i.type == PIPETYPE.Wooden)
                {
                    if (linkerPipeType != PIPETYPE.Wooden)
                    {
                        linkerPipeType = PIPETYPE.Wooden;
                        UpdateElectricityType();
                    }
                    electric = false;
                    break;
                }
            }
            if (electric)
            {
                if (linkerPipeType != PIPETYPE.Electric)
                {
                    linkerPipeType = PIPETYPE.Electric;
                    UpdateElectricityType();
                }
            }
            UpdateElectricityCost();
        }

        public bool ContainsConveyor(TransportPipeInfo info)
        {
            return conveyors.Contains(info);
        }

        public void AddConnector(bool isInput, ConnectorObject connector)
        {
            if (isInput)
                inputConveyors.Add(connector);
            else
                outputConveyors.Add(connector);
            UpdateElectricityCost();
        }

        public void RemoveConnector(bool isInput, ConnectorObject connector)
        {
            if (isInput)
                inputConveyors.Remove(connector);
            else
                outputConveyors.Remove(connector);
            UpdateElectricityCost();
        }

        public void RemoveConnector(ConnectorObject connector)
        {
            if (ContainsConnector(true, connector))
                RemoveConnector(true, connector);
            if (ContainsConnector(false, connector))
                RemoveConnector(false, connector);
        }

        public bool ContainsConnector(bool isInput, ConnectorObject connector)
        {
            if (isInput)
                return inputConveyors.Contains(connector);
            else
                return outputConveyors.Contains(connector);
        }
    }
}
