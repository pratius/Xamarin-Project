using SloperMobile.MessagingTask;
using SloperMobile.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;

namespace SloperMobile.iOS.Services
{
    public class iOSCheckUpdatesTaskService
    {
        nint _taskId;
        CancellationTokenSource _cts;

        public async Task Start()
        {
            _cts = new CancellationTokenSource();

            _taskId = UIApplication.SharedApplication.BeginBackgroundTask("CheckForUpdateTask", OnExpiration);

            try
            {
                //INVOKE THE SHARED CODE
                await CheckForUpdatesViewModel.CurrentInstance().OnPageAppearing();
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                if (_cts.IsCancellationRequested)
                {
                    var message = new UpdateMessage();
                    Device.BeginInvokeOnMainThread(
                        () => MessagingCenter.Send(message, "UpdateMessage")
                    );
                }
            }

            UIApplication.SharedApplication.EndBackgroundTask(_taskId);
        }

        public void Stop()
        {
            _cts.Cancel();
        }

        void OnExpiration()
        {
            _cts.Cancel();
        }
    }
}
