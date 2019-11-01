using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using axistracking.Droid.CustomClass;

namespace axistracking.Droid.Services
{
    [BroadcastReceiver(
        Enabled = true
        , Exported = true
        , DirectBootAware = true
        , Name = "br.com.systemsat.BootReceiver"
    )]
    [IntentFilter(new[] { Intent.ActionBootCompleted, Intent.ActionLockedBootCompleted, "android.intent.action.QUICKBOOT_POWERON" }, Priority = (int)IntentFilterPriority.HighPriority)]
    public class BootReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            UtilPlataform util = new UtilPlataform();

            util.OpenActivity(true);

            //Intent ii = new Intent(context, typeof(PosicaoService));
            //ii.AddFlags(ActivityFlags.NewTask);
            //context.StartActivity(ii);


            //Intent i = new Intent(context, typeof(MainActivity)); //Abre o App no reboot do celular
            //i.AddFlags(ActivityFlags.NewTask);
            //context.StartActivity(i);
        }
    }
}