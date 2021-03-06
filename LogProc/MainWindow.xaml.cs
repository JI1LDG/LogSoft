﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.Runtime.Serialization;
using System.Xml;
using LogProc.Definitions;
using LogProc.Interfaces;
using LogProc.Utilities;

namespace LogProc {
	public partial class MainWindow : Window {
		private WorkingData Work { get; set; }
		private ObservableCollection<List<LogData>> Duplicates { get; set; }
		private List<InterSet>[] Intersets { get; set; }
		private InterSet[] nowItst { get; set; }

		private string Version { get { return "0.8.65"; } }
		private string BuildTime { get { return "20160827"; } }

		public MainWindow() {
			try {
				if (System.IO.File.Exists("check.ls")) {
					System.IO.StreamReader sr = new System.IO.StreamReader("check.ls", System.Text.Encoding.UTF8);
					if (sr.Peek() > 0) {
						if (Version != sr.ReadLine()) {
							sr.Close();
							WarnRule();
						}  else sr.Close();
					}
				} else WarnRule();
			} catch {
				this.Close();
			}
			Work = new WorkingData();
			Work.Log = new ObservableCollection<LogData>();

			InitializeComponent();
			this.Title += "(Ver: " + Version + ")";
			SetInterset();
			List<IDefine> defPlugins = new List<IDefine>();
			foreach(var i in Intersets[0]) defPlugins.Add(i.Def);
			ConfTab.Plugins = defPlugins.ToArray();
			dgLog.ItemsSource = Work.Log;
		}

		private void WarnRule() {
			if (MessageBox.Show("このソフトウェアを使用してコンテストログの修正・確認を行なうことは、平成28年3月7日にJARLコンテスト委員会より発表された、4月以降のJARLコンテストから適用されるコンテスト規約(http://www.jarl.org/Japanese/1_Tanoshimo/1-1_Contest/Contest.htm)の禁止事項に反する可能性があります。\r\n\r\n自己責任で使用できる方は「はい」を、使用できない方は「いいえ」を選択し、終了してください。", "通知", MessageBoxButton.YesNo) == MessageBoxResult.No) {
				this.Close();
			} else {
				System.IO.StreamWriter sw = new System.IO.StreamWriter("check.ls", false, System.Text.Encoding.UTF8);
				sw.Write(Version);
				sw.Close();
			}
		}

		private void SetInterset() {
			Intersets = new List<InterSet>[2];
			for(int i = 0;i < Intersets.Length; i++) {
				Intersets[i] = new List<InterSet>();
				Intersets[i].Add(ALLJA.Property.Intersets);
				Intersets[i].Add(Tokyo.Property.Intersets);
				Intersets[i].Add(AllKanagawa.Property.Intersets);
				Intersets[i].Add(Yamanashi.Property.Intersets);
				Intersets[i].Add(SixMAndDown.Property.Intersets);
				Intersets[i].Add(FieldDay.Property.Intersets);
				Intersets[i].Add(NTTCwPh.Property.Intersets);
				Intersets[i].Add(ACAG.Property.Intersets);
				Intersets[i].Add(KantoUHF.Property.Intersets);
			}
			nowItst = new InterSet[Intersets.Length];
		}

		private void UpdateData() {
			if(Work.Log.Count == 0) return;

			int unsearchen = 0;
			int unfinden = 0;
			int erroren = 0;
			int dupen = 0;
			int dupegrpen = 0;

			var listErr = ErrorReason.GetInitial();
			ErrorReason.Set(listErr, Reason.Duplicate.ToString());
			string dup = ErrorReason.GetFailedStr(listErr);

			foreach(var l in Work.Log) {
				while (l.FailedStr.Contains(dup)) {
					int idx = l.FailedStr.IndexOf(dup);
					if (idx < 0) break;
					l.FailedStr = l.FailedStr.Remove(idx, dup.Length);
				}
			}

			Duplicates = new ObservableCollection<List<LogData>>();
			var gb = Work.Log.GroupBy(wl => new { wl.Callsign, wl.Freq }).Where(g => g.Count() > 1).ToList();
			dupegrpen = gb.Count;
			foreach(var g in gb) {
				var tmp = new List<LogData>();
				dupen += g.Count();
				for(int i = 0;i < g.Count(); i++) {
					if (!g.ElementAt(i).FailedStr.Contains(dup)) {
						g.ElementAt(i).FailedStr += dup;
					}
					tmp.Add(g.ElementAt(i));
				}
				Duplicates.Add(tmp);
			}

			lbReadenLogNum.Content = Work.Log.Count;
			Work.Log = new ObservableCollection<LogData>(Work.Log.OrderByDescending(l => l.FailedStr));
			dgLog.ItemsSource = Work.Log;
			if(Work.Config != null) lbSettingConfigen.Content = "";
			foreach(var l in Work.Log) {
				if(!l.IsSearched) unsearchen++;
				if(!l.IsFinded) unfinden++;
				if(!l.IsRate0) erroren++;
			}
			lbUnsearchenLogNum.Content = unsearchen;
			lbUnavailenLogNum.Content = unfinden;
			lbErrorenLogNum.Content = erroren;
			lbDupenLogNum.Content = dupegrpen + "(" + dupen + ")";
		}

		private void AddLog(LogData log) {
			Work.Log.Add(log);
		}

		private void miAddFile_Click(object sender, RoutedEventArgs e) {
			LoadLog ll = new LoadLog();
			if(!ll.AddFiles()) {
				MessageBox.Show("ファイル読み込みに失敗しました。", "通知");
				return;
			}
			if(ll.ContestLog == null) return;
			foreach(var ld in ll.ContestLog) {
				AddLog(ld);
			}

			UpdateData();
		}

		private void miOutputFile_Click(object sender, RoutedEventArgs e) {
			Work.Config = ConfTab.GetSetting();
			SetNowIntersets();
			UpdateData();
			if (nowItst[0] == null) {
				MessageBox.Show("コンテスト情報を設定してください。", "通知");
				return;
			}
			var ol = new OutputLog(Work, nowItst[0].Sum);
			ol.CreateLog(false, false);
			string output = ol.opLog;
			if (output == null) {
				MessageBox.Show("ログ生成に失敗しました。", "通知");
				return;
			}
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Title = "ログファイルの保存";
			sfd.Filter = "ログファイル(*.txt)|*.txt";
			if (sfd.ShowDialog() == true) {
				string filename = sfd.FileName;
				var sw = new System.IO.StreamWriter(filename, false, System.Text.Encoding.GetEncoding("Shift-JIS"));
				sw.WriteLine(output);
				sw.Close();
			}
		}

		private void miLoadWork_Click(object sender, RoutedEventArgs e) {
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Title = "作業ファイルの読み込み";
			ofd.Filter = "作業ファイル(*.work.xml)|*.work.xml";
			if(ofd.ShowDialog() != true) return;
			LoadWork(ofd.FileName);
		}

		private void LoadWork(string filename) {
			DataContractSerializer serial = new DataContractSerializer(typeof(WorkingData));
			XmlReader xr = XmlReader.Create(filename);
			Work = new WorkingData();
			Work = (WorkingData)serial.ReadObject(xr);
			ConfTab.DoLoad(Work.Config);
			UpdateData();
			xr.Close();
		}

		private void miSaveWork_Click(object sender, RoutedEventArgs e) {
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Title = "作業ファイルの保存";
			sfd.Filter = "作業ファイル(*.work.xml)|*.work.xml";
			if(sfd.ShowDialog() == true) {
				string filename = sfd.FileName;
				DataContractSerializer serial = new DataContractSerializer(typeof(WorkingData));
				XmlWriterSettings set = new XmlWriterSettings();
				set.Encoding = new System.Text.UTF8Encoding(false);
				XmlWriter xw = XmlWriter.Create(filename, set);
				serial.WriteObject(xw, Work);
				xw.Close();
			}
		}

		private void miExit_Click(object sender, RoutedEventArgs e) {
			System.Environment.Exit(0);
		}

		private void dgLog_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e) {
			DataGrid dg = sender as DataGrid;
			Point pt = e.GetPosition(dg);
			DataGridCell dgcell = null;

			VisualTreeHelper.HitTest(dg, null, (result) => {
				DataGridCell cell = FindVisualParent<DataGridCell>(result.VisualHit);
				if(cell != null) {
					dgcell = cell;
					return HitTestResultBehavior.Stop;
				} else return HitTestResultBehavior.Continue;
			}, new PointHitTestParameters(pt));

			if(dgcell == null) return;
			LogData ld = dgcell.DataContext as LogData;
			ContextMenu cm = new ContextMenu();

			var datalist = new ObservableCollection<LogData>(dg.SelectedItems.Cast<LogData>().ToList<LogData>());
			if(!datalist.Contains(ld)) {
				dg.SelectedItem = ld;
			}

			MenuItem mi = new MenuItem();
			mi.Click += SearchByWeb;
			mi.Header = "Webで検索(" + ld.Callsign + ")";
			mi.CommandParameter = Callsign.GetRemovedStroke(ld.Callsign);
			cm.Items.Add(mi);

			MenuItem miLogEdit = new MenuItem();
			miLogEdit.Click += EditLog;
			miLogEdit.Header = "ログ修正";

			MenuItem miLogSearch = new MenuItem();
			miLogSearch.Click += SearchLogEachly;
			miLogSearch.Header = "集計処理";

			if(!datalist.Contains(ld) || datalist.Count == 1) {
				cm.Items.Add(new Separator());
				miLogEdit.CommandParameter = new ObservableCollection<LogData>() { ld };
				miLogSearch.CommandParameter = new ObservableCollection<LogData>() { ld };
			} else {
				MenuItem miSearches = new MenuItem();
				miSearches.Click += SearchByWeb;
				miSearches.Header = "Webで検索(" + datalist.Count + "件)";
				miSearches.CommandParameter = datalist;
				cm.Items.Add(miSearches);

				cm.Items.Add(new Separator());

				miLogEdit.CommandParameter = datalist;
				miLogSearch.CommandParameter = datalist;
			}
			cm.Items.Add(miLogEdit);
			cm.Items.Add(miLogSearch);

			ContextMenuService.SetContextMenu(dgcell, cm);
		}

		private T FindVisualParent<T>(DependencyObject child) where T : DependencyObject {
			DependencyObject parentobj = VisualTreeHelper.GetParent(child);
			if(parentobj == null) return null;
			T parent = parentobj as T;
			if(parent != null) return parent;
			else return FindVisualParent<T>(parentobj);
		}

		private void SearchByWeb(object sender, RoutedEventArgs e) {
			var cp = (sender as MenuItem).CommandParameter;

			if(cp is string) {
				AccessStationSearch(cp.ToString());
			} else if(cp is ObservableCollection<LogData>) {
				foreach(var ld in cp as ObservableCollection<LogData>) {
					AccessStationSearch(ld.Callsign);
				}
			}
			//throw new NotImplementedException();
		}

		private void EditLog(object sender, RoutedEventArgs e) {
			LogEdit le = new LogEdit(((sender as MenuItem).CommandParameter as ObservableCollection<LogData>).ToList<LogData>());
			le.ShowDialog();
			UpdateData();
		}

		private void SearchLogEachly(object sender, RoutedEventArgs e) {
			var eventlogdata = (sender as MenuItem).CommandParameter as ObservableCollection<LogData>;
			Work.Config = ConfTab.GetSetting();
			SetNowIntersets();
			UpdateData();
			if (nowItst[0] == null || eventlogdata == null || Work.Log.Count == 0) {
				MessageBox.Show("チェックするログがない、もしくは局情報等が設定されてません。", "通知");
				return;
			}
			SearchWindow sw = new SearchWindow(new WorkingData() { Config = ConfTab.GetSetting(), Log = eventlogdata }, nowItst);
			sw.ShowDialog();
			UpdateData();
		}

		private void AccessStationSearch(string CallSign) {
			System.Diagnostics.Process.Start("http://www.tele.soumu.go.jp/musen/SearchServlet?SC=1&pageID=3&SelectID=1&CONFIRM=0&SelectOW=01&IT=&HC=&HV=&FF=&TF=&HZ=3&NA=&MA=" + Callsign.GetRemovedStroke(CallSign) + "&DFY=&DFM=&DFD=&DTY=&DTM=&DTD=&SK=2&DC=100&as_fid=2I6vX7ugLE0ekrPjPfMD#result");
		}

		private void SetNowIntersets() {
			for(int i = 0; i < Intersets.Length; i++) {
				var ret = Intersets[i].Where(x => x.Def.contestName == Work.Config.ContestName).FirstOrDefault();
				if(ret != null) nowItst[i] = ret;
			}
		}

		private void btLoadSetting_Click(object sender, RoutedEventArgs e) {
			var ld = ConfTab.SetSetting();
			if (ld == null) return;
			Work.Config = ld;
			SetNowIntersets();
            UpdateData();
		}

		private void btSaveSetting_Click(object sender, RoutedEventArgs e) {
			Work.Config = ConfTab.SaveSetting();
			SetNowIntersets();
			UpdateData();
		}

		private void btCheck_Click(object sender, RoutedEventArgs e) {
			Work.Config = ConfTab.GetSetting();
			SetNowIntersets();
			UpdateData();
			if(nowItst[0] == null || Work.Log == null || Work.Log.Count == 0) {
				MessageBox.Show("チェックするログがない、もしくは局情報等が設定されてません。", "通知");
				return;
			}
			SearchWindow sw = new SearchWindow(Work, nowItst);
			sw.ShowDialog();
			UpdateData();
			if(ConfTab.cbAutoOperator.IsChecked == true) {
				ConfTab.tbOperator.Text = Utils.GetOpList(Work);
				ConfTab.cbAutoOperator.IsChecked = false;
			}
		}

		private void btOutput_Click(object sender, RoutedEventArgs e) {
			Work.Config = ConfTab.GetSetting();
			SetNowIntersets();
			UpdateData();
			if (nowItst[0] == null || Work.Log == null || Work.Log.Count == 0) {
				MessageBox.Show("チェックするログがない、もしくは局情報等が設定されてません。", "通知");
				return;
			}
			
			OutputSummery os = new OutputSummery(new WorkingData() { Config = ConfTab.GetSetting(), Log = new ObservableCollection<LogData>(Work.Log.OrderBy(l => l.Date))
		}, nowItst);
			os.ShowDialog();
			UpdateData();
		}

		private void Window_Closed(object sender, EventArgs e) {
			System.Environment.Exit(0);
		}

		private void miVersionInfo_Click(object sender, RoutedEventArgs e) {
			MessageBox.Show("Ver:" + Version + "(Build:" + BuildTime + ")\r\nAuthor/Developer: JI1LDG\r\nForm Designer / Adviser: JI1EPL\r\n無線局情報の検索に総務省の「無線局等情報検索(http://www.tele.soumu.go.jp/musen/SearchServlet?pageID=1)」を使用しています。", "バージョン情報");
		}

		private void miDBInit_Click(object sender, RoutedEventArgs e) {
			if(MessageBox.Show("DBの新規作成・初期化を実行しますか？\r\n※必要時以外は実行しないでください。", "警告", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
				if(System.IO.File.Exists("data/RadioStation.db")) {
					string tm = System.DateTime.Now.ToString("yyMMddHHmmss");
					var path = System.Environment.CurrentDirectory;
					System.IO.File.Move(path + "/data/RadioStation.db", path + "/data/RS" + tm + ".db");
				}
				CreateDB();
			}
		}

		private void CreateDB() {
			using(var con = new SQLiteConnection()) {
				con.ConnectionString = "Data Source=data/RadioStation.db;";
				con.Open();
				using(SQLiteCommand com = con.CreateCommand()) {
					com.CommandText = "create table Stations(callsign TEXT, name TEXT, address TEXT)";
					com.ExecuteNonQuery();
				}
				con.Close();
			}
		}

		private void Window_Drop(object sender, DragEventArgs e) {
			string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
			if(files != null) {
				foreach(var f in files) {
					if(f.Substring(f.Length - 7) == "set.xml") {
						ConfTab.SetSetting(f);
						break;
					} else if(f.Substring(f.Length - 3) == "lg8" || f.Substring(f.Length - 3) == "txt" || f.Substring(f.Length - 3) == "TXT") {
						LoadLog ll = new LoadLog();
						if(!ll.AddFile(f)) {
							MessageBox.Show("ファイル読み込みに失敗しました。", "通知");
							return;
						}
						if(ll.ContestLog == null) return;
						if(Work.Log == null) Work.Log = new ObservableCollection<LogData>();
						foreach(var ld in ll.ContestLog) {
							AddLog(ld);
						}

						UpdateData();
					} else if(f.Substring(f.Length - 8) == "work.xml") {
						LoadWork(f);
					}
				}
			}
		}

		private void Window_PreviewDragOver(object sender, DragEventArgs e) {
			if(e.Data.GetDataPresent(DataFormats.FileDrop, true)) {
				e.Effects = DragDropEffects.Copy;
			} else {
				e.Effects = DragDropEffects.None;
			}
			e.Handled = true;
		}

		private void miLogAlysc_Click(object sender, RoutedEventArgs e) {
			UpdateData();
			LogAlysc la = new LogAlysc(Work.Log);
			la.ShowDialog();
		}

		private void miDupeChk_Click(object sender, RoutedEventArgs e) {
			UpdateData();
			if(Duplicates != null && Duplicates.Count > 0) {
				var ds = new DupeSolver(Work.Log, Duplicates);
				ds.ShowDialog();
				Work.Log = ds.Log;
				UpdateData();
			} else {
				MessageBox.Show("Dupeが検出されませんでした。", "通知");
			}
		}
	}

	public class InterSet {
		public IDefine Def { get; set; }
		public ISearch Sea { get; set; }
		public ISummery Sum { get; set; }

		public InterSet() { }
		public InterSet(IDefine idf, ISearch isa, ISummery ism) {
			this.Def = idf;
			this.Sea = isa;
			this.Sum = ism;
		}
	}
}
