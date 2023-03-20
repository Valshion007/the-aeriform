using System;
using BepInEx;
using UnityEngine;
using SlugBase.Features;
using static SlugBase.Features.FeatureTypes;
using MoreSlugcats;

namespace SlugTemplate
{
    [BepInPlugin(MOD_ID, "The Aeriform", "0.1.0")]
    class Plugin : BaseUnityPlugin
    {
        private const string MOD_ID = "aeriform";

        // Add hooks
        public void OnEnable()
        {
            On.RainWorld.OnModsInit += Extras.WrapInit(LoadResources);

            // Put your custom hooks here!
            On.Player.Jump += Player_Jump;
        }
        
        // Load any resources, such as sprites or sounds
        private void LoadResources(RainWorld rainWorld)
        {

        }

        // Implement Flight
        private void Player_Jump(On.Player.orig_Jump orig, Player self)
        {
            orig(self);
        }
    }
}