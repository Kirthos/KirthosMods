using Asphalt;
using Asphalt.Api.Event;
using Asphalt.Service;
using Asphalt.Storeable;
using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Gameplay.Players;
using Eco.Gameplay.Systems.Chat;
using Eco.Shared.Math;
using Eco.Shared.Serialization;
using Eco.Shared.Services;
using Eco.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;


/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 05/16/2018
 */

namespace Kirthos.Mods.MOTD
{
    [AsphaltPlugin("KirthosMods/AnotherMOTDPlugin")]
    class MOTDPlugin : IModKitPlugin, IInitializablePlugin
    {
        private static bool f = false;
        private int messageNumber = 0;

        public static AutoMessage autoMsg = null;
        public static UIMessage ruleMsg = null;
        public static UIMessage welcomeMessage = null;
        public static ScheduledMessage SchedMsg = null;

        [Inject]
        [StorageLocation("Config")]
        [DefaultValues(nameof(GetConfig))]
        public static IStorage ConfigStorage;

        public static IStorage Conf
        {
            get
            {
                if (f)
                    return ConfigStorage;
                else
                {
                    f = true;
                    try
                    {
                        ServiceHelper.InjectValues();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    return ConfigStorage;
                }
            }
        }

        public static KeyDefaultValue[] GetConfig()
        {
            return new KeyDefaultValue[]
            {
                new KeyDefaultValue("WelcomeMessage", new UIMessage("Welcome !", "Welcome.txt")),
                new KeyDefaultValue("Rules", new UIMessage("Rules", "Rules.txt")),
                new KeyDefaultValue("AutoMessage", new AutoMessage(300, false, false, new string[] { "[AutoMessage] First automatic message", "[AutoMessage] A second automatic message", "[AutoMessage] A third one"})),
                new KeyDefaultValue("ScheduledMessage", new ScheduledMessage(false, new TimeMessage[] {
                    new TimeMessage("12:00 AM", "12AM", "Scheduled message"),
                    new TimeMessage("12:00 PM", "12PM", "Scheduled message"),
                    new TimeMessage("00:00", "00H00", "Scheduled message"),
                    new TimeMessage("12:00", "12H00", "Scheduled message"),
                })),
            };
        }

        public override string ToString()
        {
            return "Another MOTD Plugin";
        }

        public string GetStatus()
        {
            return "Version 1.2.0";
        }

        public void Initialize(TimedTask timer)
        {
            Reload();
            UserManager.OnUserLoggedIn.Add((user) =>
            {
                if (user.LoginTime == 0)
                {
                    new Thread(() =>
                    {
                        while (true)
                        {
                            if (user == null)
                                break;
                            try
                            {
                                user.Player.OpenInfoPanel(String.Format(welcomeMessage.uiTitle, user.Name), String.Format(welcomeMessage.message, user.Name));
                                break;
                            }
                            catch (NullReferenceException)
                            {

                            }
                            Thread.Sleep(100);
                        }
                    }).Start();
                }
            });
            new Thread(AutoMessage).Start();
        }

        public static void Reload()
        {
            try
            {
                autoMsg = Conf.Get<AutoMessage>("AutoMessage");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when loading AutoMessage config - " + e.Message);
            }
            try
            {
                ruleMsg = Conf.Get<UIMessage>("Rules");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when loading Rules config - " + e.Message);
            }
            try
            {
                welcomeMessage = Conf.Get<UIMessage>("WelcomeMessage");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when loading WelcomeMessage config - " + e.Message);
            }
            try
            {
                SchedMsg = Conf.Get<ScheduledMessage>("ScheduledMessage");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when loading ScheduledMessage config - " + e.Message);
            }
            ruleMsg.message = File.ReadAllText("Mods/KirthosMods/AnotherMOTDPlugin/" + ruleMsg.filename);
            welcomeMessage.message = File.ReadAllText("Mods/KirthosMods/AnotherMOTDPlugin/" + welcomeMessage.filename);
        }

        private void AutoMessage()
        {
            float waitTime = 0;
            while (true)
            {
                if (autoMsg.isActive)
                {
                    if (waitTime <= 0)
                    {
                        string messageToShow = null;
                        if (autoMsg.isRandomlySelected)
                            messageToShow = autoMsg.messages[RandomUtil.Range(0, autoMsg.messages.Length)];
                        else
                        {
                            messageToShow = autoMsg.messages[messageNumber];
                            messageNumber++;
                            if (messageNumber >= autoMsg.messages.Length)
                                messageNumber = 0;
                        }
                        ChatManager.ServerMessageToAll($"{messageToShow}", autoMsg.isTemporary, DefaultChatTags.General, ChatCategory.Default);
                        waitTime = autoMsg.timerInSecond * 1000f;
                    }
                    else
                        waitTime -= 1000;
                }    
                if (SchedMsg.isActive)
                {
                    DateTime d = DateTime.Now;
                    foreach (TimeMessage msg in SchedMsg.messages)
                    {
                        try
                        {
                            if (msg.time.Length < 5)
                                throw new Exception();
                            if (d.TimeOfDay.Seconds == 0 && d.TimeOfDay.Hours == GetHours(msg.time) && d.TimeOfDay.Minutes == GetMins(msg.time))
                            {
                                ChatManager.ServerMessageToAll($"{msg.message}", SchedMsg.isTemporary, DefaultChatTags.General, ChatCategory.Default);
                                if (msg.openUI)
                                {
                                    foreach (User u in UserManager.OnlineUsers)
                                    {
                                        u.Player.OpenInfoPanel(msg.UITitle, String.Format(msg.message, u.Name));
                                    }
                                }
                                //Console.WriteLine($"[{msg.time}] - {msg.message}");
                            }
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine("Error when loading ScheduledMessage config - Unknown time format '" + msg.time + "', please use HH:MM AM or PM.");
                        }
                    }
                }
                Thread.Sleep(1000);
            }
        }

        private int GetHours(string configTime)
        {
            int hours = 0;
            if (int.TryParse(configTime.Remove(configTime.IndexOf(":")), out hours))
            {
                if (configTime.Length > 5)
                {
                    if (configTime.Remove(0, configTime.Length - 2) == "PM")
                    {
                        if (hours != 12)
                            hours += 12;
                    }
                    else if (configTime.Remove(0, configTime.Length - 2) == "AM")
                    {
                        if (hours == 12)
                            hours = 0;
                    }
                }
                return hours;
            }
            return -1;
        }

        private int GetMins(string configTime)
        {
            configTime = configTime.Remove(0, 3);
            if (configTime.Length > 2)
                configTime = configTime.Remove(2);
            int mins = 0;
            if (int.TryParse(configTime, out mins))
            {
                return mins;
            }
            return -1;
        }
    }


    public class AutoMessage
    {
        public bool isActive;
        public float timerInSecond;
        public bool isTemporary;
        public bool isRandomlySelected;
        public string[] messages;

        public AutoMessage(float time, bool temp, bool rand, string[] msg)
        {
            isActive = true;
            timerInSecond = time;
            isTemporary = temp;
            isRandomlySelected = rand;
            messages = msg;
        }
    }

    public class ScheduledMessage
    {
        public bool isActive;
        public bool isTemporary;
        public TimeMessage[] messages;

        public ScheduledMessage(bool temp, TimeMessage[] msg)
        {
            isActive = true;
            isTemporary = temp;
            messages = msg;
        }
    }

    public class TimeMessage
    {
        public string time;
        public string UITitle;
        public bool openUI;
        public string message;
        public TimeMessage(string tim, string msg, string title)
        {
            time = tim;
            message = msg;
            UITitle = title;
        }
    }

    public class UIMessage
    {
        public string uiTitle;
        public string filename;
        [JsonIgnore]
        public string message;

        public UIMessage(string title, string msg)
        {
            uiTitle = title;
            filename = msg;
        }
    }
}