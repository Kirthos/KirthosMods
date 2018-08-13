using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 05/03/2018
 */

namespace Kirthos.Mods.Config
{
    public class EcoModConfig
    {
        private string filePath;
        private Dictionary<string, string> configs = new Dictionary<string, string>();
        public EcoModConfig(string modName, string configFileName)
        {
            string folderPath = "./Configs/" + modName;
            filePath = folderPath + "/" + configFileName + ".eco";
            if (Directory.Exists(folderPath) == false)
            {
                Directory.CreateDirectory(folderPath);
                Console.WriteLine("[EcoConfig] The config folder for '" + modName + "' was created successfully at '{0}'.", folderPath);
            }
            if (File.Exists(filePath) == false)
            {
                FileStream file = File.Create(filePath);
                Console.WriteLine("[EcoConfig] The config file '" + configFileName + "' for '" + modName + "' was created successfully.");
                if (file != null)
                    file.Dispose();
            }
            else
            {
                Console.WriteLine("[EcoConfig] Loading config file '" + configFileName + "' for '" + modName + "'.");
                LoadConfig();
            }

        }

        public void ReloadConfig()
        {
            LoadConfig();
        }

        public void SaveConfig()
        {
            string strFile = "";
            foreach (KeyValuePair<string, string> keyValuePair in configs)
            {
                if (keyValuePair.Key.Contains("comment"))
                    strFile += keyValuePair.Value + Environment.NewLine;
                else if (keyValuePair.Key.Contains("empty"))
                    strFile += keyValuePair.Value + Environment.NewLine;
                else
                    strFile += keyValuePair.Key + ":" + keyValuePair.Value + Environment.NewLine;
            }
            File.WriteAllText(filePath, strFile);
        }

        public string GetConfig(string configName, string defaultValue = "defaultValue")
        {
            string result = null;
            if (configs.TryGetValue(configName, out result))
            {
                return result;
            }
            configs.Add(configName, defaultValue);
            SaveConfig();
            return defaultValue;
        }

        public int GetConfig(string configName, int defaultValue = 0)
        {
            int result = 0;
            string strResult = null;
            if (configs.TryGetValue(configName, out strResult))
            {
                if (int.TryParse(strResult, out result))
                {
                    return result;
                }
                else
                {
                    configs.Remove(configName);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[EcoConfig] Error in config file '" + filePath + "' for config '" + configName + "' found '" + strResult + "' but expect a integer. Value reset to default.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            configs.Add(configName, defaultValue.ToString());
            SaveConfig();
            return defaultValue;
        }

        public float GetConfig(string configName, float defaultValue = 0)
        {
            float result = 0;
            string strResult = null;
            if (configs.TryGetValue(configName, out strResult))
            {
                if (float.TryParse(strResult, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
                {
                    return result;
                }
                else
                {
                    configs.Remove(configName);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[EcoConfig] Error in config file '" + filePath + "' for config '" + configName + "' found '" + strResult + "' but expect a float value. Value reset to default.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            configs.Add(configName, defaultValue.ToString(CultureInfo.InvariantCulture));
            SaveConfig();
            return defaultValue;
        }

        private void LoadConfig()
        {
            int i = 0;
            foreach (string line in File.ReadAllLines(filePath))
            {
                i++;
                if (line.Length < 2)
                {
                    configs.Add("empty" + i, line);
                }
                else if (line[0] == '/' && line[1] == '/')
                {
                    configs.Add("comment" + i, line);
                }
                else
                {
                    string[] str = line.Split(':');
                    if (str.Length != 2)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[EcoConfig] Error in config file '" + filePath + "' at line " + i + " - '" + line + "'");
                        Console.ForegroundColor = ConsoleColor.White;
                        return;
                    }
                    configs.Add(str[0], str[1]);
                }
            }
        }
    }
}
