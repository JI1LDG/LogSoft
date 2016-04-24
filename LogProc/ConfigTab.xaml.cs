using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using LogProc.Definitions;
using LogProc.Interfaces;
using System.Text;

namespace LogProc {
	/// <summary>
	/// SettingTab.xaml の相互作用ロジック
	/// </summary>
	public partial class SettingTab : UserControl {
		private string extra;

		private IDefine[] _plg;
		public IDefine[] Plugins {
			private get { return _plg; }
			set {
				_plg = value;
				cbContestName.Items.Clear();
				foreach (var dp in _plg) {
					cbContestName.Items.Add(new ComboBoxItem() {
						Content = dp.contestName,
					});
				}
				cbContestName.IsEnabled = true;
			}
		}

		public SettingTab() {
			InitializeComponent();
			cbAutoOperator.IsChecked = true;
			cbContestName.IsEnabled = false;
			extra = "";
		}

		public Setting SetSetting() {
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Title = "設定ファイルの読み込み";
			ofd.Filter = "設定ファイル(*.set.xml)|*.set.xml";
			if (ofd.ShowDialog() != true) return null;
			return SetSetting(ofd.FileName);
		}

		public Setting SetSetting(string Path) {
			System.Xml.Serialization.XmlSerializer serial = new System.Xml.Serialization.XmlSerializer(typeof(Setting));
			System.IO.StreamReader sr = new System.IO.StreamReader(Path, new System.Text.UTF8Encoding(false));
			var setCl = (Setting)serial.Deserialize(sr);
			sr.Close();
			if (setCl.Version != defs.SettingVer) {
				if (MessageBox.Show("古い、互換性のない形式の設定ファイルです。新しい形式に変換しますか？\r\n変換せずに処理を続行させることはできますが、一部項目が読み込めないことがあります。", "通知", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
					switch (setCl.Version) {
						case "0.8.1":
							serial = new System.Xml.Serialization.XmlSerializer(typeof(Definitions.v0810.Setting));
							sr = new System.IO.StreamReader(Path, new UTF8Encoding(false));
							var st0810 = (Definitions.v0810.Setting)serial.Deserialize(sr);
							sr.Close();

							setCl.Version = defs.SettingVer;
							setCl.IsCoefficientEnabled = st0810.Coefficient;
							setCl.Contestno = st0810.ContestNo;
							setCl.SubContestno = st0810.SubContestNo;
							setCl.SentCnExtra = st0810.ScnExtra;
							setCl.IsSubCnEnabled = st0810.IsSubCN;
							setCl.PowerVal = st0810.PowerValue;
							setCl.IsAutoOperatorEditEnabled = st0810.AutoOperatorEdit;
							setCl.Callsign = st0810.CallSign;
							setCl.Equip = st0810.Equipment;

							if (MessageBox.Show("変換しました。上書き保存しますか？", "通知", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
								serial = new System.Xml.Serialization.XmlSerializer(typeof(Setting));
								var sw = new System.IO.StreamWriter(Path, false, new UTF8Encoding(false));
								serial.Serialize(sw, setCl);
								sw.Close();
							}
							break;
						case "0.8.50":
							break;
						default:
							MessageBox.Show("不正な設定ファイルです。", "通知");
							break;
					}
				}
			}
			DoLoad(setCl);
			sr.Close();
			return DoSave();
		}

		public Setting GetSetting() {
			Setting config;

			config = DoSave();

			return config;
		}

		public Setting SaveSetting() {
			Setting config;

			config = DoSave();
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Title = "設定ファイルの保存";
			sfd.Filter = "設定ファイル(*.set.xml)|*.set.xml";
			if (sfd.ShowDialog() == true) {
				string filename = sfd.FileName;
				System.Xml.Serialization.XmlSerializer serial = new System.Xml.Serialization.XmlSerializer(typeof(Setting));
				var sw = new System.IO.StreamWriter(filename, false, new System.Text.UTF8Encoding(false));
				serial.Serialize(sw, config);
				sw.Close();
			}

			return config;
		}



		public void DoLoad(Setting st) {
			foreach (var dp in Plugins) {
				if (dp.contestName != st.ContestName) continue;
				cbContestName.Text = st.ContestName;
				FillCategory(st.ContestName);
				foreach (var cc in dp.contestCategories) {
					if (cc.Name != st.CategoryName) continue;
					cbCategory.Text = "(" + dp.GetCodeDivPower(st.CategoryCode, GetContestPowerEnumFromStr(st.CategoryPower)) + ")" + st.CategoryName;
					ChangeEnablalContestPower(dp.GetPowerAllowed(cc.Code));
					CheckCategoryPowerByConvertenStr(st.CategoryPower);
					break;
				}
				if (dp.isCoefficientEnabled) {
					cbCoefficient.IsEnabled = true;
					cbCoefficient.IsChecked = st.IsCoefficientEnabled;
				} else {
					cbCoefficient.IsEnabled = false;
					cbCoefficient.IsChecked = false;
				}
				if (dp.isSubCnEnabled) {
					tbSubContestNo.IsEnabled = true;
					tbSubContestNo.Text = st.SubContestno;
				} else {
					tbSubContestNo.IsEnabled = false;
				}
				break;
			}
			extra = (st.SentCnExtra == null) ? "" : st.SentCnExtra;
			tbMainContestNo.Text = st.Contestno;
			tbSubContestNo.Text = st.SubContestno;
			rbNormal.IsChecked = st.PowerType == "定格出力" ? true : false;
			rbReal.IsChecked = rbNormal.IsChecked == true ? false : true;
			cbPowerValue.Text = st.PowerVal;
			cbAutoOperator.IsChecked = st.IsAutoOperatorEditEnabled;
			tbOperator.Text = st.Operator;
			tbCallSign.Text = st.Callsign;
			tbZipCode.Text = st.ZipCode;
			tbAddress.Text = st.Address;
			tbPhone.Text = st.Phone;
			tbName.Text = st.Name;
			tbMail.Text = st.Mail;
			tbLicenserName.Text = st.LicenserName;
			cbLincenserLicense.Text = st.LicenserLicense;
			tbPlace.Text = st.Place;
			tbSupply.Text = st.Supply;
			tbEquip.Text = st.Equip;
			tbUseType.Text = st.UseType;
			tbComment.Text = st.Comment;
			tbOath.Text = st.Oath;
		}

		private Setting DoSave() {
			Setting st = new Setting();
			st.Version = defs.SettingVer;

			st.ContestName = cbContestName.Text;
			foreach (var dp in Plugins) {
				if (dp.contestName != st.ContestName) continue;
				st.CategoryCode = dp.GetCodeWithPower(cbCategory.Text.Substring(1, cbCategory.Text.IndexOf(")") - 1), ConvertCategoryPowerToEnum());
				st.IsSubCnEnabled = dp.isSubCnEnabled;
				break;
			}
			st.CategoryName = cbCategory.Text.Substring(cbCategory.Text.IndexOf(")") + 1);
			st.CategoryPower = ConvertCategoryPowerToStr();
			st.IsCoefficientEnabled = cbCoefficient.IsChecked == true ? true : false;
			st.Contestno = tbMainContestNo.Text;
			st.SubContestno = tbSubContestNo.Text;
			st.SentCnExtra = extra;
			st.PowerType = rbNormal.IsChecked == true ? "定格出力" : "実測出力";
			st.PowerVal = cbPowerValue.Text;
			st.IsAutoOperatorEditEnabled = cbAutoOperator.IsChecked == true ? true : false;
			if (!st.IsAutoOperatorEditEnabled) {
				st.Operator = tbOperator.Text;
			}
			st.Callsign = tbCallSign.Text;
			st.ZipCode = tbZipCode.Text;
			st.Address = tbAddress.Text;
			st.Phone = tbPhone.Text;
			st.Name = tbName.Text;
			st.Mail = tbMail.Text;
			st.LicenserName = tbLicenserName.Text;
			st.LicenserLicense = cbLincenserLicense.Text;
			st.Place = tbPlace.Text;
			st.Supply = tbSupply.Text;
			st.Equip = tbEquip.Text;
			st.UseType = tbUseType.Text;
			st.Comment = tbComment.Text;
			st.Oath = tbOath.Text;

			return st;
		}

		private string ConvertCategoryPowerToStr() {
			if (rbLicense.IsChecked == true) return "License";
			if (rb100w.IsChecked == true) return "100W";
			if (rb1020w.IsChecked == true) return "10(20)W";
			return "5W";
		}

		private ContestPower ConvertCategoryPowerToEnum() {
			if (rbLicense.IsChecked == true) return ContestPower.License;
			if (rb100w.IsChecked == true) return ContestPower.Hundred;
			if (rb1020w.IsChecked == true) return ContestPower.TwentyTen;
			return ContestPower.Five;
		}

		private ContestPower GetContestPowerEnumFromStr(string Str) {
			switch (Str) {
				case "License":
					return ContestPower.License;
				case "100W":
					return ContestPower.Hundred;
				case "10(20)W":
					return ContestPower.TwentyTen;
				case "5W":
					return ContestPower.Five;
				default:
					return ContestPower.None;
			}
		}

		private void CheckCategoryPowerByConvertenStr(string Str) {
			switch (Str) {
				case "License":
					rbLicense.IsChecked = true;
					break;
				case "100W":
					rb100w.IsChecked = true;
					break;
				case "10(20)W":
					rb1020w.IsChecked = true;
					break;
				case "5W":
					rb5w.IsChecked = true;
					break;
				default:
					break;
			}
		}

		private void cbContestName_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			var cb = sender as ComboBox;
			var selected = cb.SelectedItem as ComboBoxItem;
			if (selected == null) return;
			FillCategory(selected.Content as string);
			foreach (var dp in Plugins) {
				if (dp.contestName != selected.Content as string) continue;
				if (dp.isCoefficientEnabled) {
					cbCoefficient.IsEnabled = true;
					cbCoefficient.IsChecked = true;
				} else {
					cbCoefficient.IsEnabled = false;
					cbCoefficient.IsChecked = false;
				}
				if (dp.isSubCnEnabled) {
					tbSubContestNo.IsEnabled = true;
				} else {
					tbSubContestNo.IsEnabled = false;
				}
				break;
			}
		}

		private void cbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			var cb = sender as ComboBox;
			var selected = cb.SelectedItem as ComboBoxItem;
			if (selected == null) return;
			foreach (var dp in Plugins) {
				if (dp.contestName != ((cbContestName as ComboBox).SelectedItem as ComboBoxItem).Content as string) continue;
				string cont = selected.Content as string;
				ChangeEnablalContestPower(dp.GetPowerAllowed(cont.Substring(1, cont.IndexOf(")") - 1)));
				break;
			}
		}

		private void FillCategory(string ContestName) {
			cbCoefficient.IsEnabled = false;
			cbCoefficient.IsChecked = false;
			ChangeEnablalContestPower(ContestPower.None);
			cbCategory.Items.Clear();
			foreach (var dp in Plugins) {
				if (dp.contestName != ContestName) continue;
				tbOath.Text = dp.oath;
				foreach (var cc in dp.contestCategories) {
					cbCategory.Items.Add(new ComboBoxItem() {
						Content = "(" + cc.Code + ")" + cc.Name,
					});
				}
				cbCategory.IsEnabled = true;
				break;
			}
		}

		private void ChangeEnablalContestPower(ContestPower cp) {
			rb100w.IsEnabled = rb1020w.IsEnabled = rb5w.IsEnabled = rbLicense.IsEnabled = false;
			rb100w.IsChecked = rb1020w.IsChecked = rb5w.IsChecked = rbLicense.IsChecked = false;
			if (cp == ContestPower.None) return;
			if (cp.HasFlag(ContestPower.License)) rbLicense.IsEnabled = true;
			if (cp.HasFlag(ContestPower.Hundred)) rb100w.IsEnabled = true;
			if (cp.HasFlag(ContestPower.TwentyTen)) rb1020w.IsEnabled = true;
			if (cp.HasFlag(ContestPower.Five)) rb5w.IsEnabled = true;
		}

		private void cbAutoOperator_Checked(object sender, RoutedEventArgs e) {
			tbOperator.IsEnabled = false;
		}

		private void cbAutoOperator_Unchecked(object sender, RoutedEventArgs e) {
			tbOperator.IsEnabled = true;
		}

		private void btEquipSet_Click(object sender, RoutedEventArgs e) {
			Equip eq = new Equip();
			eq.ShowDialog();
			if (eq.isChanged) tbEquip.Text = eq.EquipStr;
		}

		private void btScnExtra_Click(object sender, RoutedEventArgs e) {
			ContestNo cn = new ContestNo(extra);
			cn.ShowDialog();
			if (cn.retEx != null) extra = cn.retEx;
		}
	}
}