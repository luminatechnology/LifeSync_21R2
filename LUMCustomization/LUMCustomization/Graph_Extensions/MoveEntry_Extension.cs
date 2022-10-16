using LUMCustomizations.Library;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;
using System.Collections;
using System.Collections.Generic;

namespace PX.Objects.AM
{
    public class MoveEntry_Extension : PXGraphExtension<MoveEntry>
    {
        public override void Initialize()
        {
            var _lumLibrary = new LumLibrary();
            if (_lumLibrary.isCNorHK())
            {
                ReportAction.AddMenuAction(ProductionMoveAction);
                ReportAction.MenuAutoOpen = true;
            }
        }

        #region  Actions

        #region Report Action
        public PXAction<AMBatch> ReportAction;
        [PXButton(MenuAutoOpen = true, CommitChanges = true)]
        [PXUIField(DisplayName = "Report")]
        protected void reportAction() { }
        #endregion

        #region Material Issues Action
        public PXAction<AMBatch> ProductionMoveAction;
        [PXButton(DisplayOnMainToolbar = false)]
        [PXUIField(DisplayName = "Production Move", MapEnableRights = PXCacheRights.Select)]
        protected void productionMoveAction()
        {
            var curAMBatchCache = (AMBatch)Base.batch.Cache.Current;
            // create the parameter for report
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters["BatNbr"] = curAMBatchCache.BatNbr;
            parameters["AttributeID"] = "PRODLINE";

            // using Report Required Exception to call the report
            throw new PXReportRequiredException(parameters, "LM603020", "LM603020");
        }
        #endregion

        #endregion

        #region Override Action
        public PXAction<AMBatch> release;
        [PXUIField(DisplayName = Messages.Release, MapEnableRights = PXCacheRights.Update, MapViewRights = PXCacheRights.Update)]
        [PXProcessButton]
        public virtual IEnumerable Release(PXAdapter adapter)
        {
            var validResult = true;
            foreach (var row in Base.transactions.Select().RowCast<AMMTran>())
            {
                if (!ValidStandardCost(row))
                {
                    Base.transactions.Cache.RaiseExceptionHandling<AMMTran.prodOrdID>(row, row.ProdOrdID,
                        new PXSetPropertyException<AMMTran.prodOrdID>($"{GetInventoryItemInfo(row.InventoryID)?.InventoryCD} 沒有維護標準成本，請通知財務", PXErrorLevel.Error));
                    validResult = false;
                }
            }
            if (!validResult)
                throw new PXException($"沒有維護標準成本，請通知財務");
            return Base.Release(adapter);
        }
        #endregion

        #region controll customize button based on country ID
        protected void _(Events.RowSelected<AMBatch> e)
        {
            var _lumLibrary = new LumLibrary();
            if (!_lumLibrary.isCNorHK())
            {
                ReportAction.SetVisible(false);
                ProductionMoveAction.SetVisible(false);
            }
        }
        #endregion

        #region Events

        public virtual void _(Events.FieldUpdated<AMMTran.inventoryID> e, PXFieldUpdated baseHandler)
        {
            baseHandler?.Invoke(e.Cache, e.Args);
            var row = e.Row as AMMTran;
            if (row != null && !ValidStandardCost(row))
                Base.transactions.Cache.RaiseExceptionHandling<AMMTran.prodOrdID>(row, row.ProdOrdID,
                   new PXSetPropertyException<AMMTran.prodOrdID>($"{GetInventoryItemInfo(row.InventoryID)?.InventoryCD} 沒有維護標準成本，請通知財務", PXErrorLevel.Error));
        }

        public virtual void _(Events.RowPersisting<AMMTran> e, PXRowPersisting baseHandler)
        {
            baseHandler?.Invoke(e.Cache, e.Args);
            var row = e.Row;
            if (row != null && !ValidStandardCost(row))
                Base.transactions.Cache.RaiseExceptionHandling<AMMTran.prodOrdID>(row, row.ProdOrdID,
                   new PXSetPropertyException<AMMTran.prodOrdID>($"{GetInventoryItemInfo(row.InventoryID)?.InventoryCD} 沒有維護標準成本，請通知財務", PXErrorLevel.Error));
        }
        #endregion

        #region Method

        public bool ValidStandardCost(AMMTran row)
        {
            if (row == null)
                return true;
            var setup = SelectFrom<INSetup>.View.Select(Base).TopFirst;
            if ((setup.GetExtension<INSetupExt>()?.UsrValidStandardCostInMove ?? false))
            {
                var inventoryInfo = InventoryItem.PK.Find(Base, row?.InventoryID);
                var itemCurySettingInfo = SelectFrom<InventoryItemCurySettings>.Where<InventoryItemCurySettings.inventoryID.IsEqual<P.AsInt>>.View.Select(Base, row?.InventoryID).TopFirst;
                if ((itemCurySettingInfo?.StdCost ?? 0) == 0)
                    return false;
            }
            return true;
        }

        public InventoryItem GetInventoryItemInfo(int? inventoryID)
            => InventoryItem.PK.Find(Base, inventoryID);

        #endregion
    }
}