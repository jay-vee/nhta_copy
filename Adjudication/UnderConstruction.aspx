<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UnderConstruction.aspx.vb" Inherits="Adjudication.UnderConstruction" %>

<!DOCTYPE html>
<html>
<head>
    <title>Under Construction</title>
    <link href="Styles.css" type="text/css" rel="STYLESHEET" />
    <script src="Includes/Functions.js" type="text/javascript"></script>

</head>
<body >
    <form class="container" id="MainMenu" method="post" runat="server"  >
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td colspan="2" width="100%">
            </td>
        </tr>
        <tr>
            <td width="100%" align="left" valign="top">
                <table id="tblPageTitle" width="100%" border="0">
                    <tr>
                        <td class="LabelMediumBold" align="left">
                            THIS PAGE IS UNDER CONSTRUCTION
                        </td>
                    </tr>
                    <tr>
                        <td align="center" height="25" valign="top" width="100%" colspan="2" class="UnderConstruction">
                        </td>
                    </tr>
                    <tr>
                        <td class="FONTheader" align="center" bordercolor="black">
                            <p>
                                &nbsp;</p>
                            <p>
                                Please be patient while we are building the page you requested.
                            </p>
                            <p>
                                Please check back in a few days to see if this page is ready.
                            </p>
                            <p>
                                Thank you for your patience.</p>
                            <p>
                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="MainPage.aspx">Return to Main Menu</asp:HyperLink></p>
                            <p>
                                &nbsp;</p>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" height="25" valign="top" width="100%" colspan="2" class="UnderConstruction">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
