using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace WindowsTerminalQuake
{
	public class ActiveWindowManager : IDisposable
	{
		public delegate void OnActiveWindowChange(string title);

		public event OnActiveWindowChange _events;
		private bool _disposed;
		private string _activeWindowTitle;

		public ActiveWindowManager()
		{
			new Thread(Run).Start();
		}

		public void Dispose()
		{
			_disposed = true;
		}

		public void ClearEvents() => _events = null;

		private void Run()
		{
			while (!_disposed)
			{
				var newActiveWindow = GetActiveWindowTitle();
				if (_activeWindowTitle != newActiveWindow)
				{
					_activeWindowTitle = newActiveWindow;
					if (_events != null)
					{
						_events(_activeWindowTitle);
					}
				}

				Thread.Sleep(200);
			}
		}

		[DllImport("user32.dll")]
		static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll")]
		static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

		private string GetActiveWindowTitle()
		{
			const int nChars = 256;
			IntPtr handle = IntPtr.Zero;
			StringBuilder Buff = new StringBuilder(nChars);
			handle = GetForegroundWindow();

			if (GetWindowText(handle, Buff, nChars) > 0)
			{
				return Buff.ToString();
			}

			return null;
		}
	}
}