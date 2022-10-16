using LUMCustomizations.DAC;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.CS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LUMCustomization.Graph
{
    public class LUMProductionScrapMaint : PXGraph<LUMProductionScrapMaint>
    {
        public PXSave<LUMProductionScrap> Save;
        public PXCancel<LUMProductionScrap> Cancel;
        public SelectFrom<LUMProductionScrap>.OrderBy<Asc<LUMProductionScrap.scrapID>>.View ScrapTransactions;

        #region Method

        public virtual void _(Events.RowPersisting<LUMProductionScrap> e)
        {
            var row = e.Row;
            if (row != null && row.ScrapID == "<NEW>")
            {
                var newSequenceNumber = AutoNumberAttribute.GetNextNumber(e.Cache, row, "SCRAPID", Accessinfo.BusinessDate);
                row.ScrapID = newSequenceNumber;
            }
        }

        public virtual void _(Events.RowDeleting<LUMProductionScrap> e)
        {
            var row = e.Row;
            if (row != null && (row.Confirmed ?? false))
            { 
                e.Cache.RaiseExceptionHandling<LUMProductionScrap.confirmed>(e.Row, row.Confirmed,
                    new PXSetPropertyException<LUMProductionScrap.confirmed>("Can not delete Confirmed record", PXErrorLevel.Error));
                throw new PXException("Can not delete Confirmed record");
            }
        }

        public virtual void _(Events.RowSelected<LUMProductionScrap>e )
        {
            var row = e.Row as LUMProductionScrap;
            if (row != null && row.InventoryID.HasValue)
            {
                if (string.IsNullOrEmpty(row.InventoryDescr))
                {
                    object newInventoryDescr;
                    e.Cache.RaiseFieldDefaulting<LUMProductionScrap.inventoryDescr>(row, out newInventoryDescr);
                    row.InventoryDescr = (string)newInventoryDescr;
                }

                if (string.IsNullOrEmpty(row.UOM))
                {
                    object newUOM;
                    e.Cache.RaiseFieldDefaulting<LUMProductionScrap.uOM>(row, out newUOM);
                    row.UOM = (string)newUOM;
                }
            }
        }

        public virtual void _(Events.FieldUpdated<LUMProductionScrap.inventoryID> e)
        {
            var row = e.Row as LUMProductionScrap;
            if (row != null && row.InventoryID.HasValue)
            {
                if (string.IsNullOrEmpty(row.InventoryDescr))
                {
                    object newInventoryDescr;
                    e.Cache.RaiseFieldDefaulting<LUMProductionScrap.inventoryDescr>(row, out newInventoryDescr);
                    row.InventoryDescr = (string)newInventoryDescr;
                }

                if (string.IsNullOrEmpty(row.UOM))
                {
                    object newUOM;
                    e.Cache.RaiseFieldDefaulting<LUMProductionScrap.uOM>(row, out newUOM);
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
