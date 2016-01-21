using System.Collections.Generic;
using LogProc.Definitions;
using LogProc.Interfaces;
using LogProc.Utilities;

namespace LogProc {
	namespace SixMAndDown {
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
			private bool SubSelecter { get { return defCTESTWIN.GetFreqNum(Log.Frequency) < defCTESTWIN.GetFreqNum("2400MHz"); } }
			private List<Area> _mainArea;
			public List<Area> MainArea {
				get {
					if (_mainArea == null) {
						_mainArea = SearchUtil.GetAreaListFromFile("Prefectures");
					}
					return _mainArea;
				}
			}
			private List<Area> _subArea;
			public List<Area> SubArea {
				get {
					if (_subArea == null) {
						_subArea = SearchUtil.GetAreaListFromFile("ACAG");
					}
					return _subArea;
				}
			}
			public string ContestName { get { return Property.ContestName; } }
			public LogData Log { get; set; }
			public StationData Station { get; set; }
			public Setting Config { get; set; }
			public string AnvStation { get; set; }
			public bool isErrorAvailable {
				get {
					foreach (var e in _er) {
						if (e.IsSet) return true;
					}
					return false;
				}
			}

			private List<ErrorReason> _er;

			public void DoCheck() {
				_er = ErrorReason.GetInitial();
				CheckLog();
				CheckScn();
				CheckRcn();
				SetErrorStr();
			}

			private void CheckLog() {
				SearchUtil.AnvstaChk(Log.CallSign, AnvStation, _er, Station);
				if (Station == null) {
					ErrorReason.SetError(_er, "FailedToGetData");
				}
				if (SubSelecter) Log.Point = 1;
				else Log.Point = 2;
				if(defCTESTWIN.GetFreqNum(Log.Frequency) < defCTESTWIN.GetFreqNum("50MHz")) {
					Log.Point = 0;
					ErrorReason.SetError(_er, "OutOfFrequency");
				}
			}

			private void CheckScn() {
				string chk = SearchUtil.GetRSTVal(Log)[0];
				var cn = (SubSelecter ? Config.ContestNo : Config.SubContestNo);
				var excn = ContestNo.GetVal(Config.ScnExtra, Log.Frequency);
				if (excn != null) cn = excn;
				if (chk != cn || SearchUtil.CnHasPower(Log.SendenContestNo) == false) {
					ErrorReason.SetError(_er, "InvalidSentCn");
				}
			}

			private void CheckRcn() {
				if (!SearchUtil.IsJPCallSign(Log.CallSign)) {
					Log.Point = 0;
					ErrorReason.SetError(_er, "OmakuniExceptJA");
					return;
				}
				if (SearchUtil.CnHasPower(Log.ReceivenContestNo) == false) {
					ErrorReason.SetError(_er, "InvalidReceivedCn");
				}

				//59##L
				var prefno = SearchUtil.GetPrefno(Log, SubSelecter ? false : true);
				var hasStroke = SearchUtil.CallsignHasStroke(Log.CallSign);
				//11 -> 1
				var arearegion = SearchUtil.GetRegionFromCn(prefno);
				//JA#YPZ JA1YPZ/#
				var callregion = SearchUtil.GetRegionFromCallSign(Log.CallSign, hasStroke);
				var areano = SearchUtil.GetAreano(Log);
				var areanoExists = SearchUtil.AreanoExists(SubSelecter ? MainArea : SubArea, areano);
				var stationAddress = SearchUtil.GetStationAddressList(Station);
				var staAddrStr = SearchUtil.ConvToStrFromList(SearchUtil.GetAreanoFromAddressList(stationAddress, MainArea));
				var stationAreano = SearchUtil.GetAreanoFromStation(Station, SubSelecter ? MainArea : SubArea);
				var staAreanoStr = SearchUtil.ConvToStrFromList(stationAreano);
				var freqchk = SearchUtil.FrequencyChk(Log.Frequency);
				if (freqchk == false) {
					ErrorReason.SetError(_er, "UnexistedFrequency");
				}
				if (arearegion != callregion) {
					ErrorReason.SetError(_er, "UnmatchedRegion");
				}
				if (SearchUtil.AreanoMatches(areano, stationAreano) == false && Station != null && hasStroke == false) {
					ErrorReason.SetError(_er, "UnmatchedCnWithAddress", staAreanoStr);
				}
				if (areanoExists == false) {
					ErrorReason.SetError(_er, "UnexistedAreanoWithCn", staAddrStr);
				}
			}
			
			public void SetErrorStr() {
				Log.FailedStr = ErrorReason.GetFailedStr(_er);
			}
		}

		public class LogSummery : ISummery {
			public string ContestName { get { return Property.ContestName; } }
			public Setting Config { get; set; }
			public bool isEditenScore { get { return false; } }
			public List<Multiply> Multi { get; set; }
			public int FreqNum { get; set; }
			public int AreaMax { get; set; }

			public string GetContestAreaNoFromRcn(LogData Log) {
				return SearchUtil.GetAreano(Log);
			}

			public string GetScoreStr() {
				return null;
			}
		}
	}
}