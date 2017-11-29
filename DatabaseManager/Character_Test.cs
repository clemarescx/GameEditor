using MongoDB.Bson;

namespace DatabaseManager{
	public class CharacterTest{
		public ObjectId Id{ get; set; }

		public string Name{ get; set; }

		public int Strength{ get; set; }
	}
}