using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseManager
{
	public class CharacterRepository
	{
		private readonly IDatabaseConnection<CharacterTest> _client;

		public CharacterRepository(IDatabaseConnection<CharacterTest> client)
		{
			_client = client;
			Characters = GetCharactersAsync().Result;
			Console.WriteLine(@"Done.");
		}

		private List<CharacterTest> Characters{ get; }

		public CharacterTest GetCharacterByName(string name)
		{
			return Characters.Find(c => c.Name == name);
		}

		private async Task<List<CharacterTest>> GetCharactersAsync()
		{
			Console.WriteLine(@"Done.");
			return await _client.GetCollectionAsListAsync();
		}

		public void AddCharacter(CharacterTest newChar)
		{
			if(Characters.Exists(c => c.Name == newChar.Name))
			{
				Console.WriteLine("Character '{0}' already exists in the database. Pick another name.", newChar.Name);
				return;
			}

			Characters.Add(newChar);
			_client.Insert(newChar);
		}
	}
}
