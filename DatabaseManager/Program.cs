using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace DatabaseManager{
	class Program{
		static IDatabaseConnection<Character_Test> _client;

		private const string DB_URL = "mongodb://GE_admin:LhCRzy9f8VJpUMrawJB8@ds141434.mlab.com:41434/game-editor";

		static void Main(string[] args){
			_client = new MongoDbConnection<Character_Test>(url: DB_URL, database: "game-editor", collection: "Characters");
			var characterRepository = new CharacterRepository(_client);

			var character = new Character_Test{ Name = "Harvey McPotato", Strength = 10 };

			Console.WriteLine("\nAdding '{0}'...", character.Name);
			characterRepository.AddCharacter(character);

			Console.WriteLine("\n\nAdding other characters...");
			characterRepository.AddCharacter(new Character_Test{ Name = "Raynold O'Loser", Strength = 3 });
			characterRepository.AddCharacter(new Character_Test{ Name = "Tobias von Dickhead", Strength = 7 });
			characterRepository.AddCharacter(new Character_Test{ Name = "Albert Einstein", Strength = 1 });
			characterRepository.AddCharacter(new Character_Test{ Name = "Albert Zweistein", Strength = 2 });
			characterRepository.AddCharacter(new Character_Test{ Name = "Albert Dreistein", Strength = 3 });
			characterRepository.AddCharacter(new Character_Test{ Name = "Changiz Sugerballer", Strength = -100 });

			Console.WriteLine("Done!");

			Console.WriteLine("Press enter to exit...");
			Console.ReadKey();
		}
	}
}