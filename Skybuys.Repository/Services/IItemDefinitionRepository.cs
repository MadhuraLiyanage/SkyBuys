using SkyBuys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyBuys.Repository.Services
{
    public interface IItemDefinitionRepository
    {
        void UpdateData(List<ItemDefinition> itemDefinitions);
    }
}
