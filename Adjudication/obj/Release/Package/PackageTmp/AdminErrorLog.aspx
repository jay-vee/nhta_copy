<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="AdminErrorLog.aspx.vb" Inherits="Adjudication.AdminErrorLog" Title="Error Log" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:DataGrid ID="gridMain" CssClass="small" runat="server" AllowPaging="True" PagerStyle-Mode="NumericPages" PagerStyle-Position="TopAndBottom" PagerStyle-BackColor="white" PagerStyle-Font-Bold="True" PagerStyle-ForeColor="Black" PagerStyle-HorizontalAlign="Center" DataKeyField="PK_ErrorID" AutoGenerateColumns="False" AllowSorting="True" CellPadding="1" HorizontalAlign="Left" Width="100%" BorderStyle="Solid" BorderColor="#000000" GridLines="Horizontal" CellSpacing="4">
        <FooterStyle ForeColor="#000000"></FooterStyle>
        <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#FFFF99"></SelectedItemStyle>
        <AlternatingItemStyle HorizontalAlign="Left" BackColor="#ffffcc"></AlternatingItemStyle>
        <ItemStyle ForeColor="#333333" BorderStyle="Solid"></ItemStyle>
        <HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000000"></HeaderStyle>
        <Columns>
            <asp:BoundColumn DataField="LastUpdateByName" SortExpression="LastUpdateByName" HeaderText="Error for User">
                <HeaderStyle HorizontalAlign="left" Width="15%"></HeaderStyle>
                <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="ErrorMessage" SortExpression="ErrorMessage" HeaderText="Error Message">
                <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                <ItemStyle HorizontalAlign="left" Width="75%"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="LastUpdateByDate" SortExpression="LastUpdateByDate" HeaderText="Date of Error">
                <HeaderStyle HorizontalAlign="center"></HeaderStyle>
                <ItemStyle HorizontalAlign="center" Width="5%"></ItemStyle>
            </asp:BoundColumn>
        </Columns>
    </asp:DataGrid>
    
        <div class="control-group row-fluid form-inline">
            &nbsp;
            <div class="controls">
                <label for="ddlPK_UserID" class="control-label">
                    Impersonate User:
                </label>
                <asp:DropDownList ID="ddlPK_UserID" runat="server" class="form-control">
                </asp:DropDownList>
                <asp:Button ID="btnImpersonate" runat="server" CssClass="btn btn-gold" Text="Impersonate"></asp:Button>
            </div>
            &nbsp;
        </div>
    
</asp:Content>
