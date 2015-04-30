<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPageNoNav.Master" CodeBehind="ForgotPassword.aspx.vb" Inherits="Adjudication.ForgotPassword" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <table id="tblPageTitle" width="100%">
        <tr>
            <td class="fontScoringHeader" width="100%">Password Reset Request
            </td>
        </tr>
        <tr>
            <td align="left">
                <p>A new Password will be randomly generated and Emailed to you.</p>
            </td>
        </tr>
        <tr>
            <td align="center" width="100%">
                <asp:Panel ID="pnlRequestNewPassword" runat="server">
                    <table class="TableSpacing">
                        <tr>
                            <td align="right" width="50%">Enter in your NHTA registered Email Address:
                            </td>
                            <td width="50%">
                                <asp:TextBox ID="txtEmailPrimary" runat="server" CssClass="form-control" Width="60%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <hr style="width: 60%;" />
                                <asp:Label ID="lblErrors" runat="server" Visible="False" ForeColor="red">Please enter in a Valid Email Address</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-gold" Text="Submit Request"></asp:Button>
                                <div class="col-md-12" style="text-align: center; padding: 1em;">
                                    <a href="login.aspx" target="_self" class="fontDataHyperlink">Return to Login page</a>
                                </div>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>

                <asp:Panel ID="pnlSecurityQuestion" runat="server" Visible="False">
                    <table class="TableSpacing">
                        <tr>
                            <td align="center" colspan="2">
                                <div class="alert alert-warning" role="alert">
                                    You must answer this Security Question before a new Password will be emailed to you
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" width="50%">
                                <asp:Label ID="lblSecurityQuestion" runat="server"></asp:Label>
                            </td>
                            <td width="50%">
                                <asp:TextBox ID="txtSecurityAnswer" runat="server" Width="60%" CssClass="form-control"></asp:TextBox>
                                <asp:TextBox ID="txtErrorCount" runat="server" Width="40px" Visible="False" BorderStyle="Dotted">0</asp:TextBox>
                                <asp:TextBox ID="txtInfobox" runat="server" Width="40px" Visible="False" BorderStyle="Dotted"></asp:TextBox>
                                <asp:TextBox ID="txtUserName" runat="server" Width="40px" Visible="False" BorderStyle="Dotted"></asp:TextBox>
                                <asp:TextBox ID="txtUserLoginID" runat="server" Width="40px" Visible="False" BorderStyle="Dotted"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <hr style="width: 60%;" />
                                <asp:Label ID="lblErrorSecurity" runat="server" Visible="False" ForeColor="red">Please enter in the Security Question Answer</asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td align="center" colspan="2">
                                <asp:Button ID="btnSecurityCheck" runat="server" CssClass="btn btn-gold" Text="Submit Request"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>

                <asp:Panel ID="pnlResults" runat="server" Visible="False">
                    <div class="well well-lg" style="width:60%;">
                        <asp:Label ID="lblResults" runat="server" ></asp:Label>
                    </div>
                    <p class="btn btn-gold">
                        <a href="Login.aspx">Click to Login to the NHTA Adjudication Website</a>
                    </p>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
