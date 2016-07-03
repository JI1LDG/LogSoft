using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace LogProc {
	namespace Definitions {
		public class defs {
			public const string SettingVer = "0.8.53";
			public static string[][] PrefList() {
				string[][] tmp = new string[][] {
				new string[] {"長野", "新潟"},
				new string[] {"東京", "神奈川", "千葉", "埼玉", "群馬", "栃木", "茨城", "山梨"},
				new string[] {"愛知", "静岡", "岐阜", "三重"},
				new string[] {"大阪", "兵庫", "京都", "奈良", "滋賀", "和歌山"},
				new string[] {"岡山", "広島", "山口", "鳥取", "島根"},
				new string[] {"香川", "愛媛", "高知", "徳島"},
				new string[] {"福岡", "佐賀", "長崎", "熊本", "大分", "宮崎", "鹿児島", "沖縄"},
				new string[] {"宮城", "福島", "岩手", "青森", "秋田", "山形"},
				new string[] {"北海道"},
				new string[] {"石川", "福井", "富山"}
				};
			return tmp;
				}
		}

		public enum ModeStr {
			CW, RTTY, SSB, FM, AM,
		}

		public enum FreqStr {
			f19M, f35M, f7M, f10M, f14M, f18M, f21M, f24M, f28M, f50M, f144M, f430M, f1200M, f2400M, f5600M
		}

		public enum CnStr {
			RstFltred, Area, Pref, Region, None,
		}

		public static class CtestwinDefs {
			public const int CallLen = 20;
			public const int NumLen = 30;
			public const int RemLen = 50;
			public const int FreqNum = 23;
		}

		public class ScnData {
			public string Freq { get; set; }
			public string SentCn { get; set; }
		}

		public class EquipData {
			public string Name { get; set; }
			public string Rem { get; set; }
			public string Category { get; set; }
		}

		public class EquipGrid {
			public bool IsEquiped { get; set; }
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
			public bool IsSearched { get; set; }
			public bool IsFinded { get; set; }
			public DateTime Date { get; set; }
			private string callsign;
			public string Callsign { get { return callsign; } set { callsign = value.ToUpper(); } }
			private string sentCn;
			public string SentCn { get { return sentCn; } set { sentCn = value.ToUpper(); } }
			private string receivedCn;
			public string ReceivedCn { get { return receivedCn; } set { receivedCn = value.ToUpper(); } }
			public string Mode { get; set; }
			public string Freq { get; set; }
			public string Operator { get; set; }
			public string Rem { get; set; }
			public int Point { get; set; }
			private string failedStr;
			public string FailedStr {
				get {
					if(failedStr == null) return "";
					else return failedStr;
				}
				set {
					if(failedStr != value) {
						failedStr = value;
						OnPropertyChanged("FailedStr");
					}
				}
			}
			public bool IsRate5 { get { return this.FailedStr.Contains("Lv.5"); } }
			public bool IsRate4 { get { return this.FailedStr.Contains("Lv.4"); } }
			public bool IsRate3 { get { return this.FailedStr.Contains("Lv.3"); } }
			public bool IsRate2 { get { return this.FailedStr.Contains("Lv.2"); } }
			public bool IsRate1 { get { return this.FailedStr.Contains("Lv.1"); } }
			public bool IsRate0 { get { return !(IsRate5 || IsRate4 || IsRate3 || IsRate2 || IsRate1); } }

			public event PropertyChangedEventHandler PropertyChanged;
			protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) {
				if(PropertyChanged != null) PropertyChanged(this, e);
			}
			protected virtual void OnPropertyChanged(string name) {
				if(PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

		public class Multiply {
			public int Freq { get; set; }
			public string Areano { get; set; }
			public int Num { get; set; }
			public int Point { get; set; }
		}

		public class StationData {
			public string Callsign { get; set; }
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
			public bool IsCoefficientEnabled { get; set; }
			public string Contestno { get; set; }
			public string SubContestno { get; set; }
			public string SentCnExtra { get; set; }
			public bool IsSubCnEnabled { get; set; }
			public string PowerType { get; set; }
			private string powerVal;
			public string PowerVal { get { return powerVal; } set { powerVal = new Regex(@"[^0-9]").Replace(value, ""); } }
			public bool IsAutoOperatorEditEnabled { get; set; }
			public string Operator { get; set; }
			public string Callsign { get; set; }
			public string ZipCode { get; set; }
			public string Address { get; set; }
			public string Phone { get; set; }
			public string Name { get; set; }
			public string Mail { get; set; }
			public string LicenserName { get; set; }
			public string LicenserLicense { get; set; }
			public string Place { get; set; }
			public string Supply { get; set; }
			public string Equip { get; set; }
			public string UseType { get; set; }
			public string Comment { get; set; }
			public string Oath { get; set; }
		}

		namespace v0810 {
			public class Setting {
				public string Version { get; set; }
				public string ContestName { get; set; }
				public string CategoryCode { get; set; }
				public string CategoryName { get; set; }
				public string CategoryPower { get; set; }
				public bool Coefficient { get; set; }
				public string ContestNo { get; set; }
				public string SubContestNo { get; set; }
				public string ScnExtra { get; set; }
				public bool IsSubCN { get; set; }
				public string PowerType { get; set; }
				public string PowerValue { get; set; }
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
		}

		public class AlyscData {
			public string Name { get; set; }
			public int Num { get; set; }
			public int False { get; set; }
			public int Rate0 { get; set; }
			public int Rate1 { get; set; }
			public int Rate2 { get; set; }
			public int Rate3 { get; set; }
			public int Rate4 { get; set; }
			public int Rate5 { get; set; }
			public string Percentage { get { return ((double)(Num - False) / Num * 100).ToString("F2") + "%"; } }

			public AlyscData(string name) {
				Name = name;
			}

			public void Set(LogData log) {
				Num++;
				if (log.IsRate0) Rate0++;
				else {
					False++;
					if (log.IsRate1) Rate1++;
					if (log.IsRate2) Rate2++;
					if (log.IsRate3) Rate3++;
					if (log.IsRate4) Rate4++;
					if (log.IsRate5) Rate5++;
				}
			}
		}

		public class CategoryData {
			public string Name { get; set; }
			public string Code { get; set; }

			public CategoryData() { }
		}

		public enum Reason {
			ReceivedCnUnexists, OutOfFreq, InvalidFreq, OmakuniNonJA, AddressUnmatches, RegionUnmatches, InvalidReceivedCn, GetDataFailed, AnvUnchecked, InvalidSentCn, Duplicate
		}

		public class ErrorReason {
			public int Level { get; set; }
			public string ErrorStr { get; set; }
			public string Suggest { get; set; }
			public bool IsSet { get; set; }

			public ErrorReason() { }
			public ErrorReason(int level, string errorStr, bool isSet = false) {
				this.Level = level;
				this.ErrorStr = errorStr;
				this.IsSet = isSet;
			}

			public static void Put(Dictionary<string, ErrorReason> listErr, string name, ErrorReason addedErr) {
				listErr[name] = addedErr;
			}

			public static void Remove(Dictionary<string, ErrorReason> listErr, string removedName) {
				listErr.Remove(removedName);
			}

			public static Dictionary<string, ErrorReason> GetInitial() {
				var tmp = new Dictionary<string, ErrorReason>();

				tmp[Reason.ReceivedCnUnexists.ToString()] = new ErrorReason(5, "コンテストナンバーと対応する、地域番号が存在しません。\r\nもしかして: [Suggest]", false);
				tmp[Reason.OutOfFreq.ToString()] = new ErrorReason(5, "コンテストで使用される周波数ではない、もしくは時間外です。", false);
				tmp[Reason.InvalidFreq.ToString()] = new ErrorReason(5, "周波数が不正です。ログに出力されません。", false);
				tmp[Reason.OmakuniNonJA.ToString()] = new ErrorReason(5, "外国局は得点の対象外です。", false);
				tmp[Reason.Duplicate.ToString()] = new ErrorReason(5, "データが重複しています。", false);
				tmp[Reason.AddressUnmatches.ToString()] = new ErrorReason(4, "無線局常置場所とコンテストナンバーが一致しません。\r\nもしかして: [Suggest]", false);
				tmp[Reason.RegionUnmatches.ToString()] = new ErrorReason(4, "エリアナンバーとコンテストナンバーが一致しません。", false);
				tmp[Reason.InvalidReceivedCn.ToString()] = new ErrorReason(3, "相手局コンテストナンバーが不正です。", false);
				tmp[Reason.GetDataFailed.ToString()] = new ErrorReason(3, "データ取得失敗しました。手動で調べてください。", false);
				tmp[Reason.AnvUnchecked.ToString()] = new ErrorReason(2, "記念局確認ができませんでした。", false);
				tmp[Reason.InvalidSentCn.ToString()] = new ErrorReason(1, "自局コンテストナンバーが不正です。", false);

				return tmp;
			}

			public static void Set(Dictionary<string, ErrorReason> listErr, string errorName, string suggestStr = null, bool isSet = true) {
				ErrorReason er = new ErrorReason();
				if (listErr.TryGetValue(errorName, out er)) {
					listErr[errorName].IsSet = isSet;
					listErr[errorName].Suggest = suggestStr;
					return;
				}
				System.Console.WriteLine("SetError Nothing: " + errorName + "\r\n");
				throw new UnexistError() { errName = errorName };
			}

			private class UnexistError : System.Exception {
				public string errName { private get; set; }
				public override string Message {
					get {
						return "SetError: 定義されていないエラー(" + errName + ")がセットされました。";
					}
				}
			}

			public static string GetFailedStr(Dictionary<string, ErrorReason> listErr) {
				string ret = "";

				int count = 0;
				foreach(var l in listErr.Values) {
					if (l.IsSet) {
						if (count != 0) ret += "\r\n";
						count++;
						ret += "Lv." + l.Level + ": ";
						ret += l.ErrorStr.Replace("[Suggest]", l.Suggest);
					}
				}

				return ret;
			}
		}

		public enum ContestCategoryPower {
			License = 0x01, Hundred = 0x02, TwentyTen = 0x04, Five = 0x08, None = 0x00,
		}
	}
}