using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Frozen.Helpers;
using Timer = System.Timers.Timer;

#pragma warning disable 1998

namespace Frozen.Rotation
{
    public class Guardian : CombatRoutine
    {
        public static string DisplayText;


        // Variables for HP timer
        private static int PreviousHP;

        private static int CurrentHP;

        // Variables for bearcatting
        private static int bearstep;

        private static int incarnatestep;
        private static bool OpenerDone;

        // How fast HP is changing
        private static float HPRateOfChange;

        // Booleans to check for running events
        private static bool firstrun = true;

        private static bool interrupting;

        // HP Rate Of Change booleans
        private static bool HPROC1sAny;

        // HP Rate of Change Maxs
        private static int HPROC1sMax;

        private static int HPROC5sMax;
        private static int HPROC10sMax;

        // User selection booleans
        private static bool DPSBurst;

        private static bool CoolDowns;

        public static ObservableCollection<int> InterruptibleSpells = new ObservableCollection<int>(interimInterruptibleSpells);
        private readonly Stopwatch CoolDownTimer = new Stopwatch();
        private readonly Stopwatch DPSTimer = new Stopwatch();

        // The Frenzied Regen stopwatch
        private readonly Stopwatch FrenziedTimer = new Stopwatch();

        // Selections stopwatch
        private readonly Stopwatch MitigationSwitchTimer = new Stopwatch();

        // Overlay display stopwatch
        private readonly Stopwatch OverlayTimer = new Stopwatch();

        private readonly Queue<float> Queue10s = new Queue<float>();


        // HP Rate Of Change Queues 
        private readonly Queue<float> Queue1s = new Queue<float>();

        private readonly Queue<float> Queue5s = new Queue<float>();
        private GroupBox ActiveMitigation;
        private Button AddButton;
        private TextBox AddSpellBox;
        private GroupBox Artifact;
        private CheckBox BarkskinBox;
        private NumericUpDown BarkskinHPNum;
        private CheckBox BearcattingBox;
        private CheckBox BristlingFurBox;
        private CheckBox ChainPullingBox;
        private Button cmdSave;
        private ComboBox CoolDownKeyCombo;
        private ComboBox CoolDownModCombo;
        private CheckBox CoolDownUseBox;
        private CheckBox CustomDelayBox;
        private GroupBox DefensiveCooldowns;
        private TabPage Defensives;
        private CheckBox DisplayCDBox;
        private CheckBox DisplayDPSBox;
        private CheckBox DisplayFRBox;
        private Button DisplayInfo;
        private CheckBox DisplayInterruptBox;
        private CheckBox DisplayMitigationBox;
        private ComboBox DPSBurstKeyCombo;
        private ComboBox DPSBurstModCombo;
        private CheckBox FrenziedRegenBox;
        private TabPage Healing;
        private Label HPROCLabel;
        private TrackBar HPROCTrackBar;
        private Label HPROCValue;
        private CheckBox IncapacitatingBox;
        private CheckBox IncarnationBox;
        private CheckBox IncarnationDPSBox;
        private TrackBar InterruptDelayTrackBar;
        private Label InterruptDelayValue;
        private GroupBox Interrupting;
        private TabPage InterruptingTab;
        private CheckBox InterruptOnlyListedBox;
        private TabPage KeyBindings;
        private GroupBox KeyPressBindings;
        private CheckBox LunarBeamBox;
        private NumericUpDown LunarBeamNum;
        private TabPage Miscellaneous;
        private ComboBox MitigationKeyCombo;
        private ComboBox MitigationModCombo;
        private CheckBox MitigationSwitchBox;
        private GroupBox Overlay;
        private CheckBox PoolRageBox;
        private ComboBox PrioritizeCombo;
        private Label PrioritizeText;
        private CheckBox ProwlOOCBox;
        private CheckBox PulverizeBox;
        private CheckBox RageOfTheSleeperBox;
        private CheckBox RageOfTheSleeperHealthBox;
        private NumericUpDown RageOfTheSleeperHPNum;
        private GroupBox Regeneration;
        private Button RemoveButton;
        private CheckBox RendAndTearBox;
        private GroupBox SaveAbilities;
        private ComboBox SaveRageOfTheSleeperCombo;
        private CheckBox SaveRageOfTheSleeperKeyBox;
        private CheckBox SkullBashInterruptBox;
        private CheckBox SoulOfTheForestBox;
        private ListBox SpellListBox;
        private CheckBox SurvivalInstinctsBox;
        private NumericUpDown SurvivalInstinctsHPNum;
        private CheckBox TabAOEBox;
        private NumericUpDown TabAOEDelayNum;
        private TabControl TabCtrl;
        private ComboBox TabMacroCombo;
        private GroupBox Talents;
        private TabPage TalentsArtifact;
        private GroupBox TalentsHealing;

        // The HP timer
        private Timer timer;

        private CheckBox UseDPSBurstBox;
        private Label usetab;
        private CheckBox UseWildChargeBox;
        private ComboBox WildChargeCombo;

        //List for interruptible spell id's
        public static ObservableCollection<int> interimInterruptibleSpells
        {
            get
            {
                try
                {
                    var spellString = ConfigFile.ReadValue("Guardian", "InterruptibleSpells");
                    var lstInterruptibleSpells = new List<int>();
                    foreach (var g in spellString.Split(','))
                        lstInterruptibleSpells.Add(Convert.ToInt32(g));
                    var interruptibleSpells = new ObservableCollection<int>(lstInterruptibleSpells);
                    return interruptibleSpells;
                }

                catch (FormatException)
                {
                    var lstexcInterruptibleSpells = new List<int>
                    {
                        2008,
                        32546,
                        121135,
                        152108,
                        155245,
                        152118,
                        33786,
                        1064,
                        116858,
                        157695,
                        111771,
                        64843,
                        605,
                        689,
                        103103,
                        117014,
                        339,
                        124682,
                        114163,
                        5782,
                        2061,
                        19750,
                        120517,
                        102051,
                        120517,
                        2060,
                        48181,
                        2060,
                        73920,
                        8004,
                        5185,
                        77472,
                        51514,
                        82326,
                        82327,
                        85222,
                        32375,
                        115268,
                        129197,
                        118,
                        61305,
                        28272,
                        61721,
                        61025,
                        61780,
                        28271,
                        596,
                        33076,
                        20484,
                        8936,
                        20066,
                        2006,
                        115178,
                        50769,
                        113724,
                        6358,
                        686,
                        115175,
                        116694,
                        155361,
                        48438,
                        85673
                    };
                    var excinterruptibleSpells = new ObservableCollection<int>(lstexcInterruptibleSpells);
                    return excinterruptibleSpells;
                }
            }
        }

        // Variables for options	
        public override Form SettingsForm { get; set; }

        // Start of saved variables
        // Code that reads and saves to file  

        private static bool SurvivalInstincts
        {
            get
            {
                var survivalInstincts = ConfigFile.ReadValue("Guardian", "SurvivalInstincts").Trim();
                return survivalInstincts != "" && Convert.ToBoolean(survivalInstincts);
            }
            set { ConfigFile.WriteValue("Guardian", "SurvivalInstincts", value.ToString()); }
        }

        private static bool Pulverize
        {
            get
            {
                var pulverize = ConfigFile.ReadValue("Guardian", "Pulverize").Trim();
                return pulverize != "" && Convert.ToBoolean(pulverize);
            }
            set { ConfigFile.WriteValue("Guardian", "Pulverize", value.ToString()); }
        }

        private static bool Barkskin
        {
            get
            {
                var barkskin = ConfigFile.ReadValue("Guardian", "Barkskin").Trim();
                return barkskin != "" && Convert.ToBoolean(barkskin);
            }
            set { ConfigFile.WriteValue("Guardian", "Barkskin", Convert.ToString(Barkskin)); }
        }

        private static bool SkullBashInterrupt
        {
            get
            {
                var skullBashInterrupt = ConfigFile.ReadValue("Guardian", "SkullBashInterrupt").Trim();
                return skullBashInterrupt != "" && Convert.ToBoolean(skullBashInterrupt);
            }
            set { ConfigFile.WriteValue("Guardian", "SkullBashInterrupt", value.ToString()); }
        }

        private static bool RageOfTheSleeper
        {
            get
            {
                var rageOfTheSleeper = ConfigFile.ReadValue("Guardian", "RageOfTheSleeper").Trim();
                return rageOfTheSleeper != "" && Convert.ToBoolean(rageOfTheSleeper);
            }
            set { ConfigFile.WriteValue("Guardian", "RageOfTheSleeper", value.ToString()); }
        }

        private static bool SoulOfTheForest
        {
            get
            {
                var soulOfTheForest = ConfigFile.ReadValue("Guardian", "SoulOfTheForest").Trim();
                return soulOfTheForest != "" && Convert.ToBoolean(soulOfTheForest);
            }
            set { ConfigFile.WriteValue("Guardian", "SoulOfTheForest", value.ToString()); }
        }

        private static bool RendAndTear
        {
            get
            {
                var rendAndTear = ConfigFile.ReadValue("Guardian", "RendAndTear").Trim();
                return rendAndTear != "" && Convert.ToBoolean(rendAndTear);
            }
            set { ConfigFile.WriteValue("Guardian", "RendAndTear", value.ToString()); }
        }

        private static bool BristlingFur
        {
            get
            {
                var bristlingFur = ConfigFile.ReadValue("Guardian", "BristlingFur").Trim();
                return bristlingFur != "" && Convert.ToBoolean(bristlingFur);
            }
            set { ConfigFile.WriteValue("Guardian", "BristlingFur", value.ToString()); }
        }

        private static bool Incarnation
        {
            get
            {
                var incarnation = ConfigFile.ReadValue("Guardian", "Incarnation").Trim();
                return incarnation != "" && Convert.ToBoolean(incarnation);
            }
            set { ConfigFile.WriteValue("Guardian", "Incarnation", value.ToString()); }
        }

        private static bool TabAOE
        {
            get
            {
                var tabAoe = ConfigFile.ReadValue("Guardian", "TabAOE").Trim();
                return tabAoe != "" && Convert.ToBoolean(tabAoe);
            }
            set { ConfigFile.WriteValue("Guardian", "TabAOE", value.ToString()); }
        }

        private static bool Incapacitating
        {
            get
            {
                var incapacitating = ConfigFile.ReadValue("Guardian", "Incapacitating").Trim();
                return incapacitating != "" && Convert.ToBoolean(incapacitating);
            }
            set { ConfigFile.WriteValue("Guardian", "Incapacitating", value.ToString()); }
        }

        private static bool ChainPulling
        {
            get
            {
                var chainPulling = ConfigFile.ReadValue("Guardian", "ChainPulling").Trim();
                return chainPulling != "" && Convert.ToBoolean(chainPulling);
            }
            set { ConfigFile.WriteValue("Guardian", "ChainPulling", value.ToString()); }
        }

        private static bool Bearcatting
        {
            get
            {
                var bearcatting = ConfigFile.ReadValue("Guardian", "Bearcatting").Trim();
                return bearcatting != "" && Convert.ToBoolean(bearcatting);
            }
            set { ConfigFile.WriteValue("Guardian", "Bearcatting", value.ToString()); }
        }

        private static bool ProwlOOC
        {
            get
            {
                var prowlOoc = ConfigFile.ReadValue("Guardian", "ProwlOOC").Trim();
                return prowlOoc != "" && Convert.ToBoolean(prowlOoc);
            }
            set { ConfigFile.WriteValue("Guardian", "ProwlOOC", value.ToString()); }
        }

        private static bool LunarBeam
        {
            get
            {
                var lunarBeam = ConfigFile.ReadValue("Guardian", "LunarBeam").Trim();
                return lunarBeam != "" && Convert.ToBoolean(lunarBeam);
            }
            set { ConfigFile.WriteValue("Guardian", "LunarBeam", value.ToString()); }
        }

        private static bool RageOfTheSleeperDPS
        {
            get
            {
                var rageOfTheSleeperDps = ConfigFile.ReadValue("Guardian", "RageOfTheSleeperDPS").Trim();
                return rageOfTheSleeperDps != "" && Convert.ToBoolean(rageOfTheSleeperDps);
            }
            set { ConfigFile.WriteValue("Guardian", "RageOfTheSleeperDPS", value.ToString()); }
        }

        private static bool RageOfTheSleeperCD
        {
            get
            {
                var rageOfTheSleeperCd = ConfigFile.ReadValue("Guardian", "RageOfTheSleeperCD").Trim();
                return rageOfTheSleeperCd != "" && Convert.ToBoolean(rageOfTheSleeperCd);
            }
            set { ConfigFile.WriteValue("Guardian", "RageOfTheSleeperCD", value.ToString()); }
        }

        private static bool RageOfTheSleeperHealth
        {
            get
            {
                var rageOfTheSleeperHealth = ConfigFile.ReadValue("Guardian", "RageOfTheSleeperHealth").Trim();
                return rageOfTheSleeperHealth != "" && Convert.ToBoolean(rageOfTheSleeperHealth);
            }
            set { ConfigFile.WriteValue("Guardian", "RageOfTheSleeperHealth", value.ToString()); }
        }

        private static bool IncarnationDPS
        {
            get
            {
                var incarnationDps = ConfigFile.ReadValue("Guardian", "IncarnationDPS").Trim();
                return incarnationDps != "" && Convert.ToBoolean(incarnationDps);
            }
            set { ConfigFile.WriteValue("Guardian", "IncarnationDPS", value.ToString()); }
        }

        private static bool PoolRage
        {
            get
            {
                var poolRage = ConfigFile.ReadValue("Guardian", "PoolRage").Trim();
                return poolRage != "" && Convert.ToBoolean(poolRage);
            }
            set { ConfigFile.WriteValue("Guardian", "PoolRage", value.ToString()); }
        }

        private static bool FrenziedRegen
        {
            get
            {
                var frenziedRegen = ConfigFile.ReadValue("Guardian", "FrenziedRegen").Trim();
                return frenziedRegen != "" && Convert.ToBoolean(frenziedRegen);
            }
            set { ConfigFile.WriteValue("Guardian", "FrenziedRegen", value.ToString()); }
        }

        private static bool SaveRageOfTheSleeperKey
        {
            get
            {
                var saveRageOfTheSleeperKey = ConfigFile.ReadValue("Guardian", "SaveRageOfTheSleeperKey").Trim();
                return saveRageOfTheSleeperKey != "" && Convert.ToBoolean(saveRageOfTheSleeperKey);
            }
            set { ConfigFile.WriteValue("Guardian", "SaveRageOfTheSleeperKey", value.ToString()); }
        }

        private static bool UseDPSBurst
        {
            get
            {
                var useDPSBurst = ConfigFile.ReadValue("Guardian", "UseDPSBurst").Trim();
                return useDPSBurst != "" && Convert.ToBoolean(useDPSBurst);
            }
            set { ConfigFile.WriteValue("Guardian", "UseDPSBurst", value.ToString()); }
        }

        private static bool CoolDownUse
        {
            get
            {
                var coolDownUse = ConfigFile.ReadValue("Guardian", "CoolDownUse").Trim();
                return coolDownUse != "" && Convert.ToBoolean(coolDownUse);
            }
            set { ConfigFile.WriteValue("Guardian", "CoolDownUse", value.ToString()); }
        }

        private static bool InterruptOnlyListed
        {
            get
            {
                var interruptOnlyListed = ConfigFile.ReadValue("Guardian", "InterruptOnlyListed").Trim();
                return interruptOnlyListed != "" && Convert.ToBoolean(interruptOnlyListed);
            }
            set { ConfigFile.WriteValue("Guardian", "InterruptOnlyListed", value.ToString()); }
        }

        private static bool UseWildCharge
        {
            get
            {
                var useWildCharge = ConfigFile.ReadValue("Guardian", "UseWildCharge").Trim();
                return useWildCharge != "" && Convert.ToBoolean(useWildCharge);
            }
            set { ConfigFile.WriteValue("Guardian", "UseWildCharge", value.ToString()); }
        }

        private static bool CustomDelay
        {
            get
            {
                var customDelay = ConfigFile.ReadValue("Guardian", "CustomDelay").Trim();
                return customDelay != "" && Convert.ToBoolean(customDelay);
            }
            set { ConfigFile.WriteValue("Guardian", "CustomDelay", value.ToString()); }
        }

        private static bool MitigationSwitch
        {
            get
            {
                var mitigationSwitch = ConfigFile.ReadValue("Guardian", "MitigationSwitch").Trim();
                return mitigationSwitch != "" && Convert.ToBoolean(mitigationSwitch);
            }
            set { ConfigFile.WriteValue("Guardian", "MitigationSwitch", value.ToString()); }
        }

        private static bool DisplayFR
        {
            get
            {
                var displayFR = ConfigFile.ReadValue("Guardian", "DisplayFR").Trim();
                return displayFR != "" && Convert.ToBoolean(displayFR);
            }
            set { ConfigFile.WriteValue("Guardian", "DisplayFR", value.ToString()); }
        }

        private static bool DisplayInterrupt
        {
            get
            {
                var displayInterrupt = ConfigFile.ReadValue("Guardian", "DisplayInterrupt").Trim();
                return displayInterrupt != "" && Convert.ToBoolean(displayInterrupt);
            }
            set { ConfigFile.WriteValue("Guardian", "DisplayInterrupt", value.ToString()); }
        }

        private static bool DisplayMitigation
        {
            get
            {
                var displayMitigation = ConfigFile.ReadValue("Guardian", "DisplayMitigation").Trim();
                return displayMitigation != "" && Convert.ToBoolean(displayMitigation);
            }
            set { ConfigFile.WriteValue("Guardian", "DisplayMitigation", value.ToString()); }
        }

        private static bool DisplayDPS
        {
            get
            {
                var displayDPS = ConfigFile.ReadValue("Guardian", "DisplayDPS").Trim();
                return displayDPS != "" && Convert.ToBoolean(displayDPS);
            }
            set { ConfigFile.WriteValue("Guardian", "DisplayDPS", value.ToString()); }
        }

        private static bool DisplayCD
        {
            get
            {
                var displayCD = ConfigFile.ReadValue("Guardian", "DisplayCD").Trim();
                return displayCD != "" && Convert.ToBoolean(displayCD);
            }
            set { ConfigFile.WriteValue("Guardian", "DisplayCD", value.ToString()); }
        }

        private static string CoolDownKey
        {
            get
            {
                var coolDownKey = ConfigFile.ReadValue("Guardian", "CoolDownKey");
                try
                {
                    return coolDownKey;
                }
                catch (FormatException)
                {
                    return "None";
                }
            }
            set { ConfigFile.WriteValue("Guardian", "CoolDownKey", value); }
        }

        private static string CoolDownMod
        {
            get
            {
                var coolDownMod = ConfigFile.ReadValue("Guardian", "CoolDownMod");
                try
                {
                    return coolDownMod;
                }
                catch (FormatException)
                {
                    return "None";
                }
            }
            set { ConfigFile.WriteValue("Guardian", "CoolDownMod", value); }
        }

        private static string DPSBurstKey
        {
            get
            {
                var dPSBurstKey = ConfigFile.ReadValue("Guardian", "DPSBurstKey");
                try
                {
                    return dPSBurstKey;
                }
                catch (FormatException)
                {
                    return "None";
                }
            }
            set { ConfigFile.WriteValue("Guardian", "DPSBurstKey", value); }
        }

        private static string DPSBurstMod
        {
            get
            {
                var dPSBurstMod = ConfigFile.ReadValue("Guardian", "DPSBurstMod");
                try
                {
                    return dPSBurstMod;
                }
                catch (FormatException)
                {
                    return "None";
                }
            }
            set { ConfigFile.WriteValue("Guardian", "DPSBurstMod", value); }
        }

        private static string MitigationKey
        {
            get
            {
                var mitigationKey = ConfigFile.ReadValue("Guardian", "MitigationKey");
                try
                {
                    return mitigationKey;
                }
                catch (FormatException)
                {
                    return "None";
                }
            }
            set { ConfigFile.WriteValue("Guardian", "MitigationKey", value); }
        }

        private static string MitigationMod
        {
            get
            {
                var mitigationMod = ConfigFile.ReadValue("Guardian", "MitigationMod");
                try
                {
                    return mitigationMod;
                }
                catch (FormatException)
                {
                    return "None";
                }
            }
            set { ConfigFile.WriteValue("Guardian", "MitigationMod", value); }
        }

        private static int CoolKey
        {
            get
            {
                var coolKey = ConfigFile.ReadValue("Guardian", "CoolKey");
                try
                {
                    return Convert.ToInt32(coolKey);
                }
                catch (FormatException)
                {
                    return 0;
                }
            }
            set { ConfigFile.WriteValue("Guardian", "CoolKey", value.ToString()); }
        }

        private static int CoolMod
        {
            get
            {
                var coolMod = ConfigFile.ReadValue("Guardian", "CoolMod");
                try
                {
                    return Convert.ToInt32(coolMod);
                }
                catch (FormatException)
                {
                    return 0;
                }
            }
            set { ConfigFile.WriteValue("Guardian", "CoolMod", value.ToString()); }
        }

        private static int DPSKey
        {
            get
            {
                var dPSKey = ConfigFile.ReadValue("Guardian", "DPSKey");
                try
                {
                    return Convert.ToInt32(dPSKey);
                }
                catch (FormatException)
                {
                    return 0;
                }
            }
            set { ConfigFile.WriteValue("Guardian", "DPSKey", value.ToString()); }
        }

        private static int DPSMod
        {
            get
            {
                var dPSMod = ConfigFile.ReadValue("Guardian", "DPSMod");
                try
                {
                    return Convert.ToInt32(dPSMod);
                }
                catch (FormatException)
                {
                    return 0;
                }
            }
            set { ConfigFile.WriteValue("Guardian", "DPSMod", value.ToString()); }
        }

        private static int MitKey
        {
            get
            {
                var mitKey = ConfigFile.ReadValue("Guardian", "MitKey");
                try
                {
                    return Convert.ToInt32(mitKey);
                }
                catch (FormatException)
                {
                    return 0;
                }
            }
            set { ConfigFile.WriteValue("Guardian", "MitKey", value.ToString()); }
        }

        private static int MitMod
        {
            get
            {
                var mitMod = ConfigFile.ReadValue("Guardian", "MitMod");
                try
                {
                    return Convert.ToInt32(mitMod);
                }
                catch (FormatException)
                {
                    return 0;
                }
            }
            set { ConfigFile.WriteValue("Guardian", "MitMod", value.ToString()); }
        }

        private static int HPROC
        {
            get
            {
                var HPROC = ConfigFile.ReadValue("Guardian", "HPROC");
                try
                {
                    return Convert.ToInt32(HPROC);
                }
                catch (FormatException)
                {
                    return 10;
                }
            }
            set { ConfigFile.WriteValue("Guardian", "HPROC", value.ToString()); }
        }

        private static int TabAOEDelay
        {
            get
            {
                var tabAOEDelay = ConfigFile.ReadValue("Guardian", "TabAOEDelay");
                try
                {
                    return Convert.ToInt32(tabAOEDelay);
                }
                catch (FormatException)
                {
                    return 1000;
                }
            }
            set { ConfigFile.WriteValue("Guardian", "TabAOEDelay", value.ToString()); }
        }

        private static int LunarBeamHealth
        {
            get
            {
                var lunarBeamHealth = ConfigFile.ReadValue("Guardian", "LunarBeamHealth");
                try
                {
                    return Convert.ToInt32(lunarBeamHealth);
                }
                catch (FormatException)
                {
                    return 45;
                }
            }
            set { ConfigFile.WriteValue("Guardian", "LunarBeamHealth", value.ToString()); }
        }

        private static int BarkskinHealth
        {
            get
            {
                var barkskinHealth = ConfigFile.ReadValue("Guardian", "BarkskinHealth");
                try
                {
                    return Convert.ToInt32(barkskinHealth);
                }
                catch (FormatException)
                {
                    return 50;
                }
            }
            set { ConfigFile.WriteValue("Guardian", "BarkskinHealth", value.ToString()); }
        }

        private static int SurvivalInstinctsHealth
        {
            get
            {
                var survivalInstinctsHealth = ConfigFile.ReadValue("Guardian", "SurvivalInstinctsHealth");
                try
                {
                    return Convert.ToInt32(survivalInstinctsHealth);
                }
                catch (FormatException)
                {
                    return 20;
                }
            }
            set { ConfigFile.WriteValue("Guardian", "SurvivalInstinctsHealth", value.ToString()); }
        }

        private static int RageOfTheSleeperHealthPercentage
        {
            get
            {
                var rageOfTheSleeperHealthPercentage = ConfigFile.ReadValue("Guardian", "RageOfTheSleeperHealthPercentage");
                try
                {
                    return Convert.ToInt32(rageOfTheSleeperHealthPercentage);
                }
                catch (FormatException)
                {
                    return 35;
                }
            }
            set { ConfigFile.WriteValue("Guardian", "RageOfTheSleeperHealthPercentage", value.ToString()); }
        }

        private static int InterruptDelay
        {
            get
            {
                var interruptDelay = ConfigFile.ReadValue("Guardian", "InterruptDelay");
                try
                {
                    if (Convert.ToInt32(interruptDelay) > 50)
                        return Convert.ToInt32(interruptDelay);
                    return 50;
                }
                catch (FormatException)
                {
                    return 50;
                }
            }
            set { ConfigFile.WriteValue("Guardian", "InterruptDelay", value.ToString()); }
        }

        private static int TabMacro
        {
            get
            {
                var tabMacro = ConfigFile.ReadValue("Guardian", "TabMacro");
                try
                {
                    if (Convert.ToInt32(tabMacro) == 0)
                        return 0;
                    return 1;
                }
                catch (FormatException)
                {
                    return 1;
                }
            }
            set { ConfigFile.WriteValue("Guardian", "TabMacro", value.ToString()); }
        }

        private static int Prioritize
        {
            get
            {
                var prioritize = ConfigFile.ReadValue("Guardian", "Prioritize");
                try
                {
                    if (Convert.ToInt32(prioritize) == 0)
                        return 0;
                    return 1;
                }
                catch (FormatException)
                {
                    return 1;
                }
            }
            set { ConfigFile.WriteValue("Guardian", "Prioritize", value.ToString()); }
        }

        private static int SaveRageOfTheSleeper
        {
            get
            {
                var saveRageOfTheSleeper = ConfigFile.ReadValue("Guardian", "SaveRageOfTheSleeper").Trim();
                try
                {
                    if (Convert.ToInt32(saveRageOfTheSleeper) == 0)
                        return 0;
                    return 1;
                }
                catch (FormatException)
                {
                    return 1;
                }
            }
            set { ConfigFile.WriteValue("Guardian", "SaveRageOfTheSleeper", value.ToString()); }
        }

        private static int WildCharge
        {
            get
            {
                var wildCharge = ConfigFile.ReadValue("Guardian", "WildCharge").Trim();
                try
                {
                    if (Convert.ToInt32(wildCharge) == 0)
                        return 0;
                    return 1;
                }
                catch (FormatException)
                {
                    return 0;
                }
            }
            set { ConfigFile.WriteValue("Guardian", "WildCharge", value.ToString()); }
        }

        // End of config saving code	

        // Global methods for CombatRoutine

        private void InterruptibleSpells_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var lstsetinterruptibleSpells = new List<int>(InterruptibleSpells);
            var spellsetString = string.Join(",", lstsetinterruptibleSpells);
            ConfigFile.WriteValue("Guardian", "InterruptibleSpells", spellsetString);
            Log.Write("saved");
        }

        public override void Stop()
        {
        }

        // End of global methods

        // Start of Initialize method
        // Building the settings dialog and printing advice in PM

        public override void Initialize()
        {
            Log.Write("Guardian Rotation v1.0 by Inhade", Color.Green);
            Log.Write("Supports all talent choices. However, recommended talents are 2131321 and 2331321", Color.Green);
            Log.Write("Best talent and option choices:", Color.Black);
            Log.Write("Resto affinity and Galactic Guardian", Color.Black);
            Log.Write("Barkskin and Survival either manual or on CD key", Color.Black);
            Log.Write("All else is situational, but feel free to play around regardless", Color.Black);
            Log.Write("Note that CDs and utility abilities are best triggered by the user, as they tend to be reactive", Color.Green);
            Log.Write("Adjust how aggresive healing logic you want to be by changing HP Rate of change treshold in options", Color.Red);
            Log.Write("A lower threshold triggers Frenzied Regen more easily and hence more often", Color.Red);
            Log.Write("This also modulates how easily other CDs will be cast based on if you are taking damage or not", Color.Green);
            Log.Write("as well as the fail-safe to switch to bear if you are bearcatting", Color.Green);
            Log.Write("You can also adjust the delay before the rotation interupts current target", Color.Red);
            Log.Write("Add your own spell id's to be interrupted in addition to existing PvP ones", Color.LimeGreen);
            Log.Write("The rotation incorporates the function to automatically change targets in AOE", Color.Red);
            Log.Write("Tick TabAOE if you want to use it in AOE rotation", Color.Red);
            Log.Write("You are encouraged to create a /targetenemy [nodead] macro and bind it to the TabMacro spell in PM Spellbook", Color.Red);
            Log.Write("AOE rotation focuses on picking up and holding aggro both in utility and rotation priorities", Color.Green);
            Log.Write("This causes a dps loss, so if you want to DPS efficiently in AOE choose the Cleave rotation", Color.Green);
            Log.Write("You can choose to bearcat if you have the feral affinity for a 30% dps increase", Color.Red);
            Log.Write("Always be careful not to be taking damage as it can be lethal if you are in cat form", Color.Green);
            Log.Write("This talent is highly situational, always go with Resto affinity if you are unsure", Color.Green);
            Log.Write("Use keybinds for defensive CDs for DPS burst", Color.Red);
            Log.Write("If you chose to bearcat, DPS Burst will work as the trigger to start the bearcatting rotation", Color.Green);
            Log.Write("Each CD keypress will first use both your Survival Insticts, then Rage if you chose to, then Barkskin", Color.Green);
            Log.Write("Enabling the overlay will display information on top of WoW", Color.Red);
            Log.Write("Choose what you want to be shown ticking the respective boxes", Color.Green);
            Log.Write("You can also change to prioritize magic or physical mitigation using a keybind", Color.Green);
            Log.Write("Feel free to ask anything in #druid and tag me!", Color.Red);
            Log.Write("Guardian Rotation by Inhade");

            SettingsForm = new Form();

            TabCtrl = new TabControl();
            TalentsArtifact = new TabPage();
            BristlingFurBox = new CheckBox();
            RendAndTearBox = new CheckBox();
            SoulOfTheForestBox = new CheckBox();
            PulverizeBox = new CheckBox();
            Talents = new GroupBox();
            IncarnationBox = new CheckBox();
            Artifact = new GroupBox();
            RageOfTheSleeperBox = new CheckBox();
            Defensives = new TabPage();
            RageOfTheSleeperHealthBox = new CheckBox();
            SurvivalInstinctsBox = new CheckBox();
            BarkskinBox = new CheckBox();
            DefensiveCooldowns = new GroupBox();
            RageOfTheSleeperHPNum = new NumericUpDown();
            SurvivalInstinctsHPNum = new NumericUpDown();
            BarkskinHPNum = new NumericUpDown();
            ActiveMitigation = new GroupBox();
            PoolRageBox = new CheckBox();
            PrioritizeText = new Label();
            PrioritizeCombo = new ComboBox();
            Healing = new TabPage();
            FrenziedRegenBox = new CheckBox();
            LunarBeamNum = new NumericUpDown();
            LunarBeamBox = new CheckBox();
            TalentsHealing = new GroupBox();
            Regeneration = new GroupBox();
            HPROCValue = new Label();
            HPROCLabel = new Label();
            HPROCTrackBar = new TrackBar();
            KeyBindings = new TabPage();
            SaveAbilities = new GroupBox();
            SaveRageOfTheSleeperCombo = new ComboBox();
            IncarnationDPSBox = new CheckBox();
            SaveRageOfTheSleeperKeyBox = new CheckBox();
            KeyPressBindings = new GroupBox();
            DPSBurstKeyCombo = new ComboBox();
            DPSBurstModCombo = new ComboBox();
            CoolDownKeyCombo = new ComboBox();
            CoolDownModCombo = new ComboBox();
            UseDPSBurstBox = new CheckBox();
            CoolDownUseBox = new CheckBox();
            InterruptingTab = new TabPage();
            Interrupting = new GroupBox();
            InterruptDelayValue = new Label();
            RemoveButton = new Button();
            AddButton = new Button();
            AddSpellBox = new TextBox();
            SpellListBox = new ListBox();
            InterruptOnlyListedBox = new CheckBox();
            InterruptDelayTrackBar = new TrackBar();
            SkullBashInterruptBox = new CheckBox();
            CustomDelayBox = new CheckBox();
            Miscellaneous = new TabPage();
            TabMacroCombo = new ComboBox();
            usetab = new Label();
            BearcattingBox = new CheckBox();
            ProwlOOCBox = new CheckBox();
            ChainPullingBox = new CheckBox();
            IncapacitatingBox = new CheckBox();
            TabAOEDelayNum = new NumericUpDown();
            TabAOEBox = new CheckBox();
            WildChargeCombo = new ComboBox();
            UseWildChargeBox = new CheckBox();
            MitigationModCombo = new ComboBox();
            MitigationKeyCombo = new ComboBox();
            MitigationSwitchBox = new CheckBox();
            DisplayInfo = new Button();
            Overlay = new GroupBox();
            DisplayCDBox = new CheckBox();
            DisplayDPSBox = new CheckBox();
            DisplayMitigationBox = new CheckBox();
            DisplayInterruptBox = new CheckBox();
            DisplayFRBox = new CheckBox();
            cmdSave = new Button();

            SettingsForm.Text = "Guardian Druid by Inhade - Rotation Settings";
            SettingsForm.AutoScaleDimensions = new SizeF(6F, 13F);
            SettingsForm.AutoScaleMode = AutoScaleMode.Font;
            SettingsForm.ClientSize = new Size(455, 349);
            SettingsForm.Controls.Add(cmdSave);
            SettingsForm.Controls.Add(TabCtrl);
            SettingsForm.Name = "SettingsForm";
            TabCtrl.ResumeLayout(false);
            TalentsArtifact.ResumeLayout(false);
            TalentsArtifact.PerformLayout();
            Talents.ResumeLayout(false);
            Talents.PerformLayout();
            Artifact.ResumeLayout(false);
            Artifact.PerformLayout();
            Defensives.ResumeLayout(false);
            Defensives.PerformLayout();
            DefensiveCooldowns.ResumeLayout(false);
            ((ISupportInitialize) RageOfTheSleeperHPNum).EndInit();
            ((ISupportInitialize) SurvivalInstinctsHPNum).EndInit();
            ((ISupportInitialize) BarkskinHPNum).EndInit();
            ActiveMitigation.ResumeLayout(false);
            ActiveMitigation.PerformLayout();
            Healing.ResumeLayout(false);
            Healing.PerformLayout();
            ((ISupportInitialize) LunarBeamNum).EndInit();
            Regeneration.ResumeLayout(false);
            Regeneration.PerformLayout();
            ((ISupportInitialize) HPROCTrackBar).EndInit();
            KeyBindings.ResumeLayout(false);
            SaveAbilities.ResumeLayout(false);
            SaveAbilities.PerformLayout();
            KeyPressBindings.ResumeLayout(false);
            KeyPressBindings.PerformLayout();
            InterruptingTab.ResumeLayout(false);
            Interrupting.ResumeLayout(false);
            Interrupting.PerformLayout();
            ((ISupportInitialize) InterruptDelayTrackBar).EndInit();
            Miscellaneous.ResumeLayout(false);
            Miscellaneous.PerformLayout();
            ((ISupportInitialize) TabAOEDelayNum).EndInit();
            SettingsForm.ResumeLayout(false);


            // 
            // TabControl
            //
            TabCtrl.Controls.Add(TalentsArtifact);
            TabCtrl.Controls.Add(Defensives);
            TabCtrl.Controls.Add(Healing);
            TabCtrl.Controls.Add(KeyBindings);
            TabCtrl.Controls.Add(InterruptingTab);
            TabCtrl.Controls.Add(Miscellaneous);
            TabCtrl.Location = new Point(12, 12);
            TabCtrl.Name = "TabControl";
            TabCtrl.SelectedIndex = 0;
            TabCtrl.Size = new Size(431, 293);
            TabCtrl.TabIndex = 0;
            // 
            // TalentsArtifact
            // 
            TalentsArtifact.Controls.Add(BristlingFurBox);
            TalentsArtifact.Controls.Add(RendAndTearBox);
            TalentsArtifact.Controls.Add(SoulOfTheForestBox);
            TalentsArtifact.Controls.Add(PulverizeBox);
            TalentsArtifact.Controls.Add(Talents);
            TalentsArtifact.Controls.Add(Artifact);
            TalentsArtifact.Location = new Point(4, 22);
            TalentsArtifact.Name = "TalentsArtifact";
            TalentsArtifact.Padding = new Padding(3);
            TalentsArtifact.Size = new Size(423, 267);
            TalentsArtifact.TabIndex = 0;
            TalentsArtifact.Text = "Talents & Artifact";
            TalentsArtifact.UseVisualStyleBackColor = true;
            // 
            // BristlingFurBox
            // 
            BristlingFurBox.AutoSize = true;
            BristlingFurBox.Location = new Point(15, 97);
            BristlingFurBox.Name = "BristlingFurBox";
            BristlingFurBox.Size = new Size(80, 17);
            BristlingFurBox.TabIndex = 4;
            BristlingFurBox.Text = "Bristling Fur";
            BristlingFurBox.UseVisualStyleBackColor = true;
            BristlingFurBox.Checked = BristlingFur;
            BristlingFurBox.CheckedChanged += BristlingFur_Click;
            // 
            // RendAndTearBox
            // 
            RendAndTearBox.AutoSize = true;
            RendAndTearBox.Location = new Point(15, 74);
            RendAndTearBox.Name = "RendAndTearBox";
            RendAndTearBox.Size = new Size(98, 17);
            RendAndTearBox.TabIndex = 3;
            RendAndTearBox.Text = "Rend and Tear";
            RendAndTearBox.UseVisualStyleBackColor = true;
            RendAndTearBox.Checked = RendAndTear;
            RendAndTearBox.CheckedChanged += RendAndTear_Click;
            // 
            // SoulOfTheForestBox
            // 
            SoulOfTheForestBox.AutoSize = true;
            SoulOfTheForestBox.Location = new Point(15, 51);
            SoulOfTheForestBox.Name = "SoulOfTheForestBox";
            SoulOfTheForestBox.Size = new Size(109, 17);
            SoulOfTheForestBox.TabIndex = 2;
            SoulOfTheForestBox.Text = "Soul of the Forest";
            SoulOfTheForestBox.UseVisualStyleBackColor = true;
            SoulOfTheForestBox.Checked = SoulOfTheForest;
            SoulOfTheForestBox.CheckedChanged += SoulOfTheForest_Click;
            // 
            // PulverizeBox
            // 
            PulverizeBox.AutoSize = true;
            PulverizeBox.Location = new Point(15, 28);
            PulverizeBox.Name = "PulverizeBox";
            PulverizeBox.Size = new Size(69, 17);
            PulverizeBox.TabIndex = 1;
            PulverizeBox.Text = "Pulverize";
            PulverizeBox.UseVisualStyleBackColor = true;
            PulverizeBox.Checked = Pulverize;
            PulverizeBox.CheckedChanged += Pulverize_Click;
            // 
            // Talents
            // 
            Talents.Controls.Add(IncarnationBox);
            Talents.FlatStyle = FlatStyle.Popup;
            Talents.Location = new Point(6, 6);
            Talents.Name = "Talents";
            Talents.Size = new Size(411, 138);
            Talents.TabIndex = 9;
            Talents.TabStop = false;
            Talents.Text = "Talents";
            // 
            // IncarnationBox
            // 
            IncarnationBox.AutoSize = true;
            IncarnationBox.Location = new Point(9, 114);
            IncarnationBox.Name = "IncarnationBox";
            IncarnationBox.Size = new Size(171, 17);
            IncarnationBox.TabIndex = 7;
            IncarnationBox.Text = "Incarnation: Guardian of Ursoc";
            IncarnationBox.UseVisualStyleBackColor = true;
            IncarnationBox.Checked = Incarnation;
            IncarnationBox.CheckedChanged += Incarnation_Click;
            // 
            // Artifact
            // 
            Artifact.Controls.Add(RageOfTheSleeperBox);
            Artifact.Location = new Point(6, 150);
            Artifact.Name = "Artifact";
            Artifact.Size = new Size(411, 50);
            Artifact.TabIndex = 10;
            Artifact.TabStop = false;
            Artifact.Text = "Artifact Traits";
            // 
            // RageOfTheSleeperBox
            // 
            RageOfTheSleeperBox.AutoSize = true;
            RageOfTheSleeperBox.Location = new Point(9, 19);
            RageOfTheSleeperBox.Name = "RageOfTheSleeperBox";
            RageOfTheSleeperBox.Size = new Size(121, 17);
            RageOfTheSleeperBox.TabIndex = 8;
            RageOfTheSleeperBox.Text = "Rage of the Sleeper";
            RageOfTheSleeperBox.UseVisualStyleBackColor = true;
            RageOfTheSleeperBox.Checked = RageOfTheSleeper;
            RageOfTheSleeperBox.CheckedChanged += RageOfTheSleeper_Click;
            // 
            // Defensives
            // 
            Defensives.Controls.Add(RageOfTheSleeperHealthBox);
            Defensives.Controls.Add(SurvivalInstinctsBox);
            Defensives.Controls.Add(BarkskinBox);
            Defensives.Controls.Add(DefensiveCooldowns);
            Defensives.Controls.Add(ActiveMitigation);
            Defensives.Location = new Point(4, 22);
            Defensives.Name = "Defensives";
            Defensives.Padding = new Padding(3);
            Defensives.Size = new Size(423, 267);
            Defensives.TabIndex = 1;
            Defensives.Text = "Defensives";
            Defensives.UseVisualStyleBackColor = true;
            // 
            // RageOfTheSleeperHealthBox
            // 
            RageOfTheSleeperHealthBox.AutoSize = true;
            RageOfTheSleeperHealthBox.Location = new Point(15, 51);
            RageOfTheSleeperHealthBox.Name = "RageOfTheSleeperHealthBox";
            RageOfTheSleeperHealthBox.Size = new Size(238, 17);
            RageOfTheSleeperHealthBox.TabIndex = 5;
            RageOfTheSleeperHealthBox.Text = "Use Rage of the Sleeper when low on health";
            RageOfTheSleeperHealthBox.UseVisualStyleBackColor = true;
            RageOfTheSleeperHealthBox.Checked = RageOfTheSleeperHealth;
            RageOfTheSleeperHealthBox.CheckedChanged += RageOfTheSleeperHealth_Click;
            // 
            // SurvivalInstinctsBox
            // 
            SurvivalInstinctsBox.AutoSize = true;
            SurvivalInstinctsBox.Location = new Point(15, 74);
            SurvivalInstinctsBox.Name = "SurvivalInstinctsBox";
            SurvivalInstinctsBox.Size = new Size(106, 17);
            SurvivalInstinctsBox.TabIndex = 1;
            SurvivalInstinctsBox.Text = "Survival Instincts";
            SurvivalInstinctsBox.UseVisualStyleBackColor = true;
            SurvivalInstinctsBox.Checked = SurvivalInstincts;
            SurvivalInstinctsBox.CheckedChanged += SurvivalInstincts_Click;
            // 
            // BarkskinBox
            // 
            BarkskinBox.AutoSize = true;
            BarkskinBox.Location = new Point(15, 28);
            BarkskinBox.Name = "BarkskinBox";
            BarkskinBox.Size = new Size(67, 17);
            BarkskinBox.TabIndex = 0;
            BarkskinBox.Text = "Barkskin";
            BarkskinBox.UseVisualStyleBackColor = true;
            BarkskinBox.Checked = Barkskin;
            BarkskinBox.CheckedChanged += BarkskinBox_CheckedChanged;
            // 
            // DefensiveCooldowns
            // 
            DefensiveCooldowns.Controls.Add(RageOfTheSleeperHPNum);
            DefensiveCooldowns.Controls.Add(SurvivalInstinctsHPNum);
            DefensiveCooldowns.Controls.Add(BarkskinHPNum);
            DefensiveCooldowns.FlatStyle = FlatStyle.Popup;
            DefensiveCooldowns.Location = new Point(6, 6);
            DefensiveCooldowns.Name = "DefensiveCooldowns";
            DefensiveCooldowns.Size = new Size(411, 102);
            DefensiveCooldowns.TabIndex = 10;
            DefensiveCooldowns.TabStop = false;
            DefensiveCooldowns.Text = "Defensive Cooldowns";
            // 
            // RageOfTheSleeperHPNum
            // 
            RageOfTheSleeperHPNum.Location = new Point(267, 42);
            RageOfTheSleeperHPNum.Name = "RageOfTheSleeperHPNum";
            RageOfTheSleeperHPNum.Size = new Size(57, 20);
            RageOfTheSleeperHPNum.TabIndex = 11;
            RageOfTheSleeperHPNum.TextAlign = HorizontalAlignment.Right;
            RageOfTheSleeperHPNum.Value = RageOfTheSleeperHealthPercentage;
            RageOfTheSleeperHPNum.ValueChanged += RageOfTheSleeperHPNum_ValueChanged;
            // 
            // SurvivalInstinctsHPNum
            // 
            SurvivalInstinctsHPNum.Location = new Point(267, 65);
            SurvivalInstinctsHPNum.Name = "SurvivalInstinctsHPNum";
            SurvivalInstinctsHPNum.Size = new Size(57, 20);
            SurvivalInstinctsHPNum.TabIndex = 10;
            SurvivalInstinctsHPNum.TextAlign = HorizontalAlignment.Right;
            SurvivalInstinctsHPNum.Value = SurvivalInstinctsHealth;
            SurvivalInstinctsHPNum.ValueChanged += SurvivalInstinctsHPNum_ValueChanged;
            // 
            // BarkskinHPNum
            // 
            BarkskinHPNum.Location = new Point(267, 19);
            BarkskinHPNum.Name = "BarkskinHPNum";
            BarkskinHPNum.Size = new Size(57, 20);
            BarkskinHPNum.TabIndex = 9;
            BarkskinHPNum.TextAlign = HorizontalAlignment.Right;
            BarkskinHPNum.Value = BarkskinHealth;
            BarkskinHPNum.ValueChanged += BarkskinHPNum_ValueChanged;
            // 
            // ActiveMitigation
            // 
            ActiveMitigation.Controls.Add(PoolRageBox);
            ActiveMitigation.Controls.Add(PrioritizeText);
            ActiveMitigation.Controls.Add(PrioritizeCombo);
            ActiveMitigation.FlatStyle = FlatStyle.Popup;
            ActiveMitigation.Location = new Point(6, 114);
            ActiveMitigation.Name = "ActiveMitigation";
            ActiveMitigation.Size = new Size(411, 102);
            ActiveMitigation.TabIndex = 11;
            ActiveMitigation.TabStop = false;
            ActiveMitigation.Text = "Active Mitigation";
            // 
            // PoolRageBox
            // 
            PoolRageBox.AutoSize = true;
            PoolRageBox.Location = new Point(10, 47);
            PoolRageBox.Name = "PoolRageBox";
            PoolRageBox.Size = new Size(76, 17);
            PoolRageBox.TabIndex = 12;
            PoolRageBox.Text = "Pool Rage";
            PoolRageBox.UseVisualStyleBackColor = true;
            PoolRageBox.Checked = PoolRage;
            PoolRageBox.CheckedChanged += PoolRage_Click;
            // 
            // PrioritizeText
            // 
            PrioritizeText.AutoSize = true;
            PrioritizeText.FlatStyle = FlatStyle.Flat;
            PrioritizeText.Location = new Point(7, 25);
            PrioritizeText.Name = "PrioritizeText";
            PrioritizeText.Size = new Size(46, 13);
            PrioritizeText.TabIndex = 4;
            PrioritizeText.Text = "Prioritize";
            // 
            // PrioritizeCombo
            // 
            PrioritizeCombo.FormattingEnabled = true;
            PrioritizeCombo.Items.AddRange(new object[]
            {
                "Ironfur",
                "Mark of Ursol"
            });
            PrioritizeCombo.Location = new Point(59, 20);
            PrioritizeCombo.Name = "PrioritizeCombo";
            PrioritizeCombo.Size = new Size(121, 21);
            PrioritizeCombo.SelectedIndex = Prioritize;
            PrioritizeCombo.TabIndex = 3;
            PrioritizeCombo.SelectedIndex = Prioritize;
            PrioritizeCombo.SelectedIndexChanged += PrioritizeCombo_SelectedIndexChanged;
            // 
            // Healing
            // 
            Healing.Controls.Add(FrenziedRegenBox);
            Healing.Controls.Add(LunarBeamNum);
            Healing.Controls.Add(LunarBeamBox);
            Healing.Controls.Add(TalentsHealing);
            Healing.Controls.Add(Regeneration);
            Healing.Location = new Point(4, 22);
            Healing.Name = "Healing";
            Healing.Padding = new Padding(3);
            Healing.Size = new Size(423, 267);
            Healing.TabIndex = 2;
            Healing.Text = "Healing";
            Healing.UseVisualStyleBackColor = true;
            // 
            // FrenziedRegenBox
            // 
            FrenziedRegenBox.AutoSize = true;
            FrenziedRegenBox.Location = new Point(15, 84);
            FrenziedRegenBox.Name = "FrenziedRegenBox";
            FrenziedRegenBox.Size = new Size(133, 17);
            FrenziedRegenBox.TabIndex = 12;
            FrenziedRegenBox.Text = "Frenzied Regeneration";
            FrenziedRegenBox.UseVisualStyleBackColor = true;
            FrenziedRegenBox.Checked = FrenziedRegen;
            FrenziedRegenBox.CheckedChanged += FrenziedRegen_Click;
            // 
            // LunarBeamNum
            // 
            LunarBeamNum.Location = new Point(129, 25);
            LunarBeamNum.Name = "LunarBeamNum";
            LunarBeamNum.Size = new Size(57, 20);
            LunarBeamNum.TabIndex = 8;
            LunarBeamNum.TextAlign = HorizontalAlignment.Right;
            LunarBeamNum.Value = LunarBeamHealth;
            LunarBeamNum.ValueChanged += LunarBeamNum_ValueChanged;
            // 
            // LunarBeamBox
            // 
            LunarBeamBox.AutoSize = true;
            LunarBeamBox.Location = new Point(15, 28);
            LunarBeamBox.Name = "LunarBeamBox";
            LunarBeamBox.Size = new Size(83, 17);
            LunarBeamBox.TabIndex = 7;
            LunarBeamBox.Text = "Lunar Beam";
            LunarBeamBox.UseVisualStyleBackColor = true;
            LunarBeamBox.Checked = LunarBeam;
            LunarBeamBox.CheckedChanged += LunarBeam_Click;
            // 
            // TalentsHealing
            // 
            TalentsHealing.FlatStyle = FlatStyle.Popup;
            TalentsHealing.Location = new Point(6, 6);
            TalentsHealing.Name = "TalentsHealing";
            TalentsHealing.Size = new Size(411, 49);
            TalentsHealing.TabIndex = 14;
            TalentsHealing.TabStop = false;
            TalentsHealing.Text = "Talents";
            // 
            // Regeneration
            // 
            Regeneration.Controls.Add(HPROCValue);
            Regeneration.Controls.Add(HPROCLabel);
            Regeneration.Controls.Add(HPROCTrackBar);
            Regeneration.FlatStyle = FlatStyle.Popup;
            Regeneration.Location = new Point(6, 61);
            Regeneration.Name = "Regeneration";
            Regeneration.Size = new Size(411, 99);
            Regeneration.TabIndex = 15;
            Regeneration.TabStop = false;
            Regeneration.Text = "Frenzied Regeneration";
            // 
            // HPROCLabel
            // 
            HPROCLabel.AutoSize = true;
            HPROCLabel.Location = new Point(8, 49);
            HPROCLabel.Name = "HPROCLabel";
            HPROCLabel.Size = new Size(140, 13);
            HPROCLabel.TabIndex = 13;
            HPROCLabel.Text = "HP rate of change threshold";
            // 
            // HPROCTrackBar
            // 
            HPROCTrackBar.LargeChange = 2;
            HPROCTrackBar.Location = new Point(153, 46);
            HPROCTrackBar.Maximum = 20;
            HPROCTrackBar.Name = "HPROCTrackBar";
            HPROCTrackBar.Size = new Size(209, 45);
            HPROCTrackBar.TabIndex = 11;
            HPROCTrackBar.Value = HPROC;
            HPROCTrackBar.Scroll += HPROCTrackBar_Scroll;
            // 
            // KeyBindings
            // 
            KeyBindings.Controls.Add(SaveAbilities);
            KeyBindings.Controls.Add(KeyPressBindings);
            KeyBindings.Location = new Point(4, 22);
            KeyBindings.Name = "KeyBindings";
            KeyBindings.Padding = new Padding(3);
            KeyBindings.Size = new Size(423, 267);
            KeyBindings.TabIndex = 3;
            KeyBindings.Text = "Key Bindings";
            KeyBindings.UseVisualStyleBackColor = true;
            // 
            // SaveAbilities
            // 
            SaveAbilities.Controls.Add(SaveRageOfTheSleeperCombo);
            SaveAbilities.Controls.Add(IncarnationDPSBox);
            SaveAbilities.Controls.Add(SaveRageOfTheSleeperKeyBox);
            SaveAbilities.FlatStyle = FlatStyle.Popup;
            SaveAbilities.Location = new Point(6, 117);
            SaveAbilities.Name = "SaveAbilities";
            SaveAbilities.Size = new Size(411, 75);
            SaveAbilities.TabIndex = 16;
            SaveAbilities.TabStop = false;
            SaveAbilities.Text = "Abilities to use on keypress";
            // 
            // SaveRageOfTheSleeperCombo
            // 
            SaveRageOfTheSleeperCombo.FormattingEnabled = true;
            SaveRageOfTheSleeperCombo.Location = new Point(175, 18);
            SaveRageOfTheSleeperCombo.Items.AddRange(new object[]
            {
                "CoolDowns",
                "DPS Burst"
            });
            SaveRageOfTheSleeperCombo.Name = "SaveRageOfTheSleeperCombo";
            SaveRageOfTheSleeperCombo.Size = new Size(87, 21);
            SaveRageOfTheSleeperCombo.TabIndex = 2;
            SaveRageOfTheSleeperCombo.SelectedIndex = SaveRageOfTheSleeper;
            SaveRageOfTheSleeperCombo.SelectedIndexChanged += SaveRageOfTheSleeperCombo_SelectedIndexChanged;
            // 
            // IncarnationDPSBox
            // 
            IncarnationDPSBox.AutoSize = true;
            IncarnationDPSBox.Location = new Point(9, 45);
            IncarnationDPSBox.Name = "IncarnationDPSBox";
            IncarnationDPSBox.Size = new Size(174, 17);
            IncarnationDPSBox.TabIndex = 1;
            IncarnationDPSBox.Text = "Save Incarnation for DPS Burst";
            IncarnationDPSBox.UseVisualStyleBackColor = true;
            IncarnationDPSBox.Checked = IncarnationDPS;
            IncarnationDPSBox.CheckedChanged += IncarnationDPS_Click;
            // 
            // SaveRageOfTheSleeperKeyBox
            // 
            SaveRageOfTheSleeperKeyBox.AutoSize = true;
            SaveRageOfTheSleeperKeyBox.Location = new Point(9, 22);
            SaveRageOfTheSleeperKeyBox.Name = "SaveRageOfTheSleeperKeyBox";
            SaveRageOfTheSleeperKeyBox.Size = new Size(164, 17);
            SaveRageOfTheSleeperKeyBox.TabIndex = 0;
            SaveRageOfTheSleeperKeyBox.Text = "Save Rage of the Sleeper for";
            SaveRageOfTheSleeperKeyBox.UseVisualStyleBackColor = true;
            SaveRageOfTheSleeperKeyBox.Checked = SaveRageOfTheSleeperKey;
            SaveRageOfTheSleeperKeyBox.CheckedChanged += SaveRageOfTheSleeperKey_Click;
            // 
            // KeyPressBindings
            // 
            KeyPressBindings.Controls.Add(DPSBurstKeyCombo);
            KeyPressBindings.Controls.Add(DPSBurstModCombo);
            KeyPressBindings.Controls.Add(CoolDownKeyCombo);
            KeyPressBindings.Controls.Add(CoolDownModCombo);
            KeyPressBindings.Controls.Add(UseDPSBurstBox);
            KeyPressBindings.Controls.Add(CoolDownUseBox);
            KeyPressBindings.Controls.Add(MitigationKeyCombo);
            KeyPressBindings.Controls.Add(MitigationModCombo);
            KeyPressBindings.Controls.Add(MitigationSwitchBox);
            KeyPressBindings.FlatStyle = FlatStyle.Popup;
            KeyPressBindings.Location = new Point(6, 6);
            KeyPressBindings.Name = "KeyPressBindings";
            KeyPressBindings.Size = new Size(411, 95);
            KeyPressBindings.TabIndex = 15;
            KeyPressBindings.TabStop = false;
            KeyPressBindings.Text = "Key Bindings";
            // 
            // DPSBurstKeyCombo
            // 
            DPSBurstKeyCombo.FormattingEnabled = true;
            DPSBurstKeyCombo.Location = new Point(210, 41);
            DPSBurstKeyCombo.Name = "DPSBurstKeyCombo";
            DPSBurstKeyCombo.Size = new Size(87, 21);
            DPSBurstKeyCombo.TabIndex = 5;
            DPSBurstKeyCombo.DataSource = Enum.GetValues(typeof(Keys));
            DPSBurstKeyCombo.SelectedIndex = DPSBurstKeyCombo.FindStringExact(DPSBurstKey);
            DPSBurstKeyCombo.SelectedIndexChanged += DPSBurstKeyCombo_SelectedIndexChanged;
            // 
            // DPSBurstModCombo
            // 
            DPSBurstModCombo.FormattingEnabled = true;
            DPSBurstModCombo.Location = new Point(117, 41);
            DPSBurstModCombo.Name = "DPSBurstModCombo";
            DPSBurstModCombo.Size = new Size(87, 21);
            DPSBurstModCombo.TabIndex = 4;
            DPSBurstModCombo.DataSource = Enum.GetValues(typeof(ModKeys));
            DPSBurstModCombo.SelectedIndex = DPSBurstModCombo.FindStringExact(DPSBurstMod);
            DPSBurstModCombo.SelectedIndexChanged += DPSBurstModCombo_SelectedIndexChanged;
            // 
            // CoolDownKeyCombo
            // 
            CoolDownKeyCombo.FormattingEnabled = true;
            CoolDownKeyCombo.Location = new Point(210, 18);
            CoolDownKeyCombo.Name = "CoolDownKeyCombo";
            CoolDownKeyCombo.Size = new Size(87, 21);
            CoolDownKeyCombo.TabIndex = 3;
            CoolDownKeyCombo.DataSource = Enum.GetValues(typeof(Keys));
            CoolDownKeyCombo.SelectedIndex = CoolDownKeyCombo.FindStringExact(CoolDownKey);
            CoolDownKeyCombo.SelectedIndexChanged += CoolDownKeyCombo_SelectedIndexChanged;
            // 
            // CoolDownModCombo
            // 
            CoolDownModCombo.FormattingEnabled = true;
            CoolDownModCombo.Location = new Point(117, 18);
            CoolDownModCombo.Name = "CoolDownModCombo";
            CoolDownModCombo.Size = new Size(87, 21);
            CoolDownModCombo.TabIndex = 2;
            CoolDownModCombo.DataSource = Enum.GetValues(typeof(ModKeys));
            CoolDownModCombo.SelectedIndex = CoolDownModCombo.FindStringExact(CoolDownMod);
            CoolDownModCombo.SelectedIndexChanged += CoolDownModCombo_SelectedIndexChanged;
            // 
            // UseDPSBurstBox
            // 
            UseDPSBurstBox.AutoSize = true;
            UseDPSBurstBox.Location = new Point(9, 45);
            UseDPSBurstBox.Name = "UseDPSBurstBox";
            UseDPSBurstBox.Size = new Size(75, 17);
            UseDPSBurstBox.TabIndex = 1;
            UseDPSBurstBox.Text = "DPS Burst";
            UseDPSBurstBox.UseVisualStyleBackColor = true;
            UseDPSBurstBox.Checked = UseDPSBurst;
            UseDPSBurstBox.CheckedChanged += UseDPSBurst_Click;
            // 
            // CoolDownUseBox
            // 
            CoolDownUseBox.AutoSize = true;
            CoolDownUseBox.Location = new Point(9, 22);
            CoolDownUseBox.Name = "CoolDownUseBox";
            CoolDownUseBox.Size = new Size(102, 17);
            CoolDownUseBox.TabIndex = 0;
            CoolDownUseBox.Text = "Use CoolDowns";
            CoolDownUseBox.UseVisualStyleBackColor = true;
            CoolDownUseBox.Checked = CoolDownUse;
            CoolDownUseBox.CheckedChanged += CoolDownUse_Click;
            // 
            // MitigationKeyCombo
            // 
            MitigationKeyCombo.FormattingEnabled = true;
            MitigationKeyCombo.Location = new Point(210, 64);
            MitigationKeyCombo.Name = "MitigationKeyCombo";
            MitigationKeyCombo.Size = new Size(87, 21);
            MitigationKeyCombo.TabIndex = 8;
            MitigationKeyCombo.DataSource = Enum.GetValues(typeof(Keys));
            MitigationKeyCombo.SelectedIndex = MitigationKeyCombo.FindStringExact(MitigationKey);
            MitigationKeyCombo.SelectedIndexChanged += MitigationKeyCombo_SelectedIndexChanged;
            // 
            // MitigationModCombo
            // 
            MitigationModCombo.FormattingEnabled = true;
            MitigationModCombo.Location = new Point(117, 64);
            MitigationModCombo.Name = "MitigationModCombo";
            MitigationModCombo.Size = new Size(87, 21);
            MitigationModCombo.TabIndex = 7;
            MitigationModCombo.DataSource = Enum.GetValues(typeof(ModKeys));
            MitigationModCombo.SelectedIndex = MitigationModCombo.FindStringExact(MitigationMod);
            MitigationModCombo.SelectedIndexChanged += MitigationModCombo_SelectedIndexChanged;
            // 
            // MitigationSwitchBox
            // 
            MitigationSwitchBox.AutoSize = true;
            MitigationSwitchBox.Location = new Point(9, 67);
            MitigationSwitchBox.Name = "MitigationSwitchBox";
            MitigationSwitchBox.Size = new Size(106, 17);
            MitigationSwitchBox.TabIndex = 6;
            MitigationSwitchBox.Text = "Mitigation Switch";
            MitigationSwitchBox.UseVisualStyleBackColor = true;
            MitigationSwitchBox.Checked = MitigationSwitch;
            MitigationSwitchBox.CheckedChanged += MitigationSwitchBox_CheckedChanged;
            // 
            // InterruptingTab
            // 
            InterruptingTab.Controls.Add(Interrupting);
            InterruptingTab.Location = new Point(4, 22);
            InterruptingTab.Name = "InterruptingTab";
            InterruptingTab.Padding = new Padding(3);
            InterruptingTab.Size = new Size(423, 267);
            InterruptingTab.TabIndex = 4;
            InterruptingTab.Text = "Interrupting";
            InterruptingTab.UseVisualStyleBackColor = true;
            // 
            // Interrupting
            // 
            Interrupting.Controls.Add(InterruptDelayValue);
            Interrupting.Controls.Add(RemoveButton);
            Interrupting.Controls.Add(AddButton);
            Interrupting.Controls.Add(AddSpellBox);
            Interrupting.Controls.Add(SpellListBox);
            Interrupting.Controls.Add(CustomDelayBox);
            Interrupting.Controls.Add(InterruptOnlyListedBox);
            Interrupting.Controls.Add(InterruptDelayTrackBar);
            Interrupting.Controls.Add(SkullBashInterruptBox);
            Interrupting.FlatStyle = FlatStyle.Popup;
            Interrupting.Location = new Point(6, 6);
            Interrupting.Name = "Interrupting";
            Interrupting.Size = new Size(411, 258);
            Interrupting.TabIndex = 10;
            Interrupting.TabStop = false;
            Interrupting.Text = "Interrupting";
            // 
            // RemoveButton
            // 
            RemoveButton.Location = new Point(59, 160);
            RemoveButton.Name = "RemoveButton";
            RemoveButton.Size = new Size(75, 23);
            RemoveButton.TabIndex = 21;
            RemoveButton.Text = "Remove";
            RemoveButton.UseVisualStyleBackColor = true;
            RemoveButton.Click += RemoveButton_Click;
            // 
            // AddButton
            // 
            AddButton.Location = new Point(59, 131);
            AddButton.Name = "AddButton";
            AddButton.Size = new Size(75, 23);
            AddButton.TabIndex = 20;
            AddButton.Text = "Add";
            AddButton.UseVisualStyleBackColor = true;
            AddButton.Click += AddButton_Click;
            // 
            // AddSpellBox
            // 
            AddSpellBox.BorderStyle = BorderStyle.FixedSingle;
            AddSpellBox.Location = new Point(9, 105);
            AddSpellBox.Name = "AddSpellBox";
            AddSpellBox.Size = new Size(125, 20);
            AddSpellBox.TabIndex = 19;
            AddSpellBox.Text = "";
            // 
            // SpellListBox
            // 
            SpellListBox.FormattingEnabled = true;
            SpellListBox.SuspendLayout();
            SpellListBox.DataSource = InterruptibleSpells;
            SpellListBox.ResumeLayout();
            SpellListBox.Location = new Point(156, 105);
            SpellListBox.Name = "SpellListBox";
            SpellListBox.Size = new Size(120, 147);
            SpellListBox.TabIndex = 18;
            // 
            // InterruptOnlyListedBox
            // 
            InterruptOnlyListedBox.AutoSize = true;
            InterruptOnlyListedBox.BackColor = Color.Transparent;
            InterruptOnlyListedBox.Location = new Point(9, 72);
            InterruptOnlyListedBox.Name = "InterruptOnlyListedBox";
            InterruptOnlyListedBox.Size = new Size(143, 17);
            InterruptOnlyListedBox.TabIndex = 17;
            InterruptOnlyListedBox.Text = "Interrupt only listed spells";
            InterruptOnlyListedBox.UseVisualStyleBackColor = false;
            InterruptOnlyListedBox.Checked = InterruptOnlyListed;
            InterruptOnlyListedBox.CheckedChanged += InterruptOnlyListed_Click;
            // 
            // InterruptDelayTrackBar
            // 
            InterruptDelayTrackBar.LargeChange = 50;
            InterruptDelayTrackBar.Location = new Point(156, 46);
            InterruptDelayTrackBar.Maximum = 850;
            InterruptDelayTrackBar.Minimum = 50;
            InterruptDelayTrackBar.Name = "InterruptDelayTrackBar";
            InterruptDelayTrackBar.Size = new Size(209, 45);
            InterruptDelayTrackBar.SmallChange = 20;
            InterruptDelayTrackBar.TabIndex = 15;
            InterruptDelayTrackBar.TickFrequency = 50;
            InterruptDelayTrackBar.Value = InterruptDelay;
            InterruptDelayTrackBar.Scroll += InterruptDelayTrackBar_Scroll;
            // 
            // SkullBashInterruptBox
            // 
            SkullBashInterruptBox.AutoSize = true;
            SkullBashInterruptBox.Location = new Point(9, 22);
            SkullBashInterruptBox.Name = "SkullBashInterruptBox";
            SkullBashInterruptBox.Size = new Size(151, 17);
            SkullBashInterruptBox.TabIndex = 2;
            SkullBashInterruptBox.Text = "Use Skull Bash to interrupt";
            SkullBashInterruptBox.UseVisualStyleBackColor = true;
            SkullBashInterruptBox.Checked = SkullBashInterrupt;
            SkullBashInterruptBox.CheckedChanged += SkullBashInterrupt_Click;
            // 
            // CustomDelayBox
            // 
            CustomDelayBox.AutoSize = true;
            CustomDelayBox.Location = new Point(9, 47);
            CustomDelayBox.Name = "CustomDelayBox";
            CustomDelayBox.Size = new Size(111, 17);
            CustomDelayBox.TabIndex = 22;
            CustomDelayBox.Text = "Custom delay (ms)";
            CustomDelayBox.UseVisualStyleBackColor = true;
            CustomDelayBox.Checked = CustomDelay;
            CustomDelayBox.CheckedChanged += CustomDelayBox_CheckedChanged;
            // 
            // Miscellaneous
            // 
            Miscellaneous.Controls.Add(TabMacroCombo);
            Miscellaneous.Controls.Add(usetab);
            Miscellaneous.Controls.Add(BearcattingBox);
            Miscellaneous.Controls.Add(ProwlOOCBox);
            Miscellaneous.Controls.Add(ChainPullingBox);
            Miscellaneous.Controls.Add(IncapacitatingBox);
            Miscellaneous.Controls.Add(TabAOEDelayNum);
            Miscellaneous.Controls.Add(TabAOEBox);
            Miscellaneous.Controls.Add(WildChargeCombo);
            Miscellaneous.Controls.Add(UseWildChargeBox);
            Miscellaneous.Controls.Add(Overlay);
            Miscellaneous.Location = new Point(4, 22);
            Miscellaneous.Name = "Miscellaneous";
            Miscellaneous.Padding = new Padding(3);
            Miscellaneous.Size = new Size(423, 267);
            Miscellaneous.TabIndex = 5;
            Miscellaneous.Text = "Misc";
            Miscellaneous.UseVisualStyleBackColor = true;
            // 
            // TabMacroCombo
            // 
            TabMacroCombo.FormattingEnabled = true;
            TabMacroCombo.Items.AddRange(new object[]
            {
                "a macro",
                "PM key sends"
            });
            TabMacroCombo.Location = new Point(279, 49);
            TabMacroCombo.Name = "TabMacroCombo";
            TabMacroCombo.Size = new Size(113, 21);
            TabMacroCombo.TabIndex = 15;
            TabMacroCombo.SelectedIndex = TabMacro;
            TabMacroCombo.SelectedIndexChanged += TabMacroCombo_SelectedIndexChanged;
            // 
            // usetab
            // 
            usetab.AutoSize = true;
            usetab.Location = new Point(229, 52);
            usetab.Name = "usetab";
            usetab.Size = new Size(51, 13);
            usetab.TabIndex = 14;
            usetab.Text = "ms, using";
            // 
            // BearcattingBox
            // 
            BearcattingBox.AutoSize = true;
            BearcattingBox.Location = new Point(15, 143);
            BearcattingBox.Name = "BearcattingBox";
            BearcattingBox.Size = new Size(80, 17);
            BearcattingBox.TabIndex = 13;
            BearcattingBox.Text = "Bearcatting";
            BearcattingBox.UseMnemonic = false;
            BearcattingBox.UseVisualStyleBackColor = true;
            BearcattingBox.Checked = Bearcatting;
            BearcattingBox.CheckedChanged += Bearcatting_Click;
            // 
            // ProwlOOCBox
            // 
            ProwlOOCBox.AutoSize = true;
            ProwlOOCBox.Location = new Point(15, 120);
            ProwlOOCBox.Name = "ProwlOOCBox";
            ProwlOOCBox.Size = new Size(149, 17);
            ProwlOOCBox.TabIndex = 12;
            ProwlOOCBox.Text = "Prowl when out of combat";
            ProwlOOCBox.UseMnemonic = false;
            ProwlOOCBox.UseVisualStyleBackColor = true;
            ProwlOOCBox.Checked = ProwlOOC;
            ProwlOOCBox.CheckedChanged += ProwlOOC_Click;
            // 
            // ChainPullingBox
            // 
            ChainPullingBox.AutoSize = true;
            ChainPullingBox.Location = new Point(15, 97);
            ChainPullingBox.Name = "ChainPullingBox";
            ChainPullingBox.Size = new Size(87, 17);
            ChainPullingBox.TabIndex = 11;
            ChainPullingBox.Text = "Chain Pulling";
            ChainPullingBox.UseVisualStyleBackColor = true;
            ChainPullingBox.Checked = ChainPulling;
            ChainPullingBox.CheckedChanged += ChainPulling_Click;
            // 
            // IncapacitatingBox
            // 
            IncapacitatingBox.AutoSize = true;
            IncapacitatingBox.Location = new Point(15, 74);
            IncapacitatingBox.Name = "IncapacitatingBox";
            IncapacitatingBox.Size = new Size(119, 17);
            IncapacitatingBox.TabIndex = 10;
            IncapacitatingBox.Text = "Incapacitating Roar";
            IncapacitatingBox.UseVisualStyleBackColor = true;
            IncapacitatingBox.Checked = Incapacitating;
            IncapacitatingBox.CheckedChanged += Incapacitating_Click;
            // 
            // TabAOEDelayNum
            // 
            TabAOEDelayNum.Location = new Point(180, 50);
            TabAOEDelayNum.Name = "TabAOEDelayNum";
            TabAOEDelayNum.Size = new Size(49, 20);
            TabAOEDelayNum.TabIndex = 9;
            TabAOEDelayNum.TextAlign = HorizontalAlignment.Right;
            TabAOEDelayNum.Increment = 10;
            TabAOEDelayNum.Maximum = 2000;
            TabAOEDelayNum.Minimum = 200;
            TabAOEDelayNum.Value = TabAOEDelay;
            TabAOEDelayNum.ValueChanged += TabAOEDelayNum_ValueChanged;
            // 
            // TabAOEBox
            // 
            TabAOEBox.AutoSize = true;
            TabAOEBox.Location = new Point(15, 51);
            TabAOEBox.Name = "TabAOEBox";
            TabAOEBox.Size = new Size(170, 17);
            TabAOEBox.TabIndex = 5;
            TabAOEBox.Text = "Target switching in AoE every ";
            TabAOEBox.UseVisualStyleBackColor = true;
            TabAOEBox.Checked = TabAOE;
            TabAOEBox.CheckedChanged += TabAOE_Click;
            // 
            // WildChargeCombo
            // 
            WildChargeCombo.FormattingEnabled = true;
            WildChargeCombo.Items.AddRange(new object[]
            {
                "in AoE",
                "in Single Target"
            });
            WildChargeCombo.Location = new Point(127, 26);
            WildChargeCombo.Name = "WildChargeCombo";
            WildChargeCombo.Size = new Size(110, 21);
            WildChargeCombo.TabIndex = 4;
            WildChargeCombo.SelectedIndex = WildCharge;
            WildChargeCombo.SelectedIndexChanged += WildChargeCombo_SelectedIndexChanged;
            // 
            // UseWildChargeBox
            // 
            UseWildChargeBox.AutoSize = true;
            UseWildChargeBox.Location = new Point(15, 28);
            UseWildChargeBox.Name = "UseWildChargeBox";
            UseWildChargeBox.Size = new Size(106, 17);
            UseWildChargeBox.TabIndex = 3;
            UseWildChargeBox.Text = "Use Wild Charge";
            UseWildChargeBox.UseVisualStyleBackColor = true;
            UseWildChargeBox.Checked = UseWildCharge;
            UseWildChargeBox.CheckedChanged += UseWildCharge_Click;
            // 
            // DisplayInfo
            // 
            DisplayInfo.Location = new Point(330, 41);
            DisplayInfo.Name = "DisplayInfo";
            DisplayInfo.Size = new Size(75, 23);
            DisplayInfo.TabIndex = 16;
            DisplayInfo.Text = "Display Info";
            DisplayInfo.UseVisualStyleBackColor = true;
            DisplayInfo.Click += DisplayInfo_Click;
            // 
            // Overlay
            // 
            Overlay.Controls.Add(DisplayFRBox);
            Overlay.Controls.Add(DisplayInterruptBox);
            Overlay.Controls.Add(DisplayMitigationBox);
            Overlay.Controls.Add(DisplayDPSBox);
            Overlay.Controls.Add(DisplayCDBox);
            Overlay.Controls.Add(DisplayInfo);
            Overlay.FlatStyle = FlatStyle.Popup;
            Overlay.Location = new Point(6, 162);
            Overlay.Name = "Overlay";
            Overlay.Size = new Size(411, 99);
            Overlay.TabIndex = 17;
            Overlay.TabStop = false;
            Overlay.Text = "Overlay";
            // 
            // DisplayCDBox
            // 
            DisplayCDBox.AutoSize = true;
            DisplayCDBox.Location = new Point(9, 19);
            DisplayCDBox.Name = "DisplayCDBox";
            DisplayCDBox.Size = new Size(95, 17);
            DisplayCDBox.TabIndex = 18;
            DisplayCDBox.Text = "Cooldown Use";
            DisplayCDBox.UseMnemonic = false;
            DisplayCDBox.UseVisualStyleBackColor = true;
            DisplayCDBox.Checked = DisplayCD;
            DisplayCDBox.CheckedChanged += DisplayCD_Click;
            // 
            // DisplayDPSBox
            // 
            DisplayDPSBox.AutoSize = true;
            DisplayDPSBox.Location = new Point(9, 42);
            DisplayDPSBox.Name = "DisplayDPSBox";
            DisplayDPSBox.Size = new Size(97, 17);
            DisplayDPSBox.TabIndex = 19;
            DisplayDPSBox.Text = "DPS Burst Use";
            DisplayDPSBox.UseMnemonic = false;
            DisplayDPSBox.UseVisualStyleBackColor = true;
            DisplayDPSBox.Checked = DisplayDPS;
            DisplayDPSBox.CheckedChanged += DisplayDPS_Click;
            // 
            // DisplayMitigationBox
            // 
            DisplayMitigationBox.AutoSize = true;
            DisplayMitigationBox.Location = new Point(9, 65);
            DisplayMitigationBox.Name = "DisplayMitigationBox";
            DisplayMitigationBox.Size = new Size(105, 17);
            DisplayMitigationBox.TabIndex = 20;
            DisplayMitigationBox.Text = "Mitigation Priority";
            DisplayMitigationBox.UseMnemonic = false;
            DisplayMitigationBox.UseVisualStyleBackColor = true;
            DisplayMitigationBox.Checked = DisplayMitigation;
            DisplayMitigationBox.CheckedChanged += DisplayMitigation_Click;
            // 
            // DisplayInterruptBox
            // 
            DisplayInterruptBox.AutoSize = true;
            DisplayInterruptBox.Location = new Point(150, 19);
            DisplayInterruptBox.Name = "DisplayInterruptBox";
            DisplayInterruptBox.Size = new Size(79, 17);
            DisplayInterruptBox.TabIndex = 21;
            DisplayInterruptBox.Text = "Interrupting";
            DisplayInterruptBox.UseMnemonic = false;
            DisplayInterruptBox.UseVisualStyleBackColor = true;
            DisplayInterruptBox.Checked = DisplayInterrupt;
            DisplayInterruptBox.CheckedChanged += DisplayInterrupt_Click;
            // 
            // DisplayFRBox
            // 
            DisplayFRBox.AutoSize = true;
            DisplayFRBox.Location = new Point(150, 42);
            DisplayFRBox.Name = "DisplayFRBox";
            DisplayFRBox.Size = new Size(101, 17);
            DisplayFRBox.TabIndex = 22;
            DisplayFRBox.Text = "Frenzied Regen";
            DisplayFRBox.UseMnemonic = false;
            DisplayFRBox.UseVisualStyleBackColor = true;
            DisplayFRBox.Checked = DisplayFR;
            DisplayFRBox.CheckedChanged += DisplayFR_Click;
            // 
            // cmdSave
            // 
            cmdSave.Location = new Point(366, 314);
            cmdSave.Name = "cmdSave";
            cmdSave.Size = new Size(75, 23);
            cmdSave.TabIndex = 1;
            cmdSave.Text = "Save";
            cmdSave.UseVisualStyleBackColor = true;
            cmdSave.Click += CmdSave_Click;
            // 
            // HPROCValue
            // 
            HPROCValue.AutoSize = true;
            HPROCValue.Location = new Point(368, 49);
            HPROCValue.Name = "HPROCValue";
            HPROCValue.Size = new Size(25, 13);
            HPROCValue.TabIndex = 14;
            HPROCValue.Text = "" + HPROC;
            // 
            // InterruptDelayValue
            // 
            InterruptDelayValue.AutoSize = true;
            InterruptDelayValue.Location = new Point(371, 51);
            InterruptDelayValue.Name = "InterruptDelayValue";
            InterruptDelayValue.Size = new Size(25, 13);
            InterruptDelayValue.TabIndex = 22;
            InterruptDelayValue.Text = "" + InterruptDelay;
        }

        private void CmdSave_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Settings saved", "Frozen", MessageBoxButtons.OK, MessageBoxIcon.Information);
            SettingsForm.Close();
            foreach (var value in DisplayInfoForm.Processes)
                Log.Write(Convert.ToString("Connected to WoW with id: " + value.Id));
        }

        protected void AddButton_Click(object sender, EventArgs e)
        {
            InterruptibleSpells.CollectionChanged += InterruptibleSpells_CollectionChanged;
            InterruptibleSpells.Add(Convert.ToInt32(AddSpellBox.Text));
            Log.Write("Update");
            SpellListBox.SuspendLayout();
            SpellListBox.DataSource = null;
            SpellListBox.DataSource = InterruptibleSpells;
            SpellListBox.ResumeLayout();
        }

        protected void RemoveButton_Click(object sender, EventArgs e)
        {
            InterruptibleSpells.CollectionChanged += InterruptibleSpells_CollectionChanged;
            InterruptibleSpells.Remove(Convert.ToInt32(SpellListBox.SelectedItem));
            SpellListBox.SuspendLayout();
            SpellListBox.DataSource = null;
            SpellListBox.DataSource = InterruptibleSpells;
            SpellListBox.ResumeLayout();
        }

        private void InterruptDelayTrackBar_Scroll(object sender, EventArgs e)
        {
            InterruptDelay = InterruptDelayTrackBar.Value;
            InterruptDelayValue.Text = "" + InterruptDelayTrackBar.Value;
        }

        private void HPROCTrackBar_Scroll(object sender, EventArgs e)
        {
            HPROC = HPROCTrackBar.Value;
            HPROCValue.Text = "" + HPROCTrackBar.Value;
        }

        private void DisplayCD_Click(object sender, EventArgs e)
        {
            DisplayCD = DisplayCDBox.Checked;
        }

        private void DisplayDPS_Click(object sender, EventArgs e)
        {
            DisplayDPS = DisplayDPSBox.Checked;
        }

        private void DisplayInterrupt_Click(object sender, EventArgs e)
        {
            DisplayInterrupt = DisplayInterruptBox.Checked;
        }

        private void DisplayMitigation_Click(object sender, EventArgs e)
        {
            DisplayMitigation = DisplayMitigationBox.Checked;
        }

        private void DisplayFR_Click(object sender, EventArgs e)
        {
            DisplayFR = DisplayFRBox.Checked;
        }

        private void Pulverize_Click(object sender, EventArgs e)
        {
            Pulverize = PulverizeBox.Checked;
        }

        private void SkullBashInterrupt_Click(object sender, EventArgs e)
        {
            SkullBashInterrupt = SkullBashInterruptBox.Checked;
        }

        private void SoulOfTheForest_Click(object sender, EventArgs e)
        {
            SoulOfTheForest = SoulOfTheForestBox.Checked;
        }

        private void SurvivalInstincts_Click(object sender, EventArgs e)
        {
            SurvivalInstincts = SurvivalInstinctsBox.Checked;
        }

        public void BarkskinBox_CheckedChanged(object sender, EventArgs e)
        {
            Barkskin = BarkskinBox.Checked;
        }

        private void RageOfTheSleeper_Click(object sender, EventArgs e)
        {
            RageOfTheSleeper = RageOfTheSleeperBox.Checked;
        }

        private void BristlingFur_Click(object sender, EventArgs e)
        {
            BristlingFur = BristlingFurBox.Checked;
        }

        private void RendAndTear_Click(object sender, EventArgs e)
        {
            RendAndTear = RendAndTearBox.Checked;
        }

        private void Incarnation_Click(object sender, EventArgs e)
        {
            Incarnation = IncarnationBox.Checked;
        }

        private void TabAOE_Click(object sender, EventArgs e)
        {
            TabAOE = TabAOEBox.Checked;
        }

        private void Incapacitating_Click(object sender, EventArgs e)
        {
            Incapacitating = IncapacitatingBox.Checked;
        }

        private void ChainPulling_Click(object sender, EventArgs e)
        {
            ChainPulling = ChainPullingBox.Checked;
        }

        private void Bearcatting_Click(object sender, EventArgs e)
        {
            Bearcatting = BearcattingBox.Checked;
        }

        private void ProwlOOC_Click(object sender, EventArgs e)
        {
            ProwlOOC = ProwlOOCBox.Checked;
        }

        private void LunarBeam_Click(object sender, EventArgs e)
        {
            LunarBeam = LunarBeamBox.Checked;
        }

        private void RageOfTheSleeperHealth_Click(object sender, EventArgs e)
        {
            RageOfTheSleeperHealth = RageOfTheSleeperHealthBox.Checked;
        }

        private void IncarnationDPS_Click(object sender, EventArgs e)
        {
            IncarnationDPS = IncarnationDPSBox.Checked;
        }

        private void PoolRage_Click(object sender, EventArgs e)
        {
            PoolRage = PoolRageBox.Checked;
        }

        private void FrenziedRegen_Click(object sender, EventArgs e)
        {
            FrenziedRegen = FrenziedRegenBox.Checked;
        }

        private void SaveRageOfTheSleeperKey_Click(object sender, EventArgs e)
        {
            SaveRageOfTheSleeperKey = SaveRageOfTheSleeperKeyBox.Checked;
        }

        private void UseDPSBurst_Click(object sender, EventArgs e)
        {
            UseDPSBurst = UseDPSBurstBox.Checked;
        }

        private void CoolDownUse_Click(object sender, EventArgs e)
        {
            CoolDownUse = CoolDownUseBox.Checked;
        }

        private void InterruptOnlyListed_Click(object sender, EventArgs e)
        {
            InterruptOnlyListed = InterruptOnlyListedBox.Checked;
        }

        private void UseWildCharge_Click(object sender, EventArgs e)
        {
            UseWildCharge = UseWildChargeBox.Checked;
        }

        public void CustomDelayBox_CheckedChanged(object sender, EventArgs e)
        {
            CustomDelay = CustomDelayBox.Checked;
        }

        public void MitigationSwitchBox_CheckedChanged(object sender, EventArgs e)
        {
            MitigationSwitch = MitigationSwitchBox.Checked;
        }

        private void TabAOEDelayNum_ValueChanged(object sender, EventArgs e)
        {
            TabAOEDelay = (int) TabAOEDelayNum.Value;
        }

        private void LunarBeamNum_ValueChanged(object sender, EventArgs e)
        {
            LunarBeamHealth = (int) LunarBeamNum.Value;
        }

        private void BarkskinHPNum_ValueChanged(object sender, EventArgs e)
        {
            BarkskinHealth = (int) BarkskinHPNum.Value;
        }

        private void RageOfTheSleeperHPNum_ValueChanged(object sender, EventArgs e)
        {
            RageOfTheSleeperHealthPercentage = (int) RageOfTheSleeperHPNum.Value;
        }

        private void SurvivalInstinctsHPNum_ValueChanged(object sender, EventArgs e)
        {
            SurvivalInstinctsHealth = (int) SurvivalInstinctsHPNum.Value;
        }

        private void TabMacroCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabMacro = TabMacroCombo.SelectedIndex;
        }

        private void WildChargeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            WildCharge = WildChargeCombo.SelectedIndex;
        }

        private void SaveRageOfTheSleeperCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveRageOfTheSleeper = SaveRageOfTheSleeperCombo.SelectedIndex;
        }

        private void PrioritizeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Prioritize = PrioritizeCombo.SelectedIndex;
        }

        private void MitigationKeyCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            MitigationKey = Convert.ToString(MitigationKeyCombo.SelectedItem);
            MitKey = (int) Enum.Parse(typeof(Keys), MitigationKey);
        }

        private void MitigationModCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            MitigationMod = Convert.ToString(MitigationModCombo.SelectedItem);
            MitMod = (int) Enum.Parse(typeof(ModKeys), MitigationMod);
        }

        private void DPSBurstKeyCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DPSBurstKey = Convert.ToString(DPSBurstKeyCombo.SelectedItem);
            DPSKey = (int) Enum.Parse(typeof(Keys), DPSBurstKey);
        }

        private void DPSBurstModCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DPSBurstMod = Convert.ToString(DPSBurstModCombo.SelectedItem);
            DPSMod = (int) Enum.Parse(typeof(ModKeys), DPSBurstMod);
        }

        private void CoolDownKeyCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CoolDownKey = Convert.ToString(CoolDownKeyCombo.SelectedItem);
            CoolKey = (int) Enum.Parse(typeof(Keys), CoolDownKey);
        }

        private void CoolDownModCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CoolDownMod = Convert.ToString(CoolDownModCombo.SelectedItem);
            CoolMod = (int) Enum.Parse(typeof(ModKeys), CoolDownMod);
        }

        public void DisplayInfo_Click(object sender, EventArgs e)
        {
        }

        // End of Initialize method

        // Making pulse asynchronous, to accomodate for async tab-aggroing and interrupting

        public override async void Pulse()
        {
            await AsyncPulse();
        }

        // Checks for user selections

        private async Task keypress()
        {
            if ((DPSMod != 0 && DetectKeyPress.GetKeyState(DPSMod) < 0 || DPSMod == 0) && DetectKeyPress.GetKeyState(DPSKey) < 0)
                if (UseDPSBurst && DPSTimer.ElapsedMilliseconds > 1000)
                {
                    DPSBurst = !DPSBurst;
                    Log.Write("DPS Burst " + (DPSBurst ? "Activated" : "Deactivated"), Color.Red);
                    Log.Write("D P S Burst " + (DPSBurst ? "Activated" : "Deactivated"));
                    Thread.Sleep(50);
                    DPSTimer.Restart();
                    if (DisplayDPS)
                    {
                        DisplayText = "DPS Burst " + (DPSBurst ? "Activated" : "Deactivated");
                        OverlayTimer.Restart();
                    }
                }

            if ((CoolMod != 0 && DetectKeyPress.GetKeyState(CoolMod) < 0 || CoolMod == 0) && DetectKeyPress.GetKeyState(CoolKey) < 0)
                if (CoolDownUse)
                {
                    CoolDowns = true;
                    Log.Write("Cooldowns Activated", Color.Red);
                    Log.Write("Cooldowns Activated");
                    Thread.Sleep(50);
                    CoolDownTimer.Restart();
                    if (DisplayCD)
                    {
                        DisplayText = "CoolDown Activated";
                        OverlayTimer.Restart();
                    }
                }

            if ((MitMod != 0 && DetectKeyPress.GetKeyState(MitMod) < 0 || MitMod == 0) && DetectKeyPress.GetKeyState(MitKey) < 0)
                if (MitigationSwitch && MitigationSwitchTimer.ElapsedMilliseconds > 1000)
                {
                    if (Prioritize == 0)
                    {
                        Prioritize = 1;
                        Log.Write("Prioritizing for Magic Damage - Mark of Ursol", Color.Red);
                        Log.Write("Mark");
                        if (DisplayMitigation)
                        {
                            DisplayText = "Magic Mitigation";
                            OverlayTimer.Restart();
                        }
                    }
                    else
                    {
                        Prioritize = 0;
                        Log.Write("Prioritizing for Physical Damage - Ironfur", Color.Red);
                        Log.Write("Ironfur");
                        if (DisplayMitigation)
                        {
                            DisplayText = "Physical Mitigation";
                            OverlayTimer.Restart();
                        }
                    }
                    Thread.Sleep(50);
                    MitigationSwitchTimer.Restart();
                }
        }


        // Start of async task called by Pulse

        private async Task AsyncPulse()
        {
            // Call the keypress method asynchronously
            await keypress();

            // Resets Cooldown use to off if 5 seconds have passed
            // That way it doesn't have to be manually turned off, it's a panic button

            if (CoolDownTimer.ElapsedMilliseconds > 2000)
                CoolDowns = false;

            // Refreshes overlay text

            if (OverlayTimer.ElapsedMilliseconds > 2000)
                DisplayText = "";

            // Calls the methods that gets the averages and spikes of damage taken

            CalculateHPROCLists();

            // Checks if it is the first time user started the rotation
            // and if so, starts the timer that calculates the rate of change in HP
            // and the stopwatch timer for user options

            if (firstrun)
            {
                MitigationSwitchTimer.Start();
                DPSTimer.Start();
                OverlayTimer.Reset();
                OverlayTimer.Start();
                InitTimer();
                firstrun = false;
            }

            // Checks if the asynchronous task is already counting down for an interrupt,
            // and if not goes there to see if an interrupt is needed

            if (interrupting == false)
                await WaitForInterrupt();

            // Prowl when out of combat if selected

            if (!WoW.IsInCombat && ProwlOOC && WoW.CanCast("Prowl") && !WoW.PlayerHasBuff("Prowl"))
            {
                WoW.CastSpell("Prowl");
                return;
            }

            // Checks if there is enough HP loss for a Frenzied Regen to be worth activating
            // according to how low the user has set the threshold to activate it
            // and prioritizes it of over active mitigation if Guardian of Elune is up

            if (WoW.CanCast("Frenzied Regeneration") && FrenziedRegen
                && (HPROC1sAny || HPRateOfChange < -HPROC1sMax * 0.8 && WoW.PlayerHasBuff("Guardian of Elune") && WoW.PlayerHealthPercent <= 85 &&
                    !WoW.PlayerHasBuff("Frenzied Regeneration") && WoW.PlayerHealthPercent <= 80)
                && WoW.PlayerHasBuff("Bear Form") && (!FrenziedTimer.IsRunning || FrenziedTimer.ElapsedMilliseconds > 5000))
            {
                WoW.CastSpell("Frenzied Regeneration");
                FrenziedTimer.Reset();
                FrenziedTimer.Start();
                return;
            }

            // Checks if Guardian of Elune is not up, is about to expire and go unused, or there is no incoming damage
            // and fires up active mitigation with a priority to always keep Ironfur up

            if (WoW.CanCast("Ironfur") && (WoW.Rage >= 45 || PoolRage && WoW.Rage >= 90 ||
                                           WoW.PlayerHasBuff("Gory Fur") && (WoW.Rage >= 25 || PoolRage && WoW.Rage >= 90))
                && (Prioritize == 0 && (WoW.PlayerHasBuff("Mark of Ursol") || !WoW.PlayerHasBuff("Ironfur")) ||
                    Prioritize == 1 && WoW.PlayerHasBuff("Mark of Ursol"))
                && (!WoW.PlayerHasBuff("Guardian of Elune") || WoW.PlayerBuffTimeRemaining("Guardian of Elune") < 5 || HPROC5sMax > -HPROC * 0.2) &&
                WoW.PlayerHasBuff("Bear Form") && !DPSBurst)
            {
                WoW.CastSpell("Ironfur");
                return;
            }

            if (WoW.CanCast("Mark of Ursol") && (WoW.Rage >= 45 || PoolRage && WoW.Rage >= 90 ||
                                                 WoW.PlayerHasBuff("Gory Fur") && (WoW.Rage >= 25 || PoolRage && WoW.Rage >= 90))
                && (Prioritize == 0 && !WoW.PlayerHasBuff("Mark of Ursol") && WoW.PlayerHasBuff("Ironfur") ||
                    Prioritize == 1 && !WoW.PlayerHasBuff("Mark of Ursol"))
                && (!WoW.PlayerHasBuff("Guardian of Elune") || WoW.PlayerBuffTimeRemaining("Guardian of Elune") < 5 || HPROC5sMax > -HPROC * 0.2) &&
                WoW.PlayerHasBuff("Bear Form") && !DPSBurst)
            {
                WoW.CastSpell("Mark of Ursol");
                return;
            }

            // Checks if user has selected to use CDs automatically and uses them if below a certain health percent
            // or CD key has been pressed


            if (WoW.CanCast("Barkskin") &&
                (WoW.PlayerHealthPercent <= BarkskinHealth && Barkskin
                 || CoolDowns && WoW.PlayerSpellCharges("Survival Instincts") == 0 && (WoW.IsSpellOnCooldown("Rage of the Sleeper") || !RageOfTheSleeperCD)
                 && !WoW.PlayerHasBuff("Survival Instincts") && !WoW.PlayerHasBuff("Barkskin") && !WoW.PlayerHasBuff("Rage of the Sleeper")))
            {
                WoW.CastSpell("Barkskin");
                CoolDowns = false;
                return;
            }

            if (WoW.CanCast("Rage of the Sleeper") &&
                (WoW.PlayerHealthPercent <= RageOfTheSleeperHealthPercentage && RageOfTheSleeperHealth ||
                 CoolDowns && WoW.PlayerSpellCharges("Survival Instincts") == 0 && SaveRageOfTheSleeper == 0)
                && !WoW.PlayerHasBuff("Survival Instincts") && !WoW.PlayerHasBuff("Barkskin") && !WoW.PlayerHasBuff("Rage of the Sleeper"))
            {
                WoW.CastSpell("Rage of the Sleeper");
                CoolDowns = false;
                return;
            }

            if (WoW.CanCast("Survival Instincts") && (WoW.PlayerHealthPercent <= SurvivalInstinctsHealth && SurvivalInstincts || CoolDowns) &&
                !WoW.PlayerHasBuff("Survival Instincts")
                && !WoW.PlayerHasBuff("Barkskin") && !WoW.PlayerHasBuff("Rage of the Sleeper"))
            {
                WoW.CastSpell("Survival Instincts");
                CoolDowns = false;
                return;
            }

            //Bearcatting rotation if bearcatting is selected and we are not taking damage

            if (combatRoutine.Type == RotationType.SingleTarget && Bearcatting && HPROC10sMax > -HPROC * 0.25 && (DPSBurst || !UseDPSBurst))
            {
                // Wild Charge if selected to, to the enemy's location

                if (WoW.HasTarget && WoW.TargetIsEnemy && UseWildCharge && WildCharge == 1 && WoW.CanCast("Wild Charge", true, true, true, true) &&
                    WoW.PlayerHasBuff("Cat Form"))
                {
                    WoW.CastSpell("Wild Charge");
                    return;
                }

                if (WoW.IsInCombat && WoW.HasTarget && WoW.TargetIsEnemy)
                {
                    // Get into Cat Form if we are not already into it

                    if (!WoW.PlayerHasBuff("Cat Form") && OpenerDone == false)
                        WoW.CastSpell("Cat Form");

                    // Opener rotation

                    if (OpenerDone == false)
                    {
                        // Stop opener if we run out of energy

                        if (WoW.PlayerHasBuff("Cat Form") && WoW.Power < 20)
                        {
                            OpenerDone = true;
                            return;
                        }

                        // Cast Rake

                        if (WoW.CanCast("Rake") && !WoW.WasLastCasted("Rake") && WoW.PlayerHasBuff("Cat Form") && WoW.Power > 90)
                        {
                            WoW.CastSpell("Rake");
                            return;
                        }

                        // Cast Shred until we run out of energy

                        if (WoW.CanCast("Shred"))
                        {
                            WoW.CastSpell("Shred");
                            return;
                        }
                    }

                    if (OpenerDone)
                    {
                        if (WoW.PlayerHasBuff("Bear Form"))
                        {
                            // If we used both Mangle and Thrash or Rip and/or Rake are about to fall off,
                            // switch to cat form

                            if ((bearstep >= 3 || IncarnationDPS && incarnatestep >= 7) && WoW.CanCast("Cat Form") &&
                                !WoW.PlayerHasBuff("Rage of the Sleeper") && (!WoW.PlayerHasBuff("Incarnation: Guardian of Ursoc") ||
                                                                              WoW.TargetDebuffStacks("Thrash") == 3 &&
                                                                              (!WoW.TargetHasDebuff("Rip") || WoW.TargetDebuffTimeRemaining("Rip") <= 3) ||
                                                                              WoW.TargetDebuffStacks("Thrash") == 3 &&
                                                                              (!WoW.TargetHasDebuff("Rake") || WoW.TargetDebuffTimeRemaining("Rake") <= 3)))
                            {
                                WoW.CastSpell("Cat Form");
                                bearstep = 0;
                                Log.Write("Switching to cat", Color.Red);
                                return;
                            }

                            // Cast Rage of the Sleeper if selected to gain the 25% damage increase from the artifact trait

                            if (WoW.CanCast("Rage of the Sleeper") && SaveRageOfTheSleeper != 1 && !RageOfTheSleeperHealth &&
                                !WoW.PlayerHasBuff("Incarnation: Guardian of Ursoc"))
                            {
                                WoW.CastSpell("Rage of the Sleeper");
                                return;
                            }

                            // Cast Incarnation: Guardian of Ursoc if selected and available

                            if (WoW.CanCast("Incarnation: Guardian of Ursoc") && !WoW.PlayerHasBuff("Rage of the Sleeper"))
                            {
                                WoW.CastSpell("Incarnation: Guardian of Ursoc");
                                return;
                            }

                            // Always keep up Moonfire

                            if (WoW.IsSpellInRange("Moonfire") && WoW.CanCast("Moonfire") &&
                                (WoW.TargetHasDebuff("Moonfire") && WoW.TargetDebuffTimeRemaining("Moonfire") <= 3 || !WoW.TargetHasDebuff("Moonfire")))
                            {
                                WoW.CastSpell("Moonfire");
                                return;
                            }

                            // Cast Maul if we just started as bear

                            if (WoW.CanCast("Maul") && bearstep == 0 && incarnatestep == 0)
                            {
                                WoW.CastSpell("Maul");
                                Thread.Sleep(50);
                                bearstep++;
                                incarnatestep++;
                                return;
                            }

                            // Cast Mangle and Thrash as long as we can cast them
                            // More times, when incarnating

                            if (WoW.CanCast("Mangle") && (bearstep == 1 || incarnatestep == 1 || incarnatestep == 3 || incarnatestep == 5))
                            {
                                WoW.CastSpell("Mangle");
                                Thread.Sleep(50);
                                bearstep++;
                                incarnatestep++;
                                return;
                            }

                            if (WoW.CanCast("Thrash") && (bearstep == 2 || incarnatestep == 2 || incarnatestep == 4 || incarnatestep == 6))
                            {
                                WoW.CastSpell("Thrash");
                                bearstep++;
                                incarnatestep++;
                                return;
                            }
                        }

                        if (WoW.PlayerHasBuff("Cat Form"))
                        {
                            // Switch to bear form if we don't have enough energy

                            if (WoW.Power < 20)
                            {
                                WoW.CastSpell("Bear Form");
                                Log.Write("Switching to bear", Color.Blue);
                                return;
                            }

                            // Use Rake if debuff is not there or is about to fall off

                            if (WoW.CurrentComboPoints < 5 && (!WoW.TargetHasDebuff("Rake") ||
                                                               WoW.TargetHasDebuff("Rake") && WoW.TargetDebuffTimeRemaining("Rake") < 3))
                            {
                                WoW.CastSpell("Rake");
                                Thread.Sleep(50);
                                return;
                            }

                            // Otherwise, use shred to build up combo points

                            if (WoW.CurrentComboPoints < 5)
                            {
                                WoW.CastSpell("Shred");
                                Thread.Sleep(50);
                                return;
                            }

                            // If we have 5 combo points and target is below 25% so that we can refresh our Rip
                            // cast Ferocious Bite

                            if (WoW.CurrentComboPoints == 5 && WoW.TargetHasDebuff("Rip") && WoW.TargetDebuffTimeRemaining("Rip") < 5 &&
                                WoW.TargetHealthPercent < 25)
                            {
                                WoW.CastSpell("Ferocious Bite");
                                Thread.Sleep(50);
                                return;
                            }

                            // Otherwise, cast Rip as a finisher

                            if (WoW.CurrentComboPoints == 5)
                            {
                                WoW.CastSpell("Rip");
                                Thread.Sleep(50);
                                return;
                            }
                        }
                    }
                }
            }

            // Proceeds to main rotation

            switch (combatRoutine.Type)
            {
                // Single target rotation

                case RotationType.SingleTarget:

                    if (WoW.IsInCombat && WoW.HasTarget && WoW.TargetIsEnemy && (!Bearcatting || HPROC10sMax < -HPROC * 0.25 || !DPSBurst))
                    {
                        // Keep Bear Form up

                        if (!WoW.PlayerHasBuff("Bear Form"))
                            WoW.CastSpell("Bear Form");

                        // Wild Charge if selected to, to the enemy's location

                        if (WoW.HasTarget && WoW.TargetIsEnemy && UseWildCharge && WildCharge == 1 && WoW.CanCast("Wild Charge", true, true, true, true))
                        {
                            WoW.CastSpell("Wild Charge");
                            return;
                        }

                        // Always use Moonfire if there is a Galactic Guardian proc

                        if (WoW.IsSpellInRange("Moonfire") && WoW.CanCast("Moonfire") && WoW.PlayerHasBuff("Galactic Guardian"))
                        {
                            WoW.CastSpell("Moonfire");
                            return;
                        }

                        // If target does not have Moonfire running on it, or is about to expire,
                        // refresh it

                        if (WoW.IsSpellInRange("Moonfire") && WoW.CanCast("Moonfire") && WoW.TargetHasDebuff("Moonfire") &&
                            WoW.TargetDebuffTimeRemaining("Moonfire") <= 3 ||
                            !WoW.TargetHasDebuff("Moonfire"))
                        {
                            WoW.CastSpell("Moonfire");
                            return;
                        }

                        // Cast Incarnation of Ursoc if selected and available

                        if (WoW.CanCast("Incarnation: Guardian of Ursoc") && (Incarnation || IncarnationDPS && DPSBurst))
                        {
                            WoW.CastSpell("Incarnation: Guardian of Ursoc");
                            return;
                        }

                        // Cast Bristling Fur if selected, available, and we are taking at least some damage

                        if (WoW.CanCast("Bristling Fur") && BristlingFur && HPROC5sMax < -HPROC * 0.4)
                        {
                            WoW.CastSpell("Bristling Fur");
                            return;
                        }

                        // Cast Lunar Beam if selected, available and we are low on health						

                        if (WoW.CanCast("Lunar Beam") && LunarBeam && WoW.PlayerHealthPercent <= 45)
                        {
                            WoW.CastSpell("Lunar Beam");
                            return;
                        }

                        // Cast Rage of the Sleeper if selected, available and we are taking at least some damage
                        // unless DPSBurst is enabled

                        if (WoW.CanCast("Rage of the Sleeper") &&
                            (DPSBurst && RageOfTheSleeper || !RageOfTheSleeperHealth && !SaveRageOfTheSleeperKey && RageOfTheSleeper &&
                             HPROC5sMax < -HPROC * 0.4))
                        {
                            WoW.CastSpell("Rage of the Sleeper");
                            return;
                        }

                        // Then use Thrash if it is off CD

                        if (WoW.CanCast("Thrash") && IsInMeleeRange())
                        {
                            WoW.CastSpell("Thrash");
                            return;
                        }

                        // Then use Mangle if it is off CD

                        if (WoW.CanCast("Mangle") && IsInMeleeRange())
                        {
                            WoW.CastSpell("Mangle");
                            return;
                        }

                        // If you have Pulverize and selected that the rotation will use it,
                        // cast it if target has 3 stacks of Thrash

                        if (WoW.CanCast("Pulverize") && Pulverize && WoW.TargetDebuffStacks("Thrash") == 3)
                        {
                            WoW.CastSpell("Pulverize");
                            return;
                        }

                        // If you are not taking damage, the rotation will not use active mitigation 
                        // which are the rage spenders, and build up on rage.
                        // If so, use Maul to spend rage

                        if (WoW.CanCast("Maul") && WoW.Rage >= 80)
                        {
                            WoW.CastSpell("Maul");
                            return;
                        }

                        // Swipe as a filler if nothing else to do

                        if (WoW.CanCast("Swipe") && WoW.IsSpellOnCooldown("Mangle") && IsInMeleeRange())
                            WoW.CastSpell("Swipe");
                    }

                    break;

                // AOE rotation, mainly for picking up and keeping aggro

                case RotationType.AOE:

                    if (WoW.IsInCombat && WoW.HasTarget && WoW.TargetIsEnemy)
                    {
                        // Keep Bear Form up

                        if (!WoW.PlayerHasBuff("Bear Form"))
                            WoW.CastSpell("Bear Form");

                        //Taunt if target is not taunted and Growl is available

                        if (WoW.IsSpellInRange("Growl") && WoW.CanCast("Growl") && !WoW.TargetHasDebuff("Intimidated"))
                            WoW.CastSpell("Growl");

                        // Checks if we are not tab-aggroing
                        // and if not, wild charges to its position if we have the talent
                        // and the user chose that the rotation uses it

                        if (WoW.IsSpellInRange("Wild Charge") && WoW.CanCast("Wild Charge") && !TabAOE && UseWildCharge && WildCharge == 0)
                        {
                            WoW.CastSpell("Wild Charge");
                            return;
                        }

                        // Casts Moonfire if target does not have the debuff, is about to expire or we have a Galactic Guardian proc

                        if (WoW.IsSpellInRange("Moonfire") && WoW.CanCast("Moonfire") &&
                            (!WoW.TargetHasDebuff("Moonfire") || WoW.PlayerHasBuff("Galactic Guardian") ||
                             WoW.TargetHasDebuff("Moonfire") && WoW.TargetDebuffTimeRemaining("Moonfire") <= 3))
                        {
                            WoW.CastSpell("Moonfire");
                            return;
                        }

                        // Casts Mighty Bash if we have the talent and the target is not a boss (and hence not CCable)

                        if (WoW.IsSpellInRange("Mighty Bash") && WoW.CanCast("Mighty Bash"))
                        {
                            WoW.CastSpell("Mighty Bash");
                            return;
                        }

                        // Cast Incarnation of Ursoc if selected and available

                        if (WoW.CanCast("Incarnation: Guardian of Ursoc") && (Incarnation || IncarnationDPS && DPSBurst))
                        {
                            WoW.CastSpell("Incarnation: Guardian of Ursoc");
                            return;
                        }

                        // Cast Bristling Fur if selected, available, and we are taking at least some damage

                        if (WoW.CanCast("Bristling Fur") && BristlingFur && HPROC5sMax < -HPROC * 0.4)
                        {
                            WoW.CastSpell("Bristling Fur");
                            return;
                        }

                        // Cast Lunar Beam if selected, available and we are low on health						

                        if (WoW.CanCast("Lunar Beam") && LunarBeam && WoW.PlayerHealthPercent <= 45)
                        {
                            WoW.CastSpell("Lunar Beam");
                            return;
                        }

                        // Cast Rage of the Sleeper if selected, available and we are taking at least some damage
                        // unless DPSBurst is enabled

                        if (WoW.CanCast("Rage of the Sleeper") &&
                            (DPSBurst && RageOfTheSleeperDPS || !RageOfTheSleeperHealth && !SaveRageOfTheSleeperKey && RageOfTheSleeper &&
                             HPROC5sMax < -HPROC * 0.4))
                        {
                            WoW.CastSpell("Rage of the Sleeper");
                            return;
                        }

                        // Cast Thrash if it is off CD

                        if (WoW.CanCast("Thrash") && IsInMeleeRange())
                        {
                            WoW.CastSpell("Thrash");
                            return;
                        }

                        // Only cast Mangle if we have Incarnation up as it is single target otherwise.
                        // This is a dps loss, but prioritizes Swipe to pick up and keep aggro

                        if (WoW.IsSpellInRange("Mangle") && WoW.CanCast("Mangle") && Incarnation && WoW.PlayerHasBuff("Incarnation: Guardian of Ursoc"))
                        {
                            WoW.CastSpell("Mangle");
                            return;
                        }


                        // Uses Incapacitating Roar if available and selected.
                        // If we are chain pulling, it will use it at 30% to pull the next pack
                        // (hint: If we are chain pulling and Incapacitating Roar is casted, move on to the next pack!)

                        if (WoW.CanCast("Incapacitating Roar") && Incapacitating && (ChainPulling && WoW.TargetHealthPercent <= 30 || !ChainPulling))
                        {
                            WoW.CastSpell("Incapacitating Roar");
                            return;
                        }

                        // Otherwise, use Swipe

                        if (WoW.CanCast("Swipe") && IsInMeleeRange())
                            WoW.CastSpell("Swipe");
                    }

                    break;

                case RotationType.Cleave:

                    if (WoW.IsInCombat && WoW.HasTarget && WoW.TargetIsEnemy)
                    {
                        // Keep Bear Form up

                        if (!WoW.PlayerHasBuff("Bear Form"))
                            WoW.CastSpell("Bear Form");

                        // Always use Moonfire if there is a Galactic Guardian proc

                        if (WoW.IsSpellInRange("Moonfire") && WoW.CanCast("Moonfire") && WoW.PlayerHasBuff("Galactic Guardian"))
                        {
                            WoW.CastSpell("Moonfire");
                            return;
                        }

                        // Keep up Moonfire on main target if not automatically refreshed

                        if (WoW.IsSpellInRange("Moonfire") && WoW.CanCast("Moonfire") && WoW.TargetHasDebuff("Moonfire") &&
                            WoW.TargetDebuffTimeRemaining("Moonfire") <= 3 ||
                            !WoW.TargetHasDebuff("Moonfire"))
                            WoW.CastSpell("Moonfire");

                        // Cast Incarnation of Ursoc if selected and available

                        if (WoW.CanCast("Incarnation: Guardian of Ursoc") && (Incarnation || IncarnationDPS && DPSBurst))
                        {
                            WoW.CastSpell("Incarnation: Guardian of Ursoc");
                            return;
                        }

                        // Cast Bristling Fur if selected, available, and we are taking at least some damage

                        if (WoW.CanCast("Bristling Fur") && BristlingFur && HPROC5sMax < -HPROC * 0.4)
                        {
                            WoW.CastSpell("Bristling Fur");
                            return;
                        }

                        // Cast Lunar Beam if selected, available and we are low on health						

                        if (WoW.CanCast("Lunar Beam") && LunarBeam && WoW.PlayerHealthPercent <= 45)
                        {
                            WoW.CastSpell("Lunar Beam");
                            return;
                        }

                        // Cast Rage of the Sleeper if selected, available and we are taking at least some damage
                        // unless DPSBurst is enabled

                        if (WoW.CanCast("Rage of the Sleeper") &&
                            (DPSBurst && RageOfTheSleeperDPS || !RageOfTheSleeperHealth && !SaveRageOfTheSleeperKey && RageOfTheSleeper &&
                             HPROC5sMax < -HPROC * 0.4))
                        {
                            WoW.CastSpell("Rage of the Sleeper");
                            return;
                        }

                        // Then use Thrash if it is off CD

                        if (WoW.CanCast("Thrash") && IsInMeleeRange())
                        {
                            WoW.CastSpell("Thrash");
                            return;
                        }

                        // Then use Mangle if it is off CD

                        if (WoW.CanCast("Mangle") && IsInMeleeRange())
                        {
                            WoW.CastSpell("Mangle");
                            return;
                        }

                        // Swipe as a filler if nothing else to do

                        if (WoW.CanCast("Swipe") && IsInMeleeRange())
                            WoW.CastSpell("Swipe");
                    }
                    break;
            }
        }


        // Starts timer to calculate HP loss 	

        private void InitTimer()
        {
            timer = new Timer {Enabled = true};
            timer.Elapsed += timer_Tick;
            timer.Interval = 200;
        }

        // Every tick (200ms) this timer sends the % of HP that was lost in a thread-safe way.
        // This is useful to calculate if a Frenzied Regen would be worth it,
        // based on the choice the user made.
        // In options you can set how aggressively to use Frenzied Regen,
        // with a lower HP % threshold meaning Frenzied Regen will be cast more often

        private void timer_Tick(object sender, ElapsedEventArgs e)
        {
            timer.Enabled = false;
            Interlocked.Increment(ref CurrentHP);
            if (CurrentHP == 0)
            {
                CurrentHP = WoW.PlayerHealthPercent;
            }
            else
            {
                PreviousHP = CurrentHP;
                CurrentHP = WoW.PlayerHealthPercent;
                HPRateOfChange = CurrentHP - PreviousHP;
                Thread.Sleep(200);
            }
            timer.Enabled = true;
        }


        // Calculates HPROC averages and spikes

        private void CalculateHPROCLists()
        {
            // Populates the variables of the last second
            // Max is the maximum change in HP for the given time period
            // Any is a boolean that indicates whether we have taken more damage than the threshold
            // at any point in the last second
            // and Average is the averaged rate of change for the last second

            if (Queue1s.Count > 4)
                Queue1s.Dequeue();
            Queue1s.Enqueue(HPRateOfChange);

            foreach (var a in Queue1s)
            {
                if (a < -HPROC)
                {
                    HPROC1sMax = Convert.ToInt32(a);
                    HPROC1sAny = true;
                    break;
                }
                HPROC1sMax = 0;
                HPROC1sAny = false;
            }

            // Populates the variables of the last 5 seconds
            // simirarly to the previous set

            if (Queue5s.Count > 24)
                Queue5s.Dequeue();
            Queue5s.Enqueue(HPRateOfChange);

            foreach (var c in Queue5s)
            {
                if (c < -HPROC)
                {
                    HPROC5sMax = Convert.ToInt32(c);
                    break;
                }
                HPROC5sMax = 0;
            }


            // Populates the variables of the last 10 seconds
            // simirarly to the previous set

            if (Queue10s.Count > 49)
                Queue10s.Dequeue();
            Queue10s.Enqueue(HPRateOfChange);


            foreach (var e in Queue10s)
            {
                if (e < -HPROC)
                {
                    HPROC10sMax = Convert.ToInt32(e);
                    break;
                }
                HPROC10sMax = 0;
            }
        }

        // Logic for changing target in AOE if user has selected tab-aggroing
        // Will change target untill it finds one in range, every 1 s
        // in a thread-safe and asynchronous way
        // The user is provided in options which way they prefer to change target,
        // either by a /targetenemy macro (more efficient and safe)
        // or by PM sending the keystrokes themselves

        // Checks if the spell currently being cast by the target is in the list of spells to be interrupted
        // Currently the list contains only PVP spells

        private bool Interruptible()
        {
            return InterruptibleSpells.Contains(WoW.TargetCastingSpellID);
        }

        // Asynchronous task that detects when a target begins to cast a spell,
        // returns to the loop and then resumes when the interrupt delay has passed

        private async Task WaitForInterrupt()
        {
            if (WoW.TargetIsCasting && SkullBashInterrupt && WoW.IsSpellInRange("Skull Bash") && WoW.CanCast("Skull Bash") &&
                (Interruptible() || !InterruptOnlyListed))
            {
                interrupting = true;
                DisplayText = "Interrupting..";
                OverlayTimer.Restart();
                if (CustomDelay)
                    await Task.Delay(InterruptDelay);
                else
                    await Task.Delay(500);
                DisplayText = "Interrupting..";
                OverlayTimer.Restart();
                WoW.CastSpell("Skull Bash");
                Log.Write("Interrupting!", Color.Red);
                interrupting = false;
            }
        }


        // Checks if current target is in melee range
        private static bool IsInMeleeRange()
        {
            return WoW.CanCast("Mangle", false, false, true, false, false);
        }

        // List of mod keys
        private enum ModKeys
        {
            None = 0,
            Alt = 0xA4,
            Control = 0xA2
        }
    }

    public class DisplayInfoForm : Form
    {
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;


        public static Process[] processes32 = Process.GetProcessesByName("Wow");
        public static Process[] processes64 = Process.GetProcessesByName("Wow-64");

        private static readonly IntPtr WindowHandle = p.MainWindowHandle;

        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private readonly byte alpha = 255;

        // Variables for the form

        private readonly Label DisplayLabel;
        private Timer OverlayDisplayTimer;
        private readonly RECT rect;

        private DisplayInfoForm()
        {
            DisplayLabel = new Label();
            GetWindowRect(WindowHandle, ref rect);
            InitOverlayTimer();
            SuspendLayout();
            // 
            // DisplayInfoForm
            // 
            SetWindowPos(Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
            Controls.Add(DisplayLabel);
            Location = new Point(rect.Left, rect.Top);
            Size = new Size(rect.Right - rect.Left, rect.Bottom - rect.Top);
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            BackColor = Color.White;
            TransparencyKey = Color.White;
            Name = "DisplayInfoForm";
            Text = "Guardian Druid Info";
            ResumeLayout(false);
            // 
            // DisplayLabel
            // 
            DisplayLabel.AutoSize = false;
            DisplayLabel.Font = new Font("Microsoft Sans Serif", 48F, FontStyle.Regular, GraphicsUnit.Point, 161);
            DisplayLabel.Location = new Point(570, 100);
            DisplayLabel.Name = "DisplayLabel";
            DisplayLabel.Size = new Size(820, 73);
            DisplayLabel.TabIndex = 1;
            DisplayLabel.Text = Guardian.DisplayText;
            DisplayLabel.TextAlign = ContentAlignment.TopCenter;
            WindowState = FormWindowState.Maximized;
        }

        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        public sealed override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        public static Process[] Processes
        {
            get
            {
                var processes = new Process[0];
                Array.Resize(ref processes, processes32.Length + processes64.Length);
                Array.Copy(processes64, processes, processes64.Length);
                Array.Copy(processes32, 0, processes, processes64.Length, processes32.Length);
                return processes;
            }
        }

        public static Process p
        {
            get
            {
                try
                {
                    return Processes[0];
                }
                catch (Exception ex)
                {
                    MessageBox.Show("" + ex, "Frozen", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return null;
                }
            }
        }


        // Dll imports for overlay transparent drawing

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern int User32_GetWindowLong(IntPtr hWnd, GetWindowLong nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int User32_SetWindowLong(IntPtr hWnd, GetWindowLong nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetLayeredWindowAttributes")]
        private static extern bool User32_SetLayeredWindowAttributes(IntPtr hWnd, int crKey, byte bAlpha, LayeredWindowAttributes dwFlags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        // Sets transparency

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            var wl = User32_GetWindowLong(Handle, GetWindowLong.GWL_EXSTYLE);
            User32_SetWindowLong(Handle, GetWindowLong.GWL_EXSTYLE,
                wl | (int) ExtendedWindowStyles.WS_EX_LAYERED | (int) ExtendedWindowStyles.WS_EX_TRANSPARENT);
            User32_SetLayeredWindowAttributes(Handle, (TransparencyKey.B << 16) + (TransparencyKey.G << 8) + TransparencyKey.R, alpha,
                LayeredWindowAttributes.LWA_COLORKEY | LayeredWindowAttributes.LWA_ALPHA);
        }

        // Attempt to draw nicer fonts		

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
            base.OnPaint(e);
        }

        // Update overlay text event

        public void UpdateDisplayLabel(object sender, ElapsedEventArgs e)
        {
            DisplayLabel.SuspendLayout();
            DisplayLabel.Text = Guardian.DisplayText;
            DisplayLabel.ResumeLayout(false);
        }

        // Starts timer to refresh label 	

        private void InitOverlayTimer()
        {
            OverlayDisplayTimer = new Timer {Enabled = true};
            OverlayDisplayTimer.Elapsed += UpdateDisplayLabel;
            OverlayDisplayTimer.Interval = 100;
        }

        // Variables for the overlay

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public readonly int Left;
            public readonly int Top;
            public readonly int Right;
            public readonly int Bottom;
        }

        private enum GetWindowLong
        {
            GWL_EXSTYLE = -20
        }

        private enum ExtendedWindowStyles
        {
            WS_EX_TRANSPARENT = 0x20,
            WS_EX_LAYERED = 0x80000
        }

        [Flags]
        private enum LayeredWindowAttributes
        {
            LWA_COLORKEY = 0x1,
            LWA_ALPHA = 0x2
        }
    }
}

/*
[AddonDetails.db]
AddonAuthor=Inhade
AddonName=Frozen
WoWVersion=Legion - 70300
[SpellBook.db]
Spell,22812,Barkskin,Add
Spell,20484,Rebirth,F1
Spell,197626,Starsurge,F4
Spell,197628,Lunar Strike,F5
Spell,8921,Moonfire,F6
Spell,5176,Solar Wrath,NumLock
Spell,93402,Sunfire,F8
Spell,197625,Moonkin Form,F9
Spell,5215,Prowl,F11
Spell,22842,Frenzied Regeneration,Multiply
Spell,210037,Growl,F12
Spell,33917,Mangle,D4
Spell,77758,Thrash,D6
Spell,192081,Ironfur,F8
Spell,5487,Bear Form,D1
Spell,1822,Rake,Oemtilde
Spell,5221,Shred,OemCloseBrackets
Spell,1079,Rip,OemOpenBrackets
Spell,22568,Ferocious Bite,Oemplus
Spell,768,Cat Form,Subtract
Spell,1,Tier4,PageDown
Spell,80313,Pulverize,D5
Spell,106785,Swipe,F7
Spell,192083,Mark of Ursol,F10
Spell,6807,Maul,D0
Spell,2,TabMacro,D8
Spell,106839,Skull Bash,F5
Spell,61336,Survival Instincts,D7
Spell,102558,Incarnation: Guardian of Ursoc,D9
Spell,99,Incapacitating Roar,Add
Spell,204066,Lunar Beam,F2
Spell,200851,Rage of the Sleeper,NumPad0
Spell,155835,Bristling Fur,NumPad1
Spell,102401,Wild Charge,F2
Aura,22812,Barkskin
Aura,200851,Rage of the Sleeper
Aura,102558,Incarnation: Guardian of Ursoc
Aura,61336,Survival Instincts
Aura,201671,Gory Fur
Aura,155578,Guardian of Elune
Aura,192083,Mark of Ursol
Aura,192081,Ironfur
Aura,77758,Thrash
Aura,164812,Moonfire
Aura,206891,Intimidated
Aura,203964,Galactic Guardian
Aura,5487,Bear Form
Aura,768,Cat Form
Aura,5215,Prowl
Aura,1079,Rip
Aura,155722,Rake
*/