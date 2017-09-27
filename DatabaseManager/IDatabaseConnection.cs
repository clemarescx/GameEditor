using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace DatabaseManager{
	public interface IDatabaseConnection<T>{
		Task<List<T>> GetCollectionAsListAsync();
		void Insert(T newDoc);
	}
}