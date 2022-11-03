using LUMCustomization.DAC;
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

namespace LUMCustomization.Graph
{
    public class LUMProductionScrapMaint : PXGraph<LUMProductionScrapMaint, LUMProductionScrap>
    {
        public SelectFrom<LUMProductionScrap>.OrderBy<Asc<LUMProductionScrap.scrapID>>.View Document;
        public SelectFrom<LUMProductionScrapDetails>
              .Where<LUMProductionScrapDetails.scrapID.IsEqual<LUMProductionScrap.scrapID.FromCurrent>>
              .View Transactions;

        public LUMProductionScrapMaint()
        {
            Report.AddMenuAction(release);
            Report.AddMenuAction(printProductionScrapReport);
        }

        #region Action

        public PXAction<LUMProductionScrap> Report;
        [PXUIField(DisplayName = "Reports", MapEnableRights = PXCacheRights.Select)]
        [PXButton(MenuAutoOpen = true)]
        protected void report() { }

        public PXAction<LUMProductionScrap> release;
        [PXButton]
        [PXUIField(DisplayName = "Release", MapEnableRights = PXCacheRights.Select)]
        public IEnumerable Release(PXAdapter adapter)
        {
            if (this.Document.Current != null)
            {
                this.Document.Current.Confirmed = true;
                this.Save.Press();
            }
            return adapter.Get();
        }

        public PXAction<LUMProductionScrap> printProductionScrapReport;
        [PXButton]
        [PXUIField(DisplayName = "Print Production Scrap Report", MapEnableRights = PXCacheRights.Select)]
        public IEnumerable PrintProductionScrapReport(PXAdapter adapter)
        {
            this.Save.Press();
            var _reportID = "LM612001";
            //ActiveStandardReport(_reportID);
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                ["ParmScrapID"] = this.Document.Current?.ScrapID
            };
            if (parameters["ParmScrapID"] != null)
                throw new PXReportRequiredException(parameters, _reportID, PXBaseRedirectException.WindowMode.New, string.Format("Report {0}", _reportID));
            return adapter.Get();
        }
        #endregion

        #region Events

        public virtual void _(Events.RowPersisting<LUMProductionScrap> e)
        {
            var row = e.Row;
            if (row != null && row.ScrapID == "<NEW>")
            {
                var newSequenceNumber = AutoNumberAttribute.GetNextNumber(e.Cache, row, "SCRAPID", Accessinfo.BusinessDate);
                row.ScrapID = newSequenceNumber;
            }
        }

        public virtual void _(Events.RowPersisting<LUMProductionScrapDetails> e)
        {
            var row = e.Row;
            if (row != null && this.Document.Current != null && row.ScrapID != this.Document.Current?.ScrapID)
                row.ScrapID = this.Document.Current?.ScrapID;
        }

        public virtual void _(Events.RowDeleting<LUMProductionScrap> e)
        {
            var row = e.Row;
            if (row != null && (row?.Confirmed ?? false))
                throw new PXException("Can not delete Confirmed record");
        }

        public virtual void _(Events.RowSelected<LUMProductionScrap> e)
            => this.release.SetEnabled(e.Row != null && e.Row.ScrapID != "<NEW>" && !(e.Row.Confirmed ?? false));

        public virtual void _(Events.RowSelected<LUMProductionScrapDetails> e)
        {
            var row = e.Row as LUMProductionScrapDetails;
            if (row != null && row.InventoryID.HasValue)
            {
                if (string.IsNullOrEmpty(row.InventoryDescr))
                {
                    object newInventoryDescr;
                    e.Cache.RaiseFieldDefaulting<LUMProductionScrapDetails.inventoryDescr>(row, out newInventoryDescr);
                    row.InventoryDescr = (string)newInventoryDescr;
                }

                if (string.IsNullOrEmpty(row.UOM))
                {
                    object newUOM;
                    e.Cache.RaiseFieldDefaulting<LUMProductionScrapDetails.uOM>(row, out newUOM);
                    row.UOM = (string)newUOM;
                }
            }
        }

        public virtual void _(Events.FieldUpdated<LUMProductionScrapDetails.inventoryID> e)
        {
            var row = e.Row as LUMProductionScrapDetails;
            if (row != null && row.InventoryID.HasValue)
            {
                if (string.IsNullOrEmpty(row.InventoryDescr))
                {
                    object newInventoryDescr;
                    e.Cache.RaiseFieldDefaulting<LUMProductionScrapDetails.inventoryDescr>(row, out newInventoryDescr);
                    row.InventoryDescr = (string)newInventoryDescr;
                }

                if (string.IsNullOrEmpty(row.UOM))
                {
                    object newUOM;
                    e.Cache.RaiseFieldDefaulting<LUMProductionScrapDetails.uOM>(row, out newUOM);
                    row.UOM = (string)newUOM;
                }
            }
        }

        public virtual void _(Events.FieldUpdated<LUMProductionScrap.prodOrderID> e)
        {
            var row = e.Row as LUMProductionScrap;
            if (row != null && !string.IsNullOrEmpty(row.ProdOrderID))
            {
                if (string.IsNullOrEmpty(row.ProdOrderType))
                {
                    object newProdOrderType;
                    e.Cache.RaiseFieldDefaulting<LUMProductionScrap.prodOrderType>(row, out newProdOrderType);
                    row.ProdOrderType = (string)newProdOrderType;
                }
            }
        }

        public virtual void _(Events.FieldSelecting<LUMProductionScrap.reason> e)
        {
            var attributeReasonDDL = SelectFrom<CSAttributeDetail>
                                    .Where<CSAttributeDetail.attributeID.IsEqual<P.AsString>>
                                    .View.Select(this, "REASON").RowCast<CSAttributeDetail>();
            PXStringListAttribute.SetList<LUMProductionScrap.reason>(e.Cache, null, attributeReasonDDL.Select(x => x.ValueID).ToArray(), attributeReasonDDL.Select(x => x.Description).ToArray());
        }

        public virtual void _(Events.FieldSelecting<LUMProductionScrap.department> e)
        {
            var attributeDepartmentDDL = SelectFrom<CSAttributeDetail>
                                        .Where<CSAttributeDetail.attributeID.IsEqual<P.AsString>>
                                        .View.Select(this, "DEPARTMENT").RowCast<CSAttributeDetail>();
            PXStringListAttribute.SetList<LUMProductionScrap.department>(e.Cache, null, attributeDepartmentDDL.Select(x => x.ValueID).ToArray(), attributeDepartmentDDL.Select(x => x.Description).ToArray());
        }

        public virtual void _(Events.FieldSelecting<LUMProductionScrap.prodLine> e)
        {
            var attributeProdLineDDL = SelectFrom<CSAttributeDetail>
                                      .Where<CSAttributeDetail.attributeID.IsEqual<P.AsString>>
                                      .View.Select(this, "PRODLINE").RowCast<CSAttributeDetail>();
            PXStringListAttribute.SetList<LUMProductionScrap.prodLine>(e.Cache, null, attributeProdLineDDL.Select(x => x.ValueID).ToArray(), attributeProdLineDDL.Select(x => x.Description).ToArray());
        }

        public virtual void _(Events.FieldDefaulting<LUMProductionScrapDetails.lineNbr> e)
        {
            var currentList = this.Transactions.Select().RowCast<LUMProductionScrapDetails>();
            var maxLineNbr = currentList.Count() == 0 ? 0 : currentList.Max(x => x?.LineNbr ?? 0);
            e.NewValue = maxLineNbr + 1;
        }

        public virtual void _(Events.FieldDefaulting<LUMProductionScrap.reason> e)
        {
            var row = e.Row as LUMProductionScrap;
            if (row != null && string.IsNullOrEmpty(row.Reason))
            {
                var attributeReasonDDL = SelectFrom<CSAttributeDetail>
                                        .Where<CSAttributeDetail.attributeID.IsEqual<P.AsString>>
                                        .View.Select(this, "REASON").RowCast<CSAttributeDetail>();
                e.NewValue = attributeReasonDDL.FirstOrDefault(x => x.Description == "製程報廢")?.ValueID;
            }
        }

        #endregion

    }
}
