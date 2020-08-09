using System;
using System.Collections.Generic;
using DataBase.Models;

namespace DataBase.Interfaces
{
    public interface IDataProduct : IData<ProductData>
    {
        Tuple<string, string, int, int>[] GetDataByTask(int task);
        (string, string, int, int) GetDataById(string id);
    }
}