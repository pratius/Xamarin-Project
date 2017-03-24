using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SloperMobile.ViewModel;

namespace SloperMobile
{
    public partial class TermsPage : ContentPage
    {
        public TermsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new TermsViewModel();

            lblDisclaimerTitle.Text = "Disclaimer";
            lblDisclaimerContent.Text = "\nThe inclusion of topos or of a crag or routes upon them does not imply a right of access to the crag or the right to climb upon it. Neither the authors or contributors accept any liability whatsoever for injury or damage caused to (or by) climbers, third parties, or property arising from its use. Users must rely on their own judgement and are recommended to obtain suitable insurance against injury to person, property and third party risks.Sloper Inc. (“Sloper”) endorses the participation statement that Rock climbing, hill walking and mountaineering are activities with a danger of personal injury or death. Participants in these activities should be aware of and accept those risks and be responsible for their own activities and involvement.\n\nBy using this topo app you are deemed to have read and accepted these statements.\n";

            lblCopyrightTitle.Text = "Acknowledgements & Copyright";
            lblCopyrightContent.Text = "\nPowered by www.sloperclimbing.com topo apps Platform.\n\nThis application is the intellectual property and copyright of Sloper Inc (“Sloper”).\n\nThe authors assert their database copyright over the content of this guidebook (c).\n";

            lblPrivacyTitle.Text = "Privacy";
            lblPrivacyContent.Text = "\nYour privacy is important to us at Sloper Inc. (“Sloper”)\n\nThe information you provide is intended to enhance your experience while using the Sloper App.\n\nWe will not sell your private information.\n\nSloper only collets personal information about you if you voluntarily provide it and share it with Sloper. You will have an opportunity to opt in and elaborate on demographic information about yourself in your profile. This will include items such as age, weight class, years climbing and climbing abilities. This information may be used to better your experience with Sloper and tailer the information that you are notified about.\n";

            //var lblDisclaimerTitle = new Label
            //{
            //    FontAttributes = FontAttributes.Bold,
            //    TextColor = Color.White,
            //    Text = "Disclaimer"
            //};

            //var lblDisclaimerContent = new Label
            //{
            //    TextColor = Color.White,
            //    Text = "The inclusion of topos or of a crag or routes upon them does not imply a right of access to the crag or the right to climb upon it. Neither the authors or contributors accept any liability whatsoever for injury or damage caused to (or by) climbers, third parties, or property arising from its use. Users must rely on their own judgement and are recommended to obtain suitable insurance against injury to person, property and third party risks.Sloper Inc. (“Sloper”) endorses the participation statement that Rock climbing, hill walking and mountaineering are activities with a danger of personal injury or death. Participants in these activities should be aware of and accept those risks and be responsible for their own activities and involvement.\n\nBy using this topo app you are deemed to have read and accepted these statements."
            //};

            //var lblCopyrightTitle = new Label
            //{
            //    FontAttributes = FontAttributes.Bold,
            //    TextColor = Color.White,
            //    Text = "Acknowledgements & Copyright"
            //};

            //var lblCopyrightContent = new Label
            //{
            //    TextColor = Color.White,
            //    Text = "Powered by www.sloperclimbing.com topo apps Platform.\n\nThis application is the intellectual property and copyright of Sloper Inc (“Sloper”).\n\nThe authors assert their database copyright over the content of this guidebook (c)."
            //};

            //var lblPrivacyTitle = new Label
            //{
            //    FontAttributes = FontAttributes.Bold,
            //    TextColor = Color.White,
            //    Text = "Privacy"
            //};

            //var lblPrivacyContent = new Label
            //{
            //    TextColor = Color.White,
            //    Text = "Your privacy is important to us at Sloper Inc. (“Sloper”)\n\nThe information you provide is intended to enhance your experience while using the Sloper App.\n\nWe will not sell your private information.\n\nSloper only collets personal information about you if you voluntarily provide it and share it with Sloper. You will have an opportunity to opt in and elaborate on demographic information about yourself in your profile. This will include items such as age, weight class, years climbing and climbing abilities. This information may be used to better your experience with Sloper and tailer the information that you are notified about.\n\n"
            //};

            //Content = new ScrollView
            //{
            //    BackgroundColor = Color.Black,
            //    Content = new StackLayout
            //    {
            //        BackgroundColor = Color.Black,
            //        Padding = 20,
            //        Spacing = 20,
            //        VerticalOptions = LayoutOptions.StartAndExpand,
            //        Children = { lblDisclaimerTitle, lblDisclaimerContent, lblCopyrightTitle, lblCopyrightContent, lblPrivacyTitle, lblPrivacyContent }
            //    }
            //};




        }
    }
}
