using MongoDB.Bson;

namespace DatabaseManager{
	public class Character{
		public ObjectId _id{ get; set; }

		public string Name{ get; set; }

		public int Strength{ get; set; }
	}
}