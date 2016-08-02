using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Xml;
using System.IO;
using LogProc.Definitions;

namespace LogProc {
	/// <summary>
	/// Equip.xaml の相互作用ロジック
	/// </summary>
	public partial class Equip : Window {
		private List<EquipGrid> gEquip { get; set; }
		private List<EquipData> eqp;
		public string EquipStr { get { return tbPreview.Text; } }
		public bool isChanged { get; set; }

		public Equip() {
			isChanged = false;
			DoLoad();
			InitializeComponent();
			Update();
		}

		private void Update() {
			//gEquip = new ObservableCollection<EquipGrid>(gEquip.OrderByDescending(l => l.Data.Category).ThenBy(m => m.Data.Name).ThenBy(n => n.Data.Rem));
			gEquip = new List<EquipGrid>(gEquip.OrderByDescending(l => l.Data.Category).ThenBy(m => m.Data.Rem).ThenBy(n => n.Data.Name));
			dgEquip.ItemsSource = gEquip;
		}

		private void DoSave() {
			using(var str = new StreamWriter(@"Equipments.xml")) {
				var xw = XmlWriter.Create(str, new XmlWriterSettings() { Indent = true, });
				xw.WriteStartDocument();
				xw.WriteStartElement("EquipList");
				foreach(var e in gEquip) {
					xw.WriteStartElement("Equip");
					xw.WriteElementString("Category", e.Data.Category);
					xw.WriteElementString("Name", e.Data.Name);
					xw.WriteElementString("Rem", e.Data.Rem);
					xw.WriteEndElement();
				}
				xw.WriteEndElement();
				xw.WriteEndDocument();
				xw.Flush();
			}
		}

		private void DoLoad() {
			gEquip = new List<EquipGrid>();
			eqp = new List<EquipData>();
			try {
				using(var str = new StreamReader(@"data/Equipments.xml")) {
					var xr = XmlReader.Create(str, new XmlReaderSettings() { IgnoreWhitespace = true, });
					var e = new EquipData();
					while(xr.Read()) {
						if(xr.NodeType == XmlNodeType.Element) {
							switch(xr.LocalName) {
								case "Equip":
									e = new EquipData();
									break;
								case "Category":
									e.Category = xr.ReadString();
									break;
								case "Name":
									e.Name = xr.ReadString();
									break;
								case "Rem":
									e.Rem = xr.ReadString();
									break;
								default: break;
							}
						} else if(xr.NodeType == XmlNodeType.EndElement) {
							switch(xr.LocalName) {
								case "Equip":
									eqp.Add(e);
									break;
								default: break;
							}
						}
					}
				}
			} catch(System.IO.FileNotFoundException e) {
				System.Console.WriteLine(e.Message);
				return;
			}

			gEquip.AddRange(eqp.Select(x => new EquipGrid() { IsEquiped = false, Data = x }));
		}

		private void btClose_Click(object sender, RoutedEventArgs e) {
			isChanged = false;
			this.Close();
		}

		private void btPreview_Click(object sender, RoutedEventArgs e) {
			tbPreview.Text = "";
		}

		private void btAccept_Click(object sender, RoutedEventArgs e) {
			isChanged = true;
			this.Close();
		}

		private void btEqSave_Click(object sender, RoutedEventArgs e) {
			DoSave();
		}

		private void btEqReload_Click(object sender, RoutedEventArgs e) {
			DoLoad();
			Update();
		}

		private void btAdd_Click(object sender, RoutedEventArgs e) {
			gEquip.Add(new EquipGrid() {
				Data = new EquipData() {
					Category = cbCategory.Text,
					Name = tbName.Text,
					Rem = tbRem.Text,
				},
			});
			Update();
			if(cbSeqAdd.IsChecked == false) {
				tbRem.Text = tbName.Text = "";
				cbCategory.Text = "";
			}
		}

		private void CheckBox_Click(object sender, RoutedEventArgs e) {
			//Update();
			List<EquipNum> len = new List<EquipNum>();
			foreach(var eq in gEquip) {
				if(!eq.IsEquiped) continue;
				var data = len.FirstOrDefault(x => x.Name == eq.Data.Name);
				if (data != null) {
					data.Num++;
					continue;
				}
				len.Add(new EquipNum() {
					Name = eq.Data.Name,
					Num = 1,
				});
			}

			tbPreview.Text = "";
			for(int i = 0;i < len.Count;i++) {
				if(i != 0) tbPreview.Text += ",";
				tbPreview.Text += len[i].Name;
				if(len[i].Num > 1) tbPreview.Text += "x" + len[i].Num;
			}
		}

		private void tbName_GotFocus(object sender, RoutedEventArgs e) {
			if(tbName.Text == "名前") tbName.Text = "";
		}

		private void tbRem_GotFocus(object sender, RoutedEventArgs e) {
			if(tbRem.Text == "備考") tbRem.Text = "";
		}

		private void dgEquip_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e) {
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
			EquipGrid ld = dgcell.DataContext as EquipGrid;
			ContextMenu cm = new ContextMenu();

			var datalist = dg.SelectedItems.Cast<EquipGrid>().ToList<EquipGrid>();
			if(!datalist.Contains(ld)) {
				dg.SelectedItem = ld;
				datalist = new List<EquipGrid>() { ld };
			}

			MenuItem mi = new MenuItem();
			mi.Header = "削除";
			mi.Click += mi_Click;
			mi.CommandParameter = datalist;
			cm.Items.Add(mi);

			ContextMenuService.SetContextMenu(dgcell, cm);
		}

		void mi_Click(object sender, RoutedEventArgs e) {
			var cp = (sender as MenuItem).CommandParameter;
			foreach(var c in cp as List<EquipGrid>) {
				gEquip.Remove(c);
			}
			Update();
		}

		private T FindVisualParent<T>(DependencyObject child) where T : DependencyObject {
			DependencyObject parentobj = VisualTreeHelper.GetParent(child);
			if(parentobj == null) return null;
			T parent = parentobj as T;
			if(parent != null) return parent;
			else return FindVisualParent<T>(parentobj);
		}
	}
}
