using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aeriform;
using SlugBase;
using SlugBase.Features;
using static SlugBase.Features.FeatureTypes;
using MoreSlugcats;
using IL.RWCustom;
using RWCustom;
using BepInEx;
using UnityEngine;

namespace Aeriform
{
    internal class GameplayHooks
    {
        private float airTime = 0f;
        //private bool grabbingPole = false;
        private bool canFly = false;

        public void Init()
        {
            On.Player.Update += Player_Update;
            On.Player.TerrainImpact += Player_TerrainImpact;
            On.Player.GrabVerticalPole += Player_GrabVerticalPole;
            On.Player.WallJump += Player_WallJump;
        }

        // Flight Code
        private void Player_Update(On.Player.orig_Update orig, Player self, bool eu)
        {
            orig(self, eu);
            AeriStore aeristore;
            aeristore = new AeriStore();

            if (aeristore.aeri)
            {
                if (self.input[0].y > 0)
                {
                    if (canFly)
                    {
                        //self.Jump();
                        self.mainBodyChunk.vel.y += 2.3f;
                        airTime++;
                    }

                    if (airTime > AeriStore.flightMax)
                    {
                        self.mainBodyChunk.vel.y = 3;
                    }
                }

                if (airTime > AeriStore.flightMax)
                {
                    canFly = false;
                }
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
