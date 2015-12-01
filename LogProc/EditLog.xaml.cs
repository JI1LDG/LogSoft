using System;
using System.Collections.Generic;
using System.Windows;
using LogProc.Definitions;

namespace LogProc {
	/// <summary>
	/// LogEdit.xaml の相互作用ロジック
	/// </summary>
	public partial class LogEdit : Window {
		private List<LogData> Log;

		public LogEdit(List<LogData> lld) {
			Log = lld;
			InitializeComponent();
		}

		private void btTimeExecute_Click(object sender, RoutedEventArgs e) {
			var dt = new TimeSpan(int.Parse(nuHour.tbValue.Text), int.Parse(nuMinute.tbValue.Text), int.Parse(nuSecond.tbValue.Text));
			foreach(var l in Log) {
				if(rbTimeAs.IsChecked == true) {
					if(cbTimePM.Text == "+") {
						l.Date = l.Date.Add(dt);
					} else {
						l.Date = l.Date.Subtract(dt);
					}
				} else {
					l.Date = new DateTime(l.Date.Year, l.Date.Month, l.Date.Day, dt.Hours, dt.Minutes, dt.Seconds);
				}
			}
			this.Close();
		}
	}
}
