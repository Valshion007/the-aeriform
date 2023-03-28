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
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using SlugBase;

namespace Aeriform
{
    internal class AeriStore
    {
        public bool aeri;
        public static readonly float flightMax = 50f;

        static readonly PlayerFeature<bool> AeriEnabled = PlayerBool("aeri_enable");
        public void Init()
        {
            On.Player.Update += (orig, player, eu) =>
            {
                orig(player, eu);

                if (AeriEnabled.TryGet(player, out var aerienable))
                {
                    aeri = aerienable;
                }
            };
        }
    }
}
