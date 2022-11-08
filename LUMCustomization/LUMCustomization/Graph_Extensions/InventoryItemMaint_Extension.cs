using PX.Data;
using PX.Data.BQL.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Objects.AM;

namespace PX.Objects.IN
{
    public class InventoryItemMaint_Extension : PXGraphExtension<InventoryItemMaint>
    {
        public SelectFrom<AMBomCostHistory>
              .Where<AMBomCostHistory.inventoryID.IsEqual<InventoryItem.inventoryID.FromCurrent>
                .And<AMBomCostHistory.startDate.IsEqual<InventoryItem.stdCostDate.FromCurrent>>
                .And<AMBomCostHistory.isDefaultBom.IsEqual<True>>>
              .View BomCostHistory;
    }
}
