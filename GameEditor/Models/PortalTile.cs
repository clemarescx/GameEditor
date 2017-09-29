namespace GameEditor{
	public class PortalTile : Tile{
		public int DestinationId{ get; set; }

		public PortalTile(int destinationId){ DestinationId = destinationId; }
		
	}
}