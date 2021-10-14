using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tamagotchi
{
    public interface IDataStore<T>
    {
        //CRUD operations
        Task<bool> CreateItem(T item);
        Task<T> ReadItem();
        Task<bool> UpdateItem(T item);
        Task<bool> DeleteItem(T item);
    }
}
