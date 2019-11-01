using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace axistracking.Services
{
    public interface IStreetViewService
    {
        void openStreetView(double latitude, double longitude);
    }
}
