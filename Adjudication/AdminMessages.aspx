<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="AdminMessages.aspx.vb" Inherits="Adjudication.AdminMessages" Title="Messages to Administrators" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">

    <asp:DataGrid ID="gridMain" runat="server" GridLines="Horizontal" AllowPaging="False"
        DataKeyField="PK_AdminMessageID" AutoGenerateColumns="False" AllowSorting="True" CellPadding="4" CellSpacing="4"
        HorizontalAlign="Left" OnItemCommand="gridMain_ItemSelect" Width="100%" BorderStyle="None">
        <FooterStyle ForeColor="#000000"></FooterStyle>
        <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#FFFF99"></SelectedItemStyle>
        <AlternatingItemStyle HorizontalAlign="Left" BackColor="#ffffcc"></AlternatingItemStyle>
        <ItemStyle ForeColor="#333333" BorderStyle="Solid"></ItemStyle>
        <HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000000"></HeaderStyle>
        <Columns>
            <asp:TemplateColumn SortExpression="LastUpdateByDate" HeaderText="Date Posted">
                <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                <ItemStyle HorizontalAlign="Center" Width="10%" VerticalAlign="Top"></ItemStyle>
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "LastUpdateByDate","{0:MM/dd/yy}") %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="LastUpdateByName" SortExpression="LastUpdateByName" HeaderText="From User">
                <HeaderStyle HorizontalAlign="left" Width="10%"></HeaderStyle>
                <ItemStyle HorizontalAlign="left" Width="10%" VerticalAlign="Top"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Subject" SortExpression="Subject" HeaderText="Subject">
                <HeaderStyle HorizontalAlign="left" Width="35%"></HeaderStyle>
                <ItemStyle HorizontalAlign="left" Width="40%" VerticalAlign="Top"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Message" SortExpression="Message" HeaderText="Message">
                <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                <ItemStyle HorizontalAlign="left" Width="35%" VerticalAlign="Top"></ItemStyle>
            </asp:BoundColumn>
        </Columns>
    </asp:DataGrid>
</asp:Content>
