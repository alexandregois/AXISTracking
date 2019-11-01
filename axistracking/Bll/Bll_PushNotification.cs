using Com.OneSignal;
using Com.OneSignal.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using axistracking.Model;
using axistracking.Domain.Realm;
using Microsoft.AppCenter.Push;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace axistracking.Bll
{
    public class Bll_PushNotification
    {
        private App _app => (Application.Current as App);

        //private Bll_Conexao bllConexao = new Bll_Conexao(null);

        private ModelLogin bllConexao = new ModelLogin();

        private CancellationToken paramTokenCancel { get; set; }

        public async void DeletePushKey(CancellationToken paramToken)
        {
            try
            {

                await Push.SetEnabledAsync(false);

                OneSignal.Current.SetSubscription(false);
                await bllConexao.Atualiza_PushKey(
                    ""
                    , paramToken
                );

            }
            catch (Exception) { }
        }

        private void IdsAvailable(string userID, string pushToken)
        {

            OneSignal.Current.SetSubscription(true);

            //bllConexao.Atualiza_PushKey(userID, paramTokenCancel);

        }

        public async void RegistraPushKey(CancellationToken paramToken)
        {

            paramTokenCancel = paramToken;

            try
            {

                String strChave = string.Empty;


                if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
                {

                    await Push.SetEnabledAsync(true);

                    System.Guid? installId = await AppCenter.GetInstallIdAsync();

                    await bllConexao.Atualiza_PushKey(installId.ToString(), paramToken);


                    String mobileKey = String.Empty;

                    mobileKey = "8b9ba4c2-f4ec-417a-af3b-2f162072ce30";


                    if (_app.nameProject == "khronos")
                    {
                        mobileKey = "9080a1f6-9273-4d9d-96cf-f059a1e52eb2";

                    }

                    if (_app.nameProject == "maxima")
                    {
                        mobileKey = "c257e245-e0ab-40d6-9d2a-c6d91fff1883";
                    }

                    if (_app.nameProject == "gns")
                    {
                        mobileKey = "6f04c133-f0f0-4b84-ab46-4990ea3401f7";
                    }


                    if (_app.nameProject == "alltech")
                    {
                        mobileKey = "89a546f6-a508-4b4c-9385-f44bbe855cd7";
                    }


                    AppCenter.Start(
                        mobileKey
                        , typeof(Analytics)
                        , typeof(Crashes)
                        , typeof(Push)
                    );
                    


                }
                else
                {


                    strChave = "e2de4625-175f-4dcb-8b5b-d00d63b75eb6";

                    if (_app.isPersonalizado)
                    {
                        if (_app.nameProject == "khronos")
                            strChave = "c93df7ac-74ff-4eb8-b69d-3a35d8dbd74d";

                        if (_app.nameProject == "maxima")
                            strChave = "6967aeda-2235-4059-bfe2-e70281ebc90b";

                        if (_app.nameProject == "2minutos")
                            strChave = "58933542-b4be-4059-b8a2-a8c98d51c01a";

                    }



                    OneSignal.Current.StartInit(strChave)
                             .Settings(new Dictionary<string, bool>() {
                { IOSSettings.kOSSettingsKeyAutoPrompt, false },
                { IOSSettings.kOSSettingsKeyInAppLaunchURL, true } })
                    .InFocusDisplaying(OSInFocusDisplayOption.Notification)
                    .HandleNotificationOpened((result) =>
                    {
                        //Debug.WriteLine("HandleNotificationOpened: {0}", result.notification.payload.body);
                    })
                    .HandleNotificationReceived((notification) =>
                    {
                        //Debug.WriteLine("HandleNotificationReceived: {0}", notification.payload.body);
                    })
                    .EndInit();


                    OneSignal.Current.IdsAvailable(IdsAvailable);


                    OneSignal.Current.RegisterForPushNotifications();


                    OneSignal.Current.IdsAvailable((playerID, pushToken) =>
                    {
                        OneSignal.Current.SetSubscription(true);

                        bllConexao.Atualiza_PushKey(
                            playerID
                            , paramToken
                        );
                    });


                }


            }
            catch
            {

            }
        }

    }

}
