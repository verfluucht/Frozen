using System.Diagnostics;
using System.Windows.Forms;
using Frozen.Helpers;

namespace Frozen.Plugins
{
    internal class SamplePlugin : Plugin
    {
        private readonly Stopwatch jumpInterval = new Stopwatch();
        public override string Name => "Jump";

        public override Form SettingsForm { get; set; }

        public override void Initialize()
        {
            Log.Write("Plugin Loaded, Jumping will begin");
            jumpInterval.Start();
        }

        public override void Stop()
        {
            Log.Write("Why you make me stop I like jumping :(");
        }

        public override void Pulse()
        {
            if (jumpInterval.ElapsedMilliseconds >= 5000)
            {
                WoW.KeyPressRelease(WoW.Keys.Space);
                jumpInterval.Restart();
            }
        }
    }
}