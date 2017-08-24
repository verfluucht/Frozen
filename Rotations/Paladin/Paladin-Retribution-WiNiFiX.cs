using System;
using System.Drawing;
using System.Windows.Forms;
using Frozen.Helpers;

namespace Frozen.Rotation
{
    public class Paladin_Retribution : CombatRoutine
    {
        public override string Name => "Frozen Retribution";

        public override string Class => "Paladin";

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
            Log.Write("Power: " + WoW.UnitPower);            
        }
    }
}

/*
[AddonDetails.db]
AddonAuthor=WiNiFiX
AddonName=Frozen
WoWVersion=Legion - 70200
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
