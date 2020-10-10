using Serilog;
using System.Collections.Generic;

namespace WindowsTerminalQuake.Native
{
	public class HotKeyRegistry
	{
		private readonly List<int> _registeredHotKeys = new List<int>();
		private SettingsDto _previousSettings = null;

		public void RegisterHotKeys()
		{
			var s = Settings.Instance;
			if (_previousSettings == s && _registeredHotKeys.Count > 0)
			{
				return;
			}

			UnregisterHotKeys();
			s.Hotkeys.ForEach(hk =>
			{
				Log.Information($"Registering hot key {hk.Modifiers} + {hk.Key}");
				var reg = HotKeyManager.RegisterHotKey(hk.Key, hk.Modifiers);
				_registeredHotKeys.Add(reg);
			});
			_previousSettings = s;
		}

		public void UnregisterHotKeys()
		{
			_registeredHotKeys.ForEach(hk => HotKeyManager.UnregisterHotKey(hk));
			_registeredHotKeys.Clear();
		}
	}
}