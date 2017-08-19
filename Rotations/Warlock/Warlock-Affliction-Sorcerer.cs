using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Frozen.Helpers;

namespace Frozen.Rotation
{
    public class WarlockAffliction : CombatRoutine
    {
        public override string Name => "Affliction Warlock";

        public override string Class => "Warlock";

        public override Form SettingsForm { get; set; }

        public override void Initialize()
        {
            Log.Write("Welcome to Affliction Warlock", Color.Purple);
            Log.Write("If Scroll Lock is on the Auto-AOE is enabled", Color.Blue);
        }

        public override void Stop()
        {
        }

        public override void Pulse() 
        {
            if (WoW.IsInCombat && Control.IsKeyLocked(Keys.Scroll) && !WoW.TargetIsPlayer && !WoW.IsMounted)
                SelectRotation(4, 9999, 1);

            if (combatRoutine.Type == RotationType.SingleTarget) // Do Single Target Stuff here
                if (WoW.HasTarget && WoW.TargetIsEnemy && WoW.IsInCombat && !WoW.PlayerIsCasting && !WoW.IsMounted)
                {
                    if ((!WoW.TargetHasDebuff("Agony") || WoW.TargetDebuffTimeRemaining("Agony") <= 540)
                        && (!WoW.PlayerIsChanneling || WoW.TargetDebuffTimeRemaining("Agony") <= 150)
                        && WoW.CanCast("Agony")
                        && WoW.IsSpellInRange("Agony"))
                    {
                        WoW.CastSpell("Agony");
                        return;
                    }

                    if ((WoW.CurrentSoulShards >= 3 || WoW.CurrentSoulShards >= 2 && WoW.WasLastCasted("Unstable Affliction"))
                        && !WoW.IsMoving
                        && WoW.CanCast("Unstable Affliction")
                        && WoW.IsSpellInRange("Agony"))
                    {
                        WoW.CastSpell("Unstable Affliction");
                        Thread.Sleep(200);
                        return;
                    }

                    if ((WoW.TargetHasDebuff("Unstable Affliction1") && WoW.TargetHasDebuff("Unstable Affliction2")
                         || WoW.TargetHasDebuff("Unstable Affliction1") && WoW.TargetHasDebuff("Unstable Affliction3")
                         || WoW.TargetHasDebuff("Unstable Affliction1") && WoW.TargetHasDebuff("Unstable Affliction4")
                         || WoW.TargetHasDebuff("Unstable Affliction1") && WoW.TargetHasDebuff("Unstable Affliction5")
                         || WoW.TargetHasDebuff("Unstable Affliction2") && WoW.TargetHasDebuff("Unstable Affliction3")
                         || WoW.TargetHasDebuff("Unstable Affliction2") && WoW.TargetHasDebuff("Unstable Affliction4")
                         || WoW.TargetHasDebuff("Unstable Affliction2") && WoW.TargetHasDebuff("Unstable Affliction5")
                         || WoW.TargetHasDebuff("Unstable Affliction3") && WoW.TargetHasDebuff("Unstable Affliction4")
                         || WoW.TargetHasDebuff("Unstable Affliction3") && WoW.TargetHasDebuff("Unstable Affliction5")
                         || WoW.TargetHasDebuff("Unstable Affliction4") && WoW.TargetHasDebuff("Unstable Affliction5")
                         || WoW.PlayerBuffStacks("Tormented Souls") >= 12)
                        && !WoW.PlayerIsCasting
                        && WoW.CanCast("Reap Souls")
                        && !WoW.PlayerHasBuff("Deadwind Harvester")
                        && WoW.PlayerHasBuff("Tormented Souls"))
                    {
                        WoW.CastSpell("Reap Souls");
                        return;
                    }

                    if (WoW.CanCast("Life Tap") && !WoW.PlayerIsChanneling && WoW.Talent(2) == 3 && !WoW.PlayerHasBuff("Empowered Life Tap"))
                    {
                        WoW.CastSpell("Life Tap");
                        return;
                    }

                    if ((!WoW.TargetHasDebuff("Corruption") || WoW.TargetDebuffTimeRemaining("Corruption") <= 420)
                        && (!WoW.PlayerIsChanneling || WoW.TargetDebuffTimeRemaining("Corruption") <= 150)
                        && WoW.CanCast("Corruption")
                        && WoW.IsSpellInRange("Agony"))
                    {
                        WoW.CastSpell("Corruption");
                        return;
                    }

                    if ((!WoW.TargetHasDebuff("Siphon Life") || WoW.TargetDebuffTimeRemaining("Siphon Life") <= 420)
                        && (!WoW.PlayerIsChanneling || WoW.TargetDebuffTimeRemaining("Siphon Life") <= 150)
                        && WoW.Talent(4) == 1
                        && WoW.CanCast("Siphon Life")
                        && WoW.IsSpellInRange("Agony"))
                    {
                        WoW.CastSpell("Siphon Life");
                        return;
                    }

                    if (WoW.CanCast("Felhunter") && WoW.Talent(6) == 2 && !WoW.IsSpellOnCooldown("Felhunter") && WoW.IsSpellInRange("Agony") &&
                        !WoW.PlayerIsChanneling && !WoW.PlayerIsCasting)
                    {
                        WoW.CastSpell("Felhunter");
                        return;
                    }

                    if (WoW.CanCast("Unstable Affliction") && !WoW.IsMoving && WoW.Talent(2) == 1 && !WoW.IsMoving &&
                        WoW.IsSpellInRange("Unstable Affliction") && !WoW.PlayerIsChanneling && WoW.CurrentSoulShards >= 1
                        && (!WoW.TargetHasDebuff("Unstable Affliction1") || !WoW.TargetHasDebuff("Unstable Affliction2") ||
                            !WoW.TargetHasDebuff("Unstable Affliction3") || !WoW.TargetHasDebuff("Unstable Affliction4") ||
                            !WoW.TargetHasDebuff("Unstable Affliction5")
                            || WoW.TargetDebuffTimeRemaining("Unstable Affliction1") <= 150 || WoW.TargetDebuffTimeRemaining("Unstable Affliction2") <= 150 ||
                            WoW.TargetDebuffTimeRemaining("Unstable Affliction3") <= 150
                            || WoW.TargetDebuffTimeRemaining("Unstable Affliction4") <= 150 || WoW.TargetDebuffTimeRemaining("Unstable Affliction5") <= 150))
                    {
                        WoW.CastSpell("Unstable Affliction");
                        Thread.Sleep(200);
                        return;
                    }

                    if (WoW.IsInCombat && WoW.Mana < 70 && WoW.HealthPercent > 70 && WoW.CanCast("Life Tap"))
                    {
                        WoW.CastSpell("Life Tap");
                        return;
                    }

                    if (WoW.CanCast("Haunt") && WoW.Talent(1) == 1 && !WoW.IsSpellOnCooldown("Haunt") && WoW.IsSpellInRange("Agony") &&
                        !WoW.PlayerIsChanneling && !WoW.PlayerIsCasting && !WoW.IsMoving)
                    {
                        WoW.CastSpell("Haunt");
                        return;
                    }

                    if (WoW.CanCast("Drain Soul") && WoW.IsSpellInRange("Agony") && !WoW.PlayerIsChanneling && !WoW.PlayerIsCasting && !WoW.IsMoving)
                    {
                        WoW.CastSpell("Drain Soul");
                        return;
                    }
                }
            if (combatRoutine.Type == RotationType.AOE)
                if (WoW.HasTarget && WoW.TargetIsEnemy && WoW.IsInCombat && !WoW.PlayerIsChanneling && !WoW.PlayerIsCasting &&
                    !WoW.IsMounted) // Do AOE stuff here
                {
                    if (WoW.CanCast("Agony") && WoW.IsSpellInRange("Agony") && WoW.TargetHasDebuff("Seed of Corruption") &&
                        (!WoW.TargetHasDebuff("Agony") || WoW.TargetDebuffTimeRemaining("Agony") <= 540))
                    {
                        WoW.CastSpell("Agony");
                        return;
                    }

                    if (WoW.CanCast("Corruption") && WoW.IsSpellInRange("Agony") && WoW.TargetHasDebuff("Seed of Corruption") &&
                        (!WoW.TargetHasDebuff("Corruption") || WoW.TargetDebuffTimeRemaining("Corruption") <= 420))
                    {
                        WoW.CastSpell("Corruption");
                        return;
                    }

                    if (WoW.CanCast("Seed of Corruption") && WoW.IsSpellInRange("Agony") && !WoW.TargetHasDebuff("Seed of Corruption") && !WoW.IsMoving &&
                        WoW.CurrentSoulShards >= 1)
                    {
                        WoW.CastSpell("Seed of Corruption");
                        return;
                    }
                }
            if (combatRoutine.Type == RotationType.Cleave)
            {
                // Do Single Target Cleave stuff here if applicable else ignore this one
            }
        }

        private static bool lastNamePlate = true;

        public void SelectRotation(int aoe, int cleave, int single)
        {
            var count = WoW.CountEnemyNPCsInRange;
            if (!lastNamePlate)
            {
                combatRoutine.ChangeType(RotationType.SingleTarget);
                lastNamePlate = true;
            }
            lastNamePlate = WoW.Nameplates;
            if (count >= aoe)
                combatRoutine.ChangeType(RotationType.AOE);
            if (count == cleave)
                combatRoutine.ChangeType(RotationType.Cleave);
            if (count <= single)
                combatRoutine.ChangeType(RotationType.SingleTarget);
        }
    }
}


/*
[AddonDetails.db]
AddonAuthor=Sorcerer
AddonName=Frozen
WoWVersion=Legion - 70200
[SpellBook.db]
Spell,980,Agony,NumPad1
Spell,63106,Siphon Life,NumPad2
Spell,172,Corruption,NumPad3
Spell,30108,Unstable Affliction,NumPad4
Spell,216698,Reap Souls,NumPad5
Spell,1454,Life Tap,NumPad7
Spell,48181,Haunt,NumPad8
Spell,198590,Drain Soul,Add
Spell,27243,Seed of Corruption,NumPad0
Spell,111897,Felhunter,NumPad9
Aura,980,Agony
Aura,27243,Seed of Corruption
Aura,146739,Corruption
Aura,63106,Siphon Life
Aura,233490,Unstable Affliction1
Aura,233496,Unstable Affliction2
Aura,233497,Unstable Affliction3
Aura,233498,Unstable Affliction4
Aura,233499,Unstable Affliction5
Aura,216708,Deadwind Harvester
Aura,216695,Tormented Souls
Aura,235156,Empowered Life Tap
Aura,127271,Mount
*/
