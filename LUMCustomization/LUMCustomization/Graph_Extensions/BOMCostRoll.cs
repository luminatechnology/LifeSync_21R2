using PX.Data;
using PX.Objects.IN;
using System.Collections.Generic;

namespace PX.Objects.AM
{
    #region Protected Access
    [PXProtectedAccess]
    public abstract class BOMCostRoll_ProtExt : PXGraphExtension<PX.Objects.AM.BOMCostRoll>
    {
        [PXProtectedAccess(typeof(BOMCostRoll))]
        public abstract decimal? GetUnitCost(InventoryItem inventoryItem, int? siteid); //{ get; }
}
    #endregion

    public class BOMCostRoll_Extension : PXGraphExtension<BOMCostRoll_ProtExt, PX.Objects.AM.BOMCostRoll>
    {
        #region Cache Attached
        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXDefault(RollupSettings.SelectOptSM.Multi)]
        protected virtual void _(Events.CacheAttached<RollupSettings.snglMlti> e) { }

        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXDefault(true)]
        protected virtual void _(Events.CacheAttached<RollupSettings.incMatScrp> e) { }

        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXDefault(true)]
        protected virtual void _(Events.CacheAttached<RollupSettings.incFixed> e) { }
        #endregion

        #region Override Methods
        /// <summary>
        /// Remove "round up" condition according to [JIRA] (LIFESYNC-51).
        /// </summary>
        public delegate BOMCostRoll.OperationCosts GetMaterialCostDelegate(AMBomCost currentAMBomCost, IEnumerable<PXResult<AMBomMatl, InventoryItem>> material, bool isMultLevel, out List<string> materialMessage);
        [PXOverride]
        public BOMCostRoll.OperationCosts GetMaterialCost(AMBomCost currentAMBomCost, IEnumerable<PXResult<AMBomMatl, InventoryItem>> material,
                                                          bool isMultLevel, out List<string> materialMessage, GetMaterialCostDelegate baseMathod)
        {
            baseMathod(currentAMBomCost, material, isMultLevel, out materialMessage);

            var operationMaterialCosts = new BOMCostRoll.OperationCosts();

            foreach (var result in material)
            {
                var matlRec = (AMBomMatl)result;

                var totalQtyRequired = matlRec.QtyReq.GetValueOrDefault() * (1 + (Base.Settings.Current.IncMatScrp.GetValueOrDefault() ? matlRec.ScrapFactor.GetValueOrDefault() : 0m)) *
                                       (matlRec.BatchSize.GetValueOrDefault() == 0m ? 1m : currentAMBomCost.LotSize.GetValueOrDefault() / matlRec.BatchSize.GetValueOrDefault());

                decimal? unitcost = Base1.GetUnitCost((InventoryItem)result, matlRec.SiteID ?? currentAMBomCost.SiteID);

                var matlCost = totalQtyRequired * unitcost.GetValueOrDefault();

                operationMaterialCosts.Add(matlRec.OperationID, matlCost, true);
            }

            return operationMaterialCosts;
        }
        #endregion
    }
}
