using System.Collections.Generic;
using LogProc.Definitions;
using LogProc.Interfaces;
using LogProc.Utilities;
namespace LogProc {
	namespace NTTCwPh {
		public class Property {
			public static string ContestName { get { return "電信電話記念日コンテスト"; } }
			public static InterSet Intersets { get { return new InterSet(new ContestDefine(), new SearchLog(), new LogSummery()); } }
		}

		public class ContestDefine : IDefine {
			public string oath { get { return "私は、NTT R&D ハムクラブ制定のコンテスト規約および電波法令にしたがい運用した結果、ここに提出するサマリーシートおよびログシートなどが事実と相違ないものであることを、私の名誉において誓います。"; } }
			public bool isCoefficientEnabled { get { return false; } }
			public bool isSubCnEnabled { get { return false; } }
			public string contestName { get { return Property.ContestName; } }
			private List<CategoryData> _contestCategolies = new List<CategoryData>() {
				new CategoryData() {
					Name = "一般電信部門シングルオペHF",
					Code = "GCSH",
				},
				new CategoryData() {
					Name = "一般電信部門シングルオペV・UHF",
					Code = "GCSV",
				},
				new CategoryData() {
					Name = "一般電信部門シングルオペオールバンド",
					Code = "GCSA",
				},
				new CategoryData() {
					Name = "一般電信部門マルチオペオールバンド",
					Code = "GCMA"
				},
				new CategoryData() {
					Name = "一般電信電話部門シングルオペHF",
					Code = "GXSH",
				},
				new CategoryData() {
					Name = "一般電信電話部門シングルオペV・UHF",
					Code = "GXSV",
				},
				new CategoryData() {
					Name = "一般電信電話部門シングルオペオールバンド",
					Code = "GXSA",
				},
				new CategoryData() {
					Name = "一般電信電話部門マルチオペオールバンド",
					Code = "GXMA"
				},
				new CategoryData() {
					Name = "NTT電信部門シングルオペHF",
					Code = "NCSH",
				},
				new CategoryData() {
					Name = "NTT電信部門シングルオペV・UHF",
					Code = "NCSV",
				},
				new CategoryData() {
					Name = "NTT電信部門シングルオペオールバンド",
					Code = "NCSA",
				},
				new CategoryData() {
					Name = "NTT電信部門マルチオペオールバンド",
					Code = "NCMA"
				},
				new CategoryData() {
					Name = "NTT電信電話部門シングルオペHF",
					Code = "NXSH",
				},
				new CategoryData() {
					Name = "NTT電信電話部門シングルオペV・UHF",
					Code = "NXSV",
				},
				new CategoryData() {
					Name = "NTT電信電話部門シングルオペオールバンド",
					Code = "NXSA",
				},
				new CategoryData() {
					Name = "NTT電信電話部門マルチオペオールバンド",
					Code = "NXMA"
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
						_mainArea = Areano.GetList("NTT");
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
					foreach(var e in listErr.Values) {
						if (e.IsSet) return true;
					}
					return false;
				}
			}

			private bool isNTT { get { return config.CategoryCode[0] == 'G' ? false : true; } }

			private Dictionary<string, ErrorReason> listErr;
			private enum ExtraReason {
				InvalidSignOfCn,
			}

			public void check() {
				listErr = ErrorReason.GetInitial();
				ErrorReason.Put(listErr, ExtraReason.InvalidSignOfCn.ToString(), new ErrorReason(5, "コンテストナンバー末尾に'N'以外のアルファベットがあります。無効です。"));
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
				log.Point = 1;
				if (log.ReceivedCn[log.ReceivedCn.Length - 1] == 'N') log.Point = 2;
				switch (config.CategoryCode[3]) {
					case 'H':
						if(Freq.CnvTofrqnum("28MHz") < Freq.CnvTofrqnum(log.Freq)) {
							log.Point = 0;
							ErrorReason.Set(listErr, Reason.OutOfFreq.ToString());
						}
						break;
					case 'V':
						if (Freq.CnvTofrqnum("28MHz") >= Freq.CnvTofrqnum(log.Freq)) {
							log.Point = 0;
							ErrorReason.Set(listErr, Reason.OutOfFreq.ToString());
						}
						break;
					default: break;
				}
			}

			private void CheckScn() {
				Contestno.CheckSentCn(log, config.Contestno, null, isNTT, listErr);
				if((isNTT && log.SentCn[log.SentCn.Length - 1] != 'N') || (!isNTT && log.SentCn[log.SentCn.Length - 1] == 'N')) {
					ErrorReason.Set(listErr, ExtraReason.InvalidSignOfCn.ToString());
					log.Point = 0;
				}
			}

			private void CheckRcn() {
				if (!Callsign.IsJACallsign(log.Callsign)) {
					log.Point = 0;
					ErrorReason.Set(listErr, Reason.OmakuniNonJA.ToString());
					return;
				}

				var hasStroke = Callsign.HasStroke(log.Callsign);
				var areano = Contestno.GetAreano(log);
				var phoneRegion = Contestno.GetPhoneRegion(areano, listMainArea);
				var stationAreano = Areano.GetFromStation(station, listMainArea);
				char lrc = log.ReceivedCn[log.ReceivedCn.Length - 1];
				if (lrc != 'N' && !('0' <= lrc && lrc <= '9')) {
					ErrorReason.Set(listErr, ExtraReason.InvalidSignOfCn.ToString());
					log.Point = 0;
				}

				if (!Freq.IsBeen(log.Freq)) {
					ErrorReason.Set(listErr, Reason.InvalidFreq.ToString());
				}
				if(phoneRegion == null) {
					switch (areano) {
						case "050": case "070": case "080": case "090":
							break;
						default:
							ErrorReason.Set(listErr, Reason.ReceivedCnUnexists.ToString(), Utils.ConvTostrarrFromList(Areano.GetFromList(Station.GetList(station), listMainArea)));
							break;
					}
				} else {
					bool regchk = false;
					foreach(var pr in phoneRegion) {
						if(pr == Callsign.GetRegion(log.Callsign, hasStroke)) {
							regchk = true;
							break;
						}
					}
					if (!regchk) {
						ErrorReason.Set(listErr, Reason.RegionUnmatches.ToString());
					}
					if (!Station.IsMatched(areano, stationAreano) && station != null && !hasStroke) {
						ErrorReason.Set(listErr, Reason.AddressUnmatches.ToString(), Utils.ConvTostrarrFromList(stationAreano));
					}
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