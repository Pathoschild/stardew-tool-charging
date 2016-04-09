﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using StardewModdingAPI;
using Newtonsoft.Json;
using System.IO;

namespace ToolCharging
{
    public class ToolChargingMainClass : Mod
    {
        private static int extraRemove = 40;
        public static ToolChargingConfig config;

        public override void Entry(params object[] objects)
        {
            config = new ToolChargingConfig().InitializeConfig(BaseConfigPath);
            extraRemove = config.IncreaseBy;

            StardewModdingAPI.Events.GameEvents.UpdateTick += Event_Update;
        }

        static void myLog(String theString)
        {
#if DEBUG
            Log.Info(theString);
#endif

        }
        

        static void debugThing(object theObject, string descriptor = "")
        {
            String thing = JsonConvert.SerializeObject(theObject, Formatting.Indented,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            File.WriteAllText("debug.json", thing);
            Console.WriteLine(descriptor + "\n" + thing);
        }

        static void Event_Update(object sender, EventArgs e)
        {
            if(Game1.player != null)
            {
               
                int hold = Game1.player.toolHold;
                if(!Game1.player.canReleaseTool || hold <= 0) { return; } //either maxed or not being held

                if (hold - extraRemove <= 0)
                {
                    Game1.player.toolHold = 1;
                }
                else {
                    Game1.player.toolHold -= extraRemove;
                }
            }
        }

    }

    public class ToolChargingConfig : Config
    {
        private int _increaseBy;
        public int IncreaseBy {
            get { return _increaseBy; }
            set {
                if (value < -15) { _increaseBy = -15;}
                else if(value > 599) { _increaseBy = 599; }
                else { _increaseBy = value; }
            }
        }

        public override T GenerateDefaultConfig<T>()
        {
            IncreaseBy = 40; //
            return this as T;
        }

    }
}
