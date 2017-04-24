using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading;
using System.Threading.Tasks;
using SloperMobile.ViewModel;
using Xamarin.Forms;
using SloperMobile.MessagingTask;

namespace SloperMobile.Droid.Services
{
    [Service]
    public class CheckUpdatesTaskService : Service
    {
        CancellationTokenSource _cts;
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            _cts = new CancellationTokenSource();

            Task.Run(() => {
                try
                {
                    //INVOKE THE SHARED CODE
                    CheckForUpdatesViewModel.CurrentInstance().OnPageAppearing().Wait();
                }
                catch (OperationCanceledException)
                {
                }
                catch (System.AggregateException)
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

            }, _cts.Token);

            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            if (_cts != null)
            {
                _cts.Token.ThrowIfCancellationRequested();

                _cts.Cancel();
            }
            base.OnDestroy();
        }
    }
}