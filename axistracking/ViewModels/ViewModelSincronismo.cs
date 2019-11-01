using System;
using System.Threading;
using System.Threading.Tasks;
using axistracking.Domain.Dto;
using axistracking.Model;
using axistracking.ViewModels.Base;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace axistracking.ViewModels
{
    public class ViewModelSincronismo : ViewModelBase
    {
        public ViewModelSincronismo()
        {
            SincronismoPerfil();
        }

        public override void DefaultTemplateBuild()
        {
            //throw new NotImplementedException();
        }

        public override void OnAppearing()
        {
            //throw new NotImplementedException();
        }

        public override void OnDisappearing()
        {
            //throw new NotImplementedException();
        }

        public override void OnLayoutChanged()
        {
            //throw new NotImplementedException();
        }

        public async void SincronismoPerfil()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            ModelSincronismo modelSincronismo = new ModelSincronismo();

            await Task.Run(async () =>
             {

                 try
                 {
                     ServiceResult<String> resultObjeto = await modelSincronismo.RetornaObjetoKeepAlive(2, tokenSource.Token, _app.Token.IdAplicativo);

                     if (resultObjeto != null && resultObjeto.Data != null && resultObjeto.Data != "null")
                     {

                         String message = resultObjeto.Data.ToString().Replace("\"", "");

                         Application.Current.Properties["SolicitacaoRastreamentoEmAndamento"] = null;
                         Application.Current.Properties["SolicitacaoRastreamentoEmAndamento"] = false;
                     }

                 }
                 catch (Exception ex)
                 {
                     Crashes.TrackError(ex);
                 }

             });

        }
    }
}
