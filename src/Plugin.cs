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

        private bool aeri = false;

        static readonly PlayerFeature<bool> AeriEnable = PlayerBool("aeri_enable");

        // Add hooks
        public void OnEnable()
        {
            // hooks
            On.Player.Update += Player_Update;
            On.Player.TerrainImpact += Player_TerrainImpact;
            On.Player.GrabVerticalPole += Player_GrabVerticalPole;
            On.Player.WallJump += Player_WallJump;

            // visuals init
            On.RainWorld.OnModsInit += Init;
            On.PlayerGraphics.DrawSprites += PlayerGraphics_DrawSprites;
        }

        private void Init(On.RainWorld.orig_OnModsInit orig, RainWorld self)
        {
            orig(self);
            try
            {
                atlas ??= Futile.atlasManager.LoadAtlas("sprites/sunhat");
            }
            catch (InvalidCastException)
            {
                // god damn, you really have to remember to put this here huh
                Logger.LogWarning("fuck");
            }
        }

        private void PlayerGraphics_DrawSprites(On.PlayerGraphics.orig_DrawSprites orig, PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, UnityEngine.Vector2 camPos)
        {
            orig(self, sLeaser, rCam, timeStacker, camPos);

            if (atlas == null)
            {
                return;
            }
            if (aeri)
            {
                string name = sLeaser.sprites[3]?.element?.name;
                if (name != null && name.StartsWith("HeadA") && atlas._elementsByName.TryGetValue("Sun" + name, out var element))
                {
                    sLeaser.sprites[3].element = element;
                }
            }
        }

        // Flight Code
        private void Player_Update(On.Player.orig_Update orig, Player self, bool eu)
        {
            orig(self, eu);

            if (AeriEnable.TryGet(self, out var aerienable))
            {
                if (self.input[0].y > 0)
                {
                    if (canFly)
                    {
                        //self.Jump();
                        self.mainBodyChunk.vel.y += 2.3f;
                        airTime++;
                    }

                    if (airTime > 50)
                    {
                        self.mainBodyChunk.vel.y = 3;
                    }
                }

                if (airTime > 50)
                {
                    canFly = false;
                }

                aeri = aerienable;
            }
        }

        private void Player_WallJump(On.Player.orig_WallJump orig, Player self, int direction)
        {
            orig(self, direction);
            self.animation = Player.AnimationIndex.Flip;
        }

        private void Player_GrabVerticalPole(On.Player.orig_GrabVerticalPole orig, Player self)
        {
            orig(self);
            canFly = true;
            airTime = 0;
        }

        private void Player_TerrainImpact(On.Player.orig_TerrainImpact orig, Player self, int chunk, RWCustom.IntVector2 direction, float speed, bool firstContact)
        {
            orig(self, chunk, direction, speed, firstContact);
            canFly = true;
            airTime = 0;
        }
    }
}