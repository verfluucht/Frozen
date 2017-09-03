using System.Drawing;
using System.Windows.Forms;
using Frozen.GUI;
using Frozen.Helpers;


namespace Frozen.Rotation
{
    public class Monk_Mistweaver : CombatRoutine
    {
        public override Form SettingsForm
        {
            get { return null; }

            set { }
        }

        const string supportedTalents = "3133121";

        public override void Initialize()
        {
            Log.Clear();
            Log.WriteFrozen("Welcome to Frozen Mistweaver", Color.Black);
            Log.Write($"Supported Talents: {supportedTalents}");
            Log.Write("Ensure you have setup healing keybinds before.", Color.Red);
            Log.Write("DPS can be enabled/disabled via CDs on/off.", Color.Red);

            Log.DrawHorizontalLine();
            Log.Write("If you do proving grounds the Tank Id is 5 when it asks you.", Color.Red);
            Log.DrawHorizontalLine();
        }

        public override void Stop()
        {
        }
        
        public override void Pulse()
        {
            Log.Write("Mana: " + WoW.Mana, Color.Red);

            if (WoW.TankId == 0)
            {
                var f = new frmEnterTankId { TopMost = true };
                f.ShowDialog();
            }

            if (WoW.PlayerHealthPercent == 0 || WoW.IsMounted) return;
            if (WoW.PlayerIsCasting) return;

            var lowest = WoW.PartyLowestHealthPercent;

            var currentTargetId = WoW.PartyMemberIdWithLowestHealthPercent;

            var averageHp = WoW.PartyAverageHealthPercent;

            WoW.TargetMember(currentTargetId);

            if (currentTargetId == 0) return;
            if (lowest == 100) return;

            if (WoW.GroupSize == 5)
                Log.Write($"Lowest Health Target = [/target party{currentTargetId}] health = {averageHp}");
            else
                Log.Write($"Lowest Health Target = [/target raid{currentTargetId}] health = {averageHp}");

            if (lowest < 85 && !WoW.IsSpellOnCooldown("ThunderFocusTea"))
                WoW.CastSpell("ThunderFocusTea");

            if (WoW.PlayerHasBuff("ThunderFocusTea"))
            {
                if (averageHp < 70 &&
                    WoW.CanCast("EssenceFont") &&
                    !WoW.IsSpellOnCooldown("EssenceFont") &&
                    !WoW.PlayerHasBuff("EssenceFont"))
                {
                    WoW.CastSpell("EssenceFont");
                }

                if (averageHp < 90 &&
                    WoW.CanCast("Vivify") &&
                    !WoW.IsSpellOnCooldown("Vivify"))
                {
                    WoW.CastSpell("Vivify");
                }
            }

            if (WoW.IsInCombat && WoW.Mana <= 70 &&
                !WoW.IsSpellOnCooldown("Manatea"))
            {
                WoW.CastSpell("Manatea");
                return;
            }

            if (WoW.CanCast("RenewingMist") && WoW.IsInCombat &&
                !WoW.TargetHasBuff("RenewingMist") &&
                !WoW.PlayerHasBuff("RenewingMist") &&
                !WoW.IsSpellOnCooldown("RenewingMist"))
            {
                WoW.CastSpell("RenewingMist");
                return;
            }
            if (UseCooldowns)
            {
                if (lowest > 90)
                {
                    if (WoW.CanCast("RSK") && WoW.HasTarget && WoW.TargetIsEnemy && WoW.IsInCombat)
                    {
                        WoW.CastSpell("RSK");
                        return;
                    }

                    if (WoW.CanCast("BlackoutKick") && WoW.HasTarget && WoW.TargetIsEnemy && WoW.IsInCombat &&
                        WoW.PlayerBuffStacks("BlackoutBuff") == 3)
                    {
                        WoW.CastSpell("BlackoutKick");
                        return;
                    }

                    if (WoW.CanCast("Tiger") && WoW.HasTarget && WoW.TargetIsEnemy && WoW.IsInCombat &&
                        WoW.PlayerBuffStacks("BlackoutBuff") < 3)
                    {
                        WoW.CastSpell("Tiger");
                        return;
                    }

                }
            }
            if (WoW.CanCast("Revival") &&
                averageHp < 65 &&
                !WoW.IsSpellOnCooldown("Revival"))
            {
                WoW.CastSpell("Revival");
                return;
            }

            if (WoW.CanCast("LifeCocoon") &&
                lowest <= 25 &&
                !WoW.IsSpellOnCooldown("LifeCocoon"))
            {
                WoW.CastSpell("LifeCocoon");
                return;
            }


            if (averageHp < 75)
            {
                if (WoW.CanCast("InvokeChiJitheRedCrane") &&
                    !WoW.IsSpellOnCooldown("InvokeChiJitheRedCrane"))
                {
                    WoW.CastSpell("InvokeChiJitheRedCrane");
                    return;
                }

                if (WoW.CanCast("EssenceFont") &&
                    !WoW.IsSpellOnCooldown("EssenceFont") &&
                    !WoW.PlayerHasBuff("EssenceFont"))
                {
                    WoW.CastSpell("EssenceFont");
                    return;
                }

                if (WoW.CanCast("Vivify"))
                {
                    WoW.CastSpell("Vivify");
                    return;
                }
            }

            if (WoW.Level > 100)
            {
                if (WoW.CanCast("SheilunsGift") &&
                    lowest <= 75 && !WoW.IsMoving &&
                    WoW.PlayerSpellCharges("SheilunsGift") >= 4)
                {
                    WoW.CastSpell("SheilunsGift");
                    return;
                }
            }

            if (WoW.CanCast("EnvelopingMist") &&
                lowest <= 70 &&
                !WoW.IsMoving &&
                !WoW.TargetHasBuff("EnvelopingMist") &&
                WoW.LastSpell != ("EnvelopingMist"))
            {
                WoW.CastSpell("EnvelopingMist");
                return;
            }

            if (WoW.CanCast("Vivify") &&
                lowest <= 65)
            {
                WoW.CastSpell("Vivify");
                return;
            }

            if (WoW.CanCast("Effuse") && lowest <= 90 &&
                !WoW.PlayerIsChanneling)
            {
                WoW.CastSpell("Effuse");
                return;
            }

            if (WoW.CanCast("ChiWave") && lowest <= 90 &&
                WoW.IsInCombat)
            {
                WoW.CastSpell("ChiWave");
                return;
            }

            if (WoW.IsInCombat && WoW.Mana <= 70 &&
                !WoW.IsSpellOnCooldown("Manatea"))
                WoW.CastSpell("Manatea");
        }

    }
}

/*
[AddonDetails.db]
AddonAuthor=Lhasse
AddonName=Frozen
WoWVersion=Legion - 70300
[SpellBook.db]
Spell,116849,LifeCocoon,S
Spell,115098,ChiWave,D7
Spell,205406,SheilunsGift,None
Spell,197908,Manatea,D3
Spell,124682,EnvelopingMist,H
Spell,116680,ThunderFocusTea,D0
Spell,198664,InvokeChiJitheRedCrane,D2
Spell,116694,Effuse,T
Spell,115151,RenewingMist,Y
Spell,191837,EssenceFont,D5
Spell,115310,Revival,D9
Spell,107428,RSK,V
Spell,116670,Vivify,B
Spell,100780,Tiger,D2
Spell,100784,BlackoutKick,E
Aura,119611,RenewingMist
Aura,191840,EssenceFont
Aura,202090,BlackoutBuff
Aura,124682,EnvelopingMist
Aura,116680,ThunderFocusTea
Aura,205406,SheilunsGift
*/
