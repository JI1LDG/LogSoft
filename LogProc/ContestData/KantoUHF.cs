using System.Collections.Generic;
using LogProc.Definitions;
using LogProc.Interfaces;
using LogProc.Utilities;

namespace LogProc {
	namespace KantoUHF {
		public class Property {
			public static string ContestName { get { return "関東UHFコンテスト"; } }
			public static InterSet Intersets { get { return new InterSet(new ContestDefine(), new SearchLog(), new LogSummery()); } }
		}

		public class ContestDefine : IDefine {
			public string Oath { get { return "私は，JARL制定のコンテスト規約および電波法令にしたがい運用した結果，ここに提出するサマリーシートおよびログシートなどが事実と相違ないものであることを，私の名誉において誓います。"; } }
			public bool Coefficient { get { return false; } }
			public bool IsSubCN { get { return false; } }
			public string ContestName { get { return Property.ContestName; } }
			private List<CategoryData> _contestCategolies = new List<CategoryData>() {
			new CategoryData(){
				Name = "電話部門シングルオペマルチバンド",
				Code = "AM",
			},
			new CategoryData(){
				Name = "電信部門シングルオペ430MHz",
				Code = "A430",
			},
			new CategoryData(){
				Name = "電信部門シングルオペ1200MHzバンド",
				Code = "A1200",
			},
			new CategoryData(){
				Name = "電信部門シングルオペ2400MHzバンド",
				Code = "A2400",
			},
			new CategoryData(){
				Name = "電信部門シングルオペ5600MHzバンド",
				Code = "A5600",
			},
			new CategoryData(){
				Name = "電信部門シングルオペ10GHz",
				Code = "A10G",
			},
			new CategoryData(){
				Name = "電信電話部門シングルオペマルチバンド",
				Code = "BM",
			},
			new CategoryData(){
				Name = "電信電話部門シングルオペ430MHzバンド",
				Code = "B430",
			},
			new CategoryData(){
				Name = "電信電話部門シングルオペ1200MHzバンド",
				Code = "B1200",
			},
			new CategoryData(){
				Name = "電信電話部門シングルオペ2400MHzバンド",
				Code = "B2400",
			},
			new CategoryData(){
				Name = "電信電話部門シングルオペ5600MHzバンド",
				Code = "B5600",
			},
			new CategoryData(){
				Name = "電信電話部門シングルオペ10GHz",
				Code = "B10G",
			},
			new CategoryData() {
				Name = "SWL",
				Code = "C",
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
			private List<Area> _mainArea;
			public List<Area> MainArea
			{
				get
				{
					if (_mainArea == null) {
						_mainArea = SearchUtil.GetAreaListFromFile("ACAG");
					}
					return _mainArea;
				}
			}
			public List<Area> SubArea { get { return null; } }
			public string ContestName { get { return Property.ContestName; } }
			public LogData Log { get; set; }
			public StationData Station { get; set; }
			public Setting Config { get; set; }
			public string AnvStation { get; set; }
			public bool isErrorAvailable
			{
				get
				{
					foreach (var e in _er) {
						if (e.IsSet) return true;
					}
					return false;
				}
			}

			private List<ErrorReason> _er;

			public void DoCheck() {
				_er = ErrorReason.GetInitial();
				ErrorReason.PutError(_er, new ErrorReason(5, "NotInKanto", "関東圏の局ではありません。無効です。"));
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
				Log.Point = 1;
				if (defCTESTWIN.GetFreqNum(Log.Frequency) < defCTESTWIN.GetFreqNum("430MHz")) {
					Log.Point = 0;
					ErrorReason.SetError(_er, "OutOfFrequency");
				}
			}

			private void CheckScn() {
				string chk = SearchUtil.GetRSTVal(Log)[0];
				var cn = (Config.ContestNo);
				if (chk != cn || SearchUtil.CnHasPower(Log.SendenContestNo) == true) {
					ErrorReason.SetError(_er, "InvalidSentCn");
				}
			}

			private void CheckRcn() {
				if (SearchUtil.CnHasPower(Log.ReceivenContestNo) == true) {
					ErrorReason.SetError(_er, "InvalidReceivedCn");
				}

				//59##L
				var prefno = SearchUtil.GetPrefno(Log, true);
				if(10 > int.Parse(prefno) || int.Parse(prefno) > 17) {
					ErrorReason.SetError(_er, "NotInKanto");
					Log.Point = 0;
				}
				var hasStroke = SearchUtil.CallsignHasStroke(Log.CallSign);
				//11 -> 1
				var arearegion = SearchUtil.GetRegionFromCn(prefno);
				//JA#YPZ JA1YPZ/#
				var callregion = SearchUtil.GetRegionFromCallSign(Log.CallSign, hasStroke);
				var areano = SearchUtil.GetAreano(Log);
				var areanoExists = SearchUtil.AreanoExists(MainArea, areano);
				var stationAddress = SearchUtil.GetStationAddressList(Station);
				var staAddrStr = SearchUtil.ConvToStrFromList(SearchUtil.GetAreanoFromAddressList(stationAddress, MainArea));
				var stationAreano = SearchUtil.GetAreanoFromStation(Station, MainArea);
				var staAreanoStr = SearchUtil.ConvToStrFromList(stationAreano);
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
