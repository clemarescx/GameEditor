using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GameEditor{
	
	public struct Character{
		public string Name{ get; set; }
		public int HealthPoints{ get; set; }
		public int Strength{ get; set; }
		public int Dexterity{ get; set; }
		public int RaceIndex{ get; set; }
		public List<string> Inventory{ get; set; }
	}
}