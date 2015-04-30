<%@ Page Language="vb" MasterPageFile="~/MasterPageNoNav.Master" AutoEventWireup="false" CodeBehind="Timeout.aspx.vb" Inherits="Adjudication.Timeout" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <table id="tblPageTitle" width="100%" border="0">
        <tr>
            <td class="LabelMediumBold" align="left">
                <br />
                <p>
                    <font color="firebrick">Your session has Timed Out.</span>
                </p>
            </td>
        </tr>
        <tr>
            <td colspan="2"></td>
        </tr>
        <tr>
            <td align="center" height="25" valign="top" width="100%" colspan="2">
                <hr noshade>
            </td>
        </tr>
        <tr>
            <td class="FONTheader" align="center" bordercolor="black">
                <p>&nbsp;</p>
                <p>
                    <font color="firebrick">Your web session has Timed Out due to inactivity.</span>
								<br />
								<br />
								<img src="Images/Exclamation.jpg" />
                </p>
                <p>Please Re-Login to continue using this website.</p>
                <p>
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="Login.aspx" CssClass="btn btn-gold">Click here to Login</asp:HyperLink>
                </p>
                <p>&nbsp;</p>
            </td>
        </tr>
        <tr>
            <td align="center" height="25" valign="top" width="100%" colspan="2">
                <hr noshade>
            </td>
        </tr>
    </table>
</asp:Content>
