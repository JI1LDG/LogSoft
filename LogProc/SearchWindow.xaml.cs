using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using System.Threading;
using LogProc.Definitions;
using LogProc.Interfaces;

namespace LogProc {
	/// <summary>
	/// SearchWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class SearchWindow : Window {
		private SearchLog sl;
		private Thread th;
		private DispatcherTimer dt;
		public List<StationData> Station {
			get {
				return sl.Station;
			}
			private set {
				Station = value;
			}
		}

		public SearchWindow(WorkingData wd, ISearch isp) {
			InitializeComponent();
			pbCondition.Maximum = wd.Log.Count;
			sl = new SearchLog(wd, isp);
			dt = new DispatcherTimer();
			dt.Interval = new TimeSpan(0, 0, 0, 0, 100);
			dt.Tick += dt_Tick;
			btStop.IsEnabled = true;
			th = new Thread(new ThreadStart(sl.StartSearch));
			th.IsBackground = true;
			th.Start();
			dt.Start();
		}

		void dt_Tick(object sender, EventArgs e) {
			pbCondition.Value = sl.ExecutedNum;
			tbPercentage.Text = ((float)sl.ExecutedNum / (float)sl.Log.Count).ToString("P");
			lbLogNum.Content = sl.Log.Count;
			lbSearchenNum.Content = sl.ExecutedNum;
			lbUnavailenNum.Content = sl.FailedNum;
			lbErrorenNum.Content = sl.FCheckNum;
			DoEvents();
			if(sl.ExecStatus == true) {
				sl.DoAbort();
				if(th != null) th.Abort();
				th = null;
				if(dt != null) dt.Stop();
				dt = null;
				this.Close();
			}
		}

		private void btCancel_Click(object sender, RoutedEventArgs e) {
			sl.DoAbort();
			if(th != null) th.Abort();
			if(dt != null) dt.Stop();
			this.Close();
		}

		private void Window_Closed(object sender, EventArgs e) {
			sl.DoAbort();
			if(th != null) th.Abort();
			if(dt != null) dt.Stop();
		}

		private void DoEvents() {
			DispatcherFrame frame = new DispatcherFrame();
			Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(ExitFrames), frame);
			Dispatcher.PushFrame(frame);
		}

		private object ExitFrames(object frames) {
			((DispatcherFrame)frames).Continue = false;
			return null;
		}
	}
}
