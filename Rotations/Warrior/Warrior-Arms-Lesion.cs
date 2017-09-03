using System.Drawing;
using System.Windows.Forms;
using Frozen.Helpers;

namespace Frozen.Rotation
{
    public class ArmsWarriorLe : CombatRoutine
    {
        public override Form SettingsForm { get; set; }


        public override void Initialize()
        {
            Log.Write("Welcome to Arms Warrior", Color.Green);
            Log.Write("Suggested build: 1332311", Color.Green);
            Log.Write("Written based on this guide : http://tinyurl.com/jjowqs7 ", Color.Green);
        }

        public override void Stop()
        {
        }

        public override void Pulse()
        {
            if (combatRoutine.Type == RotationType.SingleTarget) // Do Single Target Stuff here
            {
                if (WoW.CanCast("Battle Cry") && !WoW.IsSpellOnCooldown("Battle Cry"))
                {
                    WoW.CastSpell("Battle Cry");
                    return;
                }

                //AOE on Press
                if (DetectKeyPress.GetKeyState(DetectKeyPress.VK_KEY_Z) < 0)
                    if (WoW.HasTarget && WoW.IsInCombat && WoW.TargetIsEnemy)
                    {
                        if (WoW.CanCast("Bladestorm") && WoW.IsSpellOnCooldown("Warbreaker"))
                        {
                            WoW.CastSpell("Bladestorm");
                            return;
                        }
                        if (!WoW.WasLastCasted("Warbreaker") && !WoW.PlayerHasBuff("Bladestorm"))
                        {
                            if (WoW.CanCast("Cleave") && !WoW.PlayerHasBuff("Cleave") && !WoW.IsSpellOnCooldown("Cleave"))
                            {
                                WoW.CastSpell("Cleave");
                                return;
                            }
                            if (WoW.CanCast("Whirlwind") && WoW.Rage > 20 && WoW.IsSpellOnCooldown("Cleave"))
                            {
                                WoW.CastSpell("Whirlwind");
                                return;
                            }
                            if (WoW.CanCast("Colossus Smash") && WoW.Rage > 28 && !WoW.TargetHasDebuff("Colossus Smash") && WoW.IsSpellOnCooldown("Cleave"))
                            {
                                WoW.CastSpell("Colossus Smash");
                                return;
                            }
                            if (WoW.CanCast("Mortal Strike") && WoW.Rage > 28 && WoW.TargetHasDebuff("Colossus Smash") && WoW.IsSpellOnCooldown("Cleave"))
                            {
                                WoW.CastSpell("Mortal Strike");
                                return;
                            }
                        }
                    }

                //Normal ST rotation
                if (!WoW.PlayerHasBuff("Battle Cry") && WoW.HasTarget && WoW.IsInCombat && WoW.TargetIsEnemy && WoW.IsSpellInRange("Mortal Strike"))
                {
                    if (WoW.TargetHealthPercent >= 20)
                        //When targets are above 20%. Not in Execute phase.
                    {
                        //opening 
                        if (WoW.CanCast("Focused Rage") && WoW.WasLastCasted("Charge") && WoW.PlayerBuffStacks("Focused Rage") == 1)
                            WoW.CastSpell("Focused Rage");
                        if (WoW.CanCast("Focused Rage") && WoW.WasLastCasted("Colossus Smash") && WoW.PlayerBuffStacks("Focused Rage") == 2)
                            WoW.CastSpell("Focused Rage");
                        //FR Control
                        if (WoW.CanCast("Focused Rage") && WoW.Rage > 50 && WoW.SpellCooldownTimeRemaining("Battle Cry") > 6 &&
                            WoW.IsSpellOnCooldown("Mortal Strike") &&
                            WoW.IsSpellOnCooldown("Colossus Smash"))
                            WoW.CastSpell("Focused Rage");
                        if (WoW.CanCast("Focused Rage") && WoW.SpellCooldownTimeRemaining("Battle Cry") < 6)
                            WoW.CastSpell("Focused Rage");


                        //CS Control
                        if (WoW.CanCast("Colossus Smash") && !WoW.PlayerHasBuff("Shattered Defenses") && WoW.SpellCooldownTimeRemaining("Battle Cry") >= 4 &&
                            !WoW.IsSpellOnCooldown("Colossus Smash"))
                        {
                            WoW.CastSpell("Colossus Smash");
                            return;
                        }
                        if (WoW.CanCast("Colossus Smash") && WoW.SpellCooldownTimeRemaining("Battle Cry") == 1)
                        {
                            WoW.CastSpell("Colossus Smash");
                            return;
                        }
                        //MS 
                        if (WoW.CanCast("Mortal Strike") && WoW.SpellCooldownTimeRemaining("Battle Cry") >= 6 && !WoW.IsSpellOnCooldown("Mortal Strike"))
                        {
                            WoW.CastSpell("Mortal Strike");
                            return;
                        }
                        //Slam
                        if (WoW.CanCast("Slam") && WoW.Rage >= 32 && WoW.IsSpellOnCooldown("Colossus Smash") && WoW.IsSpellOnCooldown("Mortal Strike") &&
                            WoW.WasLastCasted("Focused Rage"))
                        {
                            WoW.CastSpell("Slam");
                            return;
                        }
                    }
                    if (WoW.TargetHealthPercent <= 20)

                    {
                        //Non BC ST
                        if (WoW.CanCast("Colossus Smash") && WoW.SpellCooldownTimeRemaining("Battle Cry") >= 4 && !WoW.IsSpellOnCooldown("Colossus Smash") &&
                            !WoW.PlayerHasBuff("Shattered Defenses"))
                        {
                            WoW.CastSpell("Colossus Smash");
                            return;
                        }
                        if (WoW.CanCast("Colossus Smash") && WoW.SpellCooldownTimeRemaining("Battle Cry") == 1 && !WoW.IsSpellOnCooldown("Colossus Smash"))
                        {
                            WoW.CastSpell("Colossus Smash");
                            return;
                        }
                        if (WoW.CanCast("Focused Rage") && WoW.Rage >= 85 && WoW.PlayerBuffStacks("Focused Rage") <= 3)
                            WoW.CastSpell("Focused Rage");

                        if (WoW.CanCast("Execute") && WoW.Rage >= 18 && WoW.PlayerHasBuff("Shattered Defenses"))
                        {
                            WoW.CastSpell("Execute");
                            return;
                        }
                        if (WoW.CanCast("Execute") && !WoW.PlayerHasBuff("Shattered Defenses"))
                        {
                            WoW.CastSpell("Execute");
                            return;
                        }
                    }
                }

                if (WoW.PlayerHasBuff("Battle Cry") && WoW.HasTarget && WoW.IsInCombat && WoW.TargetIsEnemy && WoW.IsSpellInRange("Mortal Strike"))


                {
                    if (WoW.CanCast("Avatar") && WoW.PlayerHasBuff("Battle Cry") && WoW.IsSpellOnCooldown("Battle Cry"))
                    {
                        WoW.CastSpell("Avatar");
                        return;
                    }
                    if (WoW.TargetHealthPercent >= 20)
                    {
                        //Maintain FR Stacks
                        if (WoW.CanCast("Focused Rage") && WoW.PlayerBuffStacks("Focused Rage") <= 3)
                            WoW.CastSpell("Focused Rage");
                        //FR after CS
                        if (WoW.CanCast("Focused Rage") && WoW.WasLastCasted("Colossus Smash"))
                            WoW.CastSpell("Focused Rage");
                        //CS on cooldown but not overlapping SD
                        if (WoW.CanCast("Colossus Smash") && !WoW.IsSpellOnCooldown("Colossus Smash") && !WoW.PlayerHasBuff("Shattered Defenses"))
                        {
                            WoW.CastSpell("Colossus Smash");
                            return;
                        }
                        //MS with SD
                        if (WoW.CanCast("Mortal Strike") && WoW.PlayerHasBuff("Shattered Defenses") && !WoW.IsSpellOnCooldown("Mortal Strike") &&
                            WoW.PlayerBuffStacks("Focused Rage") == 3)
                        {
                            WoW.CastSpell("Mortal Strike");
                            return;
                        }
                        //MS on cooldown
                        if (WoW.CanCast("Mortal Strike") && !WoW.IsSpellOnCooldown("Mortal Strike") && WoW.IsSpellOnCooldown("Colossus Smash"))
                            WoW.CastSpell("Mortal Strike");
                        //if all else fails, slam.
                        if (WoW.CanCast("Slam") && WoW.IsSpellOnCooldown("Colossus Smash") && WoW.IsSpellOnCooldown("Mortal Strike") &&
                            WoW.PlayerBuffStacks("Focused Rage") >= 3)
                        {
                            WoW.CastSpell("Slam");
                            return;
                        }
                    }
                    if (WoW.TargetHealthPercent <= 20)
                    {
                        if (WoW.CanCast("Focused Rage") && WoW.PlayerBuffStacks("Focused Rage") < 3)
                            WoW.CastSpell("Focused Rage");
                        if (WoW.CanCast("Focused Rage") && !WoW.TargetHasDebuff("Colossus Smash"))
                            WoW.CastSpell("Focused Rage");
                        if (WoW.CanCast("Mortal Strike") && !WoW.IsSpellOnCooldown("Mortal Strike") && WoW.PlayerBuffStacks("Focused Rage") == 3)
                        {
                            WoW.CastSpell("Mortal Strike");
                            return;
                        }
                        if (WoW.CanCast("Colossus Smash") && !WoW.TargetHasDebuff("Colossus Smash") && !WoW.IsSpellOnCooldown("Colossus Smash"))
                        {
                            WoW.CastSpell("Colossus Smash");
                            return;
                        }
                        if (WoW.CanCast("Execute") && WoW.WasLastCasted("Mortal Strike"))
                        {
                            WoW.CastSpell("Execute");
                            return;
                        }
                    }
                }
            }
            if (combatRoutine.Type == RotationType.AOE)
            {
                // AOE stuff here
            }
            if (combatRoutine.Type == RotationType.Cleave)
            {
                // Do Single Target Cleave stuff here if applicable else ignore this one
            }
        }
    }
}

/*
[AddonDetails.db]
AddonAuthor=Lesion
AddonName=Frozen
WoWVersion=Legion - 70300
[SpellBook.db]
Spell,12294,Mortal Strike,D1
Spell,167105,Colossus Smash,D2
Spell,1464,Slam,D3
Spell,207982,Focused Rage,D4
Spell,1719,Battle Cry,F2
Spell,107574,Avatar,F4
Spell,163201,Execute,D5
Spell,100,Charge,E
Spell,209577,Warbreaker,F1
Spell,845,Cleave,F3
Spell,1680,Whirlwind,D8
Spell,227847,Bladestorm,D9
Aura,227847,Bladestorm
Aura,208086,Colossus Smash
Aura,209706,Shattered Defenses
Aura,1719,Battle Cry
Aura,207982,Focused Rage
Aura,845,Cleave
*/