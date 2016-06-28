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
using LogProc.Definitions;
using System.Collections.ObjectModel;

namespace LogProc {
	/// <summary>
	/// LogAlysc.xaml の相互作用ロジック
	/// </summary>
	/// 
	public partial class LogAlysc : Window {
		private ObservableCollection<AlyscData> alydata { get; set; }

		public LogAlysc(ObservableCollection<LogData> logdata) {
			alydata = new ObservableCollection<AlyscData>();
			foreach(var l in logdata) {
				AlyscData tmp;
				if(alydata.Any(a => a.Name == l.Operator)) {
					tmp = alydata.Where(a => a.Name == l.Operator).Single();
				} else {
					tmp = new AlyscData(l.Operator);
					alydata.Add(tmp);
				}
				tmp.Set(l);
			}
			InitializeComponent();
			dgAlysc.ItemsSource = alydata;
		}
	}
}
