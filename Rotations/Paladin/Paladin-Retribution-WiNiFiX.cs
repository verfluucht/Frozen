using System;
using System.Drawing;
using System.Windows.Forms;
using Frozen.Helpers;

namespace Frozen.Rotation
{
    public class Paladin_Retribution : CombatRoutine
    {
        public override Form SettingsForm
        {
            get { throw new NotImplementedException(); }

            set { throw new NotImplementedException(); }
        }


        public override void Initialize()
        {
            Log.DrawHorizontalLine();
            Log.WriteFrozen("Welcome to Frozen Retributin", Color.Black);
            Log.Write("Supported Talents: 2132212", Color.Red);
        }

        public override void Stop()
        {
        }

        public override void Pulse()
        {
            if (!WoW.HasTarget || WoW.TargetIsFriend)
            {
                if (!WoW.PlayerHasBuff("Greater Blessing of Kings"))
                    if (WoW.CanCast("Greater Blessing of Kings"))
                    {
                        WoW.CastSpell("Greater Blessing of Kings");
                        return;
                    }

                if (!WoW.PlayerHasBuff("Greater Blessing of Wisdom"))
                    if (WoW.CanCast("Greater Blessing of Wisdom"))
                    {
                        WoW.CastSpell("Greater Blessing of Wisdom");
                        return;
                    }
            }

            if (!WoW.HasTarget || !WoW.TargetIsEnemy) return;

            if (WoW.CanCast("Shield of Vengeance"))
            {
                WoW.CastSpell("Shield of Vengeance");  // Off the GCD no return needed
            }

            if (WoW.CanCast("Judgment") && WoW.UnitPower >= 5)
            {
                WoW.CastSpell("Judgment");
                return;
            }

            if (WoW.UnitPower == 0 && WoW.CanCast("Wake of Ashes"))
            {
                WoW.CastSpell("Wake of Ashes");
                return;
            }

            if (WoW.CanCast("Crusade") && WoW.UnitPower >= 5 && WoW.TargetHasDebuff("Judgment"))
            {
                WoW.CastSpell("Crusade");
                return;
            }

            if (WoW.CanCast("Execution Sentence") && WoW.UnitPower >= 3 && WoW.TargetHasDebuff("Judgment") &&
                !WoW.TargetHasDebuff("Execution Sentence"))
            {
                WoW.CastSpell("Execution Sentence");
                return;
            }

            if (WoW.CanCast("Divine Storm") && WoW.UnitPower >= 3 && WoW.TargetHasDebuff("Judgment") && WoW.CountEnemyNPCsInRange > 1)
            {
                WoW.CastSpell("Divine Storm");
                return;
            }

            if (WoW.CanCast("Templars Verdict") && WoW.UnitPower >= 3 && WoW.TargetHasDebuff("Judgment") && WoW.CountEnemyNPCsInRange <= 1)
            {
                WoW.CastSpell("Templars Verdict");
                return;
            }

            if (WoW.CanCast("Blade of Justice") && WoW.UnitPower <= 3) // Higher Priority because it can generate 2 holy power in 1 go
            {
                WoW.CastSpell("Blade of Justice");
                return;
            }

            if (WoW.CanCast("Crusader Strike") && WoW.UnitPower < 5 && WoW.PlayerSpellCharges("Crusader Strike") >= 0)
            {
                WoW.CastSpell("Crusader Strike");
                return;
            }

            if (WoW.CanCast("Blade of Justice"))
            {
                WoW.CastSpell("Blade of Justice");
            }
        }
    }
}

/*
[AddonDetails.db]
AddonAuthor=WiNiFiX
AddonName=Frozen
WoWVersion=Legion - 70300
[SpellBook.db]
Spell,35395,Crusader Strike,Y
Spell,184575,Blade of Justice,T
Spell,20271,Judgment,H
Spell,85256,Templars Verdict,B
Spell,213757,Execution Sentence,D8
Spell,31884,Avenging Wrath,E
Spell,231895,Crusade,E
Spell,205273,Wake of Ashes,D7
Spell,203538,Greater Blessing of Kings,D9
Spell,203539,Greater Blessing of Wisdom,D0
Spell,53385,Divine Storm,D5
Spell,184662,Shield of Vengeance,S
Aura,197277,Judgment
Aura,213757,Execution Sentence
Aura,203538,Greater Blessing of Kings
Aura,203539,Greater Blessing of Wisdom
*/
