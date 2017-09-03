using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Frozen.Helpers;

namespace Frozen.Rotation
{ 
    public class Enhancement : CombatRoutine
    {
        private const int Revision = 1;
        private const int interMin = 50;
        private const int interMax = 90;
        public Stopwatch Crash = new Stopwatch();
        public Stopwatch Pets = new Stopwatch();
        public Stopwatch Rotation = new Stopwatch();
        public override Form SettingsForm { get; set; }
        
        private static bool HailstormCheck
        {
            get
            {
                if (WoW.Talent(4) == 3 && !WoW.PlayerHasBuff("Frostbrand") || WoW.Talent(4) != 3)
                    return true;
                return false;
            }
        }
        
        private static bool FuryCheck80
        {
            get
            {
                if (WoW.Talent(6) != 2 || WoW.Talent(6) == 2 && WoW.Maelstrom > 80)
                    return true;
                return false;
            }
        }
        
        private static bool FuryCheck45
        {
            get
            {
                if (WoW.Talent(6) != 2 || WoW.Talent(6) == 2 && WoW.Maelstrom > 45)
                    return true;
                return false;
            }
        }
        
        private static bool FuryCheck25
        {
            get
            {
                if (WoW.Talent(6) != 2 || WoW.Talent(6) == 2 && WoW.Maelstrom >= 25)
                    return true;
                return false;
            }
        }
        
        private static bool OCPool70
        {
            get
            {
                if (WoW.Talent(5) != 2 || WoW.Talent(5) == 2 && WoW.Maelstrom > 70)
                    return true;
                return false;
            }
        }
        
        private static bool OCPool60
        {
            get
            {
                if (WoW.Talent(5) != 2 || WoW.Talent(5) == 2 && WoW.Maelstrom > 60)
                    return true;
                return false;
            }
        }
        
        private static bool AkainuEquip
        {
            get
            {
                if (WoW.Legendary(1) == 9 || WoW.Legendary(2) == 9)
                    return true;
                return false;
            }
        }
        private static bool Akainus
        {
            get
            {
                if (AkainuEquip && WoW.PlayerHasBuff("Hot Hands") && !WoW.PlayerHasBuff("Frostbrand"))
                    return true;
                return false;
            }
        }
        private static bool LightningCrashNotUp
        {
            get
            {
                if (!WoW.PlayerHasBuff("Lightning crash") && WoW.SetBonus(20) == 2)
                    return true;
                return false;
            }
        }

        private bool AlphaWolfCheck => Pets.IsRunning && Crash.IsRunning && Crash.ElapsedMilliseconds < 8000;

        private static float GCD => Convert.ToSingle(150 / (1 + WoW.HastePercent / 100f)) > 75f ? Convert.ToSingle(150f / (1 + WoW.HastePercent / 100f)) : 75f;

        public override void Initialize()
        {
            Log.Write("Welcome to Enhancement Shaman by Hamuel", Color.Green);
            Log.Write("version " + Revision, Color.Green);
        }

        public void EnhancementCD()
        {
            if (UseCooldowns)
            {
                if (WoW.PlayerRace == "Troll" && WoW.CanCast("Berserking") && !WoW.IsSpellOnCooldown("Berserking") &&
                    (WoW.Talent(7) != 1 || WoW.PlayerHasBuff("Ascendance") || Pets.ElapsedMilliseconds < 10000))
                {
                    WoW.CastSpell("Berserking");
                    return;
                }
                if (WoW.PlayerRace == "Orc" && WoW.CanCast("Blood Fury")
                    && (WoW.Talent(7) != 1 || WoW.PlayerHasBuff("Ascendance") || Pets.ElapsedMilliseconds < 10000))
                {
                    WoW.CastSpell("Blood Fury");
                    return;
                }
                if (WoW.CanCast("Feral Spirit") && WoW.IsSpellInRange("Rockbiter") && WoW.Maelstrom >= 20 &&
                    (WoW.CanCast("Crash lightning") ||
                     WoW.SpellCooldownTimeRemaining("Crash lightning") < GCD)) //feral spirit on boss - normally cast manually
                {
                    Pets.Start();
                    WoW.CastSpell("Feral Spirit");
                    return;
                }
                if (WoW.CanCast("Doom Winds") && WoW.IsSpellInRange("Rockbiter") &&
                    (WoW.Talent(7) == 3 && WoW.TargetHasDebuff("Earthen spike") || WoW.Talent(7) != 3))
                {
                    WoW.CastSpell("Doom Winds");
                    return;
                }
                if (WoW.CanCast("Ascendance") && WoW.PlayerHasBuff("Doom Winds"))
                    WoW.CastSpell("Ascendance");
            }
        }

        private void EnhancementBuffs()
        {
            if (WoW.CanCast("Windstrike", true, true, true) && WoW.PlayerHasBuff("Ascendance") && WoW.Maelstrom >= 8 && WoW.SetBonus(19) >= 2 &&
                (WoW.Talent(7) != 3 || WoW.SpellCooldownTimeRemaining("Earthen spike") > 1 && WoW.SpellCooldownTimeRemaining("Doom winds") > 1 ||
                 WoW.TargetHasDebuff("Earthen spike")))
            {
                WoW.CastSpell("Windstrike");
                return;
            }
            if (WoW.CanCast("Rockbiter", true, true, true) && WoW.Talent(1) == 3 && !WoW.PlayerHasBuff("Landslide"))
            {
                WoW.CastSpell("Rockbiter");
                return;
            }
            if (WoW.CanCast("FoA") && WoW.Maelstrom >= 5 && WoW.Talent(6) == 2 && !WoW.PlayerHasBuff("FoA") && WoW.PlayerHasBuff("Ascendance") &&
                Pets.IsRunning)
            {
                WoW.CastSpell("FoA");
                return;
            }
            if (WoW.CanCast("Crash lightning") && WoW.Maelstrom >= 20 && WoW.IsSpellInRange("Rockbiter") && Pets.IsRunning && !Crash.IsRunning)
            {
                Crash.Restart();
                WoW.CastSpell("Crash lightning");
                return;
            }
            if (!WoW.PlayerHasBuff("Flametongue") && WoW.CanCast("Flametongue", true, true, true))
            {
                WoW.CastSpell("Flametongue");
                return;
            }
            if (WoW.CanCast("Frostbrand", true, true, true) && WoW.Maelstrom >= 20 && WoW.Talent(4) == 3 && !WoW.PlayerHasBuff("Frostbrand") && FuryCheck45)
            {
                WoW.CastSpell("Frostbrand");
                return;
            }
            if (WoW.CanCast("Flametongue", true, true, true) && WoW.PlayerBuffTimeRemaining("Flametongue") < 600 + GCD &&
                WoW.SpellCooldownTimeRemaining("Doom Winds") < GCD * 2)
            {
                WoW.CastSpell("Flametongue");
                return;
            }
            if (WoW.CanCast("Frostbrand", true, true, true) && WoW.Maelstrom >= 20 && WoW.PlayerBuffTimeRemaining("Frostbrand") < 600 + GCD &&
                WoW.SpellCooldownTimeRemaining("Doom Winds") < GCD * 2)
                WoW.CastSpell("Frostbrand");
        }

        private void EnhancementCore()
        {
            if (WoW.CanCast("lava lash", true, true, true) && WoW.Maelstrom >= 40 &&
                (WoW.TargetDebuffStacks("Legionfall") > 90 || WoW.PlayerHasBuff("Hot Hands") && WoW.PlayerBuffTimeRemaining("Hot Hands") < 2))
            {
                Log.Write("Maelstrom overflow protection", Color.Blue);
                WoW.CastSpell("lava lash");
                return;
            }
            if (WoW.CanCast("Earthen spike") && WoW.Maelstrom >= 20 && WoW.Talent(7) == 3 && FuryCheck25)
            {
                WoW.CastSpell("Earthen spike");
                return;
            }
            if (WoW.CanCast("Crash lightning") && WoW.Maelstrom >= 20 && WoW.IsSpellInRange("Rockbiter") && !WoW.PlayerHasBuff("Crash lightning") &&
                combatRoutine.Type != RotationType.SingleTarget)
            {
                WoW.CastSpell("Crash lightning");
                return;
            }
            if (WoW.CanCast("Windsong", true, true, true) && WoW.Talent(1) == 1)
            {
                WoW.CastSpell("Windsong");
                return;
            }
            if (WoW.CanCast("Crash lightning") && WoW.Maelstrom >= 20 && WoW.IsSpellInRange("Rockbiter") && combatRoutine.Type == RotationType.AOE &&
                (WoW.CountEnemyNPCsInRange >= 8 || WoW.CountEnemyNPCsInRange >= 6 && WoW.Talent(6) == 1))
            {
                WoW.CastSpell("Crash lightning");
                return;
            }
            if (WoW.CanCast("Windstrike", true, true, true) && WoW.Maelstrom >= 8 && WoW.PlayerHasBuff("Ascendance"))
            {
                WoW.CastSpell("Windstrike");
                return;
            }
            if (WoW.CanCast("Stormstrike", true, true, true) && WoW.Maelstrom >= 20 && WoW.PlayerHasBuff("Stormbringer") && FuryCheck25)
            {
                WoW.CastSpell("Stormstrike");
                return;
            }
            if (WoW.CanCast("Crash lightning") && WoW.Maelstrom >= 20 && WoW.IsSpellInRange("Rockbiter") && combatRoutine.Type != RotationType.SingleTarget &&
                (WoW.CountEnemyNPCsInRange >= 4 || WoW.CountEnemyNPCsInRange > 2 && WoW.Talent(6) == 1))
            {
                WoW.CastSpell("Crash lightning");
                return;
            }
            if (WoW.CanCast("Lightning bolt") && WoW.Talent(5) == 2 && FuryCheck45 && WoW.Maelstrom >= 40)
            {
                WoW.CastSpell("Lightning bolt");
                return;
            }
            if (WoW.CanCast("Stormstrike", true, true, true) && WoW.Maelstrom > 40 && (WoW.Talent(5) != 2 && FuryCheck45 || WoW.Talent(5) == 2 && FuryCheck80))
            {
                WoW.CastSpell("Stormstrike");
                return;
            }
            if (WoW.CanCast("Frostbrand", true, true, true) && WoW.Maelstrom >= 20 && Akainus)
            {
                WoW.CastSpell("Frostbrand");
                return;
            }
            if (WoW.CanCast("lava lash", true, true, true) && WoW.PlayerHasBuff("Hot Hands") && (AkainuEquip && WoW.PlayerHasBuff("Frostbrand") || !AkainuEquip))
            {
                WoW.CastSpell("lava lash");
                return;
            }
            if (WoW.CanCast("Sundering") && WoW.Maelstrom >= 20 && WoW.Talent(6) == 3 && combatRoutine.Type == RotationType.AOE)
            {
                WoW.CastSpell("Sundering");
                return;
            }
            if (WoW.CanCast("Crash lightning") && WoW.Maelstrom >= 20 && WoW.IsSpellInRange("Rockbiter") && (combatRoutine.Type == RotationType.AOE || LightningCrashNotUp || AlphaWolfCheck))
            {
                Crash.Restart();
                WoW.CastSpell("Crash lightning");
            }
        }

        private void EnhancementFiller()
        {
            if (WoW.CanCast("Rockbiter", true, true, true) && WoW.Maelstrom < 120)
            {
                WoW.CastSpell("Rockbiter");
                return;
            }
            if (WoW.CanCast("Flametongue", true, true, true) && (!WoW.PlayerHasBuff("Flametongue") || WoW.PlayerBuffTimeRemaining("Flametongue") < 480))
            {
                WoW.CastSpell("Flametongue");
                return;
            }
            if (WoW.CanCast("Rockbiter", true, true, true) && WoW.Maelstrom < 40)
            {
                WoW.CastSpell("Rockbiter");
                return;
            }
            if (WoW.CanCast("Crash lightning") && WoW.Maelstrom >= 20 && WoW.IsSpellInRange("Rockbiter") && WoW.Talent(6) == 1 &&
                combatRoutine.Type != RotationType.SingleTarget && WoW.TargetHasDebuff("Earthen spike") && WoW.Maelstrom >= 40 && OCPool60)
            {
                Crash.Restart();
                WoW.CastSpell("Crash lightning");
                return;
            }
            if (WoW.CanCast("Frostbrand", true, true, true) && WoW.Maelstrom >= 20 && HailstormCheck &&
                (!WoW.PlayerHasBuff("Frostbrand") || WoW.PlayerBuffTimeRemaining("Frostbrand") < 480 && WoW.Maelstrom >= 40))
            {
                WoW.CastSpell("Frostbrand");
                return;
            }
            if (WoW.CanCast("Frostbrand", true, true, true) && WoW.Maelstrom >= 20 && AkainuEquip && !WoW.PlayerHasBuff("Frostbrand") && WoW.Maelstrom >= 75)
            {
                WoW.CastSpell("Frostbrand");
                return;
            }
            if (WoW.CanCast("Sundering") && WoW.Maelstrom >= 20 && WoW.Talent(6) == 3)
            {
                WoW.CastSpell("Sundering");
                return;
            }
            if (WoW.CanCast("lava lash", true, true, true) && WoW.Maelstrom > 50 && OCPool70 && FuryCheck80)
            {
                WoW.CastSpell("lava lash");
                return;
            }
            if (WoW.CanCast("Rockbiter", true, true, true))
            {
                WoW.CastSpell("Rockbiter");
                return;
            }
            if (WoW.CanCast("Crash lightning") && WoW.Maelstrom >= 20 && WoW.IsSpellInRange("Rockbiter") && (WoW.Maelstrom > 65) | (WoW.Talent(6) == 1) &&
                combatRoutine.Type != RotationType.SingleTarget && OCPool60 && FuryCheck45)
            {
                Crash.Restart();
                WoW.CastSpell("Crash lightning");
                return;
            }
            if (WoW.CanCast("Flametongue", true, true, true))
                WoW.CastSpell("Flametongue");
        }

        private void TimerReset()
        {
            if (Crash.Elapsed.Seconds >= 8)
                Crash.Reset();
            if (Pets.Elapsed.Seconds >= 15)
                Pets.Reset();
        }

        public override void Pulse()
        {
            TimerReset();
            if (!WoW.IsInCombat || WoW.IsMounted) return;

            Interruptcast();
            Defensive();
            EnhancementBuffs();
            EnhancementCD();
            EnhancementCore();
            EnhancementFiller();
        }

        private static void Defensive()
        {
            if (WoW.Talent(2) == 1 && WoW.CanCast("Rainfall") && !WoW.PlayerHasBuff("Rainfall") && !WoW.IsSpellOnCooldown("Rainfall")) 
            {
                WoW.CastSpell("Rainfall");
                return;
            }
            if (WoW.CanCast("Astral Shift") && WoW.PlayerHealthPercent < 60 && !WoW.IsSpellOnCooldown("Astral Shift")) 
            {
                WoW.CastSpell("Astral Shift");
                return;
            }
            if (WoW.PlayerRace == "Dreanei" && WoW.PlayerHealthPercent < 80 && !WoW.IsSpellOnCooldown("Gift Naaru"))
                WoW.CastSpell("Gift Naaru");
        }

        private static void Interruptcast()
        {
            var random = new Random();
            var randomNumber = random.Next(interMin, interMax);

            if (WoW.TargetPercentCast > randomNumber && WoW.TargetIsCastingAndSpellIsInterruptible)
            {
                if (WoW.CanCast("Wind Shear") && !WoW.IsSpellOnCooldown("Wind Shear") && WoW.TargetIsCasting &&
                    WoW.IsSpellInRange("Wind Shear")) //interupt every spell, not a boss.
                {
                    WoW.CastSpell("Wind Shear");
                    return;
                }
                if (WoW.PlayerRace == "BloodElf" && WoW.CanCast("Arcane Torrent") && !WoW.IsSpellOnCooldown("Wind Shear") &&
                    WoW.IsSpellInRange("Stormstrike")) //interupt every spell, not a boss.
                {
                    WoW.CastSpell("Arcane Torrent");
                    return;
                }
                if (WoW.PlayerRace == "Pandaren" && WoW.CanCast("Quaking palm", true, true, true)) //interupt every spell, not a boss.
                {
                    WoW.CastSpell("Quaking palm");
                }
            }
        }

        public override void Stop()
        {
        }
    }
}
/*
[AddonDetails.db]
AddonAuthor=Hamuel
AddonName=Frozen
WoWVersion=Legion - 70300
[SpellBook.db]
Spell,57994,Wind Shear,NumPad1
Spell,196884,Feral Lunge,F9
Spell,51533,Feral Spirit,NumPad0
Spell,196834,Frostbrand,NumPad2
Spell,204945,Doom Winds,F1
Spell,187874,Crash lightning,NumPad4
Spell,193796,Flametongue,NumPad3
Spell,108271,Astral Shift,F2
Spell,193786,Rockbiter,NumPad5
Spell,60103,lava lash,NumPad6
Spell,17364,Stormstrike,NumPad7
Spell,115356,Windstrike,NumPad7
Spell,187837,Lightning bolt,NumPad8
Spell,188070,Healing Surge,NumPad9
Spell,215864,Rainfall,F8
Spell,188089,Earthen spike,F4
Spell,201898,Windsong,F5
Spell,197217,Sundering,F6
Spell,114051,Ascendance,Add
Spell,197211,FoA,Subtract
Spell,59544,Gift Naaru,F10
Spell,192058,Lightning Surge,F7
Spell,26297,Berserking,F10
Spell,33697,Blood Fury,F10
Spell,20549,War Stomp,F10
Spell,155145,Arcane Torrent,F10
Spell,107079,Quaking palm,F10
Spell,142117,Prolonged Power,F11
Spell,2645,Ghost Wolf,E
Spell,3,raid3,U
Spell,2,raid2,Y
Spell,1,raid1,T
Spell,4,raid4,I
Spell,142173,Collapsing Futures,F12
Aura,194084,Flametongue
Aura,196834,Frostbrand
Aura,187878,Crashing Storm
Aura,187874,Crash lightning
Aura,201846,Stormbringer
Aura,202004,Landslide
Aura,204945,Doom Winds
Aura,215864,Rainfall
Aura,114051,Ascendance
Aura,201898,Windsong
Aura,201900,Hot Hands
Aura,197211,FoA
Aura,2645,Ghost Wolf
Aura,240842,Legionfall
Aura,234143,Temptation
Aura,242284,Lightning crash
Aura,188089,Earthen spike
Item,142117,Prolonged Power
Item,142173,Collapsing Futures
*/