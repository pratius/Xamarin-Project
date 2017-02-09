using Newtonsoft.Json;
using SloperMobile.Common.Command;
using SloperMobile.Common.Constants;
using SloperMobile.Common.Helpers;
using SloperMobile.DataBase;
using SloperMobile.Model;
using SloperMobile.UserControls;
using SloperMobile.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SloperMobile.ViewModel
{
    public class AscentProcessViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private List<string> grades;
        private string sendstypename;
        private DateTime sendsdate = DateTime.Now.Date;
        private string sendsgrade;
        private int sendrating;
        private string sendclimbingstyle;
        private string sendholdtype;
        private string sendroutecharacteristics;
        private string commenttext;
        private string routeid;
        private bool isenable = true;
        private bool isdisplaymsg;
        private string commandtext = "Submit";
        private string progressmsg = "sending...";

        private string Slab = "";
        private string Vertical = "";
        private string Overhanging = "";
        private string Roof = "";

        private string Hold_Sloper = "";
        private string Hold_Crimps = "";
        private string Hold_Jugs = "";
        private string Hold_Pockets = "";


        private string Char_Technical = "";
        private string Char_Sequential = "";
        private string Char_Powerful = "";
        private string Char_Sustained = "";
        private string Char_Onemove = "";
        private string Char_Everything = "";

        private ImageSource summaryimage;

        private ImageSource topangle;
        private ImageSource tophold;
        private ImageSource toproutechar;
        private T_ROUTE routeData;
        public AscentProcessViewModel(INavigation navigation, string routeid)
        {
            PageHeaderText = Cache.SelctedCurrentSector?.SectorName;
            SummaryImage = Cache.SelctedCurrentSector?.SectorImage;
            RouteId = routeid;
            routeData = App.DAUtil.GetRouteDataByRouteID(RouteId);

            SendTypeCommand = new DelegateCommand(ExecuteOnSendType);
            SendTypeHoldCommand = new DelegateCommand(ExecuteOnSendHold);
            SendRouteCharaterCommand = new DelegateCommand(ExecuteOnRouteCharacteristics);
            SendRouteStyleCommand = new DelegateCommand(ExecuteOnRouteStyle);
            SendRatingCommand = new DelegateCommand(ExecuteOnRating);
            SendSummaryCommand = new DelegateCommand(ExecuteOnSummary);
            var grades = App.DAUtil.GetTtechGrades(routeData.grade_type_id);
            AscentGrages = grades;
            if(grades.Count>0)
            {
                SendsGrade = grades[0];
            }
            _navigation = navigation;
            //currentInstance = this;
        }

        //private static AscentProcessViewModel currentInstance;
        //public static AscentProcessViewModel CurrentInstance(INavigation navigation)
        //{
        //    if (currentInstance == null)
        //        return new AscentProcessViewModel(navigation);
        //    else
        //        return currentInstance;
        //}

        public List<string> AscentGrages
        {
            get { return grades; }
            set { grades = value; OnPropertyChanged(); }
        }
        public string SendsTypeName
        {
            get { return sendstypename; }
            set { sendstypename = value; OnPropertyChanged(); }
        }
        public DateTime SendsDate
        {
            get { return sendsdate; }
            set { sendsdate = value; OnPropertyChanged(); }
        }
        public string SendsGrade
        {
            get { return sendsgrade; }
            set { sendsgrade = value; OnPropertyChanged(); }
        }
        public int SendRating
        {
            get { return sendrating; }
            set { sendrating = value; OnPropertyChanged(); }
        }
        public string SendClimbingStyle
        {
            get { return sendclimbingstyle; }
            set { sendclimbingstyle = value; OnPropertyChanged(); }
        }
        public string SendHoldType
        {
            get { return sendholdtype; }
            set { sendholdtype = value; OnPropertyChanged(); }
        }
        public string SendRouteCharacteristics
        {
            get { return sendroutecharacteristics; }
            set { sendroutecharacteristics = value; OnPropertyChanged(); }
        }

        public ImageSource SummaryImage
        {
            get { return summaryimage; }
            set { summaryimage = value; OnPropertyChanged(); }
        }

        public string CommandText
        {
            get { return commandtext; }
            set { commandtext = value; OnPropertyChanged(); }
        }

        public string CommentText
        {
            get { return commenttext; }
            set { commenttext = value; OnPropertyChanged(); }
        }


        public string ProgressMsg
        {
            get { return progressmsg; }
            set { progressmsg = value; OnPropertyChanged(); }
        }


        public bool IsButtonInable
        {
            get { return isenable; }
            set { isenable = value; OnPropertyChanged(); }
        }
        public bool IsDisplayMessage
        {
            get { return isdisplaymsg; }
            set { isdisplaymsg = value; OnPropertyChanged(); }
        }

        public string RouteId
        {
            get { return routeid; }
            set { routeid = value; OnPropertyChanged(); }
        }


        public ImageSource TopAngle
        {
            get
            {
                if (topangle != null)
                {
                    return topangle;
                }
                else
                {
                    return topangle = ImageSource.FromFile(GetAngleResourceName(""));
                }
            }
            set { topangle = value; OnPropertyChanged(); }
        }
        public ImageSource TopHold
        {
            get
            {
                if (tophold != null)
                {
                    return tophold;
                }
                else
                {
                    return tophold = ImageSource.FromFile(GetHoldResourceName(""));
                }

            }
            set { tophold = value; OnPropertyChanged(); }
        }
        public ImageSource TopRouteChar
        {
            get
            {
                if (toproutechar != null)
                {
                    return toproutechar;
                }
                else
                {
                    return toproutechar = ImageSource.FromFile(GetRouteResourceName(""));
                }

            }
            set { toproutechar = value; OnPropertyChanged(); }
        }


        #region Delegate Commands
        public DelegateCommand SendTypeCommand { get; set; }
        public DelegateCommand SendTypeHoldCommand { get; set; }
        public DelegateCommand SendDataCommand { get; set; }
        public DelegateCommand SendRouteCharaterCommand { get; set; }
        public DelegateCommand SendRouteStyleCommand { get; set; }
        public DelegateCommand SendRatingCommand { get; set; }
        public DelegateCommand SendSummaryCommand { get; set; }
        #endregion

        private void ExecuteOnSendType(object obj)
        {
            SendsTypeName = Convert.ToString(obj);
        }

        private void ExecuteOnRouteCharacteristics(object obj)
        {
            var routecharacteristics = "";
            string routes = "";
            if (Convert.ToString(obj) == "1")
            {
                if (Char_Technical == "")
                {
                    Char_Technical = "1";
                }
                else
                {
                    Char_Technical = "";
                }
            }


            if (Convert.ToString(obj) == "2")
            {
                if (Char_Sequential == "")
                {
                    Char_Sequential = "2";
                }
                else
                {
                    Char_Sequential = "";
                }
            }


            if (Convert.ToString(obj) == "4")
            {
                if (Char_Powerful == "")
                {
                    Char_Powerful = "4";
                }
                else
                {
                    Char_Powerful = "";
                }
            }


            if (Convert.ToString(obj) == "8")
            {
                if (Char_Sustained == "")
                {
                    Char_Sustained = "8";
                }
                else
                {
                    Char_Sustained = "";
                }
            }


            if (Convert.ToString(obj) == "16")
            {
                if (Char_Onemove == "")
                {
                    Char_Onemove = "16";
                }
                else
                {
                    Char_Onemove = "";
                }
            }


            if (Convert.ToString(obj) == "all")
            {
                if (Char_Everything == "")
                {
                    Char_Everything = "all";
                    Char_Technical = "1";
                    Char_Sequential = "2";
                    Char_Powerful = "4";
                    Char_Sustained = "8";
                    Char_Onemove = "16";
                    routes = "all";
                }
                else
                {
                    Char_Technical = "";
                    Char_Sequential = "";
                    Char_Powerful = "";
                    Char_Sustained = "";
                    Char_Onemove = "";
                    Char_Everything = "";
                }
            }


            string[] characteristics = { Char_Technical, Char_Sequential, Char_Powerful, Char_Sustained, Char_Onemove };
            foreach (string str in characteristics)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    routecharacteristics += str + ",";
                    if (routes != "all")
                    {
                        routes = str;
                    }
                }
            }
            if (!string.IsNullOrEmpty(routecharacteristics))
            {
                SendRouteCharacteristics = routecharacteristics.TrimEnd(',');
            }
            else
            {
                SendRouteCharacteristics = routecharacteristics;
            }
            TopRouteChar = ImageSource.FromFile(GetRouteResourceName(routes));
        }

        private void ExecuteOnSendHold(object obj)
        {
            var holdingstyle = "";


            if (Convert.ToString(obj) == "1")
            {
                if (Hold_Sloper == "")
                {
                    Hold_Sloper = "1";
                }
                else
                {
                    Hold_Sloper = "";
                }
            }


            if (Convert.ToString(obj) == "2")
            {
                if (Hold_Crimps == "")
                {
                    Hold_Crimps = "2";
                }
                else
                {
                    Hold_Crimps = "";
                }
            }


            if (Convert.ToString(obj) == "4")
            {
                if (Hold_Jugs == "")
                {
                    Hold_Jugs = "4";
                }
                else
                {
                    Hold_Jugs = "";
                }
            }


            if (Convert.ToString(obj) == "8")
            {
                if (Hold_Pockets == "")
                {
                    Hold_Pockets = "8";
                }
                else
                {
                    Hold_Pockets = "";
                }
            }

            string tophold = "";
            string[] holdstyles = { Hold_Sloper, Hold_Crimps, Hold_Jugs, Hold_Pockets };
            foreach (string str in holdstyles)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    holdingstyle += str + ",";
                    tophold = str;
                }
            }
            if (!string.IsNullOrEmpty(holdingstyle))
            {
                SendHoldType = holdingstyle.TrimEnd(',');
            }
            else
            {
                SendHoldType = holdingstyle;
            }

            TopHold = ImageSource.FromFile(GetHoldResourceName(tophold));
        }

        private void ExecuteOnRouteStyle(object obj)
        {
            var climbingstyles = "";
            if (Convert.ToString(obj) == "1")
            {
                if (Slab == "")
                {
                    Slab = "1";
                }
                else
                {
                    Slab = "";
                }
            }


            if (Convert.ToString(obj) == "2")
            {
                if (Vertical == "")
                {
                    Vertical = "2";
                }
                else
                {
                    Vertical = "";
                }
            }


            if (Convert.ToString(obj) == "4")
            {
                if (Overhanging == "")
                {
                    Overhanging = "4";
                }
                else
                {
                    Overhanging = "";
                }
            }


            if (Convert.ToString(obj) == "8")
            {
                if (Roof == "")
                {
                    Roof = "8";
                }
                else
                {
                    Roof = "";
                }
            }


            string[] climbstyles = { Slab, Vertical, Overhanging, Roof };
            string topangle = "";
            foreach (string str in climbstyles)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    climbingstyles += str + ",";
                    if (str != "")
                    {
                        topangle = str;
                    }
                }
            }
            if (!string.IsNullOrEmpty(climbingstyles))
            {
                SendClimbingStyle = climbingstyles.TrimEnd(',');
            }
            else
            {
                SendClimbingStyle = climbingstyles;
            }

            TopAngle = ImageSource.FromFile(GetAngleResourceName(topangle));

        }

        private void ExecuteOnRating(object obj)
        {


        }

        private async void ExecuteOnSummary(object obj)
        {
            if (CommandText == "Submit")
            {
                IsRunningTasks = true;
                IsDisplayMessage = true;
                IsButtonInable = false;
                #region Collecting Ascent Data
                AscentPostModel ascent = new AscentPostModel();

                ascent.ascent_date = SendsDate;
                ascent.route_id = RouteId;
                ascent.ascent_type_id = App.DAUtil.GetAscentTypeIdByName(SendsTypeName);
                ascent.climbing_angle = "";
                if (SendClimbingStyle.Contains(","))
                {
                    string[] sarr1 = SendClimbingStyle.Split(',');
                    int climbbitmask = 0;
                    foreach (string s in sarr1)
                    {
                        climbbitmask += Convert.ToInt32(s);
                    }
                    ascent.climbing_angle_value = climbbitmask.ToString();
                }
                else
                {
                    ascent.climbing_angle_value = SendClimbingStyle;
                }
                ascent.comment = CommentText;
                ascent.grade_id = routeData.grade_type_id;
                ascent.hold_type = "";


                if (SendHoldType.Contains(","))
                {
                    string[] sarr2 = SendHoldType.Split(',');
                    int holdbitmask = 0;
                    foreach (string s in sarr2)
                    {
                        holdbitmask += Convert.ToInt32(s);
                    }
                    ascent.hold_type_value = holdbitmask.ToString();
                }
                else
                {
                    ascent.hold_type_value = SendHoldType;
                }

                ascent.ImageData = "";
                ascent.ImageName = "";
                ascent.photo = "";
                ascent.rating = SendRating.ToString();
                ascent.route_style = "";

                if (SendRouteCharacteristics.Contains(","))
                {
                    string[] sarr3 = SendRouteCharacteristics.Split(',');
                    int routebitmask = 0;
                    foreach (string s in sarr3)
                    {
                        routebitmask += Convert.ToInt32(s);
                    }
                    ascent.route_style_value = routebitmask.ToString();
                }
                else
                {
                    ascent.route_style_value = SendRouteCharacteristics;
                }
                ascent.route_type_id = routeData.route_type_id;
                ascent.tech_grade_id = App.DAUtil.GetTTechGradeIdByGradeName(routeData.grade_type_id, SendsGrade);
                ascent.video = "";
                #endregion

                var response = await HttpSendAscentProcess(ascent);
                if (response != null)
                {
                    ProgressMsg = "Ascent saved successfully.";
                    IsRunningTasks = false;
                }
                else
                {
                    ProgressMsg = "Please try again!";
                    IsRunningTasks = false;
                }
                CommandText = "Close";
                IsButtonInable = true;
            }
            else
            {
                IsRunningTasks = false;
                IsDisplayMessage = false;
                await _navigation.PushAsync(new HomePage());
            }

        }

        private string GetAngleResourceName(string steep)
        {
            string resource = "steepSlab.png";
            switch (steep)
            {
                case "1":
                    resource = "steepSlab.png";
                    break;
                case "2":
                    resource = "steepVertical.png";
                    break;
                case "4":
                    resource = "steepOverhanging.png";
                    break;
                case "8":
                    resource = "steepRoof.png";
                    break;
            }
            return resource;
        }

        private string GetHoldResourceName(string hold)
        {
            string resource = "hold_type_1_slopers.png";
            switch (hold)
            {
                case "1":
                    resource = "hold_type_1_slopers.png";
                    break;
                case "2":
                    resource = "hold_type_2_crimps.png";
                    break;
                case "4":
                    resource = "hold_type_4_jugs.png";
                    break;
                case "8":
                    resource = "hold_type_4_jugs.png";
                    break;
            }
            return resource;
        }

        private string GetRouteResourceName(string route)
        {
            string resource = "route_style_1_technical.png";
            switch (route)
            {
                case "1":
                    resource = "route_style_1_technical.png";
                    break;
                case "2":
                    resource = "route_style_2_sequential.png";
                    break;
                case "4":
                    resource = "route_style_4_powerful.png";
                    break;
                case "8":
                    resource = "route_style_8_sustained.png";
                    break;
                case "16":
                    resource = "route_style_16_one_move.png";
                    break;
                case "all":
                    resource = "route_style_everything.png";
                    break;
            }
            return resource;
        }

        #region Services

        private async Task<AscentReponse> HttpSendAscentProcess(AscentPostModel ascent)
        {
            HttpClientHelper apicall = new HttpClientHelper(ApiUrls.Url_SendAscent_Process, Settings.AccessTokenSettings);
            var ascentjson = JsonConvert.SerializeObject(ascent);
            var response = await apicall.Post<AscentReponse>(ascentjson);
            return response;
        }
        #endregion
    }
}
