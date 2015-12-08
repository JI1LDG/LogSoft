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
			private List<Area> _areaData;
			public List<Area> AreaData {
				get {
					if (_areaData == null) {
						_areaData = SearchUtil.GetAreaListFromFile("AllKanagawa");
					}
					return _areaData;
				}
			}
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
				CheckAnv();
				CheckStationAvailable();
				CheckScn();
				CheckRcn();
				SetErrorStr();
			}

			private void CheckAnv() {
				if (Log.CallSign[0] != '8') return;
				string cs = defSearch.GetCallSignBesideStroke(Log.CallSign);
				if (!AnvStation.Contains(cs)) {
					ErrorReason.SetError(_er, "CannotConfirmAnvsta");
				}
			}

			private void CheckStationAvailable() {
				if (Station == null && Log.CallSign[0] != '8') {
					ErrorReason.SetError(_er, "FailedToGetData");
				}
			}

			private void CheckScn() {
				string chk;
				if (Log.Mode != "CW") chk = Log.SendenContestNo.Substring(2);
				else chk = Log.SendenContestNo.Substring(3);
				string cn;
				if (Config.IsSubCN && defCTESTWIN.GetFreqNum(Log.Frequency) >= 13) {
					cn = Config.SubContestNo;
				} else {
					cn = Config.ContestNo;
				}
				if (chk != cn || SearchUtil.ContestNoIsWithPower(Log.SendenContestNo)) {
					ErrorReason.SetError(_er, "InvalidSentCn");
				}
			}

			private void CheckRcn() {
				if (SearchUtil.ContestNoIsWithPower(Log.ReceivenContestNo)) {
					ErrorReason.SetError(_er, "InvalidReceivedCn");
				}

				if (Config.CategoryCode[0] == 'X' && SearchUtil.GetPrefNoFromRcn(Log) != "11") {
					ErrorReason.SetError(_er, "NotInKanagawa");
					Log.Point = 0;
				} else Log.Point = 1;

				if(SearchUtil.CallSignIsStroke(Log.CallSign)) {
					int RcnAreano = SearchUtil.GetAreaNoFromRcn(Log);
					if(RcnAreano == -1) RcnAreano = 1;
                    if(SearchUtil.GetAreaNoFromCallSign(Log.CallSign) != RcnAreano) {
						ErrorReason.SetError(_er, "UnmatchedCnWithPortable");
					}
				} else {
					var address = SearchUtil.GetAddressListOrSuggestFromContestAreaNo(AreaData, Station, Log, SearchUtil.GetContestAreaNoFromRcnDisPower(Log));
					if (address is string) {
						ErrorReason.SetError(_er, "UnexistedAreanoWithCn", address as string);
						return;
					}
					if (Station == null) return;
					foreach (var adr in address as List<string>) {
						foreach (var sa in SearchUtil.GetAddressListFromStationData(Station)) {
							if (sa.Contains(adr)) return;
						}
					}
					string ganfa = SearchUtil.GetAreaNoFromAddress(Station, AreaData);
					ErrorReason.SetError(_er, "UnmatchedCnWithAddress", ganfa);
				}
			}

			public void SetErrorStr() {
				Log.FailedStr = ErrorReason.GetFailedStr(_er);
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