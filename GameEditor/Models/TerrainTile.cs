namespace GameEditor{
	public class TerrainTile : Tile{

		public bool IsWalkable{ get; set; }

		public TerrainTile(bool walkable = true){ IsWalkable = walkable; }
	}
}