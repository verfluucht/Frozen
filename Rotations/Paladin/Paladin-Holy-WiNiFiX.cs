// winifix@gmail.com
// ReSharper disable UnusedMember.Global


using System.Drawing;
using System.Windows.Forms;
using Frozen.GUI;
using Frozen.Helpers;

namespace Frozen.Rotation
{
    public class Paladin_Holy : CombatRoutine
    {
        public override Form SettingsForm
        {
            get { return null; }

            set { }
        }


        const string supportedTalents = "2311321";

        public override void Initialize()
        {
            Log.Clear();
            Log.WriteFrozen("Welcome to Frozen Holy", Color.Black);
            Log.Write("Supported Talents: " + supportedTalents);
            Log.Write("Ensure you have setup healing keybinds before.", Color.Red);
            
            Log.DrawHorizontalLine();
            Log.Write("If you do proving grounds the Tank Id is 5 when it asks you.", Color.Red);
            Log.DrawHorizontalLine();
        }

        public override void Stop()
        {
        }

        public override void Pulse()
        {
            if (!WoW.InGame) return;

            if (WoW.TankId == 0)
            {
                string currentTalents = WoW.Talent(1) + "" + WoW.Talent(2) + "" + WoW.Talent(3) + "" + WoW.Talent(4) + "" + WoW.Talent(5) + "" + WoW.Talent(6) + "" + WoW.Talent(7);
                if (supportedTalents != currentTalents)
                {
                    MessageBox.Show("You are not using the supported talents " + supportedTalents + ", your currnet talents are "+ currentTalents + ".", "Frozen", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Log.Write("Talents are correct", Color.Green);
                }

                if (!WoW.PlayerHasBuff("Beacon of Faith") && !WoW.PlayerHasBuff("Beacon of Light") &&
                    WoW.CanCast("Beacon of Faith"))
                    WoW.CastSpellOnMe("Beacon of Faith");

                var f = new frmEnterTankId {TopMost = true};
                f.ShowDialog();
            }
            
            if (WoW.PlayerHealthPercent == 0 || WoW.IsMounted) return;
            if (WoW.PlayerIsCasting) return;

            var lowest = WoW.PartyLowestHealthPercent;

            int currentTargetId = WoW.PartyMemberIdWithLowestHealthPercent;
            
            if (WoW.PartyMemberIsNeedingADispel != 0)
            {
                currentTargetId = WoW.PartyMemberIsNeedingADispel;
            }

            if (currentTargetId == 0) return;
            if (lowest == 100) return;
            
            var averageHp = WoW.PartyAverageHealthPercent;

            if (WoW.PlayerHealthPercent < 10 && WoW.IsInCombat && WoW.CanCast("Divine Shield") &&
                !WoW.PlayerHasDebuff("Forbearance"))
                WoW.CastSpell("Divine Shield");

            if (averageHp < 80 && WoW.CanCast("Aura Mastery"))
                WoW.CastSpell("Aura Mastery");

            if (averageHp < 60 && WoW.CanCast("Avenging Wrath") && !WoW.PlayerHasBuff("Aura Mastery"))
                WoW.CastSpell("Avenging Wrath");

            WoW.TargetMember(currentTargetId); // Target the lowest health party member

            if (WoW.PartyMemberIsNeedingADispel != 0 && WoW.CanCast("Cleanse"))
            {
                WoW.CastSpell("Cleanse");
                return;
            }

            // Beacon of Light or Beacon of Virtue(if selected) maintain on your primary target at all times.
            if (WoW.Talent(7) == 3)
                if (WoW.TankHealth > 50 &&
                    averageHp > 90 &&
                    WoW.CanCast("Beacon of Virtue") &&
                    WoW.TankId == currentTargetId &&
                    !WoW.TargetHasBuff("Beacon of Virtue"))
                    WoW.CastSpell("Beacon of Virtue");
            if (WoW.Talent(7) == 1)
                if (WoW.CanCast("Beacon of Light") &&
                    WoW.TankId == currentTargetId &&
                    !WoW.TargetHasBuff("Beacon of Light"))
                    WoW.CastSpell("Beacon of Light");

            if (WoW.CanCast("Lay On Hands") &&
                lowest <= 20 && // Use LOH if tanks health < 20 %
                !WoW.TargetHasDebuff("Forbearance") && // Make sure we dont LOH a tank with forbearance debuff
                WoW.TankId == currentTargetId) // Make sure we only use LOH on tank
            {
                WoW.CastSpell("Lay On Hands");
                return;
            }

            if (WoW.CanCast("Blessing of Sacrifice") &&
                lowest <= 30 && 
                WoW.TankId == currentTargetId && 
                !WoW.TargetHasBuff("Shield Wall Oto")) 
            {
                WoW.CastSpell("Blessing of Sacrifice");
                return;
            }

            // Holy Shock use on cooldown to generate Infusion of Light procs.
            // Consume Infusion of Light procs using the appropriate heal before your next Holy Shock.
            if (WoW.CanCast("Holy Shock") && !WoW.PlayerHasBuff("Infusion of Light"))
            {
                WoW.CastSpell("Holy Shock");
                return;
            }

            if (WoW.Talent(5) == 3)
                if (WoW.CanCast("Holy Prism") &&
                    WoW.TankId == currentTargetId &&
                    WoW.IsInCombat)
                {
                    WoW.CastSpell("Holy Prism");
                    return;
                }

            //                  Bestow Faith on cooldown (if selected).
            //                  Judgment to maintain the buff from Judgment of Light (if selected).

            // Light of the Martyr a potent emergency heal as long as you have health to spare.
            if (WoW.CanCast("Light of the Martyr") && lowest <= 40 && WoW.PlayerHealthPercent >= 70)
            {
                WoW.CastSpell("Light of the Martyr");
                return;
            }

            // Flash of Light use as an emergency heal to save players facing death. 
            if (WoW.CanCast("Flash of Light") && lowest <= 50 && !WoW.IsMoving)
            {
                WoW.CastSpell("Flash of Light");
                return;
            }

            // Holy Light use to heal moderate to high damage. 
            if (WoW.CanCast("Holy Light") && lowest <= 90 && !WoW.IsMoving)
            {
                WoW.CastSpell("Holy Light");
                return;
            }

            if (WoW.CanCast("Tyr's Deliverance") && !WoW.IsMoving)
            {
                WoW.TargetNearestEnemy();
                if (WoW.HasTarget && WoW.TargetIsEnemy)
                    WoW.CastSpell("Tyr's Deliverance");
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
Spell,34767,Mount,D4
Spell,20473,Holy Shock,H
Spell,82326,Holy Light,Y
Spell,633,Lay On Hands,OemMinus
Spell,183998,Light of the Martyr,B
Spell,19750,Flash of Light,T
Spell,31842,Avenging Wrath,A
Spell,31821,Aura Mastery,D9
Spell,200652,Tyr's Deliverance,D7
Spell,200025,Beacon of Virtue,D6
Spell,53563,Beacon of Light,D6
Spell,156910,Beacon of Faith,D3
Spell,114165,Holy Prism,D8
Spell,642,Divine Shield,D0
Spell,4987,Cleanse,Z
Spell,6940,Blessing of Sacrifice,W
Aura,188370,Consecration
Aura,211210,Aura Mastery
Aura,54149,Infusion of Light
Aura,25771,Forbearance
Aura,53563,Beacon of Light
Aura,156910,Beacon of Faith
Aura,200025,Beacon of Virtue
Aura,145263,Chomp
Aura,145057,Shield Wall Oto
Dispel,145206,Aqua Bomb,1
*/