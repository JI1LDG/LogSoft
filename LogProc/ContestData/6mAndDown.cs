﻿using System.Collections.Generic;
using System.Linq;
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
			public string oath { get { return "私は，JARL制定のコンテスト規約および電波法令にしたがい運用した結果，ここに提出するサマリーシートおよびログシートなどが事実と相違ないものであることを，私の名誉において誓います。"; } }
			public bool isCoefficientEnabled { get { return false; } }
			public bool isSubCnEnabled { get { return true; } }
			public string contestName { get { return Property.ContestName; } }
			private List<CategoryData> _contestCategolies = new List<CategoryData>() {
			new CategoryData(){
				Name = "電話部門シングルオペオールバンド",
				Code = "PA",
			},
			new CategoryData() {
				Name = "電話部門シングルオペオールバンドD-STAR",
				Code = "PD",
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
				if (SubSelecter) log.Point = 1;
				else log.Point = 2;
				if(Freq.CnvTofrqnum(log.Freq) < Freq.CnvTofrqnum("50MHz")) {
					log.Point = 0;
					ErrorReason.Set(listErr, Reason.OutOfFreq.ToString());
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

				var hasStroke = Callsign.HasStroke(log.Callsign);
				var areano = Contestno.GetAreano(log);
				var stationAreano = Areano.GetNoFromStation(station, SubSelecter ? listMainArea : listSubArea);
				var suggests = Areano.GetSuggestFromStation(station, SubSelecter ? listMainArea : listSubArea);
				if (!Freq.IsBeen(log.Freq)) {
					ErrorReason.Set(listErr, Reason.InvalidFreq.ToString());
				}
				if (Contestno.GetRegion(Contestno.GetPrefno(log, SubSelecter ? false : true)) != Callsign.GetRegion(log.Callsign, hasStroke)) {
					ErrorReason.Set(listErr, Reason.RegionUnmatches.ToString());
				}
				if (!Station.IsMatched(areano, stationAreano) && station != null && !hasStroke) {
					ErrorReason.Set(listErr, Reason.AddressUnmatches.ToString(), Utils.ConvTostrarrFromList(suggests));
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