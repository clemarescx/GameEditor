using System.Windows;

namespace GameEditor{
	/// <summary>
	/// The grid template consists of a 2D array of nodes, each carrying 
	/// a value of type T
	/// 
	/// </summary>
	public class NodeGrid{
		public Node[,] Grid;
		public int Rows;
		public int Columns;
		

		public int MaxSize => Rows * Columns;

		public NodeGrid(int rows, int columns){
			Rows = rows;
			Columns = columns;
			CreateGrid();
		}

		private void CreateGrid(){
			Grid = new Node[Rows, Columns];

			for(int i = 0; i < Rows; i++){
				for(int j = 0; j < Columns; j++){
					Grid[i, j] = new Node{ GridX = j, GridY = i};
				}
			}
		}

		/// <summary>
		/// Reset all nodes to default value
		/// </summary>
		public void Reset(){
			for(int i = 0; i < Rows; i++){
				for(int j = 0; j < Columns; j++){
					Grid[i, j].Walkable = true;
				}
			}
		}

		public Node this[int i, int j]
		{
			get => Grid[i, j];
			set => Grid[i, j] = value;
		}
	}
}