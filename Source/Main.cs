using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;
using Verse.Noise;
using Verse.Grammar;
using RimWorld;
using RimWorld.Planet;

using System.Reflection;
using HarmonyLib;

namespace HARCheckMaskShaderPatch
{
    public class ContentFinderOptimizerSettings : ModSettings
    {
        public bool enabled = true;
        public int tickLifetime = 60;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref enabled, "enabled", true);
            Scribe_Values.Look(ref tickLifetime, "tickLifetime", 60);
            base.ExposeData();
        }
    }

    public class ContentFinderOptimizerMod : Mod
    {
        string intBuffer;
        public static ContentFinderOptimizerSettings settings;
        public ContentFinderOptimizerMod(ModContentPack content) : base(content)
        {
            settings = GetSettings<ContentFinderOptimizerSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.CheckboxLabeled("ContentFinderOptimizer.Enabled".Translate(), ref settings.enabled, tooltip: "EnableDesc".Translate());
            listingStandard.Label("ContentFinderOptimizer.Label".Translate());
            listingStandard.SubLabel("ContentFinderOptimizer.SubLabel".Translate(), 100f);
            listingStandard.IntEntry(ref settings.tickLifetime, ref intBuffer, min: 1);
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "ContentFinderOptimizer.ModName".Translate();
        }
    }

    [StaticConstructorOnStartup]
    public static class Start
    {
        static Start()
        {
            Log.Message("Starting ContentFinder Optimizer!");

            Harmony harmony = new Harmony("ContentFinder Optimizer");
            harmony.PatchAll( Assembly.GetExecutingAssembly() );
        }
    }
    
    [HarmonyPatch(typeof(Verse.ContentFinder<Texture2D>), "Get", new Type[] {typeof(string), typeof(bool)})]
    public static class CheckMaskShader_Patch
    {
        private static Dictionary<string, Texture2D> textureCache = new Dictionary<string, Texture2D>();
        private static Dictionary<string, int> textureTick = new Dictionary<string, int>();

        public static bool Prefix(ref Texture2D __result, ref string itemPath, ref bool reportFailure)
        {
            int tick = GenTicks.TicksGame;
            int tickLimit = LoadedModManager.GetMod<ContentFinderOptimizerMod>().GetSettings<ContentFinderOptimizerSettings>().tickLifetime;
            bool enabled = LoadedModManager.GetMod<ContentFinderOptimizerMod>().GetSettings<ContentFinderOptimizerSettings>().enabled;
            
            if(enabled == false) return true;
            
            if(textureCache.ContainsKey(itemPath))
            {
                if(textureTick.ContainsKey(itemPath))
                {
                    //Log.Message("Tick limit is " + tickLimit);
                    if(tick - textureTick[itemPath] >= tickLimit)
                    {
                        textureTick.Remove(itemPath);
                        textureCache.Remove(itemPath);
                        //Log.Message("Removed texture from cache: " + itemPath);
                        return true;
                    }
                }
                __result = textureCache[itemPath];
                return false;
            }
            
            return true;
        }

        static void Postfix(ref Texture2D __result, ref string itemPath)
        {
            bool enabled = LoadedModManager.GetMod<ContentFinderOptimizerMod>().GetSettings<ContentFinderOptimizerSettings>().enabled;
            
            if(enabled == false) return;
            
            if(textureCache.ContainsKey(itemPath) == false)
            {
                    //Log.Message("Adding texture to cache: " + itemPath);
                    textureCache.Add(itemPath, __result);
                    textureTick.Add(itemPath, GenTicks.TicksGame);
            }
        }
    }
}
