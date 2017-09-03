﻿using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Frozen.Helpers;

namespace Frozen.Rotation
{
    internal class MiestroShadow : CombatRoutine
    {
        //General constants
        private const int HEALTH_PERCENT_FOR_SWD = 20;

        private const int PANIC_INSANITY_VALUE = 45;
        private const int INTERRUPT_DELAY = 650;

        //Spell Constants
        private const string SHADOW_PAIN = "Shadow Word: Pain";

        private const string VAMPIRIC_TOUCH = "Vampiric Touch";
        private const string VOID_TORRENT = "Void Torrent";
        private const string MIND_FLAY = "Mind Flay";
        private const string MIND_BLAST = "Mind Blast";
        private const string SHADOW_MEND = "Shadow Mend";
        private const string POWER_WORD_SHIELD = "Power Word: Shield";
        private const string VOID_BOLT = "Void Bolt";
        private const string VOID_ERUPTION = "Void Eruption";
        private const string SHADOW_DEATH = "Shadow Word: Death";
        private const string SHADOWFORM = "Shadowform";
        private const string SILENCE = "Silence";
        private const string SHADOW_FIEND = "Shadowfiend";

        //Aura Constants
        private const string VOIDFORM_AURA = "Voidform";

        private const string SHADOWFORM_AURA = "Shadowform";

        /// <summary>
        ///     Private variable for timing interrupt delay.
        /// </summary>
        private readonly Stopwatch timer = new Stopwatch();

        /**
         * Member Variables
         */
        //Settings form, Right side.
        public override Form SettingsForm { get; set; }

        //Initialize, print some details so the user can prepare thier ingame character.
        public override void Initialize()
        {
            Log.Write("Welcome to Miestro's Shadow rotation", Color.Orange);
            Log.Write("Please make sure your specialization is as follows: http://us.battle.net/wow/en/tool/talent-calculator#Xba!0100000", Color.Orange);
            Log.Write("Note: legendaries are not supported. If you need one supported or something fixed, please make note of it in the discord.", Color.Orange);
            Log.Write("Needs talent 'Legacy of the void'");
        }

        public override void Stop()
        {
            //Do nothing
        }

        public override void Pulse()
        {
            if (WoW.PlayerHealthPercent <= 1)
                return;

            //Heal yourself, Can't do damage if you're dead.
            if (WoW.PlayerHealthPercent <= 60)
            {
                if (isPlayerBusy(true, false) && !WoW.PlayerHasBuff(POWER_WORD_SHIELD))
                    castWithRangeCheck(POWER_WORD_SHIELD);
                castWithRangeCheck(SHADOW_MEND);
            }
            //Shield if health is dropping.
            if (WoW.PlayerHealthPercent <= 80 && !WoW.PlayerHasBuff(POWER_WORD_SHIELD))
                castWithRangeCheck(POWER_WORD_SHIELD, true);

            //Always have shadowform.
            if (!(WoW.PlayerHasBuff(SHADOWFORM_AURA) || WoW.PlayerHasBuff(VOIDFORM_AURA)))
                castWithRangeCheck(SHADOWFORM);

            if (WoW.HasTarget && WoW.TargetIsEnemy)
            {
                if (!WoW.PlayerHasBuff(VOIDFORM_AURA))
                {
                    //Just so happens that the spell and debuff name are the same, this is not ALWAYS the case.
                    maintainDebuff(VAMPIRIC_TOUCH, VAMPIRIC_TOUCH, 5);
                    maintainDebuff(SHADOW_PAIN, SHADOW_PAIN, 2);
                }
                else
                {
                    maintainDebuff(VAMPIRIC_TOUCH, VAMPIRIC_TOUCH, WoW.SpellCooldownTimeRemaining(VOID_BOLT));
                    maintainDebuff(SHADOW_PAIN, SHADOW_PAIN, WoW.SpellCooldownTimeRemaining(VOID_BOLT));
                }

                switch (combatRoutine.Type)
                {
                    //Single target
                    case RotationType.SingleTarget:
                        doRotation();
                        break;

                    //Against 2 or more
                    case RotationType.AOE:
                    case RotationType.Cleave:
                        doRotation();
                        break;
                }
            }

            //Interrupt after a delay.
            if (WoW.TargetIsCasting && WoW.TargetIsEnemy)
                if (timer.ElapsedMilliseconds >= INTERRUPT_DELAY)
                {
                    castWithRangeCheck(SILENCE);
                }
                else
                {
                    timer.Reset();
                    timer.Start();
                }
        }

        /// <summary>
        ///     Do the rotation.
        /// </summary>
        private void doRotation()
        {
            if (WoW.Insanity >= 70 || WoW.PlayerHasBuff(VOIDFORM_AURA))
            {
                //Expend insanity in voidform.
                if (WoW.HasTarget && !WoW.PlayerHasBuff(VOIDFORM_AURA))
                {
                    castWithRangeCheck(VOID_ERUPTION);
                }
                else
                {
                    //If we can, cast it.
                    castWithRangeCheck(VOID_BOLT);

                    //Cast it.
                    if (WoW.Level > 100)
                        castWithRangeCheck(VOID_TORRENT);

                    //If the boss health is at or below our set threshold SW:D
                    if (WoW.TargetHealthPercent <= HEALTH_PERCENT_FOR_SWD)
                        if (WoW.PlayerSpellCharges(SHADOW_DEATH) == 2 && WoW.Insanity <= 70)
                            castWithRangeCheck(SHADOW_DEATH);
                        else if (WoW.PlayerSpellCharges(SHADOW_DEATH) == 1)
                            if (WoW.Insanity > PANIC_INSANITY_VALUE && !(WoW.IsSpellOnCooldown(MIND_BLAST) || WoW.IsSpellOnCooldown(VOID_BOLT)) ||
                                WoW.Insanity <= calculateInsanityDrain())
                                castWithRangeCheck(SHADOW_DEATH);

                    //Cast shadowfiend if we have more than 15 stacks of voidform aura.
                    if (WoW.PlayerBuffStacks(VOIDFORM_AURA) >= 15)
                        castWithRangeCheck(SHADOW_FIEND);

                    //If we can, cast it.
                    castWithRangeCheck(MIND_BLAST);

                    //If we have high stacks, cast shadowfiend.
                    /* Disabled, allow player to cast. 
                    if (WoW.PlayerBuffStacks(VOIDFORM_AURA)>=15) {
                        castWithRangeCheck(SHADOWFIEND);
                    }
                    */

                    if (!isPlayerBusy(ignoreChanneling: false))
                        castWithRangeCheck(MIND_FLAY);
                }
            }
            else
            {
                //Build up insanity
                if (WoW.HasTarget && WoW.TargetIsEnemy && WoW.TargetIsVisible)
                {
                    //If we can, cast mind blast.
                    if (castWithRangeCheck(MIND_BLAST))
                        return;

                    //If we don't have anything else to do, cast Mind flay.
                    if (!isPlayerBusy(ignoreChanneling: false))
                        castWithRangeCheck(MIND_FLAY);
                }
            }
        }

        /// <summary>
        ///     Calculate the amount of insanity currently drained per second
        /// </summary>
        /// <returns>The amount of insanity drained per second</returns>
        private float calculateInsanityDrain()
        {
            return 9 + (WoW.PlayerBuffStacks(VOIDFORM_AURA) - 1 / 2);
        }

        /// <summary>
        ///     Get whether we can cast spells based on what the player is currently doing.
        /// </summary>
        /// <param name="ignoreMovement">Can we ignore movement</param>
        /// <param name="ignoreChanneling"></param>
        /// <returns>True if we can not currently cast another spell.</returns>
        private bool isPlayerBusy(bool ignoreMovement = false, bool ignoreChanneling = true)
        {
            var canCast = WoW.PlayerIsCasting || WoW.PlayerIsChanneling && !(ignoreChanneling && !WoW.WasLastCasted(VOID_TORRENT)) ||
                          WoW.IsMoving && ignoreMovement;
            return canCast;
        }

        /// <summary>
        ///     Cast a spell by name. Will check range, cooldown, and visibility. After the spell is cast, the thread will sleep
        ///     for GCD.
        /// </summary>
        /// <param name="spellName">The name of the spell in the spell databse.</param>
        /// <param name="ignoreMovement">Can we cast while moving.</param>
        /// <param name="ignoreChanneling"></param>
        /// <returns>True if the spell was cast, false if it was not.</returns>
        private bool castWithRangeCheck(string spellName, bool ignoreMovement = false, bool ignoreChanneling = true)
        {
            //Can't do range check.
            if (!isPlayerBusy(ignoreMovement, ignoreChanneling) && WoW.CanCast(spellName))
            {
                WoW.CastSpell(spellName);
                if (WoW.IsSpellOnGCD(spellName))
                    Thread.Sleep(WoW.SpellCooldownTimeRemaining(spellName));
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Maintain a debuff if it is not currently on the target or if it's about to expire.
        /// </summary>
        /// <param name="debuffName">The name of the debuff we are maintaining.</param>
        /// <param name="spellName">The name of the spell that applies the debuff.</param>
        /// <param name="minTimeToExpire">The minimum amount of time to allow on the debuff before renewing.</param>
        /// <returns>True if the debuff was renewed, otherwise fasle.</returns>
        private void maintainDebuff(string debuffName, string spellName, float minTimeToExpire)
        {
            if (!WoW.TargetHasDebuff(debuffName) || WoW.TargetDebuffTimeRemaining(debuffName) < minTimeToExpire)
                castWithRangeCheck(spellName);
        }
    }
}

/*
[AddonDetails.db]
AddonAuthor=Miestro
AddonName=Frozen
WoWVersion=Legion - 70300
[SpellBook.db]
Spell,589,Shadow Word: Pain,E
Spell,34914,Vampiric Touch,V
Spell,205065,Void Torrent,
Spell,15407,Mind Flay,Y
Spell,8092,Mind Blast,T
Spell,186263,Shadow Mend,W
Spell,17,Power Word: Shield,S
Spell,205448,Void Bolt,Z
Spell,228260,Void Eruption,Z
Spell,193223,Surrender to Madness,F
Spell,32379,Shadow Word: Death,H
Spell,34433,Shadowfiend,D6
Spell,232698,Shadowform,B
Spell,15487,Silence,D2
Spell,47585,Dispersion,None
Aura,232698,Shadowform
Aura,34914,Vampiric Touch
Aura,589,Shadow Word: Pain
Aura,197937,Lingering Insanity
Aura,194249,Voidform
Aura,17,Power Word: Shield
*/