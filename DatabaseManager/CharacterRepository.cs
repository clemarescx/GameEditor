using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseManager{
	internal class CharacterRepository{
		private readonly IDatabaseConnection<Character> _client;
		public List<Character> Characters{ get; }

		public CharacterRepository(IDatabaseConnection<Character> client){
			_client = client;
			Characters = GetCharactersAsync().Result;
			Console.WriteLine("Characters loaded.");
		}

		public Character GetCharacterByName(string name){ return Characters.Find(c => c.Name == name); }

		public async Task<List<Character>> GetCharactersAsync(){
			Console.WriteLine("Getting characters...");
			return await _client.GetCollectionAsListAsync();
		}

		public void AddCharacter(Character newChar){
			if(Characters.Exists(c => c.Name == newChar.Name)){
				Console.WriteLine("Character '{0}' already exists in the database. Pick another name.", newChar.Name);
				return;
			}
			Characters.Add(newChar);
			_client.Insert(newChar);
		}
	}
}