using SloperMobile.Common.Interfaces;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SloperMobile.DataBase
{
    public class DataAccess
    {
        SQLiteConnection dbConn;
        public DataAccess()
        {
            dbConn = DependencyService.Get<ISQLite>().GetConnection();
            dbConn.CreateTable<LAST_UPDATE>();
            dbConn.CreateTable<T_AREA>();
            dbConn.CreateTable<T_ROUTE>();
            dbConn.CreateTable<T_SECTOR>();
            dbConn.CreateTable<T_CRAG>();
            dbConn.CreateTable<T_CRAG_SECTOR_MAP>();
            dbConn.CreateTable<T_TOPO>();
        }
    }
}
