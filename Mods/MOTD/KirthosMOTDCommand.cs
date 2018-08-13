using Eco.Gameplay.Players;
using Eco.Gameplay.Systems.Chat;
using Eco.Shared.Math;
using Eco.Shared.Services;
using Eco.World;
using Kirthos.Mods.MOTD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 05/16/2018
 */

namespace Kirthos.mods.MOTD.Commands
{
    public class KirthosMODTCommand : IChatCommandHandler
    {
        [ChatCommand("A command to broadcast a message", ChatAuthorizationLevel.Admin)]
        public static void Broadcast(User user, string message)
        {
            foreach (User u in UserManager.OnlineUsers)
            {
                ChatManager.ServerMessageToPlayer($"[Broadcast] {String.Format(message, u.Name)}", u, false, DefaultChatTags.General, ChatCategory.Default);
            }
        }

        [ChatCommand("A command to broadcast a message in a UI for very important message.", ChatAuthorizationLevel.Admin)]
        public static void BroadcastUI(User user, string title, string message)
        {
            foreach (User u in UserManager.OnlineUsers)
            {
                u.Player.OpenInfoPanel(title, String.Format(message, u.Name));
                ChatManager.ServerMessageToPlayer($"[Broadcast] {String.Format(message, u.Name)}", u, false, DefaultChatTags.General, ChatCategory.Default);
            }
        }

        [ChatCommand("On auto message", ChatAuthorizationLevel.Admin)]
        public static void AutoMessageOn(User user)
        {
            MOTDPlugin.autoMsg.isActive = false;
            MOTDPlugin.Conf.Set<AutoMessage>("AutoMessage", MOTDPlugin.autoMsg);
            MOTDPlugin.Reload();
            ChatManager.ServerMessageToPlayer($"Auto message turn on", user);
        }

        [ChatCommand("Off auto message", ChatAuthorizationLevel.Admin)]
        public static void AutoMessageOff(User user)
        {
            MOTDPlugin.autoMsg.isActive = false;
            MOTDPlugin.Conf.Set<AutoMessage>("AutoMessage", MOTDPlugin.autoMsg);
            MOTDPlugin.Reload();
            ChatManager.ServerMessageToPlayer($"Auto message turn off", user);
        }

        [ChatCommand("Add a autoMessage in the autoMessage list", ChatAuthorizationLevel.Admin)]
        public static void AutoMessageAdd(User user, string newMessage)
        {
            List<string> lst = MOTDPlugin.autoMsg.messages.ToList();
            lst.Add(newMessage);
            MOTDPlugin.autoMsg.messages = lst.ToArray();
            MOTDPlugin.Conf.Set<AutoMessage>("AutoMessage", MOTDPlugin.autoMsg);
            MOTDPlugin.Reload();
            ChatManager.ServerMessageToPlayer($"Successfully added {newMessage} to auto message list", user);
        }

        [ChatCommand("Set auto message timer", ChatAuthorizationLevel.Admin)]
        public static void AutoMessageSetTimer(User user, float timerInSecond)
        {
            MOTDPlugin.autoMsg.timerInSecond = timerInSecond;
            MOTDPlugin.Conf.Set<AutoMessage>("AutoMessage", MOTDPlugin.autoMsg);
            MOTDPlugin.Reload();
            ChatManager.ServerMessageToPlayer($"Successfully set the timer between each message to {timerInSecond} seconds", user);
        }

        [ChatCommand("Reload auto Message config file", ChatAuthorizationLevel.Admin)]
        public static void AutoMessageReload(User user)
        {
            MOTDPlugin.Reload();
            ChatManager.ServerMessageToPlayer($"Auto message config successfully reloaded", user);
        }

        [ChatCommand("Show the server rules", ChatAuthorizationLevel.User)]
        public static void rules(User user)
        {
            UIMessage msg = MOTDPlugin.ruleMsg;
            user.Player.OpenInfoPanel(String.Format(msg.uiTitle, user.Name), String.Format(msg.message, user.Name));
        }

        [ChatCommand("Show welcome message", ChatAuthorizationLevel.User)]
        public static void showWelcome(User user)
        {
            UIMessage msg = MOTDPlugin.welcomeMessage;
            user.Player.OpenInfoPanel(String.Format(msg.uiTitle, user.Name), String.Format(msg.message, user.Name));
        }

        [ChatCommand("Show the server date and time", ChatAuthorizationLevel.User)]
        public static void serverTime(User user)
        {
            ChatManager.ServerMessageToPlayer($"Server date and time: {DateTime.Now}", user);
        }
    }
}
