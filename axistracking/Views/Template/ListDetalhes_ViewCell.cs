using axistracking.Domain;
using axistracking.Domain.Dto;
using axistracking.Enum;
using axistracking.ViewModels;
using axistracking.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using axistracking.Resx;
using axistracking.Services;

namespace axistracking.Views.Template
{
    public class ListDetalhes_ViewCell : ViewCellBase
    {
        private readonly IMessageService _messageService;
        private readonly INavigationService _navigationService;

        private IUtilPlataform _util;

        private EnumPage _enumPage { get; set; }
        private PainelDto _painelDto { get; set; }

        private ViewModelDetalhes _viewModelUnidades;
        private ViewModelHistorico _viewModelHistorico;
        private Boolean ShowStatusRastreadorUnidadeRastreada = false;

        public ListDetalhes_ViewCell(
            PainelDto Painel
            , Object paramContext
            , EnumPage paramPage
        )
            : base(Color.White)
        {

            _enumPage = paramPage;

            if (_enumPage == EnumPage.DetalhesAlerta)
            {
                _viewModelUnidades = (ViewModelDetalhes)paramContext;
            }
            else if (_enumPage == EnumPage.DetalhesUnidade)
            {
                _viewModelUnidades = (ViewModelDetalhes)paramContext;
                ShowStatusRastreadorUnidadeRastreada = true;
            }
            else
            {
                _viewModelHistorico = (ViewModelHistorico)paramContext;
            }
            _painelDto = Painel;

            this._messageService =
                DependencyService.Get<IMessageService>();

            this._navigationService =
                DependencyService.Get<INavigationService>();


            this._util = DependencyService.Get<IUtilPlataform>();

        }

        protected override void OnBindingContextChanged()
        {
            try
            {
                base.OnBindingContextChanged();

                PosicaoHistorico unidadeRastreada = (PosicaoHistorico)BindingContext;

                if (unidadeRastreada != null)
                {
                    BuildCellDetalhes build = new BuildCellDetalhes();

                    //Stack Principal
                    Grid boxPrincipal = build.BuildCell(
                        unidadeRastreada
                        , null
                        , ShowStatusRastreadorUnidadeRastreada
                    );

                    boxPrincipal.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = new Command(BoxPrincipal_Tap),
                        CommandParameter = unidadeRastreada,
                        NumberOfTapsRequired = 1
                    });

                    View = boxPrincipal;

                }
            }
            catch
            {

            }
        }

        private async void BoxPrincipal_Tap(object obj)
        {
            String answer;

            PosicaoHistorico _posicaoDetalhes = (PosicaoHistorico)obj;
            ////Int64? posiId = posi.IdPosicao;

            if (_enumPage == EnumPage.DetalhesAlerta || _enumPage == EnumPage.DetalhesUnidade)
            {
                List<String> respostas = new List<String>
                {
                    AppResources.historico
                    //,AppResources.detalheposicao
				};


                if (_enumPage == EnumPage.DetalhesAlerta)
                    respostas.Add(AppResources.UltimaPosicao);
                else
                    respostas.Add(AppResources.detalheposicao);



                respostas.Add(AppResources.Roteirizacao);


                answer = await
                    this._messageService.ShowMessageAsync(
                        _posicaoDetalhes.IdentificacaoUnidadeRastreada
                        , AppResources.cancelar
                        , null
                        , respostas.ToArray()
                    );


                if (answer == AppResources.historico)
                {
                    this._navigationService.NavigateToHistorico(
                        _painelDto
                        , obj
                        , _enumPage
                    );
                }


                if (answer == AppResources.Roteirizacao)
                {
                    String answerStreet;

                    List<String> respostasStreet = new List<String>
                    {
                        AppResources.Waze
                        ,AppResources.GoogleMaps
                    };

                    answerStreet = await
                    this._messageService.ShowMessageAsync(
                        _posicaoDetalhes.IdentificacaoUnidadeRastreada
                        //+ "\n\n" + AppResources.Posicao + "\n" + String.Format("{0:dd/MM/yyyy HH:mm:ss}", _posicaoDetalhes.DataEvento)
                        , AppResources.cancelar
                        , null
                        , respostasStreet.ToArray()
                    );


                    if (answerStreet == AppResources.Waze)
                    {
                        OpenWaze(_posicaoDetalhes);
                    }
                    else
                    {
                        OpenGoogleMaps(_posicaoDetalhes);
                    }

                }


                if (answer == AppResources.UltimaPosicao || answer == AppResources.detalheposicao)
                {
                    if (answer == AppResources.UltimaPosicao)
                    {
                        _posicaoDetalhes.ExibeUltimaPosicao = true;
                    }
                    else
                    {
                        _posicaoDetalhes.ExibeUltimaPosicao = (_enumPage == EnumPage.DetalhesUnidade);
                    }

                    //_enumPage = EnumPage.DetalhesUnidade;

                    this._navigationService.NavigateToPosicao(
                        _painelDto
                        , _posicaoDetalhes
                        , _enumPage
                    );
                }
            }
            else if (_enumPage == EnumPage.Historico)
            {
                this._navigationService.NavigateToPosicao(
                    _painelDto
                    , _posicaoDetalhes
                    , _enumPage
                );
            }

        }

        private void OpenWaze(PosicaoHistorico unidadeRastreada)
        {
            String paramEndereco = unidadeRastreada.Endereco;
            String paramLatitude = unidadeRastreada.Latitude.ToString().Replace(",", ".");
            String paramLongitude = unidadeRastreada.Longitude.ToString().Replace(",", ".");

            String paramURL = "https://waze.com/ul?q=" + paramEndereco + "&ll="
                + paramLatitude + "," + paramLongitude + "&navigate=yes";

            //String paramURL = "https://waze.com/ul?ll=" + paramLatitude + "," + paramLongitude + "&navigate=yes";

            _util.OpenWaze(paramURL.Replace(" ", "%20"));

        }

        private void OpenGoogleMaps(PosicaoHistorico unidadeRastreada)
        {
            String paramEndereco = unidadeRastreada.Endereco;
            String paramLatitude = unidadeRastreada.Latitude.ToString().Replace(",", ".");
            String paramLongitude = unidadeRastreada.Longitude.ToString().Replace(",", ".");

            //String paramURL = "https://waze.com/ul?q=" + paramEndereco + "&ll="
            //    + paramLatitude + "," + paramLongitude + "&navigate=yes";

            String paramURL = "google.navigation:q=" + paramLatitude + "," + paramLongitude;

            if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
                paramURL = "http://maps.apple.com/?q=" + paramLatitude + "," + paramLongitude;


            _util.OpenGoogleMaps(paramURL.Replace(" ", "%20"));

        }
    }
}

