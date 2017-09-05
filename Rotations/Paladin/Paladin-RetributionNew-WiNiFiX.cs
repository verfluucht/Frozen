using System;
using System.Drawing;
using System.Windows.Forms;
using Frozen.Helpers;

namespace Frozen.Rotation
{
    public class Paladin_Retribution_New : CombatRoutine
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
            Log.Write("Leaving Already?");
        }

        public override void OutOfCombatPulse()
        {
            WoW.CastSpell("Greater Blessing of Kings", (!WoW.HasTarget || WoW.TargetIsFriend) && !WoW.PlayerHasBuff("Greater Blessing of Kings"));
            WoW.CastSpell("Greater Blessing of Wisdom", (!WoW.HasTarget || WoW.TargetIsFriend) && !WoW.PlayerHasBuff("Greater Blessing of Wisdom"));
        }

        public override void Pulse()
        {   
            if (!WoW.HasTarget || !WoW.TargetIsEnemy) return;

            WoW.CastSpell("Shield of Vengeance", true, false);  // Off the GCD no return needed
            WoW.CastSpell("Judgment", WoW.UnitPower >= 5);
            WoW.CastSpell("Wake of Ashes", WoW.UnitPower == 0);
            WoW.CastSpell("Crusade", WoW.UnitPower >= 5 && WoW.TargetHasDebuff("Judgment"));
            WoW.CastSpell("Execution Sentence", WoW.UnitPower >= 3 && WoW.TargetHasDebuff("Judgment") && !WoW.TargetHasDebuff("Execution Sentence"));
            WoW.CastSpell("Divine Storm", WoW.UnitPower >= 3 && WoW.TargetHasDebuff("Judgment") && WoW.CountEnemyNPCsInRange > 1);
            WoW.CastSpell("Templars Verdict", WoW.UnitPower >= 3 && WoW.TargetHasDebuff("Judgment") && WoW.CountEnemyNPCsInRange <= 1);
            WoW.CastSpell("Blade of Justice", WoW.UnitPower <= 3); // Higher Priority because it can generate 2 holy power in 1 go
            WoW.CastSpell("Crusader Strike", WoW.UnitPower < 5 && WoW.PlayerSpellCharges("Crusader Strike") >= 0);
            WoW.CastSpell("Blade of Justice", true);
        }

        public override void MountedPulse()
        {
            Log.Write("Mounted.");
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
