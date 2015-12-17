using System.Collections.Generic;
using LogProc.Definitions;

namespace LogProc {
	namespace Interfaces {
		public enum ContestPower {
			License = 0x01, Hundred = 0x02, TwentyTen = 0x04, Five = 0x08, None = 0x00,
		}

		public interface IDefine {
			string ContestName { get; }
			bool Coefficient { get; }
			bool IsSubCN { get; }
			string Oath { get; }
			List<CategoryData> ContestCategolies { get; }
			ContestPower AllowenPowerInCategoryCode(string Code);
			string GetCategoryCodeByPower(string Code, ContestPower Power);
			string GetCategoryCodeDivPower(string Code, ContestPower Power);
		}

		public interface ISearch {
			LogData Log { get; set; }
			StationData Station { get; set; }
			Setting Config { get; set; }
			string AnvStation { get; set; }
			string ContestName { get; }
			List<Area> MainArea { get; }
			List<Area> SubArea { get; }
			bool isErrorAvailable { get; }
			void DoCheck();
			void SetErrorStr();
		}

		public interface ISummery {
			string ContestName { get; }
			Setting Config { get; set; }
			bool isEditenScore { get; }
			List<Multiply> Multi { get; set; }
			int FreqNum { get; set; }
			int AreaMax { get; set; }
			string GetContestAreaNoFromRcn(LogData log);
			string GetScoreStr();
		}
	}
}