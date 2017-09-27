using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace DatabaseManager{
	class Program{
		static IDatabaseConnection<Character> _client;

		private const string DB_URL = "mongodb://GE_admin:LhCRzy9f8VJpUMrawJB8@ds141434.mlab.com:41434/game-editor";

		static void Main(string[] args){
			_client = new MongoDbConnection<Character>(url: DB_URL, database: "game-editor", collection: "Characters");
			var characterRepository = new CharacterRepository(_client);

			var character = new Character{ Name = "Harvey McPotato", Strength = 10 };

			Console.WriteLine("\nAdding '{0}'...", character.Name);
			characterRepository.AddCharacter(character);

			Console.WriteLine("\n\nAdding other characters...");
			characterRepository.AddCharacter(new Character{ Name = "Raynold O'Loser", Strength = 3 });
			characterRepository.AddCharacter(new Character{ Name = "Tobias von Dickhead", Strength = 7 });
			characterRepository.AddCharacter(new Character{ Name = "Albert Zweistein", Strength = 5 });
			characterRepository.AddCharacter(new Character{ Name = "Changiz Sugerballer", Strength = -100 });

			Console.WriteLine("Done!");

			Console.WriteLine("Press enter to exit...");
			Console.ReadKey();
		}
	}
}