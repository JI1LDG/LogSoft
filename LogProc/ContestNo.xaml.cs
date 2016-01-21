using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using LogProc.Definitions;

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
				scn.Add(new ScnData() { Frequency = m.Groups[1].Value, Scn = m.Groups[2].Value });
			}
			dgRcn.ItemsSource = scn;
			foreach(defCTESTWIN.FreqStr fs in Enum.GetValues(typeof(defCTESTWIN.FreqStr))) {
				cbFreq.Items.Add(defCTESTWIN.GetFreqString(fs));
			}
		}

		public static List<ScnData> GetScnList(string ex) {
			var scn = new ObservableCollection<ScnData>();
			var match = Regex.Matches(ex, @"([\w\d\.]+), ([^:,]+): ");
			foreach (Match m in match) {
				scn.Add(new ScnData() { Frequency = m.Groups[1].Value, Scn = m.Groups[2].Value });
			}
			return scn.ToList<ScnData>();
		}

		public static string GetVal(string ex, string freq) {
			var list = GetScnList(ex);
			foreach(var l in list) {
				if(l.Frequency == freq) {
					return l.Scn;
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
				tmp += s.Frequency + ", " + s.Scn + ": ";
			}
			retEx = tmp;
			this.Close();
		}

		private void btAdd_Click(object sender, RoutedEventArgs e) {
			scn.Add(new ScnData() { Frequency = cbFreq.Text, Scn = tbScn.Text, });
			scn = new ObservableCollection<ScnData>(scn);
			dgRcn.ItemsSource = scn;
		}
	}
}
