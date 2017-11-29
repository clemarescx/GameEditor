using System.Collections.Generic;

namespace GameEditor.Models
{
	public struct Character
	{
		public string Name{ get; set; }
		public int HealthPoints{ get; set; }
		public int Strength{ get; set; }
		public int Dexterity{ get; set; }
		public int RaceIndex{ get; set; }
		public List<string> Inventory{ get; set; }
	}
}
