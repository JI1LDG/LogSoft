using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using LogProc.Definitions;
using LogProc.Interfaces;

namespace LogProc {
	/// <summary>
	/// SettingTab.xaml の相互作用ロジック
	/// </summary>
	public partial class SettingTab : UserControl {
		private IDefine[] _plg;
		public IDefine[] Plugins {
			private get { return _plg; }
			set {
				_plg = value;
				cbContestName.Items.Clear();
				foreach(var dp in _plg) {
					cbContestName.Items.Add(new ComboBoxItem() {
						Content = dp.ContestName,
					});
				}
				cbContestName.IsEnabled = true;
			}
		}

		public SettingTab() {
			InitializeComponent();
			cbAutoOperator.IsChecked = true;
			cbContestName.IsEnabled = false;
		}

		public Setting SetSetting() {
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Title = "設定ファイルの読み込み";
			ofd.Filter = "設定ファイル(*.set.xml)|*.set.xml";
			if(ofd.ShowDialog() != true) return null;
			System.Xml.Serialization.XmlSerializer serial = new System.Xml.Serialization.XmlSerializer(typeof(Setting));
			System.IO.StreamReader sr = new System.IO.StreamReader(ofd.FileName, new System.Text.UTF8Encoding(false));
			DoLoad((Setting)serial.Deserialize(sr));
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
			if(sfd.ShowDialog() == true) {
				string filename = sfd.FileName;
				System.Xml.Serialization.XmlSerializer serial = new System.Xml.Serialization.XmlSerializer(typeof(Setting));
				var sw = new System.IO.StreamWriter(filename, false, new System.Text.UTF8Encoding(false));
				serial.Serialize(sw, config);
				sw.Close();
			}

			return config;
		}

		public void DoLoad(Setting st) {
			foreach(var dp in Plugins) {
				if(dp.ContestName != st.ContestName) continue;
				cbContestName.Text = st.ContestName;
				FillCategory(st.ContestName);
				foreach(var cc in dp.ContestCategolies) {
					if(cc.Name != st.CategoryName) continue;
					cbCategory.Text = "(" + dp.GetCategoryCodeDivPower(st.CategoryCode, GetContestPowerEnumFromStr(st.CategoryPower)) + ")" + st.CategoryName;
					ChangeEnablalContestPower(dp.AllowenPowerInCategoryCode(cc.Code));
					CheckCategoryPowerByConvertenStr(st.CategoryPower);
					break;
				}
				if(dp.Coefficient) {
					cbCoefficient.IsEnabled = true;
					cbCoefficient.IsChecked = st.Coefficient;
				} else {
					cbCoefficient.IsEnabled = false;
					cbCoefficient.IsChecked = false;
				}
				break;
			}
			tbContestNo.Text = st.ContestNo;
			rbNormal.IsChecked = st.PowerType == "定格出力" ? true : false;
			rbReal.IsChecked = rbNormal.IsChecked == true ? false : true;
			cbPowerValue.Text = st.PowerValue;
			cbAutoOperator.IsChecked = st.AutoOperatorEdit;
			tbOperator.Text = st.Operator;
			tbCallSign.Text = st.CallSign;
			tbZipCode.Text = st.ZipCode;
			tbAddress.Text = st.Address;
			tbPhone.Text = st.Phone;
			tbName.Text = st.Name;
			tbMail.Text = st.Mail;
			tbLicenserName.Text = st.LicenserName;
			cbLincenserLicense.Text = st.LicenserLicense;
			tbPlace.Text = st.Place;
			tbSupply.Text = st.Supply;
			tbEquip.Text = st.Equipment;
			tbComment.Text = st.Comment;
			tbOath.Text = st.Oath;
		}

		private Setting DoSave() {
			Setting st = new Setting();
			st.Version = "0.8.1";

			st.ContestName = cbContestName.Text;
			foreach(var dp in Plugins) {
				if(dp.ContestName != st.ContestName) continue;
				st.CategoryCode = dp.GetCategoryCodeByPower(cbCategory.Text.Substring(1, cbCategory.Text.IndexOf(")") - 1), ConvertCategoryPowerToEnum());
				break;
			}
			st.CategoryName = cbCategory.Text.Substring(cbCategory.Text.IndexOf(")") + 1);
			st.CategoryPower = ConvertCategoryPowerToStr();
			st.Coefficient = cbCoefficient.IsChecked == true ? true : false;
			st.ContestNo = tbContestNo.Text;
			st.PowerType = rbNormal.IsChecked == true ? "定格出力" : "実測出力";
			st.PowerValue = cbPowerValue.Text;
			st.AutoOperatorEdit = cbAutoOperator.IsChecked == true ? true : false;
			if(!st.AutoOperatorEdit) {
				st.Operator = tbOperator.Text;
			}
			st.CallSign = tbCallSign.Text;
			st.ZipCode = tbZipCode.Text;
			st.Address = tbAddress.Text;
			st.Phone = tbPhone.Text;
			st.Name = tbName.Text;
			st.Mail = tbMail.Text;
			st.LicenserName = tbLicenserName.Text;
			st.LicenserLicense = cbLincenserLicense.Text;
			st.Place = tbPlace.Text;
			st.Supply = tbSupply.Text;
			st.Equipment = tbEquip.Text;
			st.Comment = tbComment.Text;
			st.Oath = tbOath.Text;

			return st;
		}

		private string ConvertCategoryPowerToStr() {
			if(rbLicense.IsChecked == true) return "License";
			if(rb100w.IsChecked == true) return "100W";
			if(rb1020w.IsChecked == true) return "10(20)W";
			return "5W";
		}

		private ContestPower ConvertCategoryPowerToEnum() {
			if(rbLicense.IsChecked == true) return ContestPower.License;
			if(rb100w.IsChecked == true) return ContestPower.Hundred;
			if(rb1020w.IsChecked == true) return ContestPower.TwentyTen;
			return ContestPower.Five;
		}

		private ContestPower GetContestPowerEnumFromStr(string Str) {
			switch(Str) {
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
			switch(Str) {
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
			if(selected == null) return;
			FillCategory(selected.Content as string);
			foreach(var dp in Plugins) {
				if(dp.ContestName != selected.Content as string) continue;
				if(dp.Coefficient) {
					cbCoefficient.IsEnabled = true;
					cbCoefficient.IsChecked = true;
				} else {
					cbCoefficient.IsEnabled = false;
					cbCoefficient.IsChecked = false;
				}
				break;
			}
		}

		private void cbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			var cb = sender as ComboBox;
			var selected = cb.SelectedItem as ComboBoxItem;
			if(selected == null) return;
			foreach(var dp in Plugins) {
				if(dp.ContestName != ((cbContestName as ComboBox).SelectedItem as ComboBoxItem).Content as string) continue;
				string cont = selected.Content as string;
				ChangeEnablalContestPower(dp.AllowenPowerInCategoryCode(cont.Substring(1, cont.IndexOf(")") - 1)));
				break;
			}
		}

		private void FillCategory(string ContestName) {
			cbCoefficient.IsEnabled = false;
			cbCoefficient.IsChecked = false;
			ChangeEnablalContestPower(ContestPower.None);
			cbCategory.Items.Clear();
			foreach(var dp in Plugins) {
				if(dp.ContestName != ContestName) continue;
				tbOath.Text = dp.Oath;
				foreach(var cc in dp.ContestCategolies) {
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
			if(cp == ContestPower.None) return;
			if(cp.HasFlag(ContestPower.License)) rbLicense.IsEnabled = true;
			if(cp.HasFlag(ContestPower.Hundred)) rb100w.IsEnabled = true;
			if(cp.HasFlag(ContestPower.TwentyTen)) rb1020w.IsEnabled = true;
			if(cp.HasFlag(ContestPower.Five)) rb5w.IsEnabled = true;
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
			if(eq.isChanged) tbEquip.Text = eq.EquipStr;
		}
	}
}