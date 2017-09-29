namespace GameEditor{
	public class PortalTile : LogicTile{
		public int DestinationId{ get; set; }
		/// <summary>
		/// To be used for travel between zones 
		/// </summary>
		/// <param name="destinationId"></param>
		public PortalTile(int destinationId){ DestinationId = destinationId; }
		public PortalTile() :this(-1){ }
		
	}
}