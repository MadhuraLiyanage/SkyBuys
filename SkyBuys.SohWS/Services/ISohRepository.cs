using SkyBuys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyBuys.SohWS.Services
{
    public interface ISohRepository
    {
        void UpdateData(List<Soh> sohs);
        void DeleteSohData();
    }
}
