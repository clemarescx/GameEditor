using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace DatabaseManager{
	class Program{
		static IDatabaseConnection<CharacterTest> _client;

		private const string DbUrl = "mongodb://GE_admin:LhCRzy9f8VJpUMrawJB8@ds141434.mlab.com:41434/game-editor";

		static void Main(string[] args){
			_client = new MongoDbConnection<CharacterTest>(url: DbUrl, database: "game-editor", collection: "Characters");
			var characterRepository = new CharacterRepository(_client);

			var character = new CharacterTest{ Name = "Harvey McPotato", Strength = 10 };

			Console.WriteLine(@"Selected:{0}", character.Name);
			characterRepository.AddCharacter(character);

			Console.WriteLine(@"Done.");
			characterRepository.AddCharacter(new CharacterTest{ Name = "Raynold O'Loser", Strength = 3 });
			characterRepository.AddCharacter(new CharacterTest{ Name = "Tobias von Dickhead", Strength = 7 });
			characterRepository.AddCharacter(new CharacterTest{ Name = "Albert Einstein", Strength = 1 });
			characterRepository.AddCharacter(new CharacterTest{ Name = "Albert Zweistein", Strength = 2 });
			characterRepository.AddCharacter(new CharacterTest{ Name = "Albert Dreistein", Strength = 3 });
			characterRepository.AddCharacter(new CharacterTest{ Name = "Changiz Sugerballer", Strength = -100 });

			Console.WriteLine(@"Done.");

			Console.WriteLine(@"Done.");
			Console.ReadKey();
		}
	}
}