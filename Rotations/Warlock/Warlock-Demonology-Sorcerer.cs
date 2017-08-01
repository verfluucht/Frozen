using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Frozen.Helpers;

namespace Frozen.Rotation
{
    public class WarlockDemonology : CombatRoutine
    {
        public override string Name => "Demonology Warlock";

        public override string Class => "Warlock";

        public override Form SettingsForm { get; set; }

        public override void Initialize()
        {
            Log.Write("Welcome to the Demonology Warlock rotation", Color.Purple);
            Log.Write("If Scroll Lock is on the Auto-AOE is enabled", Color.Blue);
        }

        public override void Stop()
        {
        }

        public override void Pulse() // Updated for Legion (tested and working for single target)
        {
            if (WoW.IsInCombat && Control.IsKeyLocked(Keys.Scroll) && !WoW.TargetIsPlayer && !WoW.IsMounted)
                SelectRotation(4, 9999, 1);

            //Dark Pact
            if (WoW.CanCast("Dark Pact")
                && WoW.Talent(5) == 3
                && WoW.HealthPercent <= 30
                && !WoW.IsMounted)
            {
                WoW.CastSpell("Dark Pact");
                return;
            }

            //Shadowfury
            if (DetectKeyPress.GetKeyState(DetectKeyPress.VK_LMENU) < 0
                && WoW.Talent(3) == 3
                && !WoW.IsMoving
                && WoW.CanCast("Shadowfury"))
            {
                WoW.CastSpell("Shadowfury");
                return;
            }

            if (UseCooldowns)
                if (WoW.HasTarget && WoW.TargetIsEnemy && !WoW.PlayerIsChanneling && WoW.IsInCombat && !WoW.PlayerIsCasting && !WoW.IsMounted)
                {
                    //Doomguard
                    if (WoW.CanCast("Doomguard")
                        && (WoW.Talent(6) == 0 || WoW.Talent(6) == 2 || WoW.Talent(6) == 3)
                        && WoW.CurrentSoulShards >= 1
                        && WoW.IsSpellInRange("Doom"))
                    {
                        WoW.CastSpell("Doomguard");
                        return;
                    }

                    //Grimoire of Service
                    if (WoW.CanCast("Grimoire: Felguard")
                        && WoW.Talent(6) == 2
                        && WoW.CurrentSoulShards >= 1
                        && WoW.IsSpellInRange("Doom"))
                    {
                        WoW.CastSpell("Grimoire: Felguard");
                        return;
                    }

                    //Soul Harvest
                    if (WoW.CanCast("Soul Harvest")
                        && WoW.Talent(4) == 3
                        && !WoW.IsMoving
                        && WoW.IsSpellInRange("Doom")
                        && (WoW.PlayerHasBuff("Bloodlust") || WoW.PlayerHasBuff("Time Warp") || WoW.PlayerHasBuff("Netherwinds") ||
                            WoW.PlayerHasBuff("Drums of War") || WoW.PlayerHasBuff("Heroism")))
                    {
                        WoW.CastSpell("Soul Harvest");
                        return;
                    }
                }

            if (combatRoutine.Type == RotationType.SingleTarget) // Do Single Target Stuff here
                if (WoW.HasTarget && WoW.TargetIsEnemy && !WoW.PlayerIsChanneling && WoW.IsInCombat && !WoW.PlayerIsCasting && !WoW.IsMounted)
                {
                    if ((!WoW.TargetHasDebuff("Doom") || WoW.TargetDebuffTimeRemaining("Doom") <= 150)
                        && WoW.CanCast("Doom")
                        && WoW.IsSpellInRange("Doom"))
                    {
                        WoW.CastSpell("Doom");
                        return;
                    }

                    if (WoW.CanCast("Darkglare")
                        && WoW.Talent(7) == 1
                        && WoW.CurrentSoulShards >= 1
                        && WoW.IsSpellInRange("Doom"))
                    {
                        WoW.CastSpell("Darkglare");
                        return;
                    }

                    if (WoW.CanCast("Call Dreadstalkers")
                        && (WoW.CurrentSoulShards >= 2 || WoW.TargetHasDebuff("Demonic Calling"))
                        && WoW.IsSpellInRange("Doom")
                        && !WoW.IsMoving)
                    {
                        WoW.CastSpell("Call Dreadstalkers");
                        return;
                    }

                    if (WoW.CanCast("Hand of Guldan")
                        && WoW.CurrentSoulShards >= 4
                        && WoW.IsSpellInRange("Doom")
                        && !WoW.IsMoving)
                    {
                        WoW.CastSpell("Hand of Guldan");
                        return;
                    }

                    if (WoW.CanCast("Demonic Empowerment")
                        && !WoW.IsMoving
                        && !WoW.WasLastCasted("Demonic Empowerment")
                        && (!WoW.PetHasBuff("Demonic Empowerment") || WoW.PetBuffTimeRemaining("Demonic Empowerment") <= 1.5
                            || WoW.WasLastCasted("Call Dreadstalkers") || WoW.WasLastCasted("Grimoire: Felguard") || WoW.WasLastCasted("Doomguard") ||
                            WoW.WasLastCasted("Hand of Guldan")))
                    {
                        WoW.CastSpell("Demonic Empowerment");
                        Thread.Sleep(1000);
                        return;
                    }

                    if (WoW.CanCast("Talkiels Consumption")
                        && WoW.PetHasBuff("Demonic Empowerment")
                        && WoW.PetBuffTimeRemaining("Demonic Empowerment") >= 2
                        && WoW.DreadstalkersCount >= 1
                        && WoW.IsSpellInRange("Doom")
                        && !WoW.IsMoving)
                    {
                        WoW.CastSpell("Talkiels Consumption");
                        return;
                    }

                    if (WoW.CanCast("Felstorm")
                        && WoW.PetHasBuff("Demonic Empowerment")
                        && WoW.PetBuffTimeRemaining("Demonic Empowerment") >= 6
                        && WoW.IsSpellInRange("Doom"))
                    {
                        WoW.CastSpell("Felstorm");
                        return;
                    }

                    if (WoW.CanCast("Shadowflame")
                        && WoW.Talent(1) == 2
                        && !WoW.TargetHasDebuff("Shadowflame")
                        && WoW.CanCast("Shadowflame")
                        && WoW.IsSpellInRange("Doom"))
                    {
                        WoW.CastSpell("Shadowflame");
                        return;
                    }

                    if (WoW.CanCast("Life Tap")
                        && WoW.Mana < 60
                        && WoW.HealthPercent > 50)
                    {
                        WoW.CastSpell("Life Tap");
                        return;
                    }

                    if (WoW.CanCast("Demonwrath")
                        && WoW.Mana > 60
                        && WoW.IsMoving)
                    {
                        WoW.CastSpell("Demonwrath");
                        return;
                    }

                    if ((WoW.CanCast("Shadow Bolt") || WoW.CanCast("Demonbolt"))
                        && WoW.IsSpellInRange("Doom")
                        && !WoW.IsMoving)
                    {
                        WoW.CastSpell("Shadow Bolt");
                        WoW.CastSpell("Demonbolt");
                        return;
                    }
                }
            if (combatRoutine.Type == RotationType.AOE)
                if (WoW.HasTarget && WoW.TargetIsEnemy && !WoW.PlayerIsChanneling && WoW.IsInCombat && !WoW.PlayerIsCasting && !WoW.IsMounted)
                {
                    if (WoW.CanCast("Hand of Guldan")
                        && WoW.CurrentSoulShards >= 4
                        && WoW.IsSpellInRange("Doom")
                        && !WoW.IsMoving)
                    {
                        WoW.CastSpell("Hand of Guldan");
                        return;
                    }

                    if (WoW.CanCast("Implosion")
                        && WoW.Talent(2) == 3
                        && WoW.WildImpsCount >= 1
                        && WoW.IsSpellInRange("Doom"))
                    {
                        WoW.CastSpell("Implosion");
                        return;
                    }

                    if (WoW.CanCast("Darkglare")
                        && WoW.Talent(7) == 1
                        && WoW.CurrentSoulShards >= 1
                        && WoW.IsSpellInRange("Doom"))
                    {
                        WoW.CastSpell("Darkglare");
                        return;
                    }

                    if (WoW.CanCast("Demonic Empowerment")
                        && WoW.CanCast("Felstorm")
                        && !WoW.IsMoving
                        && !WoW.WasLastCasted("Demonic Empowerment")
                        && (!WoW.PetHasBuff("Demonic Empowerment") || WoW.PetBuffTimeRemaining("Demonic Empowerment") <= 6))
                    {
                        WoW.CastSpell("Demonic Empowerment");
                        Thread.Sleep(2000);
                        return;
                    }

                    if (WoW.CanCast("Felstorm")
                        && WoW.PetHasBuff("Demonic Empowerment")
                        && WoW.PetBuffTimeRemaining("Demonic Empowerment") >= 6
                        && WoW.IsSpellInRange("Doom"))
                    {
                        WoW.CastSpell("Felstorm");
                        return;
                    }

                    if (WoW.CanCast("Life Tap")
                        && WoW.Mana < 60
                        && WoW.HealthPercent > 50)
                    {
                        WoW.CastSpell("Life Tap");
                        return;
                    }

                    if (WoW.CanCast("Demonwrath")
                        && WoW.Mana > 60)
                    {
                        WoW.CastSpell("Demonwrath");
                        return;
                    }
                }

            if (combatRoutine.Type == RotationType.SingleTargetCleave)
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
                combatRoutine.ChangeType(RotationType.SingleTargetCleave);
            if (count <= single)
                combatRoutine.ChangeType(RotationType.SingleTarget);
        }
    }
}


/*
[AddonDetails.db]
AddonAuthor=Sorcerer
AddonName=Quartz
WoWVersion=Legion - 72000
[SpellBook.db]
Spell,686,Shadow Bolt,NumPad1
Spell,157695,Demonbolt,NumPad1
Spell,104316,Call Dreadstalkers,NumPad2
Spell,105174,Hand of Guldan,NumPad3
Spell,193396,Demonic Empowerment,NumPad4
Spell,603,Doom,NumPad5
Spell,193440,Demonwrath,NumPad6
Spell,1454,Life Tap,NumPad7
Spell,205180,Darkglare,NumPad8
Spell,111897,Grimoire: Felguard,NumPad9
Spell,211714,Talkiels Consumption,Add
Spell,205181,Shadowflame,NumPad0
Spell,18540,Doomguard,Decimal
Spell,119914,Felstorm,D4
Spell,196098,Soul Harvest,D0
Spell,196277,Implosion,D7
Spell,30283,Shadowfury,D3
Spell,108416,Dark Pact,Multiply
Aura,2825,Bloodlust
Aura,32182,Heroism
Aura,80353,Time Warp
Aura,160452,Netherwinds
Aura,230935,Drums of War
Aura,603,Doom
Aura,193396,Demonic Empowerment
Aura,205146,Demonic Calling
Aura,205181,Shadowflame
Aura,127271,Mount
*/