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
        //============================= LAST_UPDATE ============================
        public string GetLastUpdate()
        {
            return dbConn.ExecuteScalar<string>("Select [UPDATED_DATE] From [Employee]");
        }
        public int SaveLastUpdate(LAST_UPDATE lastUpdate)
        {
            return dbConn.Insert(lastUpdate);
        }
        //=================================T_AREA ===============================  
        public int SaveArea(T_AREA aArea)
        {
            return dbConn.Insert(aArea);
        }
        public int UpdateArea(T_AREA aArea)
        {
            return dbConn.Update(aArea);
        }
        //================================T_ROUTE ===============================
        public int SaveRoute(T_ROUTE aRoute)
        {
            return dbConn.Insert(aRoute);
        }
        public int UpdateRoute(T_ROUTE aRoute)
        {
            return dbConn.Update(aRoute);
        }
        //================================T_SECTOR===============================
        public int SaveSector(T_SECTOR aSector)
        {
            return dbConn.Insert(aSector);
        }
        public int UpdateSector(T_SECTOR aSector)
        {
            return dbConn.Update(aSector);
        }
        //================================T_CRAG_SECTOR_MAP======================
        public int SaveCragSectorMap(T_CRAG_SECTOR_MAP aCragSectorMap)
        {
            return dbConn.Insert(aCragSectorMap);
        }
        public int UpdateCragSectorMap(T_CRAG_SECTOR_MAP aCragSectorMap)
        {
            return dbConn.Update(aCragSectorMap);
        }
        //================================T_TOPO=================================
        public int SaveTopo(T_TOPO aTopo)
        {
            return dbConn.Insert(aTopo);
        }
        public int UpdateTopo(T_TOPO aTopo)
        {
            return dbConn.Update(aTopo);
        }
        //=======================================================================
    }
}
