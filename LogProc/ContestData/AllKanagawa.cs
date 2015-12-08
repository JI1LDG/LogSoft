using System.Collections.Generic;
using LogProc.Definitions;
using LogProc.Interfaces;
using LogProc.Utilities;

namespace LogProc {
	namespace AllKanagawa {
		public enum defErrorReason {
			None = 0x00, FailedGetStation = 0x01, ScnError = 0x02, PortableNCN = 0x04, AddressNCN = 0x08, RcnError = 0x10, DavaileCN = 0x40, AnvStation = 0x80, NonAnvStation = 0x0100,
			NotInKanagawa = 0x0200,
		}

		public class Property {
			public static string ContestName { get { return "オール神奈川コンテスト"; } }
			public static InterSet Intersets { get { return new InterSet(new ContestDefine(), new SearchLog(), new LogSummery()); } }
		}

		public class ContestDefine : IDefine {
			public string Oath { get { return "私は，JARL制定のコンテスト規約および電波法令にしたがい運用した結果，ここに提出するサマリーシートおよびログシートなどが事実と相違ないものであることを，私の名誉において誓います。"; } }
			public bool Coefficient { get { return false; } }
			public bool IsSubCN { get { return false; } }
			public string ContestName { get { return Property.ContestName; } }
			private List<CategoryData> _contestCategolies = new List<CategoryData>() {
			new CategoryData(){
				Name = "電信部門シングルオペオールバンド(県内局)",
				Code = "KCSA",
			},
			new CategoryData(){
				Name = "電信部門シングルオペジュニア・オールバンド(県内局)",
				Code = "KCSJA",
			},
			new CategoryData(){
				Name = "電信部門シングルオペHF-Low(1.9/3.5/7MHz)バンド(県内局)",
				Code = "KCSHL",
			},
			new CategoryData(){
				Name = "電信部門シングルオペHF-High(14/21/28MHz)バンド(県内局)",
				Code = "KCSHH",
			},
			new CategoryData(){
				Name = "電信部門シングルオペ50MHzバンド(県内局)",
				Code = "KCS50",
			},
			new CategoryData(){
				Name = "電信部門シングルオペ144MHzバンド(県内局)",
				Code = "KCS144",
			},
			new CategoryData(){
				Name = "電信部門シングルオペ430MHzバンド(県内局)",
				Code = "KCS430",
			},
			new CategoryData(){
				Name = "電信部門シングルオペUHF(1200/2400MHz)バンド(県内局)",
				Code = "KCSU",
			},
			new CategoryData(){
				Name = "電信部門マルチオペオールバンド(県内局)",
				Code = "KCMA",
			},
			new CategoryData(){
				Name = "電信部門マルチオペジュニア・オールバンド(県内局)",
				Code = "KCMJA",
			},
			new CategoryData(){
				Name = "電信電話部門マルチオペオールバンド(県内局)",
				Code = "KXSA",
			},
			new CategoryData(){
				Name = "電信電話部門マルチオペジュニア・オールバンド(県内局)",
				Code = "KXSJA",
			},
			new CategoryData(){
				Name = "電信電話部門マルチオペHF-Low(1.9/3.5/7MHz)バンド(県内局)",
				Code = "KXSHL",
			},
			new CategoryData(){
				Name = "電信電話部門マルチオペHF-High(14/21/28MHz)バンド(県内局)",
				Code = "KXSHH",
			},
			new CategoryData(){
				Name = "電信電話部門マルチオペ50MHzバンド(県内局)",
				Code = "KXS50",
			},
			new CategoryData(){
				Name = "電信電話部門マルチオペ144MHzバンド(県内局)",
				Code = "KXS144",
			},
			new CategoryData(){
				Name = "電信電話部門マルチオペ430MHzバンド(県内局)",
				Code = "KXS430",
			},
			new CategoryData(){
				Name = "電信電話部門マルチオペUHF(1200/2400MHz)バンド(県内局)",
				Code = "KXSU",
			},
			new CategoryData(){
				Name = "電信電話部門マルチオペオールバンド(県内局)",
				Code = "KXMA",
			},
			new CategoryData(){
				Name = "電信電話部門マルチオペジュニア・オールバンド(県内局)",
				Code = "KXMJA",
			},
			new CategoryData(){
				Name = "電信部門シングルオペオールバンド(県外局)",
				Code = "XCSA",
			},
			new CategoryData(){
				Name = "電信部門シングルオペジュニア・オールバンド(県外局)",
				Code = "XCSJA",
			},
			new CategoryData(){
				Name = "電信部門シングルオペHF-Low(1.9/3.5/7MHz)バンド(県外局)",
				Code = "XCSHL",
			},
			new CategoryData(){
				Name = "電信部門シングルオペHF-High(14/21/28MHz)バンド(県外局)",
				Code = "XCSHH",
			},
			new CategoryData(){
				Name = "電信部門シングルオペ50MHzバンド(県外局)",
				Code = "XCS50",
			},
			new CategoryData(){
				Name = "電信部門シングルオペ144MHzバンド(県外局)",
				Code = "XCS144",
			},
			new CategoryData(){
				Name = "電信部門シングルオペ430MHzバンド(県外局)",
				Code = "XCS430",
			},
			new CategoryData(){
				Name = "電信部門シングルオペUHF(1200/2400MHz)バンド(県外局)",
				Code = "XCSU",
			},
			new CategoryData(){
				Name = "電信部門マルチオペオールバンド(県外局)",
				Code = "XCMA",
			},
			new CategoryData(){
				Name = "電信部門マルチオペジュニア・オールバンド(県外局)",
				Code = "XCMJA",
			},
			new CategoryData(){
				Name = "電信電話部門マルチオペオールバンド(県外局)",
				Code = "XXSA",
			},
			new CategoryData(){
				Name = "電信電話部門マルチオペジュニア・オールバンド(県外局)",
				Code = "XXSJA",
			},
			new CategoryData(){
				Name = "電信電話部門マルチオペHF-Low(1.9/3.5/7MHz)バンド(県外局)",
				Code = "XXSHL",
			},
			new CategoryData(){
				Name = "電信電話部門マルチオペHF-High(14/21/28MHz)バンド(県外局)",
				Code = "XXSHH",
			},
			new CategoryData(){
				Name = "電信電話部門マルチオペ50MHzバンド(県外局)",
				Code = "XXS50",
			},
			new CategoryData(){
				Name = "電信電話部門マルチオペ144MHzバンド(県外局)",
				Code = "XXS144",
			},
			new CategoryData(){
				Name = "電信電話部門マルチオペ430MHzバンド(県外局)",
				Code = "XXS430",
			},
			new CategoryData(){
				Name = "電信電話部門マルチオペUHF(1200/2400MHz)バンド(県外局)",
				Code = "XXSU",
			},
			new CategoryData(){
				Name = "電信電話部門マルチオペオールバンド(県外局)",
				Code = "XXMA",
			},
			new CategoryData(){
				Name = "電信電話部門マルチオペジュニア・オールバンド(県外局)",
				Code = "XXMJA",
			},
		};
			public List<CategoryData> ContestCategolies { get { return _contestCategolies; } }

			public ContestPower AllowenPowerInCategoryCode(string Code) {
				return ContestPower.None;
			}

			public string GetCategoryCodeByPower(string Code, ContestPower Power) {
				return Code;
			}

			public string GetCategoryCodeDivPower(string Code, ContestPower Power) {
				return Code;
			}
		}

		public class SearchLog : ISearch {
			private List<Area> _areaData;
			public List<Area> AreaData { get { return _areaData; } }
			public List<Area> ACAGAreaData;
			public string ContestName { get { return Property.ContestName; } }
			private LogData _log;
			public LogData Log {
				get { return _log; }
				set { _log = value; }
			}
			private StationData _station;
			public StationData Station {
				get { return _station; }
				set { _station = value; }
			}
			private Setting _config;
			public Setting Config {
				get { return _config; }
				set { _config = value; }
			}
			private string _anvStation;
			public string AnvStation {
				get { return _anvStation; }
				set { _anvStation = value; }
			}
			public bool isErrorAvailable {
				get {
					if(_der == defErrorReason.None || _der == defErrorReason.AnvStation) return false;
					else return true;
				}
			}

			private defErrorReason _der;

			public SearchLog() {
				_areaData = SearchUtil.GetAreaListFromFile("AllKanagawa");
			}

			public void DoCheck() {
				_der = defErrorReason.None;
				CheckAnv();
				CheckStationAvailable();
				CheckScn();
				CheckRcn();
				SetErrorStr();
			}

			private void CheckAnv() {
				if(Log.CallSign[0] != '8') return;
				string cs = defSearch.GetCallSignBesideStroke(Log.CallSign);
				if(AnvStation.Contains(cs)) {
					_der |= defErrorReason.AnvStation;
				} else {
					_der |= defErrorReason.NonAnvStation;
				}
			}

			private void CheckStationAvailable() {
				if(Station == null && Log.CallSign[0] != '8') {
					_der |= defErrorReason.FailedGetStation;
				}
			}

			private void CheckScn() {
				string chk;
				if(Log.Mode != "CW") chk = Log.SendenContestNo.Substring(2);
				else chk = Log.SendenContestNo.Substring(3);
				string cn;
				if(Config.IsSubCN && defCTESTWIN.GetFreqNum(Log.Frequency) >= 13) {
					cn = Config.SubContestNo;
				} else {
					cn = Config.ContestNo;
				}
				if(chk != cn || SearchUtil.ContestNoIsWithPower(Log.SendenContestNo)) {
					_der |= defErrorReason.ScnError;
				}
			}

			private void CheckRcn() {
				if(SearchUtil.ContestNoIsWithPower(Log.ReceivenContestNo)) {
					_der |= defErrorReason.RcnError;
				}
				if(Config.CategoryCode[0] == 'X' && SearchUtil.GetPrefNoFromRcn(Log) != "11") {
					_der |= defErrorReason.NotInKanagawa;
					Log.Point = 0;
				} else Log.Point = 1;
				if(SearchUtil.CallSignIsStroke(Log.CallSign)) {
					int RcnAreano = SearchUtil.GetAreaNoFromRcn(Log);
					if(RcnAreano == -1) RcnAreano = 1;
                    if(SearchUtil.GetAreaNoFromCallSign(Log.CallSign) != RcnAreano) {
						_der |= defErrorReason.PortableNCN;
					}
				} else {
					List<string> address;
					if((address = SearchUtil.GetAddressListFromContestAreaNo(AreaData, Station, Log, SearchUtil.GetContestAreaNoFromRcnDisPower(Log))) == null) {
						_der |= defErrorReason.DavaileCN;
						return;
					}
					if(Station == null) return;
					foreach(var adr in address) {
						foreach(var sa in SearchUtil.GetAddressListFromStationData(Station)) {
							if(sa.Contains(adr)) return;
						}
					}
					string ganfa = SearchUtil.GetAreaNoFromAddress(Station, AreaData);
					if(ganfa != null) Log.ErrorString = ganfa;
					_der |= defErrorReason.AddressNCN;
				}
			}

			public void SetErrorStr() {
				string err = "";

				if(_der.HasFlag(defErrorReason.DavaileCN)) {
					err += "Lv.5:コンテストナンバーと対応する、地域番号が存在しません。\r\n";
					if(Log.ErrorString != "") err += "もしかして:" + Log.ErrorString + "\r\n";
				}
				if(_der.HasFlag(defErrorReason.NotInKanagawa)) {
					err += "Lv.5:神奈川県内の局ではありません。無効です。\r\n";
				}

				if(_der.HasFlag(defErrorReason.AddressNCN)) {
					err += "Lv.4:無線局常置場所とコンテストナンバーが一致しません。\r\n";
					if(Log.ErrorString != "") err += "もしかして:" + Log.ErrorString + "\r\n";
				}
				if(_der.HasFlag(defErrorReason.PortableNCN)) {
					err += "Lv.4:移動エリアとコンテストナンバーが一致しません。\r\n";
				}

				if(_der.HasFlag(defErrorReason.RcnError)) {
					err += "Lv.3:相手局コンテストナンバーが不正です。\r\n";
				}
				if(_der.HasFlag(defErrorReason.FailedGetStation)) {
					err += "Lv.3:データ取得失敗しました。手動で調べてください。\r\n";
				}

				if(_der.HasFlag(defErrorReason.NonAnvStation)) {
					err += "Lv.2:記念局確認ができませんでした。\r\n";
				}

				if(_der.HasFlag(defErrorReason.ScnError)) {
					err += "Lv.1:自局コンテストナンバーが不正です。\r\n";
				}

				if(_der.HasFlag(defErrorReason.AnvStation)) {
					err += "Lv.0:記念局です。\r\n";
				}
				Log.FailedStr = err;
			}
		}

		public class LogSummery : ISummery {
			public string ContestName { get { return Property.ContestName; } }
			private Setting _config;
			public Setting Config {
				get { return _config; }
				set { _config = value; }
			}
			public bool isEditenScore { get { return false; } }
			private List<Multiply> _multi;
			public List<Multiply> Multi {
				get { return _multi; }
				set { _multi = value; }
			}
			private int _freqNum;
			public int FreqNum {
				get { return _freqNum; }
				set { _freqNum = value; }
			}
			private int _areaMax;
			public int AreaMax {
				get { return _areaMax; }
				set { _areaMax = value; }
			}

			public string GetContestAreaNoFromRcn(LogData Log) {
				return SearchUtil.GetContestAreaNoFromRcnDisPower(Log);
			}

			public string GetScoreStr() {
				return null;
			}
		}
	}
}