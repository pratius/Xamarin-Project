using SloperMobile.Common.Interfaces;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SloperMobile.Model;
using SloperMobile.Common.Helpers;
using Newtonsoft.Json;

namespace SloperMobile.DataBase
{
    public class DataAccess
    {
        SQLiteConnection dbConn;
        public DataAccess()
        {
            dbConn = DependencyService.Get<ISQLite>().GetConnection();
            dbConn.CreateTable<APP_SETTING>();
            dbConn.CreateTable<T_AREA>();
            dbConn.CreateTable<T_ROUTE>();
            dbConn.CreateTable<T_SECTOR>();
            dbConn.CreateTable<T_CRAG>();
            dbConn.CreateTable<T_CRAG_SECTOR_MAP>();
            dbConn.CreateTable<T_TOPO>();
            dbConn.CreateTable<TASCENT_TYPE>();
            dbConn.CreateTable<TTECH_GRADE>();
            dbConn.CreateTable<T_GRADE>();
            if (dbConn.Table<TASCENT_TYPE>().Count() == 0)
            {
                string ascent_types = "Onsight,Flash,Redpoint,Repeat,One hang,Project";
                string[] asctype = ascent_types.Split(',');
                foreach (string str in asctype)
                {
                    TASCENT_TYPE tblObj = new TASCENT_TYPE();
                    tblObj.ascent_type_description = str;
                    dbConn.Insert(tblObj);
                }
            }

            if (dbConn.Table<TTECH_GRADE>().Count() == 0)
            {
                string grade_type_id = "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,1,1,1,1,1,2,2,2,2,2,1,1,1,8,8,8,8,9,9,9,9,9,9,9,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,11,11,11,11,11,11,11,11,11,11,11,11,11,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,6,6,6,6,6,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,17,17,17,17,17,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,3,3,3,19,19,19,19,19,19,19,19,19,19";
                string tech_grade = "3,4a,5a,5c,6a,6a +,6b,6b +,6c,6c +,7a,7a +,7b,7b +,7c,7c +,8a,8a +,8b,8b +,8c,8c +,9a,9a +,9b,9b +,9c,9c +,10a,5B,5B +,5C,6A,6A +,6B,6B +,6C,6C +,7A,7A +,7B,7B +,7C,7C +,8A,8A +,8B,8B +,8C,8C +,9A,V0--,V1,V2,V3,V4,V5,V6,V7,V8,V9,V10,V11,V12,V13,V14,V15,V16,V17,V18,V19,V20,3a,3b,3c,4a,4b,4c,5a,5b,5c,6a,6b,6c,7a,7b,7c,8a,4c,4b,5b,P,?,3,4,5A,5A +,5C +,3 +,-,2 +,Low,Middle,High,Crux,A0,A1,A2,A3,A4,A5,A6,II,III,IV -,IV,IV +,V -,V,V +,6a,6a +,6b,6b +,6c,6c +,7a,3 -,3,3 +,4 -,4,4 +,5 -,5,5 +,6 -,6,6 +,7 -,7,7 +,8 -,8,8 +,9 -,9,9 +,10 -,10,10 +,11 -,11,11 +,-,M4,M5,M6,M7,M8,M9,M10,M11,M12,M13,P,?,?,P,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,6 / 6 +,7 +/ 8 -,8 / 8 +,9 / 9 +,9 +/ 10 -,?,P,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,3 -,3,3 +,4 -,4,4 +,5 -,5,5 +,6 -,6,6 +,7 -,7,7 +,8 -,8,8 +,9 -,9,9 +,10 -,10,10 +,11 -,11,11 +,6 / 6 +,7 +/ 8 -,8 / 8 +,9 / 9 +,9 +/ 10 -,3 -,3,3 +,4 -,4,4 +,5 -,5,5 +,6 -,6,6 +,7 -,7,7 +,8 -,8,8 +,9 -,9,9 +,10 -,10,10 +,11 -,11,11 +,6 / 6 +,7 +/ 8 -,8 / 8 +,9 / 9 +,9 +/ 10 -,5,5.1,5.2,5.3,5.4,5.5,5.6,5.7,5.8,5.9,5.10a,5.10b,5.10c,5.10d,5.11a,5.11b,5.11c,5.11d,5.12a,5.12b,5.12c,5.12d,5.13a,5.13b,5.13c,5.13d,5.14a,5.14b,5.14c,5.14d,5.15a,5.15b,5.15c,5.15d,5,5.1,5.2,5.3,5.4,5.5,5.6,5.7,5.8,5.9,5.10a,5.10b,5.10c,5.10d,5.11a,5.11b,5.11c,5.11d,5.12a,5.12b,5.12c,5.12d,5.13a,5.13b,5.13c,5.13d,5.14a,5.14b,5.14c,5.14d,5.15a,5.15b,5.15c,5.15d,C1,C2,C3,C4,C5,1 -,1,1 +,2 -,2,2 +,3 -,3,3 +,4 -,4,4 +,5 -,5,5 +,5,5.1,5.2,5.3,5.4,5.5,5.6,5.7,5.8,5.9,5.10 -,5.1,5.10 +,5.11 -,5.11,5.11 +,5.12 -,5.12,5.12 +,5.13 -,5.13,5.13 +,5.14 -,5.14,5.14 +,5.15 -,5.15,5.15 +,V0 -,V0,V0 +,5.5 -,5.5 +,5.6 -,5.6 +,5.7 -,5.7 +,5.8 -,5.8 +,5.9 -,5.9 +";
                string sort_order = "12,20,50,70,80,90,100,110,120,130,140,150,160,170,180,190,200,210,220,230,240,250,260,270,280,290,300,310,320,50,60,70,90,100,110,120,130,140,150,160,170,180,190,200,210,220,230,240,250,260,270,0,10,20,30,40,50,60,70,80,90,100,110,120,130,140,150,160,170,180,190,200,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,40,30,60,0,-1,10,20,30,40,80,16,4,8,1,2,3,4,1,2,3,5,5,6,7,10,20,30,40,50,60,70,80,90,100,110,120,130,140,150,10,20,30,40,50,60,70,80,90,100,110,120,130,140,150,160,170,180,190,200,210,220,230,240,250,260,270,10,20,30,40,50,60,70,80,90,100,110,1000,15,3,6,10,20,30,40,50,60,70,80,90,100,110,120,130,140,150,160,170,180,190,200,210,220,230,240,250,260,270,280,290,300,310,320,330,340,350,360,370,380,390,400,410,420,430,115,155,175,205,215,3,6,10,20,30,40,50,60,70,80,90,100,110,120,130,140,150,160,170,180,190,200,210,220,230,240,250,260,270,280,290,300,310,320,330,340,350,360,370,380,390,400,10,20,30,40,50,60,70,80,90,100,110,120,130,140,150,160,170,180,190,200,210,220,230,240,250,260,270,115,155,175,205,215,10,20,30,40,50,60,70,80,90,100,110,120,130,140,150,160,170,180,190,200,210,220,230,240,250,260,270,115,155,175,205,215,10,20,30,40,50,60,70,80,90,100,110,120,130,140,150,160,170,180,190,200,210,220,230,240,250,260,270,280,290,300,310,320,330,340,10,20,30,40,50,60,70,80,90,100,110,120,130,140,150,160,170,180,190,200,210,220,230,240,250,260,270,280,290,300,310,320,330,340,1,2,3,4,5,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,10,20,30,40,50,60,70,80,90,100,110,120,130,150,160,170,190,200,210,230,240,250,270,280,290,310,320,330,2,4,6,59,61,69,71,79,81,89,91,99,101";
                string grade_bucket_id = "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,3,3,3,3,3,3,4,4,4,4,4,4,5,5,5,5,5,5,5,1,3,3,4,4,4,4,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,2,2,2,2,3,3,3,3,4,4,4,4,5,5,5,5,5,5,5,5,5,5,5,5,1,2,3,4,5,1,1,1,2,2,2,3,3,3,4,4,4,5,5,5,1,1,1,1,1,1,1,1,1,1,2,2,2,3,3,3,4,4,4,5,5,5,5,5,5,5,5,5,2,2,2,1,1,1,1,1,1,1,1,1,1";

                string[] grade_typ_id = grade_type_id.Split(',');
                string[] tec_grade = tech_grade.Split(',');
                string[] sort_ordr = sort_order.Split(',');
                string[] grade_bkt_id = grade_bucket_id.Split(',');
                for (int i = 0; i < grade_typ_id.Length; i++)
                {
                    TTECH_GRADE tblObj = new TTECH_GRADE();
                    tblObj.grade_type_id = grade_typ_id[i];
                    tblObj.tech_grade = tec_grade[i];
                    tblObj.sort_order = sort_ordr[i];
                    tblObj.grade_bucket_id = grade_bkt_id[i];
                    dbConn.Insert(tblObj);
                }
            }



        }
        //============================= LAST_UPDATE ============================
        public string GetLastUpdate()
        {
            return dbConn.ExecuteScalar<string>("Select [UPDATED_DATE] From [APP_SETTING]");
        }

        public bool CheckAppInitialization()
        {
            return dbConn.ExecuteScalar<bool>("Select [IS_INITIALIZED] From [APP_SETTING]");
        }

        public int SaveLastUpdate(APP_SETTING lastUpdate)
        {
            var item = dbConn.Table<APP_SETTING>().FirstOrDefault(lastdate => lastdate.ID == 1);
            if (item != null)
            {
                lastUpdate.ID = item.ID;
                lastUpdate.IS_INITIALIZED = true;
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
        //========================================================================

        //================================T_GRADE=================================
        public int SaveGrade(T_GRADE aGrade)
        {
            return dbConn.Insert(aGrade);
        }
        //=======================================================================
        //================================ Drop and Create Table ================
        public void DropAndCreateTable(Type aTable)
        {
            if (aTable.GetType() == typeof(T_GRADE))
            {
                dbConn.DropTable<T_GRADE>();
                dbConn.CreateTable<T_GRADE>();
            }
        }
        //=======================================================================



        #region Get Methods
        public List<TASCENT_TYPE> GetAscentType()
        {
            var ascentlist = dbConn.Table<TASCENT_TYPE>().ToList();
            return ascentlist;
        }
        public T_SECTOR GetSectorDataBySectorID(string secid)
        {
            var item = dbConn.Table<T_SECTOR>().FirstOrDefault(sector => sector.sector_id == secid);
            if (item != null)
            {
                return item;
            }
            else
            {
                return null;
            }
        }

        public List<string> GetSectorIdForSelectedCrag()
        {
            if (!string.IsNullOrEmpty(Settings.SelectedCragSettings))
            {
                var cragid = Settings.SelectedCragSettings;
                var item = from ts in dbConn.Table<T_SECTOR>() where ts.crag_id == cragid select ts.sector_id;
                var x = item.Count();
                return item.ToList();
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<T_TOPO> GetSectorImages(int skip, int take)
        {
            List<string> strid = GetSectorIdForSelectedCrag();
            var secimglist = (dbConn.Table<T_TOPO>().Where(tp => strid.Contains(tp.sector_id))).Skip(skip).Take(take);
            return secimglist;
        }

        public string GetSectorLines(string sectorid)
        {
            var sector = dbConn.Table<T_TOPO>().FirstOrDefault(sec => sec.sector_id == sectorid);
            return sector.topo_json;
        }

        public string GetAscentTypeIdByName(string ascent_name)
        {
            var ascenttype = dbConn.Table<TASCENT_TYPE>().FirstOrDefault(asc => asc.ascent_type_description == ascent_name);
            return ascenttype.ascent_type_id.ToString();
        }



        public string GetTTechGradeIdByGradeName(string grade_id, string grade_name)
        {
            var ttgrade = dbConn.Table<TTECH_GRADE>().FirstOrDefault(ttg => ttg.grade_type_id == grade_id && ttg.tech_grade == grade_name);
            return ttgrade.tech_grade_id.ToString();
        }

        //------------------------------------------

        public List<T_CRAG> GetCragList()
        {
            var craglist = dbConn.Table<T_CRAG>().ToList();
            return craglist;
        }

        /// <summary>
        /// Get Crag Details by Selected CragID
        /// </summary>
        /// <returns></returns>
        public T_CRAG GetSelectedCragData()
        {
            if (!string.IsNullOrEmpty(Settings.SelectedCragSettings))
            {
                var cragid = Settings.SelectedCragSettings;
                var item = dbConn.Table<T_CRAG>().FirstOrDefault(crag => crag.crag_id == cragid);
                return item;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get Routes List for Selected Sector
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T_ROUTE> GetRoutesBySectorId(string sectorid)
        {
            var item = dbConn.Table<T_ROUTE>().Where(route => route.sector_id == sectorid);
            return item;
        }

        /// <summary>
        /// Get Bucket Counts for Selected Sector
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T_GRADE> GetBucketCountsBySectorId(string sectorid)
        {
            var item = dbConn.Table<T_GRADE>().Where(grade => grade.sector_id == sectorid);
            return item;
        }

        public T_ROUTE GetRouteDataByRouteID(string routeid)
        {
            var item = dbConn.Table<T_ROUTE>().FirstOrDefault(route => route.route_id == routeid);
            if (item != null)
            {
                return item;
            }
            else
            {
                return null;
            }
        }

        public List<string> GetTtechGrades(string grade_typeid)
        {
            var item = from tg in dbConn.Table<TTECH_GRADE>() where tg.grade_type_id == grade_typeid select tg.tech_grade;
            return item.ToList();
        }

        #endregion

    }
}
