using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using LogProc.Definitions;
using LogProc.Utilities;

namespace LogProc {
	public partial class ContestNo : Window {
		private ObservableCollection<ScnData> scn { get; set; }
		public string retEx = null;
		public ContestNo() { }
		public ContestNo(string extra) {
			InitializeComponent();
			scn = new ObservableCollection<ScnData>();
			var match = Regex.Matches(extra, @"([\w\d\.]+), ([^:,]+): ");
			foreach (Match m in match) {
				scn.Add(new ScnData() { Freq = m.Groups[1].Value, SentCn = m.Groups[2].Value });
			}
			dgRcn.ItemsSource = scn;
			foreach(FreqStr fs in Enum.GetValues(typeof(FreqStr))) {
				cbFreq.Items.Add(Freq.CnvTostr(fs));
			}
		}

		public static List<ScnData> GetScnList(string ex) {
			var scn = new ObservableCollection<ScnData>();
			var match = Regex.Matches(ex, @"([\w\d\.]+), ([^:,]+): ");
			foreach (Match m in match) {
				scn.Add(new ScnData() { Freq = m.Groups[1].Value, SentCn = m.Groups[2].Value });
			}
			return scn.ToList<ScnData>();
		}

		public static string GetVal(string ex, string freq) {
			var list = GetScnList(ex);
			foreach(var l in list) {
				if(l.Freq == freq) {
					return l.SentCn;
				}
			}
			return null;
		}

		private void btCancel_Click(object sender, RoutedEventArgs e) {
			this.Close();
		}

		private void btConfirm_Click(object sender, RoutedEventArgs e) {
			string tmp = "";
			foreach(var s in scn) {
				tmp += s.Freq + ", " + s.SentCn + ": ";
			}
			retEx = tmp;
			this.Close();
		}

		private void btAdd_Click(object sender, RoutedEventArgs e) {
			scn.Add(new ScnData() { Freq = cbFreq.Text, SentCn = tbScn.Text, });
			scn = new ObservableCollection<ScnData>(scn);
			dgRcn.ItemsSource = scn;
		}
	}
}
