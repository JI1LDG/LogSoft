using System.Collections.Generic;
using LogProc.Definitions;
using LogProc.Interfaces;
using LogProc.Utilities;
using System.Linq;

namespace LogProc {
	namespace KantoUHF {
		public class Property {
			public static string ContestName { get { return "関東UHFコンテスト"; } }
			public static InterSet Intersets { get { return new InterSet(new ContestDefine(), new SearchLog(), new LogSummery()); } }
		}

		public class ContestDefine : IDefine {
			public string oath { get { return "私は，JARL制定のコンテスト規約および電波法令にしたがい運用した結果，ここに提出するサマリーシートおよびログシートなどが事実と相違ないものであることを，私の名誉において誓います。"; } }
			public bool isCoefficientEnabled { get { return false; } }
			public bool isSubCnEnabled { get { return false; } }
			public string contestName { get { return Property.ContestName; } }
			private List<CategoryData> _contestCategolies = new List<CategoryData>() {
				new CategoryData() {
					Name ="ヤング部門",
					Code = "YM",
				},
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
			public List<Area> listMainArea
			{
				get
				{
					if (_mainArea == null) {
						_mainArea = Areano.GetList("ACAG");
					}
					return _mainArea;
				}
			}
			public List<Area> listSubArea { get { return null; } }
			public string contestName { get { return Property.ContestName; } }
			public LogData log { get; set; }
			public StationData station { get; set; }
			public Setting config { get; set; }
			public string anvStr { get; set; }
			public bool isErrorAvailable { get { return listErr.Any(x => x.Value.IsSet); } }

			private Dictionary<string, ErrorReason> listErr;
			private enum ExtraReason {
				NotInKanto,
			}

			public void check() {
				listErr = ErrorReason.GetInitial();
				ErrorReason.Put(listErr, ExtraReason.NotInKanto.ToString(), new ErrorReason(5, "関東圏の局ではありません。無効です。"));
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
				if (Freq.CnvTofrqnum(log.Freq) < Freq.CnvTofrqnum("430MHz")) {
					log.Point = 0;
					ErrorReason.Set(listErr, Reason.OutOfFreq.ToString());
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

				//59##L
				var prefno = Contestno.GetPrefno(log, true);
				if (10 > int.Parse(prefno) || int.Parse(prefno) > 17) {
					ErrorReason.Set(listErr, ExtraReason.NotInKanto.ToString());
					log.Point = 0;
				}
				var hasStroke = Callsign.HasStroke(log.Callsign);
				var areano = Contestno.GetAreano(log);
				var stationAreano = Areano.GetNoFromStation(station, listMainArea);
				var suggests = Areano.GetSuggestFromStation(station, listMainArea);
				if(!Freq.IsBeen(log.Freq)) {
					ErrorReason.Set(listErr, Reason.InvalidFreq.ToString());
				}
				if (Contestno.GetRegion(prefno) != Callsign.GetRegion(log.Callsign, hasStroke)) {
					ErrorReason.Set(listErr, Reason.RegionUnmatches.ToString());
				}
				if (!Station.IsMatched(areano, stationAreano) && station != null && !hasStroke) {
					ErrorReason.Set(listErr, Reason.AddressUnmatches.ToString(), Utils.ConvTostrarrFromList(suggests));
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
