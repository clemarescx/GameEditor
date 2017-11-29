namespace GameEditor
{
	public class TerrainTile : Tile
	{
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

		public bool IsWalkable{ get; set; }
	}
}
