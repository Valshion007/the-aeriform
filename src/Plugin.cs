using System;
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
        FAtlas atlas;
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
            On.Player.Update += Player_Update;
            //On.Player.Die += PlayerDieHook;
            //On.Player.MovementUpdate += MovementHook;
            On.Player.TerrainImpact += Player_TerrainImpact;
            On.Player.GrabVerticalPole += Player_GrabVerticalPole;

            //Visuals Init
            On.RainWorld.OnModsInit += Init;
            On.PlayerGraphics.DrawSprites += PlayerGraphics_DrawSprites;
        }
        
        // Load any resources, such as sprites or sounds
        private void LoadResources(RainWorld rainWorld)
        {

        }

        private void Init(On.RainWorld.orig_OnModsInit orig, RainWorld self)
        {
            orig(self);

            atlas ??= Futile.atlasManager.LoadAtlas("sprites/aerihead");
        }

        private void PlayerGraphics_DrawSprites(On.PlayerGraphics.orig_DrawSprites orig, PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, UnityEngine.Vector2 camPos)
        {
            orig(self, sLeaser, rCam, timeStacker, camPos);

            if (atlas == null)
            {
                return;
            }

            string name = sLeaser.sprites[3]?.element?.name;
            if (name != null && name.StartsWith("HeadA") && atlas._elementsByName.TryGetValue("Aeri" + name, out var element))
            {
                sLeaser.sprites[3].element = element;
            }
        }

        // Flight Code
        private void Player_Update(On.Player.orig_Update orig, Player self, bool eu)
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

            if (airTime > 50)
            {
                canFly = false;
            }
        }

        private void Player_GrabVerticalPole(On.Player.orig_GrabVerticalPole orig, Player self)
        {
            orig(self);
            canFly = false;
        }

        private void Player_TerrainImpact(On.Player.orig_TerrainImpact orig, Player self, int chunk, RWCustom.IntVector2 direction, float speed, bool firstContact)
        {
            orig(self, chunk, direction, speed, firstContact);
            canFly = true;
            airTime = 0;
        }
        //no clue what im using this for. comming.
        /*
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