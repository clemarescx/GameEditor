using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseManager
{
	public interface IDatabaseConnection<T>
	{
		Task<List<T>> GetCollectionAsListAsync();
		void Insert(T newDoc);
	}
}
