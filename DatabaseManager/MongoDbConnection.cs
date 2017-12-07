using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DatabaseManager{
	public class MongoDbConnection<T> : IDatabaseConnection<T>{
		private static IMongoCollection<T> _collection;


		public MongoDbConnection(
			string url = "mongodb://localhost:27017",
			string database = "CharacterCreator",
			string collection = "Characters"){
			Console.WriteLine(@"Selected:{0}", url);
			try{
				IMongoClient client = new MongoClient(url);
				var database1 = client.GetDatabase(database);
				if(database != null)
					Console.WriteLine(@"Selected:{0}", database);

				_collection = database1.GetCollection<T>(collection);
				if(_collection != null)
					Console.WriteLine(@"Selected:{0}", collection);
			}
			catch(Exception e){
				Console.WriteLine(e);
				throw;
			}
		}

		public async Task<List<T>> GetCollectionAsListAsync(){
			Console.WriteLine(@"Selected:{0}", typeof(T));
			var documentList = new List<T>();
			var filter = new BsonDocument();

			using(var cursor = await _collection.Find(filter).ToCursorAsync()){
				while(await cursor.MoveNextAsync()){
					documentList.AddRange(cursor.Current);
				}
			}

			return documentList;
		}

		public async void Insert(T doc){ await _collection.InsertOneAsync(doc); }
	}
}