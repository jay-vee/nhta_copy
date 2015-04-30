<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" Title="Website Login Settings" CodeBehind="AdminLoginSettings.aspx.vb" Inherits="Adjudication.AdminLoginSettings" %>

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
    <div style="text-align: center; padding: 1em;">
        <asp:Label ID="lblErrors" runat="server" ForeColor="red" Visible="False"></asp:Label>
        <asp:Label ID="lblSucessfulUpdate" runat="server" ForeColor="SeaGreen" Visible="False">Update Successful!</asp:Label>
    </div>
    <div class="panel panel-dark">
        <div class="panel-heading">General Login ID &amp; Password Settings</div>
        <div class="panel-body">
            <table class="TableSpacing">
                <tr>
                    <td align="right" width="40%">Minimum Login ID Length:
                    </td>
                    <td align="left" width="60%">
                        <asp:TextBox ID="txtMinLoginIDLength" Width="20%" CssClass="form-control" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" width="40%">Maximum Login ID Length:
                    </td>
                    <td align="left" width="60%">
                        <asp:TextBox ID="txtMaxLoginIDLength" Width="20%" CssClass="form-control" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">Minimum Password Length:
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtMinPasswordLength" Width="20%" CssClass="form-control" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" width="40%">Maximum PasswordLength:
                    </td>
                    <td align="left" width="60%">
                        <asp:TextBox ID="txtMaxPasswordLength" Width="20%" CssClass="form-control" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <div class="panel panel-dark">
        <div class="panel-heading">Detailed Login Settings</div>
        <div class="panel-body">
            <table class="TableSpacing">
                <tr>
                    <td align="right"># Password Tries before Account Lockout:
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtNumOfLoginAttempts" Width="20%" CssClass="form-control" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">Allow Password Reuse:
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlAllowPasswordReuse" runat="server" Width="40%" CssClass="form-control">
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                            <asp:ListItem Value="0">No</asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;<span class="FontSmaller">[Will Track the last 4 passwords used]</span>
                    </td>
                </tr>
                <tr>
                    <td align="right">Expire Passwords:
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlExpirePasswords" runat="server" Width="40%" CssClass="form-control">
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                            <asp:ListItem Value="0">No</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="left">Expire Passwords After X Days:
                                <asp:TextBox ID="txtExpirePasswordsAfterXDays" Width="20%" CssClass="form-control" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">Disable Expired Accounts:
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlDisableExpiredAccounts" Width="40%" runat="server" CssClass="form-control">
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                            <asp:ListItem Value="0">No</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="left">Expire Accounts After X Days:
                                <asp:TextBox ID="txtExpireAccountsAfterXDays" Width="20%" CssClass="form-control" runat="server"></asp:TextBox>
                    </td>
                </tr>

            </table>
        </div>
    </div>
    <div style="text-align: center; padding: 1em;">
        Last Updated By:
         <asp:Label ID="lblLastUpdateByName" runat="server" ForeColor="Gray"></asp:Label>&nbsp;on&nbsp;
         <asp:Label ID="lblLastUpdateByDate" runat="server" ForeColor="Gray"></asp:Label>

    </div>
    <div class="TextCenter">
        <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-gold" Text="Update "></asp:Button>
    </div>
</asp:Content>
