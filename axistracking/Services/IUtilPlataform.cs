using System;
namespace axistracking.Services
{
	public interface IUtilPlataform
	{
        string GetIdentifier();
        String GetAppName();
		Double GetHeightStatusBar();
		Double GetScreenWidth();
		Double GetScreenHeight();
		Double GetDeviceFontSize();

        void OpenWaze(String paramUrl);
        void OpenGoogleMaps(String paramUrl);
    }
}
