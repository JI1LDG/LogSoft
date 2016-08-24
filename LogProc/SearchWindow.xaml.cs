using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
		private BackgroundWorker[] bw;
		private WorkingData[] wdx;

		public SearchWindow(WorkingData wd, InterSet[] isp) {
			int ispl = isp.Length;
			sl = new SearchLog[ispl];
			bw = new BackgroundWorker[ispl];
			InitializeComponent();
			DevideLog(wd);

			pbCondition1.Maximum = wdx[0].Log.Count;
			pbCondition2.Maximum = wdx[1].Log.Count;
			pbConditionA.Maximum = wd.Log.Count;
			pbCondition1.Value = pbCondition2.Value = pbConditionA.Value = 0;

			btStop.IsEnabled = true;
			for(int i = 0;i < ispl; i++) {
				sl[i] = new SearchLog(wdx[i], isp[i].Sea);
				bw[i] = new BackgroundWorker();
				sl[i].SetWorker(bw[i]);
				bw[i].DoWork += new DoWorkEventHandler(sl[i].StartSearch);
				bw[i].ProgressChanged += new ProgressChangedEventHandler(ProgressChanged);
				bw[i].RunWorkerCompleted += new RunWorkerCompletedEventHandler(RunWorkerCompleted);

				bw[i].WorkerReportsProgress = true;
				bw[i].WorkerSupportsCancellation = true;
				bw[i].RunWorkerAsync();
			}
		}

		private void ProgressChanged(object sender, ProgressChangedEventArgs e) {
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
		}

		private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
			if(!bw.Any(x => x.IsBusy)) {
				CloseFunc();
				this.Close();
			}
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

		private void btCancel_Click(object sender, RoutedEventArgs e) {
			CloseFunc();
			this.Close();
		}

		private void Window_Closed(object sender, EventArgs e) {
			CloseFunc();
		}

		private void CloseFunc() {
			foreach(var arbw in bw) {
				if(arbw == null) continue;
				arbw.CancelAsync();
			}
		}
	}
}
