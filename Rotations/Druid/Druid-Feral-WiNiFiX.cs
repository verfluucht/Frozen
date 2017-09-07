using System.Windows.Forms;
using Frozen.Helpers;

namespace Frozen.Rotation
{
    public class Feral_WiNiFiX : CombatRoutine
    {
        public override void Initialize()
        {
            Log.Write("Welcome to Feral 7.3 by WiNiFiX...");
        }

        public override void Stop()
        {
            Log.Write("Hope you has a good time.");
        }

        public override void Pulse()
        {
            var SavageRoarTime = WoW.PlayerBuffTimeRemaining("SavageRoar"); // 200 = 2 seconds
            var RakeTime = WoW.TargetDebuffTimeRemaining("Rake");
            var RipTime = WoW.TargetDebuffTimeRemaining("Rip");
            var MoonfireTime = WoW.TargetDebuffTimeRemaining("Moonfire");

            WoW.CastSpell("Regrowth", WoW.PlayerHasBuff("PredatorySwiftness") && WoW.PlayerBuffStacks("Bloodtalons") != 2);

            // Cast Ferocious Bite if at 5 Combo Points and Rip / Savage Roar do not need refreshing within 10 sec.
            WoW.CastSpell("FerociousBite", WoW.CurrentComboPoints >= 5 && RipTime > 1000 && SavageRoarTime > 1000);

            // Maintain Savage Roar if taken.
            WoW.CastSpell("SavageRoar", WoW.Talent(6) == 3 && SavageRoarTime <= 200 && WoW.CurrentComboPoints >= 1);

            // Maintain Rake.
            WoW.CastSpell("Rake", RakeTime <= 200);

            // Maintain Rip (at below 25%, or with Sabertooth taken, replace with Ferocious Bite).
            WoW.CastSpell("Rip", RipTime <= 200 && WoW.Talent(6) != 1 && WoW.CurrentComboPoints >= 1);
            WoW.CastSpell("FerociousBite", RipTime <= 200 && WoW.TargetHealthPercent <= 25 && WoW.Talent(6) == 1 && WoW.CurrentComboPoints >= 1);

            // Maintain Moonfire if Lunar Inspiration is taken.
            WoW.CastSpell("Moonfire", MoonfireTime <= 200);

            // Use any Omen of Clarity procs to maintain Thrash if using Luffa Wrappings.
            // Will code this when i get the item till then stuff it.... :)

            // Cast Tiger's Fury at 20 Energy or less.
            WoW.CastSpell("TigersFury", WoW.Energy <= 20, false);

            // Cast Ashamane's Frenzy, try to sync this with Tiger's Fury uses.
            WoW.CastSpell("Ashamane", true);

            // Cast Shred to build combo points.
            WoW.CastSpell("Shred", WoW.CurrentComboPoints < 5);
        }



        public override Form SettingsForm { get; set; }
    }
}

/*
[AddonDetails.db]
AddonAuthor=WiNiFiX
AddonName=Frozen
WoWVersion=Legion - 70300
[SpellBook.db]
Spell,768,Cat Form,D1
Spell,1079,Rip,V
Spell,1822,Rake,Y
Spell,5215,Prowl,X
Spell,5217,TigersFury,S
Spell,5221,Shred,H
Spell,8936,Regrowth,D0
Spell,22568,FerociousBite,B
Spell,52610,SavageRoar,A
Spell,58984,Shadowmeld,U
Spell,106830,Thrash,D5
Spell,106785,Swipe,D6
Spell,202028,BrutalSlash,D6
Spell,155625,Moonfire,T
Spell,210722,Ashamane,D7
Spell,106951,Berserk,K
Spell,102543,Incarnation,A
Spell,202060,ElunesGuidance,E
Spell,106839,SkullBash,Minus
Aura,5215,Prowl
Aura,768,Cat Form
Aura,1079,Rip
Aura,5217,TigersFury
Aura,52610,SavageRoar
Aura,58984,Shadowmeld
Aura,69369,PredatorySwiftness
Aura,106830,Thrash
Aura,106951,Berserk
Aura,145152,Bloodtalons
Aura,135700,ClearCasting
Aura,155580,LunarinspirationTalent
Aura,155672,BloodtalonsTalent
Aura,155722,Rake
Aura,202031,Sabertooth
Aura,202032,JaggedwoundsTalent
Aura,155625,Moonfire
*/
