using System.Collections.Generic;
using LogProc.Definitions;
using LogProc.Interfaces;
using LogProc.Utilities;

namespace LogProc {
	namespace FieldDay {
		public class Property {
			public static string ContestName { get { return "フィールドデーコンテスト"; } }
			public static InterSet Intersets { get { return new InterSet(new ContestDefine(), new SearchLog(), new LogSummery()); } }
		}

		public class ContestDefine : IDefine {
			public string oath { get { return "私は，JARL制定のコンテスト規約および電波法令にしたがい運用した結果，ここに提出するサマリーシートおよびログシートなどが事実と相違ないものであることを，私の名誉において誓います。"; } }
			public bool isCoefficientEnabled { get { return true; } }
			public bool isSubCnEnabled { get { return true; } }
			public string contestName { get { return Property.ContestName; } }
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
				Name = "電信部門シングルオペオールバンドモーニング",
				Code ="CAR",
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
					if(Code == "CP" || Code == "XP") return ContestPower.Five;
					else return ContestPower.License;
				}
			}

			public string GetCodeWithPower(string code, ContestPower power) {
				return code;
			}

			public string GetCodeDivPower(string code, ContestPower power) {
				return code;
			}
		}

		public class SearchLog : ISearch {
			private bool SubSelecter { get { return Freq.CnvTofrqnum(log.Freq) < Freq.CnvTofrqnum("2400MHz"); } }
			private List<Area> _mainArea;
			public List<Area> listMainArea {
				get {
					if (_mainArea == null) {
						_mainArea = Areano.GetList("Prefectures");
					}
					return _mainArea;
				}
			}
			private List<Area> _subArea;
			public List<Area> listSubArea {
				get {
					if (_subArea == null) {
						_subArea = Areano.GetList("ACAG");
					}
					return _subArea;
				}
			}
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
					case "1.9MHz": case "10MHz":
					case "18MHz": case "24MHz":
						log.Point = 0;
						ErrorReason.Set(listErr, Reason.OutOfFreq.ToString());
						break;
					default: break;
				}
			}

			private void CheckScn() {
				Contestno.CheckSentCn(log, SubSelecter ? config.Contestno : config.SubContestno, ContestNo.GetVal(config.SentCnExtra, log.Freq), true, listErr);
			}

			private void CheckRcn() {
				if (!Callsign.IsJACallsign(log.Callsign)) {
					log.Point = 0;
					ErrorReason.Set(listErr, Reason.OmakuniNonJA.ToString());
					return;
				}
				if (Contestno.HasPower(log.ReceivedCn) == false) {
					ErrorReason.Set(listErr, Reason.InvalidReceivedCn.ToString());
				}

				//59##L
				var hasStroke = Callsign.HasStroke(log.Callsign);
				var areano = Contestno.GetAreano(log);
				var stationAreano = Areano.GetFromStation(station, SubSelecter ? listMainArea : listSubArea);
				if (!Freq.IsBeen(log.Freq)) {
					ErrorReason.Set(listErr, Reason.InvalidFreq.ToString());
				}
				if (Contestno.GetRegion(Contestno.GetPrefno(log, SubSelecter ? false : true))  != Callsign.GetRegion(log.Callsign, hasStroke)) {
					ErrorReason.Set(listErr, Reason.RegionUnmatches.ToString());
				}
				if (!Station.IsMatched(areano, stationAreano) && station != null && !hasStroke) {
					ErrorReason.Set(listErr, Reason.AddressUnmatches.ToString(), Utils.ConvTostrarrFromList(stationAreano));
				}
				if (!Areano.IsBeen(SubSelecter ? listMainArea : listSubArea, areano)) {
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
			public bool isScoreEdited { get { return true; } }
			public List<Multiply> listMulti { get; set; }
			public int freqNum { get; set; }
			public int areaMax { get; set; }

			public string getAreano(LogData Log) {
				return Contestno.GetAreano(Log);
			}

			public string getScoreStr() {
				string output = "";
				int totalLog = 0;
				int totalPts = 0;
				int totalMulti = 0;
				for(int i = 0;i < freqNum;i++) {
					int multiNum = 0;
					int logNum = 0;
					int ptsNum = 0;
					foreach(var m in listMulti) {
						if(m.Freq != i) continue;
						multiNum++;
						logNum += m.Num;
						ptsNum += m.Point;
					}
					if(multiNum == 0) continue;
					output += "<SCORE BAND=" + Freq.CnvTostr((FreqStr)i) + ">";
					output += logNum + "," + ptsNum + "," + multiNum + "</SCORE>\r\n";
					totalLog += logNum;
					totalPts += ptsNum;
					totalMulti += multiNum;
				}
				if(totalLog == 0) return "";
				int coeff;
				if(config.IsCoefficientEnabled) coeff = 2;
				else coeff = 1;
				output += "<SCORE BAND=TOTAL>" + totalLog + "," + totalPts + "," + totalMulti + "</SCORE>\r\n";
				output += "<FDCOEFF>" + coeff + "</FDCOEFF>\r\n";
				output += "<TOTALSCORE>" + totalPts * totalMulti * coeff + "</TOTALSCORE>\r\n";
				return output;
			}
		}
	}
}