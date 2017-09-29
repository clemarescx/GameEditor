namespace GameEditor{
	public class SpawnTile : Tile{
		public int CharacterId{ get; set; }
		public SpawnTile(int characterId){ CharacterId = characterId; }
		
	}
}