<%@ Page Language="vb" MasterPageFile="~/MasterPage.Master" AutoEventWireup="false" Title="Theater Company Administration" CodeBehind="AdminCompany.aspx.vb" Inherits="Adjudication.AdminCompany" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <style>
        .form-control {
            display: inline-block;
        }

        td {
            vertical-align: middle;
        }
    </style>
    <div class="TextCenter">
        <asp:Label ID="lblErrors" runat="server" Visible="False" ForeColor="red"></asp:Label>
        <asp:Label ID="lblSucessfulUpdate" runat="server" Visible="False" ForeColor="SeaGreen">Update Successful!</asp:Label></td>
    </div>

    <asp:Panel ID="pnlUserData" runat="server">
        <asp:TextBox ID="txtPK_CompanyID" runat="server" Visible="False" Width="64px" BorderStyle="Dotted">0</asp:TextBox>
        <div class="panel panel-dark">
            <div class="panel-heading">Theatre Company Information</div>
            <div class="panel-body">
                <table class="TableSpacing">
                    <tr>
                        <td align="right" style="width: 40%;">Company Name</td>
                        <td align="left" style="width: 60%;">
                            <asp:TextBox ID="txtCompanyName" Font-Bold="True" runat="server" Width="65%" CssClass="form-control"></asp:TextBox><span style="color: red">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Company Phone #</td>
                        <td align="left">
                            <asp:TextBox ID="txtPhone" runat="server" Width="30%" CssClass="form-control"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="right">Company Mailing Address</td>
                        <td align="left">
                            <asp:TextBox ID="txtAddress" runat="server" Width="65%" CssClass="form-control"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="right">City</td>
                        <td align="left">
                            <asp:TextBox ID="txtCity" runat="server" Width="30%" CssClass="form-control"></asp:TextBox>&nbsp;State:
											<asp:TextBox ID="txtState" runat="server" Width="8%" CssClass="form-control">NH</asp:TextBox>&nbsp; 
											ZIP:
											<asp:TextBox ID="txtZIP" runat="server" Width="14%" CssClass="form-control"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="right">Company Email Address</td>
                        <td align="left">
                            <asp:TextBox ID="txtEmailAddress" runat="server" Width="65%" CssClass="form-control"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="right">Company Website</td>
                        <td align="left">
                            <asp:TextBox ID="txtWebsite" runat="server" Width="65%" CssClass="form-control"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td valign="top" align="right">Company Comments</td>
                        <td align="left">
                            <asp:TextBox ID="txtComments" runat="server" Width="65%" TextMode="MultiLine" CssClass="form-control"></asp:TextBox></td>
                    </tr>
                </table>
            </div>
        </div>

        <div class="panel panel-dark">
            <div class="panel-heading">Theatre Company Administrative Information</div>
            <div class="panel-body">
                <table class="TableSpacing">
                    <tr>
                        <td align="right" style="width: 40%;">Company Actively Participating:
                        </td>
                        <td align="left" style="width: 60%;">
                            <asp:DropDownList ID="ddlActiveCompany" runat="server" Enabled="False" Width="30%" CssClass="form-control">
                                <asp:ListItem Value="1">Active</asp:ListItem>
                                <asp:ListItem Value="0">InActive</asp:ListItem>
                            </asp:DropDownList><span style="color: red">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="right"># of Productions to Adjudicate</td>
                        <td align="left">
                            <asp:TextBox ID="txtNumOfProductions" runat="server" Width="20%" Enabled="False" CssClass="form-control">0</asp:TextBox><span style="color: red">*</span></td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 40%;">Last Updated By</td>
                        <td align="left" style="width: 60%;">
                            <asp:Label ID="lblLastUpdateByName" runat="server" ForeColor="Gray"></asp:Label>&nbsp;on&nbsp;
                            <asp:Label ID="lblLastUpdateByDate" runat="server" ForeColor="Gray"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </asp:Panel>

    <div class="TextCenter">
        <asp:Button ID="btnUpdate" runat="server" OnClientClick="ScrollToTop();" CssClass="btn btn-gold" Text="Update "></asp:Button>
        <a href="AdminCompanyList.aspx" class="btn btn-gold" style="margin: 0.5em;">Theatre Company List</a>
        <br />
        Note: Items with a red asterisk (<span style="color: red;">*</span>) indicate Required Fields.
    </div>
    <script>
        function ScrollToTop() {
            window.scrollTo(0, 0);
        }
    </script>
</asp:Content>

