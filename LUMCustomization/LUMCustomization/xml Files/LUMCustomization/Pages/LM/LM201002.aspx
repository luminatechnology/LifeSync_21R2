<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM201002.aspx.cs" Inherits="Page_LM201002" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="LUMCustomization.Graph.LUMProductionScrapMaint" PrimaryView="ScrapTransactions">
        <CallbackCommands>
            <%--<px:PXDSCallbackCommand Name="Report" Visible="True" CommitChanges="true" StartNewGroup="true"></px:PXDSCallbackCommand>--%>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXGrid SyncPosition="True" ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Primary" AllowAutoHide="false">
        <Levels>
            <px:PXGridLevel DataMember="ScrapTransactions">
                <Columns>
                    <px:PXGridColumn DataField="ScrapID" Width="140"></px:PXGridColumn>
                    <px:PXGridColumn CommitChanges="True" DataField="Confirmed" Width="80" Type="CheckBox"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Trandate" Width="140" CommitChanges="True"></px:PXGridColumn>
                    <px:PXGridColumn DataField="ProdOrderID" Width="140" CommitChanges="True"></px:PXGridColumn>
                    <px:PXGridColumn DataField="ProdOrderType" Width="70"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Department" Width="280"></px:PXGridColumn>
                    <px:PXGridColumn DataField="ProdLine" Width="140"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Reason" Width="140"></px:PXGridColumn>
                    <px:PXGridColumn DataField="InventoryID" Width="140" CommitChanges="True"></px:PXGridColumn>
                    <px:PXGridColumn DataField="InventoryDescr" Width="200"></px:PXGridColumn>
                    <px:PXGridColumn DataField="UOM" Width="100"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Qty" Width="100"></px:PXGridColumn>
                    <px:PXGridColumn DataField="TransDescription" Width="200"></px:PXGridColumn>
                </Columns>

                <RowTemplate>
                    <%--<px:PXSelector runat="server" ID="edProdOrderType" DataField="ProdOrderType"></px:PXSelector>--%>
                    <px:PXSelector runat="server" ID="edProdOrderID" DataField="ProdOrderID"></px:PXSelector>
                    <px:PXDropDown runat="server" ID="edDepartment" DataField="Department"></px:PXDropDown>
                    <px:PXDropDown runat="server" ID="edProdLine" DataField="ProdLine"></px:PXDropDown>
                    <px:PXDropDown runat="server" ID="edReason" DataField="Reason"></px:PXDropDown>
                </RowTemplate>
            </px:PXGridLevel>
        </Levels>
        <AutoSize Container="Window" Enabled="True" MinHeight="150"></AutoSize>
        <ActionBar>
        </ActionBar>

        <Mode InitNewRow="True"></Mode>
    </px:PXGrid>
</asp:Content>

