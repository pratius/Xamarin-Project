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
            dbConn.CreateTable<T_BUCKET>();
            dbConn.CreateTable<TCRAG_IMAGE>();
            dbConn.CreateTable<TCRAG_PORTRAIT_IMAGE>();
            dbConn.CreateTable<TCRAG_LANDSCAPE_IMAGE>();

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
            /* Commented By Ravi on 28-apr-2017========================================
             
            if (dbConn.Table<TTECH_GRADE>().Count() == 0)
            {
                //string tech_grade_id = "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,83,84,85,86,87,88,89,90,91,92,93,94,95,96,97,98,99,100,102,104,105,106,107,108,109,110,111,112,113,114,115,116,117,118,119,120,121,122,123,124,125,126,127,128,129,130,131,132,133,134,135,136,137,138,139,140,141,142,143,144,145,146,147,148,149,150,151,152,153,154,155,156,157,158,159,1133,1134,1135,1136,1137,1138,1139,1140,1141,1142,1143,1144,1145,1146,1147,1148,1149,1150,1151,1152,1153,1154,1155,1156,1157,1158,1159,1160,1161,1162,1163,1164,1165,1166,1167,1168,1169,1170,1171,1172,1173,1174,1175,1176,1177,1178,1179,1180,1181,1182,1183,1184,1185,1186,1187,1188,1189,1190,1191,1192,1193,1194,1195,1196,1197,1198,1199,1200,1201,1202,1203,1204,1205,1206,1207,1208,1209,1210,1211,1212,1213,1214,1215,1216,1217,1218,1219,1220,1221,1222,1223,1224,1225,1226,1227,1228,1229,1230,1231,1232,1233,1234,1235,1236,1237,1238,1239,1240,1241,1242,1243,1244,1245,1246,1247,1248,1249,1250,1251,1252,1253,1254,1255,1256,1257,1258,1259,1260,1261,1262,1263,1264,1265,1266,1267,1268,1269,1270,1271,1272,1273,1274,1275,1276,1277,1278,1279,1280,1281,1282,1283,1284,1285,1286,1287,1288,1289,1290,1291,1292,1293,1294,1295,1296,1297,1298,1299,1300,1301,1302,1303,1304,1305,1306,1307,1308,1309,1310,1311,1312,1313,1314,1315,1316,1317,1318,1319,1320,1321,1322,1323,1324,1325,1326,1327,1328,1329,1330,1331,1332,1333,1334,1335,1336,1337,1338,1339,1340,1341,1342,1343,1344,1345,1346,1347,1348,1349,1350,1351,1352,1353,1354,1355,1356,1357,1358,1359,1360,1361,1362,1363,1364,1365,1366,1367,1368,1369,1370,1371,1372,1373,1374,1375,1376,1377,1378,1379,1380,1381,1382,1383,1384,1385,1386,1387,1388,1389,1390,1391,1392,1393,1394,1395,1396,1397,1398,1399,1400,1401,1402,1403,1404,1405,1406,1407,1408,1409,1410,1411,1412,1413,1414,1415,1416,1417,1419,1420,1421,1422,1423,1424,1425,1426,1427,1428,1429,1430,1431";
                //string grade_type_id = "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,1,1,1,1,1,2,2,2,2,2,1,1,1,8,8,8,8,9,9,9,9,9,9,9,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,11,11,11,11,11,11,11,11,11,11,11,11,11,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,6,6,6,6,6,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,17,17,17,17,17,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,3,3,3,19,19,19,19,19,19,19,19,19,19";
                //string tech_grade = "3,4a,5a,5c,6a,6a+,6b,6b+,6c,6c+,7a,7a+,7b,7b+,7c,7c+,8a,8a+,8b,8b+,8c,8c+,9a,9a+,9b,9b+,9c,9c+,10a,5B,5B+,5C,6A,6A+,6B,6B+,6C,6C+,7A,7A+,7B,7B+,7C,7C+,8A,8A+,8B,8B+,8C,8C+,9A,V0--,V1,V2,V3,V4,V5,V6,V7,V8,V9,V10,V11,V12,V13,V14,V15,V16,V17,V18,V19,V20,3a,3b,3c,4a,4b,4c,5a,5b,5c,6a,6b,6c,7a,7b,7c,8a,4c,4b,5b,P,?,3,4,5A,5A+,5C+,3+,-,2+,Low,Middle,High,Crux,A0,A1,A2,A3,A4,A5,A6,II,III,IV-,IV,IV+,V-,V,V+,6a,6a+,6b,6b+,6c,6c+,7a,3-,3,3+,4-,4,4+,5-,5,5+,6-,6,6+,7-,7,7+,8-,8,8+,9-,9,9+,10-,10,10+,11-,11,11+,-,M4,M5,M6,M7,M8,M9,M10,M11,M12,M13,P,?,?,P,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,6 / 6+,7 +/ 8-,8 / 8+,9 / 9+,9 +/ 10-,?,P,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,3-,3,3+,4-,4,4+,5-,5,5+,6-,6,6+,7-,7,7+,8-,8,8+,9-,9,9+,10-,10,10+,11-,11,11+,6 / 6+,7 +/ 8-,8 / 8+,9 / 9+,9 +/ 10-,3-,3,3+,4-,4,4+,5-,5,5+,6-,6,6+,7-,7,7+,8-,8,8+,9-,9,9+,10-,10,10+,11-,11,11+,6 / 6+,7 +/ 8-,8 / 8+,9 / 9+,9 +/ 10-,5,5.1,5.2,5.3,5.4,5.5,5.6,5.7,5.8,5.9,5.10a,5.10b,5.10c,5.10d,5.11a,5.11b,5.11c,5.11d,5.12a,5.12b,5.12c,5.12d,5.13a,5.13b,5.13c,5.13d,5.14a,5.14b,5.14c,5.14d,5.15a,5.15b,5.15c,5.15d,5,5.1,5.2,5.3,5.4,5.5,5.6,5.7,5.8,5.9,5.10a,5.10b,5.10c,5.10d,5.11a,5.11b,5.11c,5.11d,5.12a,5.12b,5.12c,5.12d,5.13a,5.13b,5.13c,5.13d,5.14a,5.14b,5.14c,5.14d,5.15a,5.15b,5.15c,5.15d,C1,C2,C3,C4,C5,1-,1,1+,2-,2,2+,3-,3,3+,4-,4,4+,5-,5,5+,5,5.1,5.2,5.3,5.4,5.5,5.6,5.7,5.8,5.9,5.10-,5.1,5.10+,5.11-,5.11,5.11+,5.12-,5.12,5.12+,5.13-,5.13,5.13+,5.14-,5.14,5.14+,5.15-,5.15,5.15+,V0-,V0,V0+,5.5-,5.5+,5.6-,5.6+,5.7-,5.7+,5.8-,5.8+,5.9-,5.9 +";
                //string sort_order = "12,20,50,70,80,90,100,110,120,130,140,150,160,170,180,190,200,210,220,230,240,250,260,270,280,290,300,310,320,50,60,70,90,100,110,120,130,140,150,160,170,180,190,200,210,220,230,240,250,260,270,0,10,20,30,40,50,60,70,80,90,100,110,120,130,140,150,160,170,180,190,200,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,40,30,60,0,-1,10,20,30,40,80,16,4,8,1,2,3,4,1,2,3,5,5,6,7,10,20,30,40,50,60,70,80,90,100,110,120,130,140,150,10,20,30,40,50,60,70,80,90,100,110,120,130,140,150,160,170,180,190,200,210,220,230,240,250,260,270,10,20,30,40,50,60,70,80,90,100,110,1000,15,3,6,10,20,30,40,50,60,70,80,90,100,110,120,130,140,150,160,170,180,190,200,210,220,230,240,250,260,270,280,290,300,310,320,330,340,350,360,370,380,390,400,410,420,430,115,155,175,205,215,3,6,10,20,30,40,50,60,70,80,90,100,110,120,130,140,150,160,170,180,190,200,210,220,230,240,250,260,270,280,290,300,310,320,330,340,350,360,370,380,390,400,10,20,30,40,50,60,70,80,90,100,110,120,130,140,150,160,170,180,190,200,210,220,230,240,250,260,270,115,155,175,205,215,10,20,30,40,50,60,70,80,90,100,110,120,130,140,150,160,170,180,190,200,210,220,230,240,250,260,270,115,155,175,205,215,10,20,30,40,50,60,70,80,90,100,110,120,130,140,150,160,170,180,190,200,210,220,230,240,250,260,270,280,290,300,310,320,330,340,10,20,30,40,50,60,70,80,90,100,110,120,130,140,150,160,170,180,190,200,210,220,230,240,250,260,270,280,290,300,310,320,330,340,1,2,3,4,5,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,10,20,30,40,50,60,70,80,90,100,110,120,130,150,160,170,190,200,210,230,240,250,270,280,290,310,320,330,2,4,6,59,61,69,71,79,81,89,91,99,101";
                //string grade_bucket_id = "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,3,3,3,3,3,3,4,4,4,4,4,4,5,5,5,5,5,5,5,1,3,3,4,4,4,4,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,2,2,2,2,3,3,3,3,4,4,4,4,5,5,5,5,5,5,5,5,5,5,5,5,1,2,3,4,5,1,1,1,2,2,2,3,3,3,4,4,4,5,5,5,1,1,1,1,1,1,1,1,1,1,2,2,2,3,3,3,4,4,4,5,5,5,5,5,5,5,5,5,2,2,2,1,1,1,1,1,1,1,1,1,1";

                // tech grades last updated March 19 2017
                string tech_grade_id = "96,95,105,106,1,104,2,93,92,3,94,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,97,98,99,100,30,31,32,102,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,1419,1420,1421,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,1146,1147,1148,1149,1150,1151,1152,1153,1154,1155,1156,1157,1158,1159,1160,1161,1162,1163,1164,1165,1166,1167,1168,1169,1170,1171,1172,1173,1174,1175,1176,1177,1178,1179,1180,1181,1182,1183,1184,1185,1186,1187,1188,1189,1190,133,134,135,136,137,138,139,140,141,142,143,1191,144,145,146,147,1192,148,149,1193,150,151,152,1194,153,1195,154,155,156,157,158,159,76,77,78,79,80,81,82,83,84,85,86,87,88,89,90,91,107,108,109,110,111,112,113,114,115,116,117,118,119,120,121,122,123,124,125,126,127,128,129,130,131,132,1133,1145,1134,1135,1136,1137,1138,1139,1140,1141,1142,1143,1144,1196,1197,1198,1199,1200,1201,1202,1203,1204,1205,1206,1207,1208,1209,1210,1211,1212,1213,1214,1215,1216,1217,1218,1219,1220,1221,1222,1223,1224,1225,1226,1227,1228,1229,1230,1231,1232,1233,1234,1235,1236,1237,1238,1239,1240,1241,1242,1243,1244,1245,1246,1247,1248,1265,1249,1250,1251,1252,1266,1253,1254,1267,1255,1256,1257,1268,1258,1269,1259,1260,1261,1262,1263,1264,1270,1271,1272,1273,1274,1275,1276,1277,1278,1279,1280,1297,1281,1282,1283,1284,1298,1285,1286,1299,1287,1288,1289,1300,1290,1301,1291,1292,1293,1294,1295,1296,1336,1337,1338,1339,1340,1341,1342,1343,1344,1345,1346,1347,1348,1349,1350,1351,1352,1353,1354,1355,1356,1357,1358,1359,1360,1361,1362,1363,1364,1365,1366,1367,1368,1369,1302,1303,1304,1305,1306,1307,1308,1309,1310,1311,1312,1313,1314,1315,1316,1317,1318,1319,1320,1321,1322,1323,1324,1325,1326,1327,1328,1329,1330,1331,1332,1333,1334,1335,1370,1371,1372,1373,1374,1432,1433,1434,1375,1376,1377,1378,1379,1380,1381,1382,1383,1384,1385,1386,1387,1388,1389,1390,1391,1392,1393,1394,1422,1395,1423,1424,1396,1425,1426,1397,1427,1428,1398,1429,1430,1399,1431,1400,1401,1402,1403,1404,1405,1406,1407,1408,1409,1410,1411,1412,1413,1414,1415,1416,1417";
                string grade_type_id = "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,8,8,8,8,9,9,9,9,9,9,9,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,11,11,11,11,11,11,11,11,11,11,11,11,11,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,17,17,17,17,17,17,17,17,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19,19";
                string tech_grade = "?,P,-,2+,3,3+,4a,4b,4c,5a,5b,5c,6a,6a+,6b,6b+,6c,6c+,7a,7a+,7b,7b+,7c,7c+,8a,8a+,8b,8b+,8c,8c+,9a,9a+,9b,9b+,9c,9c+,10a,3,4,5A,5A+,5B,5B+,5C,5C+,6A,6A+,6B,6B+,6C,6C+,7A,7A+,7B,7B+,7C,7C+,8A,8A+,8B,8B+,8C,8C+,9A,V0--,V0-,V0,V0+,V1,V2,V3,V4,V5,V6,V7,V8,V9,V10,V11,V12,V13,V14,V15,V16,V17,V18,V19,V20,?,P,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,3-,3,3+,4-,4,4+,5-,5,5+,6-,6,6/6+,6+,7-,7,7+,7+/8-,8-,8,8/8+,8+,9-,9,9/9+,9+,9+/10-,10-,10,10+,11-,11,11+,3a,3b,3c,4a,4b,4c,5a,5b,5c,6a,6b,6c,7a,7b,7c,8a,Low,Middle,High,Crux,A0,A1,A2,A3,A4,A5,A6,II,III,IV-,IV,IV+,V-,V,V+,6a,6a+,6b,6b+,6c,6c+,7a,-,?,M4,M5,M6,M7,M8,M9,M10,M11,M12,M13,P,?,P,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,3-,3,3+,4-,4,4+,5-,5,5+,6-,6,6/6+,6+,7-,7,7+,7+/8-,8-,8,8/8+,8+,9-,9,9/9+,9+,9+/10-,10-,10,10+,11-,11,11+,3-,3,3+,4-,4,4+,5-,5,5+,6-,6,6/6+,6+,7-,7,7+,7+/8-,8-,8,8/8+,8+,9-,9,9/9+,9+,9+/10-,10-,10,10+,11-,11,11+,5,5.1,5.2,5.3,5.4,5.5,5.6,5.7,5.8,5.9,5.10a,5.10b,5.10c,5.10d,5.11a,5.11b,5.11c,5.11d,5.12a,5.12b,5.12c,5.12d,5.13a,5.13b,5.13c,5.13d,5.14a,5.14b,5.14c,5.14d,5.15a,5.15b,5.15c,5.15d,5,5.1,5.2,5.3,5.4,5.5,5.6,5.7,5.8,5.9,5.10a,5.10b,5.10c,5.10d,5.11a,5.11b,5.11c,5.11d,5.12a,5.12b,5.12c,5.12d,5.13a,5.13b,5.13c,5.13d,5.14a,5.14b,5.14c,5.14d,5.15a,5.15b,5.15c,5.15d,C1,C2,C3,C4,C5,C6,C7,C8,1-,1,1+,2-,2,2+,3-,3,3+,4-,4,4+,5-,5,5+,5,5.1,5.2,5.3,5.4,5.5-,5.5,5.5+,5.6-,5.6,5.6+,5.7-,5.7,5.7+,5.8-,5.8,5.8+,5.9-,5.9,5.9+,5.10-,5.10,5.10+,5.11-,5.11,5.11+,5.12-,5.12,5.12+,5.13-,5.13,5.13+,5.14-,5.14,5.14+,5.15-,5.15,5.15+";
                string sort_order = "-1,0,4,8,12,16,20,30,40,50,60,70,80,90,100,110,120,130,140,150,160,170,180,190,200,210,220,230,240,250,260,270,280,290,300,310,320,10,20,30,40,50,60,70,80,90,100,110,120,130,140,150,160,170,180,190,200,210,220,230,240,250,260,270,0,2,4,6,10,20,30,40,50,60,70,80,90,100,110,120,130,140,150,160,170,180,190,200,3,6,10,20,30,40,50,60,70,80,90,100,110,120,130,140,150,160,170,180,190,200,210,220,230,240,250,260,270,280,290,300,310,320,330,340,350,360,370,380,390,400,410,420,430,10,20,30,40,50,60,70,80,90,100,110,115,120,130,140,150,155,160,170,175,180,190,200,205,210,215,220,230,240,250,260,270,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,2,3,4,1,2,3,5,5,6,7,10,20,30,40,50,60,70,80,90,100,110,120,130,140,150,10,15,20,30,40,50,60,70,80,90,100,110,1000,3,6,10,20,30,40,50,60,70,80,90,100,110,120,130,140,150,160,170,180,190,200,210,220,230,240,250,260,270,280,290,300,310,320,330,340,350,360,370,380,390,400,10,20,30,40,50,60,70,80,90,100,110,115,120,130,140,150,155,160,170,175,180,190,200,205,210,215,220,230,240,250,260,270,10,20,30,40,50,60,70,80,90,100,110,115,120,130,140,150,155,160,170,175,180,190,200,205,210,215,220,230,240,250,260,270,10,20,30,40,50,60,70,80,90,100,110,120,130,140,150,160,170,180,190,200,210,220,230,240,250,260,270,280,290,300,310,320,330,340,10,20,30,40,50,60,70,80,90,100,110,120,130,140,150,160,170,180,190,200,210,220,230,240,250,260,270,280,290,300,310,320,330,340,1,2,3,4,5,6,7,8,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,10,20,30,40,50,59,60,61,69,70,71,79,80,81,89,90,91,99,100,101,110,120,130,150,160,170,190,200,210,230,240,250,270,280,290,310,320,330";
                string grade_bucket_id = "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,2,2,2,2,2,2,3,3,3,3,3,3,4,4,4,4,4,4,5,5,5,5,5,5,5,1,2,2,2,3,3,4,4,4,4,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,2,2,2,2,3,3,3,3,4,4,4,4,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,2,2,3,3,4,4,5,1,1,1,2,2,2,3,3,3,4,4,4,5,5,5,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,3,3,3,4,4,4,5,5,5,5,5,5,5,5,5";


                string[] tech_grd_id = tech_grade_id.Split(',');
                string[] grade_typ_id = grade_type_id.Split(',');
                string[] tec_grade = tech_grade.Split(',');
                string[] sort_ordr = sort_order.Split(',');
                string[] grade_bkt_id = grade_bucket_id.Split(',');
                for (int i = 0; i < grade_typ_id.Length; i++)
                {
                    TTECH_GRADE tblObj = new TTECH_GRADE();
                    tblObj.tech_grade_id = tech_grd_id[i];
                    tblObj.grade_type_id = grade_typ_id[i];
                    tblObj.tech_grade = tec_grade[i];
                    tblObj.sort_order = Int32.Parse(sort_ordr[i]);
                    tblObj.grade_bucket_id = grade_bkt_id[i];
                    dbConn.Insert(tblObj);
                }
            }
            */
            //if (dbConn.Table<T_BUCKET>().Count() == 0)
            //{
            //    string grade_type_id = "1,1,1,1,1,3,3,3,3,3,7,7,7,7,7,15,15,15,15,15,17,17,17,17,17,18,18,18,18,18,19,19,19,19,19";
            //    string[] grd_typ_id = grade_type_id.Split(',');

            //    string grade_bucket_id = "1,2,3,4,5,1,2,3,4,5,1,2,3,4,5,1,2,3,4,5,1,2,3,4,5,1,2,3,4,5,1,2,3,4,5";
            //    string[] grd_bkt_id = grade_bucket_id.Split(',');

            //    string bucket_name = "< 5,5+ - 6a+,6b - 7a,7a+ - 7c+,8a >,< V1,V2 - V3 ,V4 - V5,V5 - V8 ,V8 >,< HS,VS - E1,E2 - E4,E5 - E6,E7 >,< 5.9,10a - 10d,11a - 11d,12a - 12d,13a >,1,2 - 3,4 - 5 ,6 - 7 ,8,< 1,2 - 3,4 - 5 ,6 - 7 ,8 >,< 5.9,10a - 10d,11a - 11d,12a - 12d,13a >";
            //    string[] bkt_name = bucket_name.Split(',');

            //    for (int i = 0; i < grd_typ_id.Length; i++)
            //    {
            //        T_BUCKET tblObj = new T_BUCKET();
            //        tblObj.grade_type_id = grd_typ_id[i];
            //        tblObj.grade_bucket_id = grd_bkt_id[i];
            //        tblObj.bucket_name = bkt_name[i];
            //        dbConn.Insert(tblObj);
            //    }
            //}

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

        //================================T_BUCKET=================================
        public int SaveGradeBucket(T_BUCKET aBucket)
        {
            return dbConn.Insert(aBucket);
        }
        //=======================================================================
        //================= Added by Ravi on 28-Apr-2017=============
        public int SaveTTechGrade(TTECH_GRADE aTTGrade)
        {
            var item = dbConn.Table<TTECH_GRADE>().FirstOrDefault(ttgrade => ttgrade.tech_grade_id == aTTGrade.tech_grade_id);
            if (item != null)
            {
                aTTGrade.id = item.id;
                return dbConn.Update(aTTGrade);
            }
            else
            {
                return dbConn.Insert(aTTGrade);
            }
        }
        //=======================================================================
        //================= Added by Ravi on 02-May-2017=============
        public int SaveTCragImage(TCRAG_IMAGE acragimg)
        {
            var item = dbConn.Table<TCRAG_IMAGE>().FirstOrDefault(tcragimg => tcragimg.crag_id == acragimg.crag_id);
            if (item != null)
            {
                acragimg.id = item.id;
                return dbConn.Update(acragimg);
            }
            else
            {
                return dbConn.Insert(acragimg);
            }
        }
        //================= Added by Sandeep on 23-May-2017=============
        public int SaveTCragPortraitImage(TCRAG_PORTRAIT_IMAGE acragimg)
        {
            var item = dbConn.Table<TCRAG_PORTRAIT_IMAGE>().FirstOrDefault(tcragimg => tcragimg.crag_id == acragimg.crag_id);
            if (item != null)
            {
                acragimg.id = item.id;
                return dbConn.Update(acragimg);
            }
            else
            {
                return dbConn.Insert(acragimg);
            }
        }
        //================= Added by Sandeep on 23-May-2017=============
        public int SaveTCragLandscapeImage(TCRAG_LANDSCAPE_IMAGE acragimg)
        {
            var item = dbConn.Table<TCRAG_LANDSCAPE_IMAGE>().FirstOrDefault(tcragimg => tcragimg.crag_id == acragimg.crag_id);
            if (item != null)
            {
                acragimg.id = item.id;
                return dbConn.Update(acragimg);
            }
            else
            {
                return dbConn.Insert(acragimg);
            }
        }
        //================================ Drop and Create Table ================
        public void DropAndCreateTable(Type aTable)
        {
            if (aTable.Name == "T_GRADE")
            {
                dbConn.DropTable<T_GRADE>();
                dbConn.CreateTable<T_GRADE>();
            }

            if (aTable.Name == "T_BUCKET")
            {
                dbConn.DropTable<T_BUCKET>();
                dbConn.CreateTable<T_BUCKET>();
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
            var item = dbConn.Table<T_SECTOR>().Where(s => s.is_enabled == true).FirstOrDefault(sector => sector.sector_id == secid);
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
                var item = from ts in dbConn.Table<T_SECTOR>() where ts.crag_id == cragid && ts.is_enabled == true select ts.sector_id;
                var x = item.Count();
                return item.ToList();
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<T_TOPO> GetAllSectorImages()
        {
            List<string> strid = GetSectorIdForSelectedCrag();
            var secimglist = (dbConn.Table<T_TOPO>().Where(tp => strid.Contains(tp.sector_id)));
            return secimglist;
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
            //var sector = dbConn.Table<T_TOPO>().Where(sec => sec.sector_id == sectorid);
            return sector.topo_json;
        }
        public T_TOPO GetSectorTopoBySectorId(string sectorid)
        {
            var sector = dbConn.Table<T_TOPO>().FirstOrDefault(sec => sec.sector_id == sectorid);
            return sector;
        }

        public string GetAscentTypeIdByName(string ascent_name)
        {
            var ascenttype = dbConn.Table<TASCENT_TYPE>().FirstOrDefault(asc => asc.ascent_type_description == ascent_name);
            return ascenttype.ascent_type_id.ToString();
        }



        public string GetTTechGradeIdByGradeName(string grade_id, string grade_name)
        {
            var ttgrade = dbConn.Table<TTECH_GRADE>().FirstOrDefault(ttg => ttg.grade_type_id == grade_id && ttg.tech_grade == grade_name);
            if (ttgrade != null)
            {
                return ttgrade.tech_grade_id.ToString();
            }
            else
            {
                return null;
            }
        }

        //------------------------------------------

        public List<T_CRAG> GetCragList()
        {
            var craglist = dbConn.Table<T_CRAG>().Where(x => x.is_enabled == true).ToList();
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
                var item = dbConn.Table<T_CRAG>().Where(c => c.is_enabled == true).FirstOrDefault(crag => crag.crag_id == cragid);
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
            var item = dbConn.Table<T_ROUTE>().Where(route => route.sector_id == sectorid && route.is_enabled == true).OrderBy(x => x.sort_order);
            return item;
        }


        /// <summary>
        /// Get Routes List for Selected Crag
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T_ROUTE> GetRoutesForSelectedCrag()
        {
            if (!string.IsNullOrEmpty(Settings.SelectedCragSettings))
            {
                var cragid = Settings.SelectedCragSettings;
                var item = dbConn.Table<T_ROUTE>().Where(route => route.crag_id == cragid && route.is_enabled == true).OrderByDescending(x => x.date_created);
                return item;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Get Bucket Counts for Selected Sector
        /// </summary>
        /// <returns></returns>
        public List<T_GRADE> GetBucketCountsBySectorId(string sectorid)
        {
            var item = dbConn.Table<T_GRADE>().Where(grade => grade.sector_id == sectorid).OrderBy(x => x.grade_bucket_id);
            return item.ToList();
        }

        public T_ROUTE GetRouteDataByRouteID(string routeid)
        {
            var item = dbConn.Table<T_ROUTE>().Where(r => r.is_enabled == true).FirstOrDefault(route => route.route_id == routeid);
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
            var item = from tg in dbConn.Table<TTECH_GRADE>() where tg.grade_type_id == grade_typeid orderby tg.sort_order ascending select tg.tech_grade;
            return item.ToList();
        }

        //public List<GradeId> GetGradeTypeIdByCragId(string cragid)
        //{
        //    //var item = (from tr in dbConn.Table<T_ROUTE>() join ts in dbConn.Table<T_SECTOR>() 
        //    //            on tr.sector_id equals ts.sector_id where ts.crag_id==cragid select tr.grade_type_id).Distinct();

        //    var item = dbConn.Query<GradeId>("SELECT distinct T_ROUTE.grade_type_id FROM T_ROUTE JOIN T_SECTOR ON T_ROUTE.sector_id = T_SECTOR.sector_id WHERE T_SECTOR.crag_id = ?", cragid);
        //    return item;
        //}

        public List<GradeId> GetGradeTypeIdBySectorId(string sectorid)
        {
            var item = dbConn.Query<GradeId>("SELECT distinct grade_type_id FROM T_ROUTE  WHERE sector_id = ?", sectorid);
            return item;
        }

        public List<Buckets> GetBucketsByCragID(string cragid)
        {
            var item = dbConn.Query<Buckets>("SELECT DISTINCT bucket_name as BucketName, hex_code as HexColor FROM T_BUCKET WHERE (grade_type_id IN(SELECT DISTINCT T_ROUTE.grade_type_id FROM T_SECTOR INNER JOIN T_ROUTE ON T_SECTOR.sector_id = T_ROUTE.sector_id WHERE(T_SECTOR.crag_id = ?))) ORDER BY grade_bucket_group, grade_bucket_id", cragid);
            //dbConn.Table<T_BUCKET>().Where(x => x.grade_type_id == gradetypeid).OrderBy(x => x.grade_bucket_id).ToList();
            return item;
        }
        public List<Buckets> GetBucketsBySectorID(string sectorid)
        {
            var item = dbConn.Query<Buckets>("SELECT DISTINCT bucket_name as BucketName, hex_code as HexColor FROM T_BUCKET WHERE (grade_type_id IN(SELECT DISTINCT grade_type_id FROM T_ROUTE WHERE (sector_id = ? ))) ORDER BY grade_bucket_group, grade_bucket_id", sectorid);
            return item;
        }

        public int GetTotalBucketForApp()
        {
            return dbConn.ExecuteScalar<int>("SELECT Count(grade_type_id) As BucketCount FROM T_BUCKET GROUP BY grade_type_id LIMIT 1");
        }

        public string GetBucketCountBySectorIdAndGradeBucketId(string sectorid, string grbucketid)
        {
            return dbConn.ExecuteScalar<string>("SELECT grade_bucket_id_count FROM T_GRADE WHERE sector_id = ? and grade_bucket_id = ?", sectorid, grbucketid);
        }

        public string GetBucketHexColorByGradeBucketId(string grbucketid)
        {
            return dbConn.ExecuteScalar<string>("SELECT hex_code FROM T_BUCKET Where grade_bucket_id= ? Limit 1", grbucketid);
        }

        public List<NewsModel> GetAppNews(int skip, int take)
        {
            //var item = dbConn.Query<NewsModel>("SELECT T_SECTOR.sector_id as id, T_ROUTE.date_created as date_created, T_SECTOR.sector_name, COUNT(T_SECTOR.sector_name) as new_route_count,'nr' as news_type FROM T_SECTOR INNER JOIN T_ROUTE ON T_SECTOR.sector_id = T_ROUTE.sector_id GROUP BY T_ROUTE.date_created, T_SECTOR.sector_name, T_SECTOR.sort_order ORDER BY T_ROUTE.date_created DESC, T_SECTOR.sort_order ");
            var item = dbConn.Query<NewsModel>("SELECT T_ROUTE.date_created AS date, T_SECTOR.sector_id AS id, UPPER(T_CRAG.crag_name) as title, UPPER(T_SECTOR.sector_name) as sub_title, COUNT(T_SECTOR.sector_name) AS count, (COUNT(T_SECTOR.sector_name) || ' NEW ' || (CASE WHEN COUNT(T_SECTOR.sector_name) = 1 THEN 'ROUTE' ELSE 'ROUTES' END)) as message, 'newRoutes' AS news_type FROM T_SECTOR INNER JOIN T_ROUTE ON T_SECTOR.sector_id = T_ROUTE.sector_id INNER JOIN T_CRAG ON T_SECTOR.crag_id = T_CRAG.crag_id WHERE T_SECTOR.is_enabled=1 GROUP BY T_ROUTE.date_created, T_SECTOR.sector_id, T_CRAG.crag_name, T_SECTOR.sector_name, T_SECTOR.sort_order ORDER BY date DESC, T_SECTOR.sort_order");
            var newslist = (item).Skip(skip).Take(take);
            return newslist.ToList();
           // return item.ToList();
        }


        public TCRAG_IMAGE GetScenicImageForCrag(string cragid)
        {
            var item = dbConn.Table<TCRAG_IMAGE>().Where(tci => tci.crag_id == cragid).FirstOrDefault();
            if (item != null)
            {
                return item;
            }
            else
            {
                return null;
            }
        }

        public List<string> GetRouteTypesByCragID(string cragid)
        {
            var item = (from tr in dbConn.Table<T_ROUTE>() where tr.crag_id == cragid && tr.is_enabled == true orderby tr.route_type select tr.route_type).Distinct() ;
            return item.ToList();
        }

        #endregion

    }
}
