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
			scn = new ObservableCollection<ScnData>(GetScnList(extra));
			this.dgRcn.ItemsSource = new ObservableCollection<ScnData>(scn.ToList());
			foreach(FreqStr fs in Enum.GetValues(typeof(FreqStr))) {
				cbFreq.Items.Add(Freq.CnvTostr(fs));
			}
		}

		public static List<ScnData> GetScnList(string ex) {
			return Regex.Matches(ex, @"([\w\d\.]+), ([^:,]+): ").Cast<Match>().Select(x => new ScnData() { Freq = x.Groups[1].Value, SentCn = x.Groups[2].Value }).ToList();
		}

		public static string GetVal(string ex, string freq) {
			var list = GetScnList(ex);
			if (list.Count == 0) return null;

			var litmp = list.Where(x => x.Freq == freq).FirstOrDefault();
			return litmp != null ? litmp.SentCn : null;
		}

		private void btCancel_Click(object sender, RoutedEventArgs e) {
			this.Close();
		}

		private void btConfirm_Click(object sender, RoutedEventArgs e) {
			retEx = "";
			scn.ToList().ForEach(s => {
				retEx += s.Freq + ", " + s.SentCn + ": ";
			});
			this.Close();
		}

		private void btAdd_Click(object sender, RoutedEventArgs e) {
			scn.Add(new ScnData() { Freq = cbFreq.Text, SentCn = tbScn.Text, });
			scn = new ObservableCollection<ScnData>(scn);
			dgRcn.ItemsSource = scn;
		}
	}
}
