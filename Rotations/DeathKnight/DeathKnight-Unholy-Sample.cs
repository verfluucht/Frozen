// winifix@gmail.com
// ReSharper disable UnusedMember.Global
// ReSharper disable ConvertPropertyToExpressionBody

using System.Drawing;
using System.Windows.Forms;
using Frozen.Helpers;

namespace Frozen.Rotation
{
    public class DKUnholy : CombatRoutine
    {
        public override Form SettingsForm { get; set; }

        public override void Initialize()
        {
            Log.Write("Welcome to Frozen Unholy", Color.Green);
        }

        public override void Stop()
        {
        }

        public override void Pulse() // Updated for Legion (tested and working for single target)
        {
            if (combatRoutine.Type == RotationType.SingleTarget) // Do Single Target Stuff here
                if (WoW.HasTarget && WoW.TargetIsEnemy)
                {
                    if (!WoW.TargetHasDebuff("Virulent Plague") && WoW.CurrentRunes >= 1 && WoW.CanCast("Outbreak", true, false, true))
                    {
                        WoW.CastSpell("Outbreak");
                        return;
                    }
                    if (WoW.CanCast("Dark Transformation", true, true, true))
                    {
                        WoW.CastSpell("Dark Transformation");
                        return;
                    }
                    if (WoW.CanCast("Death Coil") && WoW.RunicPower >= 80 || 
                        WoW.PlayerHasBuff("Sudden Doom") && WoW.IsSpellOnCooldown("Dark Arbiter"))
                    {
                        WoW.CastSpell("Death Coil");
                        return;
                    }
                    if (WoW.CanCast("Festering Strike", true, true, true) &&
                        WoW.TargetDebuffStacks("Festering Wound") <= 4)
                    {
                        WoW.CastSpell("Festering Strike");
                        return;
                    }
                    if (WoW.CanCast("Clawing Shadows") && WoW.CurrentRunes >= 3)
                        WoW.CastSpell("Clawing Shadows");
                }
            if (combatRoutine.Type == RotationType.AOE)
            {
                // Do AOE stuff here
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
AddonAuthor=WiNiFiX
AddonName=PixelMagic
WoWVersion=Legion - 70300
[SpellBook.db]
Spell,85948,Festering Strike,D1
Spell,77575,Outbreak,D2
Spell,207311,Clawing Shadows,D3
Spell,47541,Death Coil,D4
Spell,194918,Blighted Rune Weapon,D5
Spell,63560,Dark Transformation,D6
Spell,207349,Dark Arbiter,Q
Aura,81340,Sudden Doom
Aura,194310,Festering Wound
Aura,191587,Virulent Plague
*/