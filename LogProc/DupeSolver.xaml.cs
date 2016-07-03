using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using LogProc.Definitions;

namespace LogProc {
	/// <summary>
	/// DupeSolver.xaml の相互作用ロジック
	/// </summary>
	public partial class DupeSolver : Window {
		public ObservableCollection<LogData> Log { get; set; }
		private ObservableCollection<List<LogData>> Duplis { get; set; }
		private List<LogData> Delete { get; set; }
		private ObservableCollection<LogData> Shown { get; set; }
		private int nowCount;

		public DupeSolver(ObservableCollection<LogData> log, ObservableCollection<List<LogData>> dupe) {
			InitializeComponent();
			btMinus.Content = "<-";
			btPlus.Content = "->";
			nowCount = 0;

			this.Log = log;
			this.Duplis = dupe;

			Delete = new List<LogData>();
			Tryon(nowCount);
		}

		private void btMinus_Click(object sender, RoutedEventArgs e) {
			var cnt = int.Parse(tbCount.Text);
			if(--cnt <= 0) {
				cnt += Duplis.Count;
			}
			tbCount.Text = cnt.ToString();
			Tryon(cnt - 1);
		}

		private void btPlus_Click(object sender, RoutedEventArgs e) {
			var cnt = int.Parse(tbCount.Text);
			if(++cnt == Duplis.Count + 1) {
				cnt -= Duplis.Count;
			}
			tbCount.Text = cnt.ToString();
			Tryon(cnt - 1);
		}

		private void tbCount_PreviewTextInput(object sender, TextCompositionEventArgs e) {
			int count;
			var tmp = tbCount.Text + e.Text;
			bool parse = int.TryParse(tmp, out count);

			e.Handled = !parse;
		}

		private void btTryon_Click(object sender, RoutedEventArgs e) {
			Tryon(int.Parse(tbCount.Text) - 1);
		}

		private void Tryon(int count) {
			nowCount = count;
			Shown = new ObservableCollection<LogData>();
			for(int i = 0;i < Duplis[count].Count; i++) { 
				if(!Delete.Exists(x => x == Duplis[count][i])) {
					Shown.Add(Duplis[count][i]);
				}
			}
			dgLog.ItemsSource = Shown;
		}

		private void btReset_Click(object sender, RoutedEventArgs e) {
			foreach(var d in Duplis[nowCount]) {
				Delete.Remove(d);
			}
			Tryon(nowCount);
		}

		private void btCancel_Click(object sender, RoutedEventArgs e) {
			this.Close();
		}

		private void btExecute_Click(object sender, RoutedEventArgs e) {
			var listErr = ErrorReason.GetInitial();
			ErrorReason.Set(listErr, Reason.Duplicate.ToString());
			foreach(var d in Delete) {
				Log.Remove(d);
			}
			foreach(var l in Log) {
				l.FailedStr.Replace(ErrorReason.GetFailedStr(listErr), "");
			}
			this.Close();
		}

		private void btDelete_Click(object sender, RoutedEventArgs e) {
			var tmp = (LogData)dgLog.SelectedItem;
			Shown.Remove(tmp);
			Delete.Add(tmp);
			dgLog.ItemsSource = Shown;
		}
	}
}
