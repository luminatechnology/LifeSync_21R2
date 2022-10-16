using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.CS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PX.Objects.IN
{

    #region Protected Access
    [PXProtectedAccess]
    public abstract class INUpdateStdCost_ProtectedExt : PXGraphExtension<INUpdateStdCost>
    {
        [PXProtectedAccess()]
        public abstract IEnumerable initemlist();
    }
    #endregion

    public class INUpdateStdCost_Extension : PXGraphExtension<INUpdateStdCost_ProtectedExt, INUpdateStdCost>
    {
        [IN.Site(typeof(Where<INSite.buildingID, NotEqual<Buildingttr>, Or<INSite.buildingID, IsNull>>), false)]
        [PXMergeAttributes(Method = MergeMethod.Replace)]
        public virtual void _(Events.CacheAttached<INSiteFilter.siteID> e) { }

        [PXOverride]
        public IEnumerable initemlist()
        {
            var excludeBuildingID = SelectFrom<INSiteBuilding>
                                   .Where<INSiteBuilding.buildingCD.IsEqual<P.AsString>>
                                   .View.Select(Base, "MARK").TopFirst?.BuildingID ?? -1;
            var excludeWarehouseID = SelectFrom<INSite>
                                   .Where<INSite.buildingID.IsEqual<P.AsInt>>
                                   .View.Select(Base, excludeBuildingID).RowCast<INSite>().ToList();
            INSiteFilter filter = Base.Filter.Current;
            if (filter == null)
            {
                yield break;
            }
            bool found = false;
            foreach (INUpdateStdCostRecord item in Base.INItemList.Cache.Inserted)
            {
                found = true;
                if (!excludeWarehouseID.Any(x => x.SiteID == item.SiteID))
                    yield return item;
            }
            if (found)
                yield break;


            if (Base.Filter.Current.SiteID == null)
            {
                //Non-Stock:

                PXSelectBase<InventoryItem> inventoryItems = new PXSelectReadonly2<InventoryItem,
                    LeftJoin<InventoryItemCurySettings,
                        On<InventoryItemCurySettings.inventoryID, Equal<InventoryItem.inventoryID>>>,
                    Where<InventoryItem.stkItem, Equal<boolFalse>,
                    And<InventoryItem.isTemplate, Equal<False>,
                    And<InventoryItem.itemStatus, NotEqual<INItemStatus.inactive>,
                    And<InventoryItem.itemStatus, NotEqual<INItemStatus.toDelete>,
                    And<InventoryItemCurySettings.pendingStdCostDate, LessEqual<Current<INSiteFilter.pendingStdCostDate>>>>>>>>(Base);

                foreach (PXResult<InventoryItem, InventoryItemCurySettings> row in inventoryItems.Select())
                {
                    InventoryItem item = row;
                    InventoryItemCurySettings itemCurySettings = row;
                    INUpdateStdCostRecord record = new INUpdateStdCostRecord();
                    record.InventoryID = item.InventoryID;
                    record.RecordID = 1;
                    record.InvtAcctID = item.InvtAcctID;
                    record.InvtSubID = item.InvtSubID;
                    record.PendingStdCost = itemCurySettings?.PendingStdCost ?? 0m;
                    record.PendingStdCostDate = itemCurySettings?.PendingStdCostDate;
                    record.StdCost = itemCurySettings?.StdCost ?? 0m;
                    record.CuryID = itemCurySettings.CuryID;
                    if (!excludeWarehouseID.Any(x => x.SiteID == record.SiteID))
                        yield return Base.INItemList.Insert(record);
                }

                //Stock:

                PXSelectBase<INItemSite> itemSites = new PXSelectJoin<INItemSite,
                    InnerJoin<InventoryItem, On2<INItemSite.FK.InventoryItem,
                        And<Match<InventoryItem, Current<AccessInfo.userName>>>>>,
                    Where<INItemSite.valMethod, Equal<INValMethod.standard>,
                        And<INItemSite.active, Equal<True>,
                        And<INItemSite.siteStatus, Equal<INItemStatus.active>,
                        And<InventoryItem.isTemplate, Equal<False>,
                        And<InventoryItem.itemStatus, NotEqual<INItemStatus.inactive>, And<InventoryItem.itemStatus, NotEqual<INItemStatus.toDelete>,
                        And<Where<INItemSite.pendingStdCostDate, LessEqual<Current<INSiteFilter.pendingStdCostDate>>,
                            Or<INItemSite.pendingStdCostReset, Equal<boolTrue>>>>>>>>>>>(Base);

                foreach (INItemSite item in itemSites.Select())
                {
                    INUpdateStdCostRecord record = new INUpdateStdCostRecord();
                    record.InventoryID = item.InventoryID;
                    record.SiteID = item.SiteID;
                    record.RecordID = item.SiteID;
                    record.InvtAcctID = item.InvtAcctID;
                    record.InvtSubID = item.InvtSubID;
                    record.PendingStdCost = item.PendingStdCost;
                    record.PendingStdCostDate = item.PendingStdCostDate;
                    record.PendingStdCostReset = item.PendingStdCostReset;
                    record.StdCost = item.StdCost;
                    record.StdCostOverride = item.StdCostOverride;
                    record.CuryID = INSite.PK.Find(Base, item.SiteID)?.BaseCuryID;
                    if (!excludeWarehouseID.Any(x => x.SiteID == record.SiteID))
                        yield return Base.INItemList.Insert(record);
                }
            }
            else
            {
                //Stock

                PXSelectBase<INItemSite> s = new PXSelectJoin<INItemSite,
                    InnerJoin<InventoryItem, On2<INItemSite.FK.InventoryItem,
                        And<Match<InventoryItem, Current<AccessInfo.userName>>>>>,
                    Where<INItemSite.valMethod, Equal<INValMethod.standard>,
                        And<INItemSite.active, Equal<True>,
                        And<INItemSite.siteStatus, Equal<INItemStatus.active>,
                        And<INItemSite.siteID, Equal<Current<INSiteFilter.siteID>>,
                        And<InventoryItem.itemStatus, NotEqual<INItemStatus.inactive>,
                        And<InventoryItem.itemStatus, NotEqual<INItemStatus.toDelete>,
                        And<Where<INItemSite.pendingStdCostDate, LessEqual<Current<INSiteFilter.pendingStdCostDate>>,
                            Or<Current<INSiteFilter.revalueInventory>, Equal<boolTrue>,
                            Or<INItemSite.pendingStdCostReset, Equal<boolTrue>>>>>>>>>>>>(Base);

                foreach (INItemSite item in s.Select())
                {
                    INUpdateStdCostRecord record = new INUpdateStdCostRecord();
                    record.InventoryID = item.InventoryID;
                    record.SiteID = item.SiteID;
                    record.RecordID = item.SiteID;
                    record.InvtAcctID = item.InvtAcctID;
                    record.InvtSubID = item.InvtSubID;
                    record.PendingStdCost = item.PendingStdCost;
                    record.PendingStdCostDate = item.PendingStdCostDate;
                    record.PendingStdCostReset = item.PendingStdCostReset;
                    record.StdCost = item.StdCost;
                    record.StdCostOverride = item.StdCostOverride;
                    record.CuryID = INSite.PK.Find(Base, item.SiteID)?.BaseCuryID;
                    if (!excludeWarehouseID.Any(x => x.SiteID == record.SiteID))
                        yield return Base.INItemList.Insert(record);
                }

            }
        }

        public class Buildingttr : PX.Data.BQL.BqlInt.Constant<Buildingttr>
        {
            public Buildingttr() : base(-1) { }

            public override int Value
                => SelectFrom<INSiteBuilding>
                   .Where<INSiteBuilding.buildingCD.IsEqual<P.AsString>>
                   .View.Select(new PXGraph(), "MARK").TopFirst?.BuildingID ?? -1;
        }
    }
}
