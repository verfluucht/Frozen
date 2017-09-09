// winifix@gmail.com
// ReSharper disable UnusedMember.Global

using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Frozen.Helpers;

namespace Frozen.Rotation
{
    public class DemonHunterVeng : CombatRoutine
    {   
        public override Form SettingsForm { get; set; }

        public override void Initialize()
        {
            Log.DrawHorizontalLine();
            Log.WriteFrozen("Welcome to Frozen Demon Hunter", Color.Black);
        }

        public override void Stop()
        {
            Log.Write("Leaving already?");
        }

        public override void Pulse()
        {
            if (!WoW.HasTarget || !WoW.TargetIsEnemy || !WoW.IsSpellInRange("Throw Glaive")) return;
            
            WoW.CastSpell("Metamorphasis", WoW.PlayerHealthPercent < 30 && !WoW.IsSpellOnCooldown("Metamorphasis") && UseCooldowns, false);
            WoW.CastSpell("Sever", WoW.PlayerHasBuff("Metamorphasis"));
            WoW.CastSpell("Soul Cleave", WoW.PlayerHasBuff("Metamorphasis") && WoW.PlayerHasBuff("Soul Fragments") && WoW.PlayerBuffStacks("Soul Fragments") >= 5 && WoW.Pain >= 50);
            WoW.CastSpell("Throw Glaive", !WoW.IsSpellInRange("Soul Carver") && !WoW.IsSpellOnCooldown("Throw Glaive"));

            if (!WoW.IsSpellInRange("Soul Carver")) return; // If we are out of melee range return
            
            if (WoW.TargetIsCastingAndSpellIsInterruptible && WoW.TargetPercentCast > Random.Next(50, 70))
            {
                if (!WoW.IsSpellOnCooldown("Sigil of Silence"))
                {
                    Log.Write("Interrupting spell");
                    WoW.CastSpell("Sigil of Silence", true);
                }

                if (!WoW.IsSpellOnCooldown("Arcane Torrent"))
                {
                    Log.Write("Interrupting spell");
                    WoW.CastSpell("Arcane Torrent", true);
                }

                if (!WoW.IsSpellOnCooldown("Consume Magic"))
                {
                    Log.Write("Interrupting spell");
                    WoW.CastSpell("Consume Magic", true);
                }
            }

            WoW.CastSpell("Fiery Brand", !WoW.TargetHasDebuff("Fiery Demise") && !WoW.IsSpellOnCooldown("Fiery Brand"));
            WoW.CastSpell("Demon Spikes", !WoW.PlayerHasBuff("Demon Spikes") && WoW.Pain > 20 && !WoW.PlayerHasBuff("Magnum Opus"));
            WoW.CastSpell("Soul Carver", true);
            WoW.CastSpell("Fel Devastation", WoW.Pain >= 30);
            WoW.CastSpell("Soul Cleave", WoW.Pain >= 50);
            WoW.CastSpell("Immolation Aura", true);
            WoW.CastSpell("Sigil of Flame", !WoW.TargetHasDebuff("Sigil of Flame"));
            WoW.CastSpell("Shear", true); // Pain Generator
        }

        public override void OutOfCombatPulse()
        {
         
        }

        public override void MountedPulse()
        {
         
        }
    }
}

/*
[AddonDetails.db]
AddonAuthor=WiNiFiX
AddonName=Frozen
WoWVersion=Legion - 70300
[SpellBook.db]
Spell,203782,Shear,T
Spell,235964,Sever,T
Spell,228477,Soul Cleave,Y
Spell,207407,Soul Carver,H
Spell,212084,Fel Devastation,W
Spell,178740,Immolation Aura,D5
Spell,204513,Sigil of Flame,D6
Spell,204157,Throw Glaive,D7
Spell,207682,Sigil of Silence,D8
Spell,202719,Arcane Torrent,D0
Spell,187827,Metamorphasis,D9
Spell,204021,Fiery Brand,F
Spell,203720,Demon Spikes,S
Spell,183752,Consume Magic,B
Aura,203819,Demon Spikes
Aura,212818,Fiery Demise
Aura,200175,Mount
Aura,207472,Magnum Opus
Aura,187827,Metamorphasis
Aura,204598,Sigil of Flame
Aura,203981,Soul Fragments
Item,80610,Mana
*/