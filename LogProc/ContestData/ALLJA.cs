using System.Collections.Generic;
using LogProc.Definitions;
using LogProc.Interfaces;
using LogProc.Utilities;
using System.Linq;

namespace LogProc {
	namespace ALLJA {
		public class Property {
			public static string ContestName { get { return "ALL JAコンテスト"; } }
			public static InterSet Intersets { get { return new InterSet(new ContestDefine(), new SearchLog(), new LogSummery()); } }
		}

		public class ContestDefine : IDefine {
			public string oath { get { return "私は，JARL制定のコンテスト規約および電波法令にしたがい運用した結果，ここに提出するサマリーシートおよびログシートなどが事実と相違ないものであることを，私の名誉において誓います。"; } }
			public bool isCoefficientEnabled { get { return false; } }
			public bool isSubCnEnabled { get { return false; } }
			public string contestName { get { return Property.ContestName; } }
			private List<CategoryData> _contestCategolies = new List<CategoryData>() {
			new CategoryData(){
				Name = "電話部門シングルオペオールバンド",
				Code = "PA",
			},
			new CategoryData(){
				Name = "電話部門シングルオペ3.5MHzバンド",
				Code = "P35",
			},
			new CategoryData(){
				Name = "電話部門シングルオペ7MHzバンド",
				Code = "P7",
			},
			new CategoryData(){
				Name = "電話部門シングルオペ21MHzバンド",
				Code = "P21",
			},
			new CategoryData(){
				Name = "電話部門シングルオペ28MHzバンド",
				Code = "P28",
			},
			new CategoryData(){
				Name = "電話部門シングルオペ50MHzバンド",
				Code = "P50",
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
				Name = "電信部門シングルオペ3.5MHzバンド",
				Code = "C35",
			},
			new CategoryData(){
				Name = "電信部門シングルオペ7MHzバンド",
				Code = "C7",
			},
			new CategoryData(){
				Name = "電信部門シングルオペ14MHzバンド",
				Code = "C14",
			},
			new CategoryData(){
				Name = "電信部門シングルオペ21MHzバンド",
				Code = "C21",
			},
			new CategoryData(){
				Name = "電信部門シングルオペ28MHzバンド",
				Code = "C28",
			},
			new CategoryData(){
				Name = "電信部門シングルオペ50MHzバンド",
				Code = "C50",
			},
			new CategoryData(){
				Name = "電信部門シングルオペシルバー",
				Code = "CS",
			},
			new CategoryData(){
				Name = "電信部門マルチオペオールバンド",
				Code = "CMA",
			},
			new CategoryData(){
				Name = "電信部門マルチオペ2波",
				Code = "CM2",
			},
			new CategoryData(){
				Name = "電信電話部門シングルオペオールバンド",
				Code = "XA",
			},
			new CategoryData(){
				Name = "電信電話部門シングルオペ3.5MHzバンド",
				Code = "X35",
			},
			new CategoryData(){
				Name = "電信電話部門シングルオペ7MHzバンド",
				Code = "X7",
			},
			new CategoryData(){
				Name = "電信電話部門シングルオペ14MHzバンド",
				Code = "X14",
			},
			new CategoryData(){
				Name = "電信電話部門シングルオペ21MHzバンド",
				Code = "X21",
			},
			new CategoryData(){
				Name = "電信電話部門シングルオペ28MHzバンド",
				Code = "X28",
			},
			new CategoryData(){
				Name = "電信電話部門シングルオペ50MHzバンド",
				Code = "X50",
			},
			new CategoryData(){
				Name = "電信電話部門シングルオペシルバー",
				Code = "XS",
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
				Name = "電信電話部門マルチオペ2波",
				Code = "XM2",
			},
			new CategoryData(){
				Name = "電信電話部門マルチオペジュニア",
				Code = "XMJ",
			},
		};
			public List<CategoryData> contestCategories { get { return _contestCategolies; } }

			public ContestPower GetPowerAllowed(string Code) {
				if(Code[0] == 'P') return ContestPower.TwentyTen;
				else {
					switch(Code) {
						case "CS":
						case "CM2":
						case "XS":
						case "XSWL":
						case "XM2":
						case "XMJ":
							return ContestPower.License;
						case "CMA":
						case "XMA":
							return ContestPower.License | ContestPower.Hundred;
						default:
							return ContestPower.License | ContestPower.Hundred | ContestPower.Five;
					}
				}
			}

			public string GetCodeWithPower(string code, ContestPower power) {
				if(power.HasFlag(ContestPower.TwentyTen) || GetPowerAllowed(code) == ContestPower.License)
					return code;
				else {
					if(power.HasFlag(ContestPower.License)) return code + "H";
					else if(power.HasFlag(ContestPower.Hundred)) return code + "M";
					else return code + "P";
				}
			}

			public string GetCodeDivPower(string code, ContestPower power) {
				if(power.HasFlag(ContestPower.TwentyTen) || GetPowerAllowed(code) == ContestPower.License)
					return code;
				else {
					return code.Substring(0, code.Length - 1);
				}
			}
		}

		public class SearchLog : ISearch {
			private List<Area> _mainArea;
			public List<Area> listMainArea {
				get {
					if(_mainArea == null) {
						_mainArea = Areano.GetList("Prefectures");
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
				if(station == null) {
					ErrorReason.Set(listErr, Reason.GetDataFailed.ToString());
				}
				switch (log.Freq) {
					case "3.5MHz": case "7MHz": case "14MHz": case "21MHz": case "28MHz": case "50MHz":
						log.Point = 1;
						break;
					default:
						log.Point = 0;
						ErrorReason.Set(listErr, Reason.OutOfFreq.ToString());
						break;
				}
			}

			private void CheckScn() {
				Contestno.CheckSentCn(log, config.Contestno, ContestNo.GetVal(config.SentCnExtra, log.Freq), true,  listErr);
			}

			private void CheckRcn() {
				if (!Callsign.IsJACallsign(log.Callsign)) {
					log.Point = 0;
					ErrorReason.Set(listErr, Reason.OmakuniNonJA.ToString());
					return;
				}
				if (!Contestno.HasPower(log.ReceivedCn)) {
					ErrorReason.Set(listErr, Reason.InvalidReceivedCn.ToString());
				}

				//59##L
				var prefno = Contestno.GetPrefno(log);
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