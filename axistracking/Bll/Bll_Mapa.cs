using axistracking.Domain;
using axistracking.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace axistracking.Bll
{
    public class Bll_Mapa
    {
        public Map _mapa { get; set; }

        private IMessageService _messageService { get; }
        private INavigationService _navigationService { get; }

        public Bll_Mapa(Map paramMapa)
        {
            _mapa = paramMapa;
        }


		#region CentralizarMapa
		public void CentralizarMapa(
			Double paramLatitude
			, Double paramLontitude
			,  Double paramDistancia = 1
			, Boolean paramAnimate = true
		)
		{
			//Device.BeginInvokeOnMainThread(() =>
			//{
			try
			{
				_mapa.MoveToRegion(
					MapSpan.FromCenterAndRadius(
						new Position(
							paramLatitude
							, paramLontitude
						)
						, Distance.FromKilometers(paramDistancia)
					)
					, paramAnimate
				);

			} catch (Exception) { }
			//});
		}

		public void CentralizarMapa(
			Position paramPosition
			, Double paramDistancia = 1
			, Boolean paramAnimate = true
		)
		{
			//Device.BeginInvokeOnMainThread(() =>
			//{
			try
			{

				_mapa.MoveToRegion(
					MapSpan.FromCenterAndRadius(
						paramPosition
						, Distance.FromKilometers(paramDistancia)
					)
					, paramAnimate
				);
			} catch (Exception) { }
			//});
		}

		public void CentralizarMapa(
			Bounds paramBounds
			, Boolean paramAnimate = true
		)
		{
			//Device.BeginInvokeOnMainThread(() =>
			//{
			try
			{
				_mapa.MoveToRegion(MapSpan.FromBounds(paramBounds), paramAnimate);
			} catch (Exception) { }
			//});
		}
		#endregion


        public void LimpaMapa()
        {
            try
            {
                if (_mapa.Pins != null)
                    _mapa.Pins.Clear();

                if (_mapa.Circles != null)
                    _mapa.Circles.Clear();

                if (_mapa.Polylines != null)
                    _mapa.Polylines.Clear();

                if (_mapa.Polygons != null)
                    _mapa.Polygons.Clear();
            }
            catch (Exception)
            {
            }
        }

        public void RemoverCircle()
        {
            try
            {

                if (_mapa.Circles != null)
                    _mapa.Circles.Clear();

            }
            catch (Exception)
            {
            }
        }

        public List<Position> MontaMapaListaHistorico(List<PosicaoHistorico> Historico)
        {

            List<Position> lstPosition = new List<Position>();
            try
            {
                Polyline polyline = new Polyline()
                {
                    StrokeColor = Color.Red,
                    StrokeWidth = 2f
                };
                String iconePrincipal = "pin_historico_reduzido.png";
                Pin pinPosicao;

                PosicaoHistorico item;
                for (Int32 i = 0; i < Historico.Count; i++)
                {

                    item = Historico[i];

                    if (item.Latitude != null)
                    {
                        Position position = new Position(item.Latitude.Value, item.Longitude.Value);

                        pinPosicao = new Pin
                        {
                            Type = PinType.Place,
                            Position = position,
                            Label = String.Format("{0:dd/MM/yyyy HH:mm:ss}", item.DataEvento.Value.ToLocalTime()),
                            //Icon = BitmapDescriptorFactory.FromView(new BindingPinPosNormal("", iconePrincipal))
                            Icon = BitmapDescriptorFactory.FromBundle(iconePrincipal)
                        };

						_mapa.PinClicked += (sender, e) =>
                        {
							pinPosicao.Label = String.Format("{0:dd/MM/yyyy HH:mm:ss}", item.DataEvento.Value.ToLocalTime());
                        };


                        _mapa.Pins.Add(pinPosicao);
                        lstPosition.Add(position);
                        if (Historico.Count > 1)
                            polyline.Positions.Add(position);
                    }
                }

                if (polyline.Positions.Count > 2)
                    _mapa.Polylines.Add(polyline);

            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }
            return lstPosition;
        }

        public Position MontaMapaPosicao(PosicaoHistorico paramPosicao)
        {
            //acaoNome = "Lista pin histórico";
            Position position = new Position();
            try
            {

                position = new Position(
                    paramPosicao.Latitude.Value
                    , paramPosicao.Longitude.Value
                );

                BitmapDescriptor bmp = paramPosicao.MontaIconPin();


                if (paramPosicao.OrdemRastreador == null)
                    paramPosicao.OrdemRastreador = 0;


                Pin pinPosicao = new Pin
                {
                    Type = PinType.Place,
                    Position = position,
                    Icon = bmp,
                    Label = paramPosicao.IdentificacaoUnidadeRastreada
                };

				_mapa.PinClicked += (sender, e) =>
                {
					pinPosicao.Label = paramPosicao.IdentificacaoUnidadeRastreada;
                };

                _mapa.Pins.Add(pinPosicao);

            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }
            return position;
        }

        public double DistanceToInMiles(Position baseCoordinates, Position targetCoordinates)
        {
            var baseRad = Math.PI * baseCoordinates.Latitude / 180;
            var targetRad = Math.PI * targetCoordinates.Latitude / 180;
            var theta = baseCoordinates.Longitude - targetCoordinates.Longitude;
            var thetaRad = Math.PI * theta / 180;

            double dist =
                Math.Sin(baseRad) * Math.Sin(targetRad) + Math.Cos(baseRad) *
                Math.Cos(targetRad) * Math.Cos(thetaRad);
            dist = Math.Acos(dist);

            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            return dist;
        }

        public Circle MontaMapaAncora(PosicaoHistorico posicaoHistorico
                                               , Int32 paramTolerancia
                                               //, String paramLabel
                                               //, String paramIcone
                                               )
        {

            Circle circle = null;
            try
            {
                Position posi = new Position(posicaoHistorico.Latitude.Value, posicaoHistorico.Longitude.Value);
                circle = MontaMapaAncora(posi, paramTolerancia
                    //,paramLabel, paramIcone
                    );
            }
            catch
            {
                this._messageService.ShowAlertAsync("Exception");

            }
            return circle;
        }

        public Circle MontaMapaAncora(Position paramPosition, Int32? paramRaio
            //, String paramLabel, String paramIcone
            )
        {

            Circle circle = null;
            try
            {
                var position2 = new Position(paramPosition.Latitude, paramPosition.Longitude);
                //Pin pinPosicao = new Pin
                //{
                //    Type = PinType.Place,
                //    Position = paramPosition,
                //    Label = paramLabel,
                //    Icon = BitmapDescriptorFactory.FromBundle(paramIcone),
                //    IsDraggable = false

                //};

                circle = new Circle();
                circle.Center = new Position(paramPosition.Latitude, paramPosition.Longitude);
                circle.Radius = Distance.FromMeters(Convert.ToDouble(paramRaio));

                circle.StrokeColor = Color.FromRgba(0, 145, 179, 204); // #0091B3 80%
                circle.StrokeWidth = 1f;
                circle.FillColor = Color.FromRgba(0, 146, 178, 89); // #0091B3 35%
                circle.Tag = "CIRCLE"; // Can set any object

				_mapa.Circles.Add(circle);
            }
            catch(Exception)
            {
                this._messageService.ShowAlertAsync("Exception");

            }
            return circle;

        }

    }
}
