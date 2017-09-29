namespace GameEditor{
	/// <summary>
	/// Carries the ID of a character to be spawned
	/// </summary>
	public class SpawnTile : LogicTile{
		public int CharacterId{ get; set; }
		public SpawnTile(int characterId){ CharacterId = characterId; }
		public SpawnTile():this(-1){ }
		
	}
}