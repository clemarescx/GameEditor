using System.Collections.Generic;
using System.Windows.Documents;

namespace GameEditor{
	public class GameMap{
		public int Rows;
		public int Columns;

		public Dictionary<int, string> NodeTable{ get; set; }

		public NodeGrid grid{ get; set; }

		public List<int> Triggers{ get; set; }

		public GameMap(int rows, int columns){
			this.Rows = rows;
			this.Columns = columns;
			grid = new NodeGrid(rows, columns);
			Triggers = new List<int>();
		}
	}
}