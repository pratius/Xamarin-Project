using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloperMobile.Common.Enumerators
{
    public enum ApplicationActivity
    {
        CheckForUpdatesPage = 1,
        ClimbingDaysPage = 2,
        HomePage = 3,
        LoginPage = 4,
        MapPage = 5,
        MenuListPage = 6,
        MenuNavigationPage = 7,
        NewsPage = 8,
        ProfilePage = 9,
        PyramidPage = 10,
        RegistrationPage = 11,
        SearchPage = 12,
        SendsPage = 13,
        SettingsPage = 14,
        CragMapPage = 15

    }

    public enum AppSteepness
    {
        Slab=1 ,
        Vertical=2 ,
        Overhanging= 4,
        Roof= 8 
    }
}

