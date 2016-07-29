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
			public string oath { get { return "私は，JARL制定のコンテスト規約および電波法令にしたがい運用した結果，ここに提出するサマリーシートおよびログシートなどが事実と相違ないものであることを，私の名誉において誓います。"; } }
			public bool isCoefficientEnabled { get { return false; } }
			public bool isSubCnEnabled { get { return false; } }
			public string contestName { get { return Property.ContestName; } }
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
			public List<CategoryData> contestCategories { get { return _contestCategolies; } }

			public ContestPower GetPowerAllowed(string Code) {
				return ContestPower.None;
			}

			public string GetCodeWithPower(string code, ContestPower power) {
				return code;
			}

			public string GetCodeDivPower(string code, ContestPower power) {
				return code;
			}
		}

		public class SearchLog : ISearch {
			private List<Area> _mainArea;
			public List<Area> listMainArea {
				get {
					if (_mainArea == null) {
						//_mainArea = Areano.GetList("AllKanagawa");
						_mainArea = Areano.GetMixedListFromPref(new string[] { "11" }, new string[] { "11" });
					}
					return _mainArea;
				}
			}
			public List<Area> listSubArea { get; }
			public string contestName { get { return Property.ContestName; } }
			public LogData log { get; set; }
			public StationData station { get; set; }
			public Setting config { get; set; }
			public string anvStr { get; set; }
			public bool isErrorAvailable {
				get {
					foreach (var e in listErr.Values) {
						if (e.IsSet) return true;
					}
					return false;
				}
			}

			private Dictionary<string, ErrorReason> listErr;
			private enum ExtraReason {
				NotInKanagawa,
			}

			public void check() {
				listErr = ErrorReason.GetInitial();
				ErrorReason.Put(listErr, ExtraReason.NotInKanagawa.ToString(), new ErrorReason(5, "神奈川県内の局ではありません。無効です。"));
				CheckLog();
				CheckScn();
				CheckRcn();
				setErrorStr();
			}

			private void CheckLog() {
				Anv.Check(log.Callsign, anvStr, listErr, station);
				if (station == null) {
					ErrorReason.Set(listErr, Reason.GetDataFailed.ToString());
				}
				log.Point = 1;
				switch (log.Freq) {
					case "10MHz":
					case "18MHz":
					case "24MHz":
					case "5600MHz":
						log.Point = 0;
						ErrorReason.Set(listErr, Reason.OutOfFreq.ToString());
						break;
					default: break;
				}
			}

			private void CheckScn() {
				Contestno.CheckSentCn(log, config.Contestno, ContestNo.GetVal(config.SentCnExtra, log.Freq), false, listErr);
			}

			private void CheckRcn() {
				if (!Callsign.IsJACallsign(log.Callsign)) {
					log.Point = 0;
					ErrorReason.Set(listErr, Reason.OmakuniNonJA.ToString());
					return;
				}
				if (Contestno.HasPower(log.ReceivedCn) == true) {
					ErrorReason.Set(listErr, Reason.InvalidReceivedCn.ToString());
				}

				var prefno = Contestno.GetPrefno(log);
				if (prefno.Length > 3) prefno = prefno.Substring(0, 2);
				if (config.CategoryCode[0] == 'X' && prefno != "11") {
					ErrorReason.Set(listErr, ExtraReason.NotInKanagawa.ToString());
					log.Point = 0;
				}
				var hasStroke = Callsign.HasStroke(log.Callsign);
				var areano = Contestno.GetAreano(log);
				var stationAreano = Areano.GetFromStation(station, listMainArea);
				if (!Freq.IsBeen(log.Freq)) {
					ErrorReason.Set(listErr, Reason.InvalidFreq.ToString());
				}
				if (Contestno.GetRegion(prefno) != Callsign.GetRegion(log.Callsign, hasStroke)) {
					ErrorReason.Set(listErr, Reason.RegionUnmatches.ToString());
				}
				if (!Station.IsMatched(areano, stationAreano) && station != null && !hasStroke) {
					ErrorReason.Set(listErr, Reason.AddressUnmatches.ToString(), Utils.ConvTostrarrFromList(stationAreano));
				}
				if (!Areano.IsBeen(listMainArea, areano)) {
					ErrorReason.Set(listErr, Reason.ReceivedCnUnexists.ToString(), Utils.ConvTostrarrFromList(Areano.GetFromList(Station.GetList(station), listMainArea)));
				}
			}

			public void setErrorStr() {
				log.FailedStr = ErrorReason.GetFailedStr(listErr);
			}
		}

		public class LogSummery : ISummery {
			public string contestName { get { return Property.ContestName; } }
			public Setting config { get; set; }
			public bool isScoreEdited { get { return false; } }
			public List<Multiply> listMulti { get; set; }
			public int freqNum { get; set; }
			public int areaMax { get; set; }

			public string getAreano(LogData Log) {
				return Contestno.GetAreano(Log);
			}

			public string getScoreStr() {
				return null;
			}
		}
	}
}