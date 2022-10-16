using LumCustomizations.DAC;
using LUMCustomizations.Library;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.AM;
using PX.Objects.IN;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PX.Objects.AM
{
    public class MaterialEntry_Extension : PXGraphExtension<MaterialEntry>
    {
        public override void Initialize()
        {
            var _lumLibrary = new LumLibrary();
            if (_lumLibrary.isCNorHK())
            {
                ReportAction.AddMenuAction(MaterialIssuesAction);
                ReportAction.AddMenuAction(MaterialReturnAction);
                ReportAction.MenuAutoOpen = true;
            }
        }

        #region Override transactions view
        [PXImport(typeof(AMBatch))]
        public PXSelect<AMMTran, Where<AMMTran.docType, Equal<Current<AMBatch.docType>>, And<AMMTran.batNbr, Equal<Current<AMBatch.batNbr>>>>, OrderBy<Asc<AMMTran.inventoryID>>> transactions;
        #endregion

        #region Override Attribute
        [PXDBQuantity(2, typeof(AMMTran.uOM), typeof(AMMTran.baseQty), HandleEmptyKey = true)]
        [PXDefault(TypeCode.Decimal, "0")]
        [PXUIField(DisplayName = "Quantity")]
        [PXFormula(null, typeof(SumCalc<AMBatch.totalQty>))]
        public virtual void _(Events.CacheAttached<AMMTran.qty> e) { }

        #endregion

        #region  Actions

        #region Report Action
        public PXAction<AMBatch> ReportAction;
        [PXButton(MenuAutoOpen = true, CommitChanges = true)]
        [PXUIField(DisplayName = "Report")]
        protected void reportAction() { }
        #endregion

        #region Material Issues Action
        public PXAction<AMBatch> MaterialIssuesAction;
        [PXButton(DisplayOnMainToolbar = false)]
        [PXUIField(DisplayName = "Material Issues", MapEnableRights = PXCacheRights.Select)]
        protected void materialIssuesAction()
        {
            var curAMBatchCache = (AMBatch)Base.batch.Cache.Current;
            var _printCount = curAMBatchCache.GetExtension<AMBatchExt>().UsrPrintCount ?? 0;

            //Calculate Print Count
            PXUpdate<Set<AMBatchExt.usrPrintCount, Required<AMBatchExt.usrPrintCount>>,
                         AMBatch,
                         Where<AMBatch.batNbr, Equal<Required<AMBatch.batNbr>>,
                           And<AMBatch.docType, Equal<Required<AMBatch.docType>>>
                     >>.Update(Base, ++_printCount, curAMBatchCache.BatNbr, curAMBatchCache.DocType);

            // create the parameter for report
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters["BatNbr"] = curAMBatchCache.BatNbr;
            parameters["AttributeID"] = "PRODLINE";

            // using Report Required Exception to call the report
            throw new PXReportRequiredException(parameters, "LM206100", "LM206100");
        }
        #endregion

        #region Material Return Action
        public PXAction<AMBatch> MaterialReturnAction;
        [PXButton(DisplayOnMainToolbar = false)]
        [PXUIField(DisplayName = "Material Return", MapEnableRights = PXCacheRights.Select)]
        protected void materialReturnAction()
        {
            var curAMBatchCache = (AMBatch)Base.batch.Cache.Current;
            // create the parameter for report
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters["BatNbr"] = curAMBatchCache.BatNbr;
            parameters["AttributeID"] = "PRODLINE";

            // using Report Required Exception to call the report
            throw new PXReportRequiredException(parameters, "LM206105", "LM206105");
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
                    Base.transactions.Cache.RaiseExceptionHandling<AMMTran.unitCost>(row, row.UnitCost,
                        new PXSetPropertyException<AMMTran.unitCost>($"{GetInventoryItemInfo(row.InventoryID)?.InventoryCD} 沒有維護標準成本，請通知採購。採購維護標準成本之後請刪除此領料單並重新建立", PXErrorLevel.Error));
                    validResult = false;
                }
            }
            if (!validResult)
                throw new PXException($"沒有維護標準成本，請通知採購。採購維護標準成本之後請刪除此領料單並重新建立");
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
                MaterialIssuesAction.SetVisible(false);
                MaterialReturnAction.SetVisible(false);
            }
        }
        #endregion

        #region Events
        public virtual void _(Events.FieldVerifying<AMMTran.qty> e, PXFieldVerifying baseMethod)
        {
            var row = e.Row as AMMTran;
            var inputValue = e.NewValue;
            if (!(row.GetExtension<AMMTranExt>().UsrOverIssue ?? false))
                baseMethod?.Invoke(e.Cache, e.Args);
            // valid over Issue
            else
            {
                var inventoryItem = SelectFrom<InventoryItem>.View.Select(Base).RowCast<InventoryItem>().ToList();
                var amProdMatl = SelectFrom<AMProdMatl>
                     .Where<AMProdMatl.prodOrdID.IsEqual<P.AsString>
                        .And<AMProdMatl.inventoryID.IsEqual<P.AsInt>>>.View.Select(Base, row.ProdOrdID, row.InventoryID)
                     .RowCast<AMProdMatl>().FirstOrDefault();
                var maxOverIssue = SelectFrom<LifeSyncPreference>.View.Select(Base).RowCast<LifeSyncPreference>().FirstOrDefault().MaxOverIssue;
                var overIssueQty = Math.Round((amProdMatl?.TotalQtyRequired ?? 0) * (1 + (maxOverIssue ?? 0) / 100), 4) - amProdMatl?.QtyActual;
                if ((decimal?)e.NewValue > overIssueQty)
                {
                    e.NewValue = overIssueQty;
                    e.Cache.SetValue<AMMTran.qty>(row, overIssueQty);
                    throw new PXSetPropertyException<AMMTran.qty>($"Material Quantity {inputValue} to be issued is greater then maximum allowed over issue {overIssueQty} ({amProdMatl.ProdOrdID}-{inventoryItem.FirstOrDefault(x => x.InventoryID == amProdMatl.InventoryID).InventoryCD})");
                }
            }
        }

        public virtual void _(Events.RowPersisting<AMMTran> e, PXRowPersisting baseMethod)
        {
            baseMethod?.Invoke(e.Cache, e.Args);
            var row = e.Row;
            var amTran = Base.transactions.Select().RowCast<AMMTran>().ToList();
            var inventoryItem = SelectFrom<InventoryItem>.View.Select(Base).RowCast<InventoryItem>().ToList();
            var duplicateData = amTran.GroupBy(x => new { x.ProdOrdID, x.InventoryID })
                                      .Where(g => g.Count() > 1)
                                      .Select(x => new
                                      {
                                          x.Key.ProdOrdID,
                                          x.Key.InventoryID,
                                          sumQty = x.Sum(y => y.Qty)
                                      });

            var duplicateDataWithWH = amTran.GroupBy(x => new { x.ProdOrdID, x.InventoryID, x.SiteID })
                                      .Where(g => g.Count() > 1)
                                      .Select(x => new
                                      {
                                          x.Key.ProdOrdID,
                                          x.Key.InventoryID,
                                          x.Key.SiteID
                                      });

            if (duplicateDataWithWH.Any())
                throw new PXException($"Duplicated Materials {inventoryItem.Where(x => x.InventoryID == duplicateDataWithWH.FirstOrDefault()?.InventoryID).FirstOrDefault()?.InventoryCD} in Production Order {duplicateDataWithWH.FirstOrDefault()?.ProdOrdID}");


            foreach (var item in duplicateData)
            {
                object qty = item.sumQty;
                var temp = amTran.Where(x => x.ProdOrdID == item.ProdOrdID && x.InventoryID.Value == item.InventoryID);
                foreach (var aa in temp)
                {
                    var checkResult = Base.transactions.Cache.RaiseFieldVerifying<AMMTran.qty>(aa, ref qty);
                    if (!checkResult)
                        throw new PXException("You cannot save, please check error message");
                }
            }

            if (!(row.GetExtension<AMMTranExt>().UsrOverIssue ?? false))
            {
                object qty = row.Qty;
                var valid = e.Cache.RaiseFieldVerifying<AMMTran.qty>(e.Row, ref qty);
                if (!valid)
                    throw new PXException("You cannot save, please check error message");
            }

            // Valid Standard Cost
            if (!ValidStandardCost(row))
                e.Cache.RaiseExceptionHandling<AMMTran.unitCost>(e.Row, (e.Row as AMMTran).UnitCost,
                    new PXSetPropertyException<AMMTran.unitCost>($"{GetInventoryItemInfo(row.InventoryID)?.InventoryCD} 沒有維護標準成本，請通知採購。採購維護標準成本之後請刪除此領料單並重新建立", PXErrorLevel.Error));
        }

        public virtual void _(Events.FieldUpdated<AMMTranExt.usrOverIssue> e)
        {
            var row = e.Row as AMMTran;
            object qty = ((AMMTran)e.Row).Qty;
            var matl = SelectFrom<AMProdMatl>
                       .Where<AMProdMatl.prodOrdID.IsEqual<P.AsString>
                            .And<AMProdMatl.inventoryID.IsEqual<P.AsInt>>>
                       .View.Select(Base, row.ProdOrdID, row.InventoryID).RowCast<AMProdMatl>().FirstOrDefault();
            if (!(bool)e.NewValue)
                e.Cache.RaiseFieldVerifying<AMMTran.qty>(e.Row, ref qty);
        }

        public virtual void _(Events.FieldVerifying<AMMTranSplit.qty> e, PXFieldVerifying baseMethod)
        {
            var row = e.Row as AMMTranSplit;
            var amTranRow = Base.transactions.Cache.Current as AMMTran;
            var inputValue = e.NewValue;
            if (!(amTranRow.GetExtension<AMMTranExt>().UsrOverIssue ?? false))
                baseMethod?.Invoke(e.Cache, e.Args);
            else
            {
                var inventoryItem = SelectFrom<InventoryItem>.View.Select(Base).RowCast<InventoryItem>().ToList();
                var amProdMatl = SelectFrom<AMProdMatl>
                     .Where<AMProdMatl.prodOrdID.IsEqual<P.AsString>
                        .And<AMProdMatl.inventoryID.IsEqual<P.AsInt>>>.View.Select(Base, amTranRow.ProdOrdID, amTranRow.InventoryID)
                     .RowCast<AMProdMatl>().FirstOrDefault();
                var maxOverIssue = SelectFrom<LifeSyncPreference>.View.Select(Base).RowCast<LifeSyncPreference>().FirstOrDefault().MaxOverIssue;
                var overIssueQty = Math.Round((amProdMatl?.TotalQtyRequired ?? 0) * (1 + (maxOverIssue ?? 0) / 100), 4) - amProdMatl?.QtyActual;
                if ((decimal?)e.NewValue > overIssueQty)
                {
                    e.NewValue = overIssueQty;
                    e.Cache.SetValue<AMMTranSplit.qty>(row, overIssueQty);
                    throw new PXSetPropertyException<AMMTranSplit.qty>($"Material Quantity {inputValue} to be issued is greater then maximum allowed over issue {overIssueQty} ({amProdMatl.ProdOrdID}-{inventoryItem.FirstOrDefault(x => x.InventoryID == amProdMatl.InventoryID).InventoryCD})");
                }
            }
        }

        public virtual void _(Events.FieldUpdated<AMMTran.inventoryID> e, PXFieldUpdated baseHandler)
        {
            baseHandler?.Invoke(e.Cache, e.Args);
            var row = e.Row as AMMTran;
            if (row != null && !ValidStandardCost(row))
                Base.transactions.Cache.RaiseExceptionHandling<AMMTran.unitCost>(row, row.UnitCost,
                   new PXSetPropertyException<AMMTran.unitCost>($"{GetInventoryItemInfo(row.InventoryID)?.InventoryCD} 沒有維護標準成本，請通知採購。採購維護標準成本之後請刪除此領料單並重新建立", PXErrorLevel.Error));
        }
        #endregion

        #region Method

        public bool ValidStandardCost(AMMTran row)
        {
            if (row == null)
                return true;
            var setup = SelectFrom<INSetup>.View.Select(Base).TopFirst;
            if ((setup.GetExtension<INSetupExt>()?.UsrValidStandardCostInMaterials ?? false))
            {
                // and status=Balanced, system check the Unit Cost in each line. If the unit cost is 0
                if (Base.batch.Current != null && Base.batch.Current?.Status == "B" && row.UnitCost == 0)
                    return false;
            }
            return true;
        }

        public InventoryItem GetInventoryItemInfo(int? inventoryID)
           => InventoryItem.PK.Find(Base, inventoryID);
        #endregion

    }
}