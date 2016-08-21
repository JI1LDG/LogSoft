using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
		private SearchLog[] sl;
		private Thread[] th;
		private DispatcherTimer dt;
		private WorkingData[] wdx;

		public SearchWindow(WorkingData wd, InterSet[] isp) {
			sl = new SearchLog[isp.Length];
			th = new Thread[isp.Length];
			InitializeComponent();
			DevideLog(wd);
			pbCondition1.Maximum = wdx[0].Log.Count;
			pbCondition2.Maximum = wdx[1].Log.Count;
			pbConditionA.Maximum = wd.Log.Count;
			dt = new DispatcherTimer();
			dt.Interval = new TimeSpan(0, 0, 0, 0, 100);
			dt.Tick += dt_Tick;
			btStop.IsEnabled = true;
			for(int i = 0;i < isp.Length; i++) {
				sl[i] = new SearchLog(wdx[i], isp[i].Sea);
				th[i] = new Thread(new ThreadStart(sl[i].StartSearch));
				th[i].IsBackground = true;
				th[i].Start();
			}
			dt.Start();
		}

		private void DevideLog(WorkingData wd) {
			wdx = new WorkingData[sl.Length];
			int SkipCount = 0;
			for(int i = 0;i < sl.Length; i++) {
				wdx[i] = new WorkingData();
				wdx[i].Config = wd.Config;
				wdx[i].Log = new ObservableCollection<LogData>(wd.Log.Skip(SkipCount).Take(wd.Log.Count / sl.Length));
				SkipCount += wd.Log.Count / sl.Length;
			}
		}

		void dt_Tick(object sender, EventArgs e) {
			pbCondition1.Value = sl[0].ExecutedNum;
			pbCondition2.Value = sl[1].ExecutedNum;
			pbConditionA.Value = sl[0].ExecutedNum + sl[1].ExecutedNum;
			tbPercentage1.Text = "Thread1: " + ((float)sl[0].ExecutedNum / (float)sl[0].Log.Count).ToString("P");
			tbPercentage2.Text = "Thread2: " + ((float)sl[1].ExecutedNum / (float)sl[1].Log.Count).ToString("P");
			tbPercentageA.Text = "All: " + ((float)(sl[0].ExecutedNum + sl[1].ExecutedNum) / (float)(sl[0].Log.Count + sl[1].Log.Count)).ToString("P");
			lbLogNum.Content = sl[0].Log.Count + sl[1].Log.Count;
			lbSearchenNum.Content = sl[0].ExecutedNum + sl[1].ExecutedNum;
			lbUnavailenNum.Content = sl[0].FailedNum + sl[1].FailedNum;
			lbErrorenNum.Content = sl[0].FCheckNum + sl[1].FCheckNum;
			DoEvents();
			if(sl[0].IsFinished == true) {
				sl[0].DoAbort();
				if(th[0] != null) th[0].Abort();
				th[0] = null;
			}
			if (sl[1].IsFinished == true) {
				sl[1].DoAbort();
				if (th[1] != null) th[1].Abort();
				th[1] = null;
			}
			if(th[0] == null && th[1] == null) {
				if (dt != null) dt.Stop();
				dt = null;
				this.Close();
			}
		}

		private void btCancel_Click(object sender, RoutedEventArgs e) {
			CloseFunc();
			this.Close();
		}

		private void Window_Closed(object sender, EventArgs e) {
			CloseFunc();
		}

		private void CloseFunc() {
			sl[0].DoAbort();
			sl[1].DoAbort();
			if(th[0] != null) th[0].Abort();
			if(th[1] != null) th[1].Abort();
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
