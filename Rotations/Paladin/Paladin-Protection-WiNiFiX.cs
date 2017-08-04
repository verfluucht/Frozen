// winifix@gmail.com
// ReSharper disable UnusedMember.Global


using System.Drawing;
using System.Windows.Forms;
using Frozen.Helpers;

namespace Frozen.Rotation
{
    public class Paladin_Protection : CombatRoutine
    {
        public override string Name => "Frozen Protection";

        public override string Class => "Paladin";

        public override Form SettingsForm
        {
            get { return null; }
            set { }
        }

        public override void Initialize()
        {
            Log.DrawHorizontalLine();
            Log.WriteFrozen("Welcome to Frozen Protection", Color.Black);
            Log.Write("Supported Talents: 2212321");
            Log.Write("Current Talents  : " + WoW.Talent(1) + WoW.Talent(2) + WoW.Talent(3) + WoW.Talent(4) +
                      WoW.Talent(5) + WoW.Talent(6) + WoW.Talent(7));
        }

        public override void Stop()
        {
        }

        public override void Pulse()
        {
            if (WoW.HealthPercent == 0 || WoW.IsMounted) return;
            if (!WoW.IsInCombat && WoW.CanCast("Mount") && WoW.IsOutdoors && !WoW.IsMoving && !WoW.PlayerIsChanneling &&
                ConfigFile.ReadValue<bool>("Protection-Paladin-WiNiFiX", "AutoMount"))
                WoW.CastSpell("Mount");

            if (WoW.TargetHealthPercent == 0) return;

            if (!WoW.TargetIsEnemy &&
                WoW.HealthPercent < 100 &&
                WoW.CanCast("FlashHeal") &&
                WoW.Mana > 25 &&
                !WoW.IsMoving &&
                !WoW.PlayerIsChanneling)
            {
                WoW.CastSpell("FlashHeal");
                return;
            }

            if (!WoW.TargetIsEnemy) return;

            if (WoW.TargetIsCastingAndSpellIsInterruptible &&
                WoW.TargetPercentCast > 60 &&
                WoW.CanCast("Rebuke"))
            {
                WoW.CastSpell("Rebuke");
                return;
            }

            if (WoW.CanCast("ArdentDefender") && WoW.HealthPercent < 15)
            {
                Log.Write("Health < 15% using CD: [Ardent Defender]", Color.Red);
                WoW.CastSpell("ArdentDefender");
                return;
            }

            if (WoW.HealthPercent < 20 && !WoW.PlayerHasBuff("ArdentDefender"))
            {
                if (WoW.CanCast("LayOnHands") &&
                    !WoW.PlayerHasDebuff("Forbearance"))
                {
                    Log.Write("Health < 20% using CD: [Lay On Hands]", Color.Red);
                    WoW.CastSpell("LayOnHands");
                    return;
                }

                if (WoW.CanCast("DivineShield") &&
                    !WoW.PlayerHasDebuff("Forbearance") &&
                    WoW.CanCast("HandOfReckoning"))
                {
                    Log.Write("Health < 20% using CD: [Taunt & Divine Shield]", Color.Red);
                    WoW.CastSpell("HandOfReckoning");
                    WoW.CastSpell("DivineShield");
                    return;
                }
            }

            if (WoW.HealthPercent < 50)
                if (WoW.CanCast("GuardianOfAncientKings") &&
                    !WoW.PlayerHasBuff("ArdentDefender"))
                {
                    Log.Write("Health < 50% using CD: [Guardian Of Ancient Kings]", Color.Red);
                    WoW.CastSpell("GuardianOfAncientKings");
                    return;
                }

            if (!WoW.HasTarget) return;

            if (WoW.HasBossTarget &&
                WoW.CanCast("AvengingWrath"))
                WoW.CastSpell("AvengingWrath"); // Off the GCD no return needed.

            if (WoW.CanCast("AvengersShield"))
            {
                WoW.CastSpell("AvengersShield");
                return;
            }

            if (WoW.HealthPercent < 100)
                if (WoW.CanCast("EyeOfTyr"))
                {
                    WoW.CastSpell("EyeOfTyr");
                    return;
                }

            if (WoW.CanCast("Judgment"))
            {
                WoW.CastSpell("Judgment");
                return;
            }

            if (WoW.CanCast("Consecration"))
            {
                WoW.CastSpell("Consecration");
                return;
            }

            if (WoW.CanCast("LightOfTheProtector") &&
                WoW.PlayerHasBuff("Consecration") &&
                WoW.HealthPercent < 70)
            {
                WoW.CastSpell("LightOfTheProtector");
                return;
            }

            if (WoW.CanCast("BlessedHammer") &&
                WoW.CountEnemyNPCsInRange > 1)
            {
                WoW.CastSpell("BlessedHammer");
                return;
            }

            if (WoW.CanCast("BastionOfLight") &&
                WoW.PlayerSpellCharges("ShieldOfTheRighteous") == 0 &&
                !WoW.PlayerHasBuff("ShieldOfTheRighteous"))
            {
                WoW.CastSpell("BastionOfLight");
                return;
            }

            if (WoW.CanCast("ShieldOfTheRighteous") &&
                WoW.PlayerHasBuff("Consecration") &&
                WoW.PlayerSpellCharges("ShieldOfTheRighteous") > 0 &&
                !WoW.PlayerHasBuff("ShieldOfTheRighteous"))

            {
                WoW.CastSpell("ShieldOfTheRighteous");
                return;
            }

            if (WoW.CanCast("BlessedHammer"))
            {
                WoW.CastSpell("BlessedHammer");
            }
        }
    }
}