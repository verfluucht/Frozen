using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Frozen.Helpers;

namespace Frozen.Rotation
{
    public class NilremProtLeg : CombatRoutine
    {
        public Stopwatch CombatWatch = new Stopwatch();

        public override string Name => "Prot Warrior";

        public override string Class => "Warrior";

        public override Form SettingsForm { get; set; }
        
        public override void Initialize()
        {
            Log.Write("Welcome to Protection Warrior", Color.Green);
            Log.Write("Suggested build: 1213312", Color.Green);
            Log.Write("LEFT CTRL - Heroic Leap (please make @Cursor macro for it)", Color.Black);
            Log.Write("LEFT ALT - Shockwave + Neltharion's Fury if not on CD", Color.Black);
        }

        public override void Stop()
        {
        }

        public override void Pulse()
        {
            if (WoW.PlayerHasBuff("Mount")) return;

            if (WoW.IsInCombat && WoW.HealthPercent < 35 && WoW.CanCast("Last Stand") && !WoW.IsSpellOnCooldown("Last Stand"))
            {
                WoW.CastSpell("Last Stand");
                return;
            }
            if (WoW.IsInCombat && WoW.HealthPercent < 20 && WoW.CanCast("Shield Wall") && !WoW.IsSpellOnCooldown("Shield Wall"))
            {
                WoW.CastSpell("Shield Wall");
                return;
            }

            if (DetectKeyPress.GetKeyState(DetectKeyPress.VK_LCONTROL) < 0)
            {
                if (WoW.IsInCombat && !WoW.IsSpellOnCooldown("HeroicLeap"))
                {
                    WoW.CastSpell("HeroicLeap");
                    return;
                }
                return;
            }
            if (DetectKeyPress.GetKeyState(DetectKeyPress.VK_KEY_1) < 0)
            {
                if (WoW.IsInCombat && !WoW.IsSpellOnCooldown("Thunder Clap"))
                {
                    WoW.CastSpell("Thunder Clap");
                    return;
                }
                return;
            }
            if (DetectKeyPress.GetKeyState(DetectKeyPress.VK_LMENU) < 0)
            {
                if (WoW.IsInCombat && !WoW.IsSpellOnCooldown("Shockwave"))
                {
                    WoW.CastSpell("Shockwave");
                    return;
                }
                return;
            }

            if (combatRoutine.Type == RotationType.SingleTarget || combatRoutine.Type == RotationType.AOE) // Do Single Target Stuff here
            {
                if (CombatWatch.IsRunning && !WoW.IsInCombat)
                    CombatWatch.Reset();
                if (!CombatWatch.IsRunning && WoW.IsInCombat)
                    CombatWatch.Start();

                if (WoW.HasTarget && WoW.TargetIsEnemy && !WoW.PlayerIsChanneling)
                {
                    if (!WoW.TargetHasDebuff("ShockWavestun") && WoW.IsInCombat)
                    {
                        if (WoW.CanCast("Shield Block") && WoW.Rage >= 15 && !WoW.PlayerIsChanneling && WoW.HealthPercent < 100 &&
                            (WoW.PlayerSpellCharges("Shield Block") == 2 ||
                             WoW.PlayerSpellCharges("Shield Block") >= 1 && WoW.HealthPercent <= 90 && WoW.PlayerBuffTimeRemaining("ShieldBlockAura") <= 2))
                        {
                            WoW.CastSpell("Shield Block");
                            return;
                        }

                        if (WoW.IsSpellInRange("Shield Slam") && WoW.CanCast("Thunder Clap") && !WoW.IsSpellOnCooldown("Thunder Clap") &&
                            CombatWatch.ElapsedMilliseconds > 1000 &&
                            CombatWatch.ElapsedMilliseconds < 5000)
                        {
                            WoW.CastSpell("Thunder Clap");
                            return;
                        }

                        /* ------------------ IGNORE PAIN MANAGEMENT----------------------*/

                        if (WoW.CanCast("Ignore Pain") && WoW.PlayerHasBuff("Vengeance: Ignore Pain") && WoW.Rage >= 39)
                        {
                            WoW.CastSpell("Ignore Pain");
                            return;
                        }

                        if (WoW.CanCast("Ignore Pain") && WoW.Rage > 30 && WoW.HealthPercent < 100 &&
                            (!WoW.PlayerHasBuff("Ignore Pain") || WoW.PlayerBuffTimeRemaining("Ignore Pain") <= 2) &&
                            !WoW.PlayerHasBuff("Vengeance: Ignore Pain") && !WoW.PlayerHasBuff("Vengeance: Focused Rage"))
                        {
                            WoW.CastSpell("Ignore Pain");
                            return;
                        }

                        /* ------------------ END IGNORE PAIN MANAGEMENT-------------------*/

                        if (WoW.TargetIsCasting && WoW.CanCast("SpellReflect") && !WoW.IsSpellOnCooldown("SpellReflect"))
                            WoW.CastSpell("SpellReflect");
                        if (WoW.IsSpellInRange("Shield Slam") && WoW.CanCast("Battle Cry") && !WoW.IsSpellOnCooldown("Battle Cry"))
                        {
                            WoW.CastSpell("Battle Cry");
                            return;
                        }
                        if (WoW.IsSpellInRange("Shield Slam") && WoW.CanCast("Shield Slam") && !WoW.IsSpellOnCooldown("Shield Slam") &&
                            WoW.PlayerHasBuff("Legendary"))
                        {
                            WoW.CastSpell("Shield Slam");
                            return;
                        }
                        if (WoW.IsSpellInRange("Shield Slam") && WoW.CanCast("Thunder Clap") && !WoW.IsSpellOnCooldown("Thunder Clap") &&
                            WoW.PlayerHasBuff("Legendary"))
                        {
                            WoW.CastSpell("Thunder Clap");
                            return;
                        }
                        if (WoW.CanCast("Revenge") && !WoW.IsSpellOnCooldown("Revenge") && WoW.IsSpellInRange("Shield Slam") &&
                            WoW.IsSpellOverlayed("Revenge") &&
                            !WoW.PlayerHasBuff("Vengeance: Ignore Pain"))
                        {
                            WoW.CastSpell("Revenge");
                            return;
                        }
                        if (WoW.CanCast("Revenge") && !WoW.IsSpellOnCooldown("Revenge") && WoW.IsSpellInRange("Shield Slam") &&
                            WoW.PlayerHasBuff("Vengeance: Focused Rage") && WoW.Rage > 59)
                        {
                            WoW.CastSpell("Revenge");
                            return;
                        }
                        if (WoW.CanCast("Revenge") && !WoW.IsSpellOnCooldown("Revenge") && WoW.IsSpellInRange("Shield Slam") &&
                            !WoW.PlayerHasBuff("Ignore Pain") && WoW.Rage > 35 &&
                            WoW.HealthPercent < 100)
                        {
                            WoW.CastSpell("Revenge");
                            return;
                        }
                        if (WoW.CanCast("Revenge") && !WoW.IsSpellOnCooldown("Revenge") && WoW.IsSpellInRange("Shield Slam") &&
                            WoW.PlayerHasBuff("Ignore Pain") &&
                            WoW.PlayerBuffTimeRemaining("Ignore Pain") <= 3 && WoW.Rage > 40 && WoW.HealthPercent < 100)
                        {
                            WoW.CastSpell("Revenge");
                            return;
                        }
                        if (WoW.CanCast("Revenge") && !WoW.IsSpellOnCooldown("Revenge") && WoW.IsSpellInRange("Shield Slam") &&
                            !WoW.PlayerHasBuff("Vengeance: Focused Rage") &&
                            !WoW.PlayerHasBuff("Vengeance: Ignore Pain") && WoW.Rage > 69)
                        {
                            WoW.CastSpell("Revenge");
                            return;
                        }
                        if (WoW.CanCast("Victory Rush") && !WoW.IsSpellOnCooldown("Victory Rush") && WoW.IsSpellInRange("Shield Slam") &&
                            WoW.HealthPercent < 90 &&
                            WoW.PlayerHasBuff("VictoryRush"))
                        {
                            WoW.CastSpell("Victory Rush");
                            return;
                        }
                        if (WoW.IsSpellInRange("Devastate") && WoW.CanCast("Devastate"))
                        {
                            WoW.CastSpell("Devastate");
                            return;
                        }
                    }
                    if (WoW.CanCast("Neltharion's Fury") && WoW.TargetHasDebuff("ShockWavestun"))
                    {
                        WoW.CastSpell("Neltharion's Fury");
                        return;
                    }
                }
            }
            if (combatRoutine.Type == RotationType.AOE)
            {
                // Do AOE Stuff here
            }
        }
    }
}

/*
[AddonDetails.db]
AddonAuthor=Nilrem2004
AddonName=Frozen
WoWVersion=Legion - 70200
[SpellBook.db]
Spell,6343,Thunder Clap,D1
Spell,23922,Shield Slam,D2
Spell,6572,Revenge,D3
Spell,20243,Devastate,D4
Spell,34428,Victory Rush,D5
Spell,204488,Focused Rage,D6
Spell,203526,Neltharion's Fury,D7
Spell,46968,Shockwave,D8
Spell,871,Shield Wall,F5
Spell,12975,Last Stand,F6
Spell,6552,Pummel,F7
Spell,2565,Shield Block,F8
Spell,190456,Ignore Pain,F9
Spell,1719,Battle Cry,F10
Spell,6544,HeroicLeap,F11
Spell,23920,SpellReflect,F12
Aura,132168,ShockWavestun
Aura,202573,Vengeance: Focused Rage
Aura,202574,Vengeance: Ignore Pain
Aura,190456,Ignore Pain
Aura,132404,ShieldBlockAura
Aura,32216,VictoryRush
Aura,207844,Legendary
Aura,186305,Mount
*/