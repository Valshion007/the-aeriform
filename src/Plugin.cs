using System;
using BepInEx;
using UnityEngine;
using SlugBase.Features;
using static SlugBase.Features.FeatureTypes;
using MoreSlugcats;
using IL.RWCustom;
using RWCustom;
using System.Diagnostics.Eventing.Reader;
using Aeriform;

namespace SlugTemplate
{
    [BepInPlugin(MOD_ID, "The Aeriform", "0.1.0")]
    class Plugin : BaseUnityPlugin
    {
        private const string MOD_ID = "aeriform";
        private float airTime = 0f;
        //private bool grabbingPole = false;
        private bool canFly = false;

        private bool isInit = false;

        public void OnEnable()
        {
            // runs the other parts
            On.RainWorld.OnModsInit += Init;
        }

        private void Init(On.RainWorld.orig_OnModsInit orig, RainWorld self)
        {
            orig(self);

            GameplayHooks gameplayhooks = new GameplayHooks();
            PlayerVisualHooks playervisualhooks = new PlayerVisualHooks();
            AeriStore aeristore = new AeriStore();

            try
            {
                if (isInit) return;
                isInit = true;

                Futile.atlasManager.LoadAtlas("sprites/wings/wingsatlas");
                Futile.atlasManager.LoadAtlas("sprites/sunhat");

                playervisualhooks.Init();
                gameplayhooks.Init();

                Debug.Log($"Plugin Aeriform is loaded!");
            }
            catch (InvalidCastException)
            {
                // god damn, you really have to remember to put this here huh
                Logger.LogWarning("damnit");
                throw;
            }
        }
    }
}