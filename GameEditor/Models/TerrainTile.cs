namespace GameEditor
{
	public class TerrainTile : Tile
	{
		public bool IsWalkable{ get; set; }

		/// <summary>
		///     Carries a value to be used for a pathfinding grid
		/// </summary>
		/// <param name="walkable"></param>
		public TerrainTile(bool walkable)
		{
			IsWalkable = walkable;
		}

		public TerrainTile() : this(true)
		{ }
	}
}
