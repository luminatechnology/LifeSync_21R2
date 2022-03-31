using LUMCustomizations.Library;
using PX.Data;
using PX.Objects.AM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LUMCustomizations.Graph_Extensions
{
    public class VendorShipmentEntry_Extension: PXGraphExtension<VendorShipmentEntry>
    {
        public override void Initialize()
        {
            base.Initialize();
            var _lumLibrary = new LumLibrary();
            if (_lumLibrary.isCNorHK())
            {
                this.lumReport.AddMenuAction(VendorShipment);
            }
        }

        #region Action

        public PXMenuAction<AMVendorShipment> lumReport;
        [PXUIField(DisplayName = "REPORT", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        [PXButton(MenuAutoOpen = true, CommitChanges = true)]
        public virtual void LumReport() { }

        public PXAction<AMVendorShipment> VendorShipment;
        [PXButton(DisplayOnMainToolbar = false)]
        [PXUIField(DisplayName = "Print Vendor Shipmetn Report", Enabled = true, MapEnableRights = PXCacheRights.Select)]
        protected virtual IEnumerable vendorShipment(PXAdapter adapter)
        {
            var _reportID = "lm611003";
            if (Base.Document.Current != null)
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters["ShipmentID"] = Base.Document.Current.ShipmentNbr;
                throw new PXReportRequiredException(parameters, _reportID, $"Report {_reportID}");
            }
            return adapter.Get();
        }
        #endregion

        #region controll customize button based on country ID
        protected void _(Events.RowSelected<AMVendorShipment> e)
        {
            var _lumLibrary = new LumLibrary();
            if (!_lumLibrary.isCNorHK())
            {
                VendorShipment.SetVisible(false);
            }
        }
        #endregion
    }
}
