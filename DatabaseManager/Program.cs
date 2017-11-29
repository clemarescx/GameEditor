using System;

namespace DatabaseManager
{
	internal static class Program
	{
		private const string DbUrl = "mongodb://GE_admin:LhCRzy9f8VJpUMrawJB8@ds141434.mlab.com:41434/game-editor";
		private static IDatabaseConnection<CharacterTest> _client;

		private static void Main(string[] args)
		{
			_client = new MongoDbConnection<CharacterTest>(DbUrl, "game-editor");
			var characterRepository = new CharacterRepository(_client);

			var character = new CharacterTest{ Name = "Harvey McPotato", Strength = 10 };

			Console.WriteLine("\nAdding '{0}'...", character.Name);
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
