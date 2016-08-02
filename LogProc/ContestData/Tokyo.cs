using System.Collections.Generic;
using LogProc.Definitions;
using LogProc.Interfaces;
using LogProc.Utilities;
using System.Linq;

namespace LogProc {
	namespace Tokyo {
		public class Property {
			public static string ContestName { get { return "東京コンテスト"; } }
			public static InterSet Intersets { get { return new InterSet(new ContestDefine(), new SearchLog(), new LogSummery()); } }
		}

		public class ContestDefine : IDefine {
			public string oath { get { return "私は，JARL制定のコンテスト規約および電波法令にしたがい運用した結果，ここに提出するサマリーシートおよびログシートなどが事実と相違ないものであることを，私の名誉において誓います。"; } }
			public bool isCoefficientEnabled { get { return false; } }
			public bool isSubCnEnabled { get { return false; } }
			public string contestName { get { return Property.ContestName; } }
			private List<CategoryData> _contestCategolies = new List<CategoryData>() {
				new CategoryData() {
					Name = "都内電信マルチバンド",
				},
				new CategoryData() {
					Name = "都内電信21MHzバンド",
				},
				new CategoryData() {
					Name = "都内電信28MHzバンド",
				},
				new CategoryData() {
					Name = "都内電信50MHzバンド",
				},
				new CategoryData() {
					Name = "都内電信144MHzバンド",
				},
				new CategoryData() {
					Name = "都内電信SWL",
				},
				new CategoryData() {
					Name = "都内電信電話マルチバンド",
				},
				new CategoryData() {
					Name = "都内電信電話21MHzバンド",
				},
				new CategoryData() {
					Name = "都内電信電話28MHzバンド",
				},
				new CategoryData() {
					Name = "都内電信電話50MHzバンド",
				},
				new CategoryData() {
					Name = "都内電信電話144MHzバンド",
				},
				new CategoryData() {
					Name = "都内電信電話SWL",
				},
				new CategoryData() {
					Name = "都外電信マルチバンド",
				},
				new CategoryData() {
					Name = "都外電信21MHzバンド",
				},
				new CategoryData() {
					Name = "都外電信28MHzバンド",
				},
				new CategoryData() {
					Name = "都外電信50MHzバンド",
				},
				new CategoryData() {
					Name = "都外電信144MHzバンド",
				},
				new CategoryData() {
					Name = "都外電信SWL",
				},
				new CategoryData() {
					Name = "都外電信電話マルチバンド",
				},
				new CategoryData() {
					Name = "都外電信電話21MHzバンド",
				},
				new CategoryData() {
					Name = "都外電信電話28MHzバンド",
				},
				new CategoryData() {
					Name = "都外電信電話50MHzバンド",
				},
				new CategoryData() {
					Name = "都外電信電話144MHzバンド",
				},
				new CategoryData() {
					Name = "都外電信電話SWL",
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
					if(_mainArea == null) {
						_mainArea = Areano.GetMixedListFromPref(new string[] { "01", "10", "48" }, "Tokyo");
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
			public bool isErrorAvailable { get { return listErr.Any(x => x.Value.IsSet); } }

			private Dictionary<string, ErrorReason> listErr;

			public void check() {
				listErr = ErrorReason.GetInitial();
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
					case "1.9MHz":
					case "3.5MHz":
					case "7MHz":
					case "10MHz":
					case "14MHz":
					case "18MHz":
					case "24MHz":
					case "430MHz":
					case "1200MHz":
					case "2400MHz":
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
				var areano = Contestno.GetAreano(log);
				var prefno = (areano.Length == 2) ? areano : "10";
				var hasStroke = Callsign.HasStroke(log.Callsign);
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
				if (prefno == "10") log.Point = 2;
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