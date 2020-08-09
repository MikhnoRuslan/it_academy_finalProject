namespace DataBase.Interfaces
{
    public interface IData<in T> where T : class
    {
        void Create(T item);
        void Delete<T>(T id);
        void GetAllData();
    }
}