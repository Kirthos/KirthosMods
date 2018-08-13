using Asphalt.Api.Event;
using Asphalt.Api.Event.PlayerEvents;
using Eco.Core.Localization;
using Eco.Gameplay;
using Eco.Gameplay.Components;
using Eco.Gameplay.Interactions;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Players;
using Eco.Gameplay.Property;
using Eco.Gameplay.Systems.Chat;
using Eco.Shared.Items;
using Eco.Shared.Localization;
using Eco.Shared.Math;
using Eco.Shared.Services;
using Eco.Shared.Utils;
using Eco.World;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 06/13/2018
 */

namespace Kirthos.Mods.ActiveCraft
{
    public class ActiveCraftEventListener
    {
        public ActiveCraftEventListener() { }

        [EventHandler]
        public void OnPlayerInteract(PlayerInteractEvent evt)
        {
            if (evt.Context != null)
                evt.SetCancelled(ContextOnInterraction(evt.Context));
        }

        public bool ContextOnInterraction(InteractionContext context)
        {
            if (context.Method == InteractionMethod.Left && (context.SelectedItem is HammerItem) == false)
            {
                if (context.HasTarget && context.Target is WorldObject && (context.Target as WorldObject).GetComponent<CraftingComponent>() != null)
                {
                    WorldObject obj = (context.Target as WorldObject);
                    CraftingComponent craft = obj.GetComponent<CraftingComponent>();
                    CreditComponent credit = obj.GetComponent<CreditComponent>();
                    if (craft.Parent.Operating && craft.BottleNecked == false)
                    {
                        float time = (float)craft.TimeLeft;
                        if (time >= 10)
                        {
                            time = 10;
                        }
                        float calories = time * 1.5f;
                        bool canPay = true;
                        bool TestCredit = credit != null && credit.UsingCurrency;
                        if (TestCredit)
                            canPay = CanPay(context.Player, time/60.0f, credit);
                        if (context.Player.User.Stomach.Calories >= calories && canPay)
                        {
                            craft.ProcessWorkOrders(time);
                            context.Player.User.ConsumeCalories(calories);
                            if (TestCredit)
                            {
                                FormattableString msg = $"{ProcessPay(context.Player, time/60.0f, credit)}";
                                ChatManager.ServerMessageToPlayer(msg, context.Player.User, false, DefaultChatTags.Trades);
                                ChatManager.ServerMessageToPlayer(msg, obj.OwnerUser, false, DefaultChatTags.Trades);
                            }
                        }
                    }
                }
            }
            return false;
        }
        private string ProcessPay(Player player, float time, CreditComponent credit)
        {

            string msg = string.Empty;
            float fee = (float)(credit.FeePerMinute * time);
            float tax = fee * credit.TaxManager.CraftingFeeTax / 100.0f;
            if (fee != 0)
            {
                credit.Currency.Transfer(player.FriendlyName, credit.Parent.OwnerUser.Name, fee);
                var taxstring = tax > 0 ? $" and {credit.Currency.UILinkContent(tax)} in tax" : string.Empty;
                msg += $"{player.User.UILinkContent()} fee of {credit.Currency.UILinkContent(fee)} paid to table owner {credit.Parent.OwnerUser.UILinkContent()}{taxstring}.";
            }

            if (tax > 0)
                Legislation.Government.PayTax(credit.Currency, player.User, tax, $"Crafting Fees Tax {credit.TaxManager.CraftingFeeTax.Format()}%");

            if (fee != 0 || tax > 0) player?.SendTemporaryMessageLoc($"You now have {credit.Currency.UILinkContent(player.FriendlyName):Currency name and value}.");

            return msg;
        }

        private bool CanPay(Player player, float time, CreditComponent credit)
        {
            float fee = (float)(credit.FeePerMinute * time);
            float tax = fee * credit.TaxManager.CraftingFeeTax / 100.0f;
            var newCredit = credit.Account(player) - fee - tax;
            if (newCredit < 0)
            {
                player.SendTemporaryErrorLoc($"You have {credit.Cash(player)} and cannot afford the fee of {Text.StyledNum(fee)}{(tax > 0 ? $" and tax of {Text.StyledNum(tax)}" : string.Empty)}.\n(Sell goods or complete contracts to gain credits.)");
                return false;
            }
            return true;
        }
    
    }
}
