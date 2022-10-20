using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.CS;
using PX.Objects.IN;
using PX.Objects.PO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PX.Objects.RQ
{
    public class RQRequisitionEntryExt : PXGraphExtension<RQRequisitionEntry>
    {
        public PXAction<RQRequisition> createPOOrder;
        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = Messages.CreateOrders, MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        public virtual IEnumerable CreatePOOrder(PXAdapter adapter)
        {
            var validResult = true;
            var setup = SelectFrom<INSetup>.View.Select(Base).TopFirst;
            if ((setup.GetExtension<INSetupExt>()?.UsrValidStandardCostInPurchase ?? false))
            {
                foreach (var item in Base.Lines.Select().RowCast<RQRequisitionLine>())
                {
                    if (item?.LineType == POLineType.GoodsForInventory || item?.LineType == POLineType.GoodsForSalesOrder || item?.LineType == POLineType.GoodsForManufacturing)
                    {
                        var inventoryInfo = InventoryItem.PK.Find(Base, item?.InventoryID);
                        var attrVENDCONSIG = SelectFrom<CSAnswers>
                                            .Where<CSAnswers.refNoteID.IsEqual<P.AsGuid>
                                              .And<CSAnswers.attributeID.IsEqual<P.AsString>>>
                                            .View.Select(Base, inventoryInfo?.NoteID, "VENDCONSIG").TopFirst;
                        var itemCurySettingInfo = SelectFrom<InventoryItemCurySettings>
                                                 .Where<InventoryItemCurySettings.inventoryID.IsEqual<P.AsInt>>
                                                 .View.Select(Base, item?.InventoryID).TopFirst;
                        var itemClassInfo = INItemClass.PK.Find(Base, inventoryInfo?.ItemClassID);
                        if ((itemCurySettingInfo?.StdCost ?? 0) == 0 && attrVENDCONSIG?.Value != "1" && itemClassInfo?.ItemClassCD?.Trim() != "MRO")
                        {
                            Base.Lines.Cache.RaiseExceptionHandling<RQRequisitionLine.lineType>(item, item?.LineType,
                                new PXSetPropertyException<RQRequisitionLine.lineType>($"{inventoryInfo?.InventoryCD} 沒有維護標準成本，請通知採購。", PXErrorLevel.Error));
                            validResult = false;
                        }
                    }
                }
                if (!validResult)
                    throw new PXException("沒有維護標準成本，請通知採購。");
            }
            return Base.CreatePOOrder(adapter);
        }
    }
}
