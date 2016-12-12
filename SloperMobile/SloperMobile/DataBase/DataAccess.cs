using SloperMobile.Common.Interfaces;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SloperMobile.Model;

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
            dbConn.CreateTable<TASCENT_TYPE>();
            dbConn.CreateTable<TTECH_GRADE>();
        }
        //============================= LAST_UPDATE ============================
        public string GetLastUpdate()
        {
            return dbConn.ExecuteScalar<string>("Select [UPDATED_DATE] From [LAST_UPDATE]");
        }
        public int SaveLastUpdate(LAST_UPDATE lastUpdate)
        {
            var item = dbConn.Table<LAST_UPDATE>().FirstOrDefault(lastdate => lastdate.ID == 1);
            if(item!=null)
            {
                lastUpdate.ID = item.ID;
                return dbConn.Update(lastUpdate);
            }
            else
            {
                return dbConn.Insert(lastUpdate);
            }
        }
        //=================================T_AREA ===============================  
        public int SaveArea(T_AREA aArea)
        {
            var item = dbConn.Table<T_AREA>().FirstOrDefault(area => area.area_id == aArea.area_id);
            if (item != null)
            {
                aArea.id = item.id;
                return dbConn.Update(aArea);
            }
            else
            {
                return dbConn.Insert(aArea);
            }
        }
        //================================T_ROUTE ===============================
        public int SaveRoute(T_ROUTE aRoute)
        {
            var item = dbConn.Table<T_ROUTE>().FirstOrDefault(route => route.route_id == aRoute.route_id);
            if (item != null)
            {
                aRoute.id = item.id;
                return dbConn.Update(aRoute);
            }
            else
            {
                return dbConn.Insert(aRoute);
            }
        }
        //================================T_SECTOR===============================
        public int SaveSector(T_SECTOR aSector)
        {
            var item = dbConn.Table<T_SECTOR>().FirstOrDefault(sector => sector.sector_id == aSector.sector_id);
            if (item != null)
            {
                aSector.id = item.id;
                return dbConn.Update(aSector);
            }
            else
            {
                return dbConn.Insert(aSector);
            }
        }

        //================================T_CRAG===============================
        public int SaveCrag(T_CRAG aCrag)
        {
            var item = dbConn.Table<T_CRAG>().FirstOrDefault(crag => crag.crag_id == aCrag.crag_id);
            if (item != null)
            {
                aCrag.id = item.id;
                return dbConn.Update(aCrag);
            }
            else
            {
                return dbConn.Insert(aCrag);
            }

        }

        //================================T_CRAG_SECTOR_MAP======================
        public int SaveCragSectorMap(T_CRAG_SECTOR_MAP aCragSectorMap)
        {
            var item = dbConn.Table<T_CRAG_SECTOR_MAP>().FirstOrDefault(cragsecmap => cragsecmap.crag_id == aCragSectorMap.crag_id);
            if (item != null)
            {
                aCragSectorMap.id = item.id;
                return dbConn.Update(aCragSectorMap);
            }
            else
            {
                return dbConn.Insert(aCragSectorMap);
            }

        }
        //================================T_TOPO=================================
        public int SaveTopo(T_TOPO aTopo)
        {
            var item = dbConn.Table<T_TOPO>().FirstOrDefault(topo => topo.sector_id == aTopo.sector_id);
            if (item != null)
            {
                aTopo.topo_id = item.topo_id;
                return dbConn.Update(aTopo);
            }
            else
            {
                return dbConn.Insert(aTopo);
            }
        }
        //=======================================================================
    }
}
