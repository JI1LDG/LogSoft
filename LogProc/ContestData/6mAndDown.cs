using System.Collections.Generic;
using LogProc.Definitions;
using LogProc.Interfaces;
using LogProc.Utilities;

namespace LogProc {
	namespace SixMAndDown {
		public enum defErrorReason {
			None = 0x00, FailedGetStation = 0x01, ScnError = 0x02, PortableNCN = 0x04, AddressNCN = 0x08, RcnError = 0x10, DavaileCN = 0x40, AnvStation = 0x80, NonAnvStation = 0x0100,
		}

		public class Property {
			public static string ContestName { get { return "6m AND DOWNコンテスト"; } }
			public static InterSet Intersets { get { return new InterSet(new ContestDefine(), new SearchLog(), new LogSummery()); } }
		}

		public class ContestDefine : IDefine {
			public string Oath { get { return "私は，JARL制定のコンテスト規約および電波法令にしたがい運用した結果，ここに提出するサマリーシートおよびログシートなどが事実と相違ないものであることを，私の名誉において誓います。"; } }
			public bool Coefficient { get { return false; } }
			public bool IsSubCN { get { return true; } }
			public string ContestName { get { return Property.ContestName; } }
			private List<CategoryData> _contestCategolies = new List<CategoryData>() {
			new CategoryData(){
				Name = "電話部門シングルオペオールバンド",
				Code = "PA",
			},
			new CategoryData(){
				Name = "電話部門シングルオペニューカマー",
				Code = "PN",
			},
			new CategoryData(){
				Name = "電話部門マルチオペオールバンド",
				Code = "PMA",
			},
			new CategoryData(){
				Name = "電信部門シングルオペオールバンド",
				Code = "CA",
			},
			new CategoryData(){
				Name = "電信部門シングルオペ50MHzバンド",
				Code = "C50",
			},
			new CategoryData(){
				Name = "電信部門シングルオペ144MHzバンド",
				Code = "C144",
			},
			new CategoryData(){
				Name = "電信部門シングルオペ430MHzバンド",
				Code = "C430",
			},
			new CategoryData(){
				Name = "電信部門シングルオペ1200MHzバンド",
				Code = "C1200",
			},
			new CategoryData(){
				Name = "電信部門シングルオペ2400MHzバンド",
				Code = "C2400",
			},
			new CategoryData(){
				Name = "電信部門シングルオペ5600MHzバンド",
				Code = "C5600",
			},
			new CategoryData(){
				Name = "電信部門シングルオペ10.1GHzバンド以上",
				Code = "C10G",
			},
			new CategoryData(){
				Name = "電信部門シングルオペシルバー",
				Code = "CS",
			},
			new CategoryData(){
				Name = "電信部門シングルオペQRP",
				Code ="CP",
			},
			new CategoryData(){
				Name = "電信部門マルチオペオールバンド",
				Code = "CMA",
			},
			new CategoryData(){
				Name = "電信電話部門シングルオペオールバンド",
				Code = "XA",
			},
			new CategoryData(){
				Name = "電信電話部門シングルオペ50MHzバンド",
				Code = "X50",
			},
			new CategoryData(){
				Name = "電信電話部門シングルオペ144MHzバンド",
				Code = "X144",
			},
			new CategoryData(){
				Name = "電信電話部門シングルオペ430MHzバンド",
				Code = "X430",
			},
			new CategoryData(){
				Name = "電信電話部門シングルオペ1200MHzバンド",
				Code = "X1200",
			},
			new CategoryData(){
				Name = "電信電話部門シングルオペ2400MHzバンド",
				Code = "X2400",
			},
			new CategoryData(){
				Name = "電信電話部門シングルオペ5600MHzバンド",
				Code = "X5600",
			},
			new CategoryData(){
				Name = "電信電話部門シングルオペ10.1GHzバンド以上",
				Code = "X10G",
			},
			new CategoryData(){
				Name = "電信電話部門シングルオペシルバー",
				Code = "XS",
			},
			new CategoryData(){
				Name = "電信電話部門シングルオペQRP",
				Code = "XP",
			},
			new CategoryData(){
				Name = "電信電話部門シングルオペSWL",
				Code = "XSWL",
			},
			new CategoryData(){
				Name = "電信電話部門マルチオペオールバンド",
				Code = "XMA",
			},
			new CategoryData(){
				Name = "電信電話部門マルチオペジュニア",
				Code = "XMJ",
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
				_areaData = SearchUtil.GetAreaListFromFile("Prefectures");
				ACAGAreaData = SearchUtil.GetAreaListFromFile("ACAG");
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
				if(chk != cn || !SearchUtil.ContestNoIsWithPower(Log.SendenContestNo)) {
					_der |= defErrorReason.ScnError;
				}
			}

			private void CheckRcn() {
				if(!SearchUtil.ContestNoIsWithPower(Log.ReceivenContestNo)) {
					_der |= defErrorReason.RcnError;
				}
				if(defCTESTWIN.GetFreqNum(Log.Frequency) >= 13) {
					Log.Point = 2;
				} else Log.Point = 1;

				if(SearchUtil.CallSignIsStroke(Log.CallSign)) {
					if(SearchUtil.GetAreaNoFromCallSign(Log.CallSign) != SearchUtil.GetAreaNoFromRcn(Log)) {
						_der |= defErrorReason.PortableNCN;
					}
				} else {
					List<string> address;
					if((address = SearchUtil.GetAddressListFromContestAreaNo((defCTESTWIN.GetFreqNum(Log.Frequency) >= 13 ? ACAGAreaData : AreaData), Station, Log, SearchUtil.GetContestAreaNoFromRcnWithPower(Log))) == null) {
						_der |= defErrorReason.DavaileCN;
						return;
					}
					if(Station == null) return;
					foreach(var adr in address) {
						foreach(var sa in SearchUtil.GetAddressListFromStationData(Station)) {
							if(sa.Contains(adr)) return;
						}
					}
					string ganfa = SearchUtil.GetAreaNoFromAddress(Station, (defCTESTWIN.GetFreqNum(Log.Frequency) >= 13 ? ACAGAreaData : AreaData));
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
				return SearchUtil.GetContestAreaNoFromRcnWithPower(Log);
			}

			public string GetScoreStr() {
				return null;
			}
		}
	}
}