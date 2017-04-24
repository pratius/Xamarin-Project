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
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.IO;
using Acr.UserDialogs;
using Plugin.Connectivity;

namespace SloperMobile.ViewModel
{
    public class AscentProcessViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private List<string> grades;
        private List<string> listimgs;
        private string sendstypename;
        private string sendscongratswording;
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

        private bool isdisplaysummaryimg = false;
        private bool isdisplaysummaryweb = true;

        private string commandtext = "Log Ascent";
        private string progressmsg = "Sending, please wait...";

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
        private Stream camera_image;
        private T_ROUTE routeData;
        private T_CRAG currentCrag;
        public AscentProcessViewModel(INavigation navigation, string routeid)
        {
            currentCrag = App.DAUtil.GetSelectedCragData();
            SummaryImage = Cache.SelctedCurrentSector?.SectorImage;
            RouteId = routeid;
            routeData = App.DAUtil.GetRouteDataByRouteID(RouteId);

            PageHeaderText = (routeData.route_name).ToUpper() + " " + routeData.tech_grade;
            PageSubHeaderText = (currentCrag.crag_name).Trim() + ", " + Cache.SelctedCurrentSector.SectorName;

            SendTypeCommand = new DelegateCommand(ExecuteOnSendType);
            SendTypeHoldCommand = new DelegateCommand(ExecuteOnSendHold);
            SendRouteCharaterCommand = new DelegateCommand(ExecuteOnRouteCharacteristics);
            SendRouteStyleCommand = new DelegateCommand(ExecuteOnRouteStyle);
            SendRatingCommand = new DelegateCommand(ExecuteOnRating);
            SendSummaryCommand = new DelegateCommand(ExecuteOnSummary);
            CameraClickCommand = new DelegateCommand(ExecuteOnCameraClick);
            CommentClickCommand = new DelegateCommand(ExecuteOnCommentClick);
            var grades = App.DAUtil.GetTtechGrades(routeData.grade_type_id);
            AscentGrades = grades;
            if (grades.Count > 0)
            {
                SendsGrade = routeData.tech_grade; //grades[0];
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

        public List<string> AscentGrades
        {
            get { return grades; }
            set { grades = value; OnPropertyChanged(); }
        }

        public List<string> ListImages
        {
            get { return listimgs; }
            set { listimgs = value; OnPropertyChanged(); }
        }

        public string SendsTypeName
        {
            get { return sendstypename; }
            set { sendstypename = value; OnPropertyChanged(); }
        }
        

        public string SendsCongratsWording
        {
            get { return sendscongratswording; }
            set { sendscongratswording = value; OnPropertyChanged(); }
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

        public Stream CameraImage
        {
            get { return camera_image; }
            set { camera_image = value; OnPropertyChanged(); }
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

        public bool IsDisplaySummaryImg
        {
            get { return isdisplaysummaryimg; }
            set { isdisplaysummaryimg = value; OnPropertyChanged(); }
        }

        public bool IsDisplaySummaryWeb
        {
            get { return isdisplaysummaryweb; }
            set { isdisplaysummaryweb = value; OnPropertyChanged(); }
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

        public DelegateCommand CameraClickCommand { get; set; }
        public DelegateCommand CommentClickCommand { get; set; }
        #endregion

        private void ExecuteOnSendType(object obj)
        {

            SendsTypeName = Convert.ToString(obj);
            //SendsCongratsWording = string.Empty;
            switch (Convert.ToString(obj))
            {
                case "Onsight":
                    SendsCongratsWording = "Boom! Nice ";
                    break;
                case "Flash":
                    SendsCongratsWording = "Cool ";
                    break;
                case "Redpoint":
                    SendsCongratsWording = "Awesome ";
                    break;
                case "Repeat":
                    SendsCongratsWording = "Good ";
                    break;
                case "Making Progress":
                    SendsCongratsWording = "(Project burn) ";
                    break;
                case "Good Work":
                    SendsCongratsWording = "One hang! ";
                    break;
            }

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
        private async void ExecuteOnCameraClick(object obj)
        {
            var action = await Application.Current.MainPage.DisplayActionSheet("Choose Ascent Image", "Cancel", null, "Take photo", "Pick a file");
            if (action == "Take photo")
            {
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert("No Camera", "No camera available!", "OK");
                    return;
                }
                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    AllowCropping = true,
                    Name = "Ascent_" + DateTime.Now.ToString()
                });
                if (file == null)
                {
                    return;
                }
                CameraImage = file.GetStream();
                SummaryImage = ImageSource.FromStream(() =>
                {
                    var imgstream = file.GetStream();
                    file.Dispose();
                    return imgstream;
                });
                IsDisplaySummaryWeb = false;
                IsDisplaySummaryImg = true;
            }
            if (action == "Pick a file")
            {
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert("No Gallery", "Picking a photo is not supported.", "OK");
                    return;
                }
                var file = await CrossMedia.Current.PickPhotoAsync();
                if (file == null)
                {
                    return;
                }
                SummaryImage = ImageSource.FromStream(() => file.GetStream());
                CameraImage = file.GetStream();
                IsDisplaySummaryWeb = false;
                IsDisplaySummaryImg = true;
            }
        }

        private async void ExecuteOnCommentClick(object obj)
        {
            PromptResult result = await UserDialogs.Instance.PromptAsync(new PromptConfig
            {
                Title = "Comments",
                InputType = InputType.Name,
                Message = "Give us your thoughts on this climb",
                Text = CommentText,
                MaxLength = 250,
                Placeholder = "type here",
                OkText = "Ok",
                IsCancellable = true,
                CancelText = "Cancel"
            });
            if (result.Ok)
            {
                CommentText = result.Text;
            }
        }

        private async void ExecuteOnSummary(object obj)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (CommandText == "Log Ascent")
                {
                    IsRunningTasks = true;
                    IsDisplayMessage = true;
                    IsButtonInable = false;
                    #region Collecting Ascent Data
                    AscentPostModel ascent = new AscentPostModel();

                    ascent.ascent_date = SendsDate;
                    ascent.route_id = RouteId;
                    ascent.ascent_type_id = SendsTypeName != null ? App.DAUtil.GetAscentTypeIdByName(SendsTypeName) : "0";
                    ascent.climbing_angle = "";
                    if (SendClimbingStyle != null)
                    {
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
                    }
                    else { ascent.climbing_angle_value = "0"; }
                    ascent.comment = CommentText;
                    ascent.grade_id = routeData.grade_type_id;
                    ascent.hold_type = "";

                    if (SendHoldType != null)
                    {
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
                    }
                    else { ascent.hold_type_value = "0"; }
                    ascent.ImageData = "";
                    if (CameraImage != null)
                    {
                        //StreamImageSource strmimg = (StreamImageSource)CameraImage;
                        //System.Threading.CancellationToken cancellationToken = System.Threading.CancellationToken.None;
                        //Task<Stream> task = strmimg.Stream(cancellationToken);
                        //Stream stream = task.Result;
                        byte[] imageBytes = ReadStreamByte(CameraImage);
                        ascent.ImageData = Convert.ToBase64String(imageBytes);
                    }
                    ascent.ImageName = "";
                    ascent.photo = "";
                    ascent.rating = SendRating.ToString();
                    ascent.route_style = "";

                    if (SendRouteCharacteristics != null)
                    {
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
                    }
                    else { ascent.route_style_value = "0"; }
                    ascent.route_type_id = routeData.route_type_id;
                    ascent.tech_grade_id = App.DAUtil.GetTTechGradeIdByGradeName(routeData.grade_type_id, SendsGrade);
                    ascent.video = "";
                    #endregion

                    var response = await HttpSendAscentProcess(ascent);
                    if (response != null && response.id != null)
                    //if (response != null)
                    {
                        if (!string.IsNullOrEmpty(response.climbingDays))
                        {
                            Settings.ClimbingDaysSettings = Convert.ToInt32(response.climbingDays);
                        }
                        await CheckForUpdatesViewModel.CurrentInstance().OnPageAppearing();
                        ProgressMsg = "Ascent saved successfully.";
                        IsRunningTasks = false;
                    }
                    else
                    {
                        ProgressMsg = "Please try again!";
                        IsRunningTasks = false;
                    }
                    CommandText = "Continue";
                    IsButtonInable = true;
                    return;
                }
                else
                {
                    IsRunningTasks = false;
                    IsDisplayMessage = false;
                    await _navigation.PushAsync(new MapPage());
                }
            }
            else
                await _navigation.PushAsync(new NetworkErrorPage());
        }

        public string GetAngleResourceName(string steep)
        {
            string resource = "icon_steepness_1_slab_border_80x80";
            switch (steep)
            {
                case "1":
                    resource = "icon_steepness_1_slab_border_80x80";
                    break;
                case "2":
                    resource = "icon_steepness_2_vertical_border_80x80";
                    break;
                case "4":
                    resource = "icon_steepness_4_overhanging_border_80x80";
                    break;
                case "8":
                    resource = "icon_steepness_8_roof_border_80x80";
                    break;
            }
            return resource;
        }

        public string GetHoldResourceName(string hold)
        {
            string resource = "icon_hold_type_1_slopers_text_58x92";
            switch (hold)
            {
                case "1":
                    resource = "icon_hold_type_1_slopers_text_58x92";
                    break;
                case "2":
                    resource = "icon_hold_type_2_crimps_text_41x68";
                    break;
                case "4":
                    resource = "icon_hold_type_4_jugs_text_58x74";
                    break;
                case "8":
                    resource = "icon_hold_type_8_pockets_text_63x94";
                    break;
            }
            return resource;
        }

        public string GetRouteResourceName(string route)
        {
            string resource = "icon_route_style_1_technical_border_80x80";
            switch (route)
            {
                case "1":
                    resource = "icon_route_style_1_technical_border_80x80";
                    break;
                case "2":
                    resource = "icon_route_style_2_sequential_border_80x80";
                    break;
                case "4":
                    resource = "icon_route_style_4_powerful_border_80x80";
                    break;
                case "8":
                    resource = "icon_route_style_8_sustained_border_80x80";
                    break;
                case "16":
                    resource = "icon_route_style_16_one_move_border_80x80";
                    break;
                case "all":
                    resource = "icon_route_style_32_everything_border_80x80";
                    break;
            }
            return resource;
        }

        public string GetSummarySteepnessResourceName(string steep)
        {
            string resource = "icon_steepness_1_slab_border_80x80";
            switch (steep)
            {
                case "1":
                    resource = "icon_steepness_1_slab_border_80x80";
                    break;
                case "2":
                    resource = "icon_steepness_2_vertical_border_80x80";
                    break;
                case "4":
                    resource = "icon_steepness_4_overhanging_border_80x80";
                    break;
                case "8":
                    resource = "icon_steepness_8_roof_border_80x80";
                    break;
            }
            return resource;
        }

        public string GetSummaryHoldTypeResourceName(string hold)
        {
            string resource = "icon_hold_type_1_slopers_border_80x80";
            switch (hold)
            {
                case "1":
                    resource = "icon_hold_type_1_slopers_border_80x80";
                    break;
                case "2":
                    resource = "icon_hold_type_2_crimps_border_80x80";
                    break;
                case "4":
                    resource = "icon_hold_type_4_jugs_border_80x80";
                    break;
                case "8":
                    resource = "icon_hold_type_8_pockets_border_80x80";
                    break;
            }
            return resource;
        }

        public string GetSummaryRouteStyleResourceName(string route)
        {
            string resource = "icon_route_style_1_technical_border_80x80";
            switch (route)
            {
                case "1":
                    resource = "icon_route_style_1_technical_border_80x80";
                    break;
                case "2":
                    resource = "icon_route_style_2_sequential_border_80x80";
                    break;
                case "4":
                    resource = "icon_route_style_4_powerful_border_80x80";
                    break;
                case "8":
                    resource = "icon_route_style_8_sustained_border_80x80";
                    break;
                case "16":
                    resource = "icon_route_style_16_one_move_border_80x80";
                    break;
                case "all":
                    resource = "icon_route_style_32_everything_border_80x80";
                    break;
            }
            return resource;
        }




        private static byte[] ReadStreamByte(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
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
