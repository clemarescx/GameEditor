using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseManager{
	public class CharacterRepository{
		private readonly IDatabaseConnection<Character_Test> _client;
		public List<Character_Test> Characters{ get; }

		public CharacterRepository(IDatabaseConnection<Character_Test> client){
			_client = client;
			Characters = GetCharactersAsync().Result;
			Console.WriteLine("Characters loaded.");
		}

		public Character_Test GetCharacterByName(string name){ return Characters.Find(c => c.Name == name); }

		public async Task<List<Character_Test>> GetCharactersAsync(){
			Console.WriteLine("Getting characters...");
			return await _client.GetCollectionAsListAsync();
		}

		public void AddCharacter(Character_Test newChar){
			if(Characters.Exists(c => c.Name == newChar.Name)){
				Console.WriteLine("Character '{0}' already exists in the database. Pick another name.", newChar.Name);
				return;
			}
			Characters.Add(newChar);
			_client.Insert(newChar);
		}
	}
}