using System.Collections.Generic;
using LogProc.Definitions;

namespace LogProc {
	namespace Interfaces {
		public enum ContestPower {
			License = 0x01, Hundred = 0x02, TwentyTen = 0x04, Five = 0x08, None = 0x00,
		}

		public interface IDefine {
			string contestName { get; }
			bool isCoefficientEnabled { get; }
			bool isSubCnEnabled { get; }
			string oath { get; }
			List<CategoryData> contestCategories { get; }
			ContestPower GetPowerAllowed(string code);
			string GetCodeWithPower(string code, ContestPower power);
			string GetCodeDivPower(string code, ContestPower power);
		}

		public interface ISearch {
			LogData log { get; set; }
			StationData station { get; set; }
			Setting config { get; set; }
			string anvStr { get; set; }
			string contestName { get; }
			List<Area> listMainArea { get; }
			List<Area> listSubArea { get; }
			bool isErrorAvailable { get; }
			void check();
			void setErrorStr();
		}

		public interface ISummery {
			string contestName { get; }
			Setting config { get; set; }
			bool isScoreEdited { get; }
			List<Multiply> listMulti { get; set; }
			int freqNum { get; set; }
			int areaMax { get; set; }
			string getAreano(LogData log);
			string getScoreStr();
		}
	}
}