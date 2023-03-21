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
        private bool inAir = false;

        // Add hooks
        public void OnEnable()
        {
            On.RainWorld.OnModsInit += Extras.WrapInit(LoadResources);

            // Put your custom hooks here!
            //On.Player.Jump += PlayerJumpHook;
            On.Player.Update += PlayerUpdateHook;
            On.Player.Die += PlayerDieHook;
        }
        
        // Load any resources, such as sprites or sounds
        private void LoadResources(RainWorld rainWorld)
        {

        }

        // Flight Code
        private void PlayerUpdateHook(On.Player.orig_Update orig, Player self, bool eu)
        {
            orig(self, eu);

            if (self.input[0].y > 0)
            {
                self.Jump();
            }
        }

        private void PlayerDieHook(On.Player.orig_Die orig, Player self)
        {
            orig(self);
        }

        // Implement Flight (old method)
        /*
        private void PlayerJumpHook(On.Player.orig_Jump orig, Player self)
        {
            orig(self);
            inAir = true;
        }
        
        private void PlayerCollideHook(On.Player.orig_Collide orig, Player self, bool eu)
        {
            orig(self, eu);
            inAir = false;
        }
        */
    }
}