<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPageNoNav.Master" CodeBehind="UnAuthorized.aspx.vb" Inherits="Adjudication.UnAuthorized" Title="Unauothorized Access" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ MasterType VirtualPath="~/MasterPageNoNav.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <div class="panel panel-warning">
        <div class="panel-heading">You do not have access to the requested page.</div>
        <div class="panel-body">

            <table id="tblPageTitle" class="TableSpacing">
                <tr>
                    <td align="center">
                        <p>
                            You do not have access to the requested Webpage. 
                        </p>
                        <img src="Images/Exclamation_60x60.jpg" />
                        <p>
                            If you believe you *should* have access to the requested page, please email the System Administrator.
                        </p>
                        <hr />
                        <p>Please Return to the Main Page or Re-Login to continue.</p>
                        <p>
                            <asp:HyperLink ID="HyperLink1" runat="server" CssClass="btn btn-gold" NavigateUrl="MainPage.aspx">Return to Main Menu</asp:HyperLink>
                            <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="Login.aspx" CssClass="btn btn-gold">Login to Website</asp:HyperLink>
                        </p>
                    </td>
                </tr>

            </table>
        </div>
    </div>
</asp:Content>
