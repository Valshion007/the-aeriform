﻿using System;
using BepInEx;
using UnityEngine;
using SlugBase.Features;
using static SlugBase.Features.FeatureTypes;
using MoreSlugcats;
using IL.RWCustom;
using RWCustom;
using System.Diagnostics.Eventing.Reader;

namespace SlugTemplate
{
    [BepInPlugin(MOD_ID, "The Aeriform", "0.1.0")]
    class Plugin : BaseUnityPlugin
    {
        private const string MOD_ID = "aeriform";
        private float airTime = 0f;
        //private bool grabbingPole = false;
        private bool canFly = false;

        // Add hooks
        public void OnEnable()
        {
            On.RainWorld.OnModsInit += Extras.WrapInit(LoadResources);

            // Put your custom hooks here!
            //On.Player.Jump += PlayerJumpHook;
            On.Player.Update += PlayerUpdateHook;
            On.Player.Die += PlayerDieHook;
            On.Player.MovementUpdate += MovementHook;
            On.Player.TerrainImpact += LevelCollisionHook;
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
                if (canFly)
                {
                    self.Jump();
                    airTime++;
                }
            }

            if (airTime > 200)
            {
                canFly = false;
            }
        }

        private void LevelCollisionHook(On.Player.orig_TerrainImpact orig, Player self, int chunk, RWCustom.IntVector2 direction, float speed, bool firstContact)
        {
            orig(self, chunk, direction, speed, firstContact);
            canFly = true;
        }

        private void PlayerDieHook(On.Player.orig_Die orig, Player self)
        {
            orig(self);
            //wait what was i using this for?
        }

        private void MovementHook(On.Player.orig_MovementUpdate orig, Player self, bool eu)
        {
            orig(self, eu);
            //uhh do something with self.GrabVerticalPole or something
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