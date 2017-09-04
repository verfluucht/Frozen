using System;
using System.Drawing;
using System.Windows.Forms;
using Frozen.Helpers;

namespace Frozen.Rotation
{
    public class WarriorFury : CombatRoutine
    {
              
        public override Form SettingsForm { get; set; }

	public static bool DragonRoar
        {
            get
            {
                var dragonRoar = ConfigFile.ReadValue("WarriorFury", "Dragonroar").Trim();
                return dragonRoar == "" || Convert.ToBoolean(dragonRoar);
            }
            set { ConfigFile.WriteValue("WarriorFury", "DragonRoar", value.ToString()); }
        }

        public static bool BattleCry
        {
            get
            {
                var battleCry = ConfigFile.ReadValue("WarriorFury", "BattleCry").Trim();
                return battleCry == "" || Convert.ToBoolean(battleCry);
            }
            set { ConfigFile.WriteValue("WarriorFury", "BattleCry", value.ToString()); }
        }


        public override void Initialize()
        {
            Log.Write("Welcome to Fury Warrior", Color.Green);
            Log.Write("Suggested build: 2x1x233", Color.Green);
        }

        public override void Stop()
        {
        }

        public override void Pulse()
        {
            if (WoW.HasTarget && WoW.TargetIsEnemy && WoW.IsSpellInRange("Bloodthirst"))
            {
                if (WoW.CanCast("DragonRoar") && DragonRoar && !WoW.PlayerHasBuff("DragonRoarBuff"))
                {
                    WoW.CastSpell("DragonRoar");
                    return;
                }
                if (WoW.CanCast("BattleCry") && BattleCry && WoW.PlayerHasBuff("DragonRoarBuff"))
                {
                    WoW.CastSpell("BattleCry");
                    return;
                }
                if (WoW.CanCast("Bloodthirst") && !WoW.PlayerHasBuff("Enrage") && !WoW.IsSpellOnCooldown("Bloodthirst"))
                {
                    WoW.CastSpell("Bloodthirst");
                    return;
                }
                if (WoW.CanCast("OdynsFury") && WoW.PlayerHasBuff("BattleCry"))
                {
                    WoW.CastSpell("OdynsFury");
                    return;
                }
                
            }

            if (combatRoutine.Type == RotationType.SingleTarget && WoW.TargetHealthPercent >= 21) // Do Single Target Stuff here
                if (WoW.HasTarget && WoW.TargetIsEnemy && WoW.IsSpellInRange("Bloodthirst"))
                {
                    if (WoW.CanCast("Raging Blow") && WoW.PlayerHasBuff("Enrage") || WoW.Rage <= 99 && WoW.IsSpellOnCooldown("Bloodthirst") && WoW.CanCast("Raging Blow"))
                    {
                        WoW.CastSpell("Raging Blow");
                        return;
                    }

		    if (WoW.CanCast("Rampage") && WoW.Rage >= 100)
                    {
                        WoW.CastSpell("Rampage");
                        return;
                    }

		    if (WoW.CanCast("Bloodthirst") && WoW.Rage < 100 || WoW.CanCast("Bloodthirst") && !WoW.PlayerHasBuff("Enrage") && WoW.IsSpellOnCooldown("Raging Blow"))
                    {
                        WoW.CastSpell("Bloodthirst");
                        return;
                    }
		    if (WoW.CanCast("Furious Slash") && WoW.IsSpellOnCooldown("Bloodthirst") && WoW.IsSpellOnCooldown("Raging Blow") && WoW.Rage < 100)
                    {
                        WoW.CastSpell("Furious Slash");
                        return;
                    }

                }

	    if (combatRoutine.Type == RotationType.SingleTarget && WoW.TargetHealthPercent <= 20) // Do Single Target Execute Stuff here
                if (WoW.HasTarget && WoW.TargetIsEnemy && WoW.IsSpellInRange("Bloodthirst"))
                {
		    if (WoW.CanCast("Execute") && WoW.TargetHealthPercent <= 20 && WoW.PlayerHasBuff("SenseDeath"))
                    {
                        WoW.CastSpell("Execute");
                        return;
		    }
	            if (WoW.CanCast("Execute") && WoW.TargetHealthPercent <= 20 && WoW.Rage >= 25)
                    {
                        WoW.CastSpell("Execute");
                        return;
                    }
		    if (WoW.CanCast("Bloodthirst") && WoW.Rage < 25)
                    {
                        WoW.CastSpell("Bloodthirst");
                        return;
                    }
                    if (WoW.CanCast("Raging Blow") && WoW.Rage < 25 && WoW.IsSpellOnCooldown("Bloodthirst"))
                    {
                        WoW.CastSpell("Raging Blow");
                        return;
                    }
		    if (WoW.CanCast("Furious Slash") && WoW.IsSpellOnCooldown("Bloodthirst") && WoW.IsSpellOnCooldown("Raging Blow") && WoW.Rage < 25)
                    {
                        WoW.CastSpell("Furious Slash");
                        return;
                    }

                }
                            

            if (combatRoutine.Type == RotationType.AOE)
                if (WoW.HasTarget && WoW.IsSpellInRange("Bloodthirst") && WoW.IsInCombat)
                {	
		    if (WoW.CanCast("Raging Blow") && WoW.PlayerHasBuff("Enrage") && WoW.CountEnemyNPCsInRange < 4 || WoW.Rage <= 99 && WoW.IsSpellOnCooldown("Bloodthirst") && WoW.CanCast("Raging Blow"))
                    {
                        WoW.CastSpell("Raging Blow");
                        return;
                    }

		    if (WoW.CanCast("Rampage") && WoW.Rage >= 100 && WoW.PlayerHasBuff("Meat-Cleaver"))
                    {
                        WoW.CastSpell("Rampage");
                        return;
                    }
		    if (WoW.CanCast("Whirlwind") && !WoW.PlayerHasBuff("Meat-Cleaver") || WoW.CanCast("Whirlwind") && WoW.PlayerHasBuff("Wrecking Ball"))
                    {
                        WoW.CastSpell("Whirlwind");
                        return;
		    }
		    if (WoW.CanCast("Bloodthirst") && WoW.Rage < 100 && WoW.CountEnemyNPCsInRange < 8 && WoW.PlayerHasBuff("Meat-Cleaver") || WoW.CanCast("Bloodthirst") && !WoW.PlayerHasBuff("Enrage") && WoW.IsSpellOnCooldown("Raging Blow"))
                    {
                        WoW.CastSpell("Bloodthirst");
                        return;
                    }
		    if (WoW.CanCast("Furious Slash") && WoW.IsSpellOnCooldown("Bloodthirst") && WoW.IsSpellOnCooldown("Raging Blow") && WoW.Rage < 100 && WoW.CountEnemyNPCsInRange < 3)
                    {
                        WoW.CastSpell("Furious Slash");
                        return;
                    }
		    

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
AddonAuthor=Lelefi
AddonName=Frozen
WoWVersion=Legion - 70300
[SpellBook.db]
Spell,23881,Bloodthirst,D3
Spell,85288,Raging Blow,D4
Spell,100130,Furious Slash,D5
Spell,5308,Execute,D6
Spell,184367,Rampage,D7
Spell,190411,Whirlwind,D8
Spell,1719,BattleCry,D9
Spell,118000,DragonRoar,T
Spell,107574,Avatar,D0
Spell,205545,OdynsFury,G
Aura,118000,DragonRoarBuff
Aura,1719,BattleCry
Aura,200863,SenseDeath
Aura,184362,Enrage
Aura,201009,Juggernaut
Aura,206316,Massacre
Aura,85739,Meat-Cleaver
Aura,215570,Wrecking Ball
Aura,215572,Frothing
*/
