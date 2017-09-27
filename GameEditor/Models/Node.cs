using System;
using System.Collections;
using System.Net.NetworkInformation;

namespace GameEditor{
	public class Node{
		public int GridX{ get; set; }
		public int GridY{ get; set; }
		public bool Walkable{ get; set; }
		public string NodeType{ get; set; }

		public override string ToString(){
			return $"X:{GridX} Y:{GridY} Walkable:{Walkable} Type:{NodeType}";
		}
	}
}