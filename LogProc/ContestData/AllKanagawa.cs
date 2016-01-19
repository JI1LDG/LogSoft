using System.Collections.Generic;
using LogProc.Definitions;
using LogProc.Interfaces;
using LogProc.Utilities;

namespace LogProc {
	namespace AllKanagawa {
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
			private List<Area> _mainArea;
			public List<Area> MainArea {
				get {
					if (_mainArea == null) {
						_mainArea = SearchUtil.GetAreaListFromFile("AllKanagawa");
					}
					return _mainArea;
				}
			}
			public List<Area> SubArea { get; }
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
				ErrorReason.PutError(_er, new ErrorReason(5, "NotInKanagawa", "神奈川県内の局ではありません。無効です。"));
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
				if (15 <= Log.Date.Hour && Log.Date.Hour <= 18) {
					switch (Log.Frequency) {
						case "14MHz": case "21MHz": case "28MHz":
						case "50MHz": case "1200MHz": case "2400MHz":
							return;
						default: break;
					}
				} else if (21 <= Log.Date.Hour && Log.Date.Hour < 24) {
					switch (Log.Frequency) {
						case "1.9MHz": case "3.5MHz": case "7MHz":
						case "144MHz": case "430MHz":
							return;
						default: break;
					}
				}
				Log.Point = 0;
				ErrorReason.SetError(_er, "OutOfFrequency");
			}

			private void CheckScn() {
				string chk = SearchUtil.GetRSTVal(Log)[0];
				var cn = Config.ContestNo;
				if (chk != cn || SearchUtil.CnHasPower(Log.SendenContestNo) == true) {
					ErrorReason.SetError(_er, "InvalidSentCn");
				}
			}

			private void CheckRcn() {
				if (SearchUtil.CnHasPower(Log.ReceivenContestNo) == true) {
					ErrorReason.SetError(_er, "InvalidReceivedCn");
				}

				//59##L
				var prefno = SearchUtil.GetPrefno(Log);
				if (prefno.Length > 3) prefno = prefno.Substring(0, 2);
				if (Config.CategoryCode[0] == 'X' && prefno != "11") {
					ErrorReason.SetError(_er, "NotInKanagawa");
					Log.Point = 0;
				} else Log.Point = 1;
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