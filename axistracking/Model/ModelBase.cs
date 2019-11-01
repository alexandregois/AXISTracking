using axistracking.Services;
using Xamarin.Forms;

namespace axistracking.Model
{
	public class ModelBase
    {
        public IDataStore DataStore => DependencyService.Get<IDataStore>();
        public App _app => (Application.Current as App);

    }
}
