using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.ComponentModel;
using LogProc.Interfaces;

namespace LogProc {
	namespace Definitions {
		public static class defCTESTWIN {
			public enum ModeStr {
				CW, RTTY, SSB, FM, AM,
			}
			public enum FreqStr {
				f19M, f35M, f7M, f10M, f14M, f18M, f21M, f24M, f28M, f50M, f144M, f430M, f1200M, f2400M, f5600M
			}

			public const int CallLen = 20;
			public const int NumLen = 30;
			public const int RemLen = 50;
			public const int FreqNum = 23;
			public static string GetFreqString(this FreqStr f) {
				string[] s = new string[] { "1.9MHz", "3.5MHz", "7MHz", "10MHz", "14MHz", "18MHz", "21MHz", "24MHz", "28MHz", "50MHz", "144MHz", "430MHz", "1200MHz", "2400MHz", "5600MHz" };
				return s[(int)f];
			}
			public static int GetFreqNum(string f) {
				string[] s = new string[] { "1.9MHz", "3.5MHz", "7MHz", "10MHz", "14MHz", "18MHz", "21MHz", "24MHz", "28MHz", "50MHz", "144MHz", "430MHz", "1200MHz", "2400MHz", "5600MHz" };
				int i = 0;
				for(;i < s.Length;i++) {
					if(s[i] == f) break;
				}
				if(i == s.Length) return -1;
				else return i;
			}
		}
		public static class defSearch {
			public static string GetCallSignBesideStroke(string CallSign) {
				if(CallSign.IndexOf("/") != -1) {
					CallSign = CallSign.Substring(0, CallSign.IndexOf("/"));
				}
				return CallSign;
			}
		}
		static class contest {
			public enum Multi {
				ACAG, Other,
			}
			public enum Operation {
				Single, Multi, Other,
			}
			public enum Category {
				Phone, CW, Both, Other,
			}
		}

		public class EquipData {
			public string Name { get; set; }
			public string Rem { get; set; }
			public string Category { get; set; }
		}

		public class EquipGrid {
			public bool isEquipen { get; set; }
			public EquipData Data { get; set; }
		}

		public class EquipNum {
			public string Name { get; set; }
			public int Num { get; set; }
		}

		public class WorkingData {
			public Setting Config { get; set; }
			public ObservableCollection<LogData> Log { get; set; }
		}

		public class LogData : INotifyPropertyChanged {
			public bool Searchen { get; set; }
			public bool Finden { get; set; }
			public DateTime Date { get; set; }
			private string _call;
			public string CallSign { get { return _call; } set { _call = value.ToUpper(); } }
			private string _mcn;
			public string SendenContestNo { get { return _mcn; } set { _mcn = value.ToUpper(); } }
			private string _ucn;
			public string ReceivenContestNo { get { return _ucn; } set { _ucn = value.ToUpper(); } }
			public string Mode { get; set; }
			public string Frequency { get; set; }
			public string Operator { get; set; }
			public string Rem { get; set; }
			public int Point { get; set; }
			private string _fstr;
			public string FailedStr {
				get {
					if(_fstr == null) return "";
					else return _fstr;
				}
				set {
					if(_fstr != value) {
						_fstr = value;
						OnPropertyChanged("FailedStr");
					}
				}
			}
			public string ErrorString;
			public bool Rate5 { get { return this.FailedStr.Contains("Lv.5"); } }
			public bool Rate4 { get { return this.FailedStr.Contains("Lv.4"); } }
			public bool Rate3 { get { return this.FailedStr.Contains("Lv.3"); } }
			public bool Rate2 { get { return this.FailedStr.Contains("Lv.2"); } }
			public bool Rate1 { get { return this.FailedStr.Contains("Lv.1"); } }
			public bool Rate0 { get { return !(Rate5 || Rate4 || Rate3 || Rate2 || Rate1); } }

			public event PropertyChangedEventHandler PropertyChanged;
			protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) {
				if(PropertyChanged != null) PropertyChanged(this, e);
			}
			protected virtual void OnPropertyChanged(string name) {
				if(PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

		public class Multiply {
			public int Frequency { get; set; }
			public string AreaNo { get; set; }
			public int Num { get; set; }
			public int Point { get; set; }
		}

		public class StationData {
			public string CallSign { get; set; }
			public string Name { get; set; }
			public string Address { get; set; }
			public string Url { get; set; }
		}

		public class Area {
			public string No { get; set; }
			public List<string> Address { get; set; }
		}

		public class Setting {
			public string Version { get; set; }
			public string ContestName { get; set; }
			public string CategoryCode { get; set; }
			public string CategoryName { get; set; }
			public string CategoryPower { get; set; }
			public bool Coefficient { get; set; }
			public string ContestNo { get; set; }
			public string SubContestNo { get; set; }
			public bool IsSubCN { get; set; }
			public string PowerType { get; set; }
			private string _pwvl;
			public string PowerValue { get { return _pwvl; } set { _pwvl = new Regex(@"[^0-9]").Replace(value, ""); } }
			public bool AutoOperatorEdit { get; set; }
			public string Operator { get; set; }
			public string CallSign { get; set; }
			public string ZipCode { get; set; }
			public string Address { get; set; }
			public string Phone { get; set; }
			public string Name { get; set; }
			public string Mail { get; set; }
			public string LicenserName { get; set; }
			public string LicenserLicense { get; set; }
			public string Place { get; set; }
			public string Supply { get; set; }
			public string Equipment { get; set; }
			public string Comment { get; set; }
			public string Oath { get; set; }
		}

		public class CategoryData {
			public string Name { get; set; }
			public string Code { get; set; }
		}
	}
}