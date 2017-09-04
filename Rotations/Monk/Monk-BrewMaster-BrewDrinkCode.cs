// ReSharper disable UnusedMember.Global
// ReSharper disable UseStringInterpolation

using System.Drawing;
using System.Windows.Forms;
using Frozen.Helpers;

namespace Frozen.Rotation
{
    public class BrewMaster : CombatRoutine
    {
        public enum StaggerLevel
        {
            Light = 1,
            Medium,
            Heavy,
            None
        }

        public override Form SettingsForm
        {
            get { return null; }

            set { }
        }

        public StaggerLevel Stagger
        {
            get
            {
                if (WoW.PlayerHasDebuff("Light Stagger")) return StaggerLevel.Light;
                if (WoW.PlayerHasDebuff("Moderate Stagger")) return StaggerLevel.Medium;
                return WoW.PlayerHasDebuff("Heavy Stagger") ? StaggerLevel.Heavy : StaggerLevel.None;
            }
        }

        public override void Initialize()
        {
            Log.DrawHorizontalLine();
            Log.WriteFrozen("Welcome to Monk BM rotation.  Where you're only cool when you're drunk.", Color.SlateBlue);
            Log.WriteFrozen("Supported Talents: 1,2,2,3,1,3,2", Color.SlateBlue);
            Log.DrawHorizontalLine();
        }

        public override void Stop()
        {
        }

        private void DrinkPurifyingBrew()
        {
            Log.Write("Purify!", Color.CadetBlue);
            WoW.CastSpell("Purifying Brew");
        }

        private void DrinkBlackOxBrew()
        {
            Log.WriteFrozen("Black Ox Brew", Color.CadetBlue);
            WoW.CastSpell("Black Ox Brew");
        }

        public bool NoIsbOrIsbExpiresSoon()
        {
            return !WoW.PlayerHasBuff("Ironskin Brew") || WoW.PlayerHasBuff("Ironskin Brew") && WoW.PlayerBuffTimeRemaining("Ironskin Brew") <= 3;
        }

        public override void Pulse()
        {
            var FreeBeers = WoW.PlayerSpellCharges("Ironskin Brew");

            //Don't Interrupt.
            if (WoW.PlayerIsCasting || WoW.PlayerIsChanneling) return;

            //Don't continue out of combat
            if (!WoW.IsInCombat) return;

            if (FreeBeers == 3)
            {
                //Light Stagger (But still might kill us.  Throw ISB
                if ((Stagger == StaggerLevel.Light || Stagger == StaggerLevel.None) && NoIsbOrIsbExpiresSoon())
                {
                    Log.Write("FreeBeers == 3 and Stagger is Light or less, and (NoISB or ISB Expiring)", Color.SlateBlue);
                    if (WoW.CanCast("Ironskin Brew")) WoW.CastSpell("Ironskin Brew");
                    return;
                }

                //If we have ISB on and > 3 seconds left throw Purifying
                if (WoW.PlayerHasBuff("Ironskin Brew") && WoW.PlayerBuffTimeRemaining("Ironskin Brew") > 3 && (Stagger == StaggerLevel.Medium || Stagger == StaggerLevel.Heavy))
                {
                    Log.Write("FreeBeers == 3 and ISB uptime > 3 and our stagger > light ", Color.SlateBlue);
                    if (WoW.CanCast("Purifying Brew")) WoW.CastSpell("Purifying Brew");
                    return;
                }
            }

            if (FreeBeers == 2)
            {
                //If ISB has slipped or less than 3 seconds left then cast
                if (!WoW.PlayerHasBuff("Ironskin Brew"))
                {
                    Log.Write("FreeBeers == 2 but Player doesn't have Ironskin Brew Buff");
                    if (WoW.CanCast("Ironskin Brew")) WoW.CastSpell("Ironskin Brew");
                    return;
                }
                if (WoW.PlayerHasBuff("Ironskin Brew") && WoW.PlayerBuffTimeRemaining("Ironskin Brew") <= 3)
                {
                    Log.Write("FreeBeers == 2 but player's ISB buff has less than 3 secos");
                    if (WoW.CanCast("Ironskin Brew")) WoW.CastSpell("Ironskin Brew");
                    return;
                }

                if (Stagger != StaggerLevel.None && WoW.PlayerHealthPercent <= 70)
                {
                    Log.Write("FreeBeers == 2, Stagger > None, and Health less than 70%");
                    if (WoW.CanCast("Purifying Brew")) WoW.CastSpell("Purifying Brew");
                    return;
                }
            }

            if (FreeBeers == 1)
            {
                if (Stagger != StaggerLevel.None && WoW.PlayerHealthPercent <= 60 &&
                    WoW.PlayerBuffTimeRemaining("Ironskin Brew") <= 3 && WoW.CanCast("Black Ox Brew"))
                {
                    Log.Write("FreeBeers == 1, Stagger > None, and Health less than 60% an CanCast Black Ox Brew");
                    DrinkBlackOxBrew();
                    return;
                }
                if (Stagger != StaggerLevel.None && WoW.PlayerBuffTimeRemaining("Ironskin Brew") >= 3)
                {
                    Log.Write("FreeBeers == 1, Stagger > None, and Uptime on ISB >= 3 seconds");
                    DrinkPurifyingBrew();
                    return;
                }
                if (WoW.CanCast("Black Ox Brew"))
                {
                    DrinkBlackOxBrew();
                    return;
                }
            }

            if (WoW.PlayerHealthPercent <= 85 && WoW.PlayerSpellCharges("Healing Elixir") > 1 &&
                !WoW.IsSpellOnCooldown("Healing Elixir"))
            {
                WoW.CastSpell("Healing Elixir");
                return;
            }

            if (WoW.TargetIsCasting && WoW.CanCast("Spear Hand Strike") &&
                !WoW.IsSpellOnCooldown("Spear Hand Strike") &&
                WoW.IsSpellInRange("Spear Hand Strike"))
            {
                WoW.CastSpell("Spear Hand Strike");
                return;
            }
            
            if (!WoW.HasTarget || WoW.HasTarget && WoW.TargetIsFriend)
            {
                Log.WriteFrozen("No Target", Color.Red);
                return;
            }


            //Always have Eye of the tiger up, it saves this profile
            //Maintain Eye of the Tiger
            if (
                !WoW.PlayerHasBuff("Eye of the Tiger") && !WoW.IsSpellOnCooldown("Tiger Palm") &&
                WoW.IsSpellInRange("Tiger Palm") ||
                WoW.Energy >= 75
            )
            {
                WoW.CastSpell("Tiger Palm");
                return;
            }

            if (WoW.TargetHasDebuff("Keg Smash") && WoW.CanCast("Breath of Fire"))
            {
                Log.WriteFrozen("Casting Breath of Fire", Color.CadetBlue);
                WoW.CastSpell("Breath of Fire");
                return;
            }

            if (WoW.CanCast("Blackout Strike"))
            {
                Log.WriteFrozen("Casting Blackout Strike", Color.CadetBlue);
                WoW.CastSpell("Blackout Strike");
                return;
            }

            if (!WoW.TargetHasDebuff("Keg Smash") && WoW.CanCast("Keg Smash") && WoW.Energy >= 40)
            {
                Log.WriteFrozen("Casting Keg Smash", Color.CadetBlue);
                WoW.CastSpell("Keg Smash");
                return;
            }

            //if We Can Cast Exploding Keg and in Melee range then do so
            if (WoW.CanCast("Exploding Keg") && !WoW.IsSpellOnCooldown("Exploding Keg") &&
                WoW.IsSpellInRange("Tiger Palm"))
            {
                WoW.CastSpellOnMe("Exploding Keg");
                return;
            }


            if (WoW.CanCast("Tiger Palm") && WoW.Energy >= 40)
            {
                Log.WriteFrozen("Casting Tiger Palm Filler", Color.CadetBlue);
                WoW.CastSpell("Tiger Palm");
                return;
            }

            Log.WriteFrozen("Nothing!", Color.Chocolate);
        }
    }
}

/*
[AddonDetails.db]
AddonAuthor=BrewDrinkCode
AddonName=Frozen
WoWVersion=Legion - 70300
[SpellBook.db]
Spell,121253,Keg Smash,D1
Spell,115181,Breath of Fire,D2
Spell,100780,Tiger Palm,D3
Spell,205523,Blackout Strike,D4
Spell,115308,Ironskin Brew,D5
Spell,119582,Purifying Brew,D6
Spell,115399,Black Ox Brew,D7
Spell,122281,Healing Elixir,D8
Spell,116705,Spear Hand Strike,D9
Spell,115072,Expel Harm,D0
Spell,209153,Zen Meditation,F1
Spell,123986,Chi Burst,F2
Spell,214326,Exploding Keg,F3
Aura,121253,Keg Smash
Aura,115181,Breath of Fire
Aura,196736,Blackout Combo
Aura,115308,Ironskin Brew
Aura,124275,Light Stagger
Aura,124274,Moderate Stagger
Aura,124273,Heavy Stagger
Aura,196607,Eye of the Tiger
*/