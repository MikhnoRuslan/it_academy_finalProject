using DataBase.Models;

namespace DataBase.Interfaces
{
    public interface IDataUser : IData<User>
    {
        (string T, string T1) GetDataByItem(string login);
    }
}