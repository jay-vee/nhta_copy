<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="AdminUser.aspx.vb" Inherits="Adjudication.AdminUser" Title="User Account Administration" %>

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

        .listItemSpace td label {
            margin-right: 2em;
            margin-left: 0.5em;
        }
    </style>
    <asp:UpdatePanel runat="server" ID="UpdatePanelMain">
        <ContentTemplate>
            <asp:Panel ID="pnlUserData" runat="server">
                <div class="TextCenter">
                    <asp:Label ID="lblErrors" runat="server" Visible="False" CssClass="alert alert-warning" role="alert"></asp:Label>
                    <asp:Label ID="lblSucessfulUpdate" runat="server" Visible="False" CssClass="alert alert-success" role="alert">Update Successful!</asp:Label>
                </div>
                <asp:TextBox ID="txtPK_UserID" runat="server" Visible="False" BorderStyle="Dotted" Width="64px"></asp:TextBox>

                <div class="panel panel-dark">
                    <div class="panel-heading">User Account Administration</div>
                    <div class="panel-body">
                        <asp:Label ID="lblMainpageApplicationNotes" runat="server"></asp:Label>
                        <table class="TableSpacing">
                            <tr>
                                <td align="center" colspan="2"></td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 40%">Login ID:
                                </td>
                                <td style="text-align: left; width: 60%">
                                    <asp:TextBox ID="txtUserLoginID" Font-Bold="True" Visible="False" runat="server" Width="52%" CssClass="form-control"></asp:TextBox>
                                    <asp:Button ID="lblUserLoginID" runat="server" Font-Bold="True" BorderColor="White" BorderStyle="None" BackColor="White"></asp:Button>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 40%">
                                    <b>Access Level:</b>
                                </td>
                                <td style="text-align: left; width: 60%">
                                    <asp:CheckBox runat="server" ID="chkLiaison" Text="&nbsp;Liaison" Visible="false" />
                                    <asp:DropDownList ID="ddlFK_AccessLevelID" runat="server" Visible="False" Font-Bold="true" CssClass="form-control" Width="40%">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblFK_AccessLevelID" runat="server" Font-Bold="True"></asp:Label>
                                    <asp:CheckBox runat="server" ID="chkAdmin" Text="&nbsp;Administrator" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 40%">
                                    <b>User Status:</b>
                                </td>
                                <td style="text-align: left; width: 60%">
                                    <asp:DropDownList ID="ddlPK_UserStatusID" runat="server" Visible="false" CssClass="form-control" Width="20%">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblPK_UserStatusID" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 40%">Receive Emails:
                                </td>
                                <td style="text-align: left; width: 60%">
                                    <asp:DropDownList ID="ddlActive" runat="server" Visible="False" CssClass="form-control" Width="20%">
                                        <asp:ListItem Value="1" Selected="True">Yes</asp:ListItem>
                                        <asp:ListItem Value="0">No</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label ID="lblActive" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 40%">First Name:
                                </td>
                                <td style="text-align: left; width: 60%">
                                    <asp:TextBox ID="txtFirstName" runat="server" Width="52%" CssClass="form-control"></asp:TextBox><span style="color: red">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 40%">Last Name:
                                </td>
                                <td style="text-align: left; width: 60%">
                                    <asp:TextBox ID="txtLastName" runat="server" Width="52%" CssClass="form-control"></asp:TextBox><span style="color: red">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 40%">Affiliated Theater Company:
                                </td>
                                <td style="text-align: left; width: 60%">
                                    <asp:DropDownList ID="ddlFK_CompanyID" runat="server" Visible="False" Width="52%" CssClass="form-control">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblFK_CompanyID" runat="server"></asp:Label><span style="color: red">*</span>
                                    <asp:TextBox ID="txtOrig_lblFK_CompanyID" runat="server" Visible="False" BorderStyle="Dotted"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Primary Contact Phone #:
                                </td>
                                <td style="text-align: left; width: 60%">
                                    <asp:TextBox ID="txtPhonePrimary" runat="server" Width="40%" CssClass="form-control"></asp:TextBox><span style="color: red">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Secondary Contact Phone #:
                                </td>
                                <td style="text-align: left; width: 60%">
                                    <asp:TextBox ID="txtPhoneSecondary" runat="server" Width="40%" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Address:
                                </td>
                                <td style="text-align: left; width: 60%">
                                    <asp:TextBox ID="txtAddress" runat="server" Width="52%" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">City:
                                </td>
                                <td style="text-align: left; width: 60%">
                                    <asp:TextBox ID="txtCity" runat="server" Width="20%" CssClass="form-control"></asp:TextBox>&nbsp;State:
                                        <asp:TextBox ID="txtState" runat="server" Width="8%" CssClass="form-control">NH</asp:TextBox>&nbsp; ZIP:
                                        <asp:TextBox ID="txtZIP" runat="server" Width="12%" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Primary Email Address:
                                </td>
                                <td style="text-align: left; width: 60%">
                                    <asp:TextBox ID="txtEmailPrimary" runat="server" Width="52%" CssClass="form-control"></asp:TextBox><span style="color: red">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Secondary Email Address:
                                </td>
                                <td style="text-align: left; width: 60%">
                                    <asp:TextBox ID="txtEmailSecondary" runat="server" Width="52%" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Website:
                                </td>
                                <td style="text-align: left; width: 60%">
                                    <asp:TextBox ID="txtWebsite" runat="server" Width="52%" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="right">Account Notes:
                                </td>
                                <td style="text-align: left; width: 60%">
                                    <asp:TextBox ID="txtUserInformation" runat="server" Width="456px" Height="60px" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Date Last Attended Training:
                                </td>
                                <td style="text-align: left; width: 60%">
                                    <asp:Label ID="lblLastTrainingDate" runat="server" Font-Bold="True"></asp:Label>
                                    <asp:TextBox ID="txtLastTrainingDate" Visible="False" runat="server" Width="20%" CssClass="form-control"></asp:TextBox><span class="FontSmall">&nbsp;[mm/dd/yy]</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="right"></td>
                                <td valign="top" align="left" width="60%" class="FontSmaller">
                                    <em>Please email the administrator if you believe your Last Training is incorrect</em></span>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>

                <div class="panel panel-dark">
                    <div class="panel-heading">User Account Security</div>
                    <div class="panel-body">
                        <table class="TableSpacing">
                            <tr>
                                <td style="height: 15px" align="center" colspan="2">
                                    <asp:Label ID="lblSecurityNote" runat="server" ForeColor="#C00000">The following Question will be asked if/when you request a new password.</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Security Question:
                                </td>
                                <td style="text-align: left; width: 60%">
                                    <asp:DropDownList ID="ddlSecurityQuestion" runat="server" Width="52%" CssClass="form-control">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem Value="What was my first Pet's Name?">What was my first Pet's Name?</asp:ListItem>
                                        <asp:ListItem Value="What City have I always wanted to visit?">What City have I always wanted to visit?</asp:ListItem>
                                        <asp:ListItem Value="What is the name of the City/Town where I born?">What is the name of the City/Town where I born?</asp:ListItem>
                                        <asp:ListItem Value="What is my Mothers Maiden name?">What is my Mothers Maiden name?</asp:ListItem>
                                        <asp:ListItem Value="What is my Favorite Movie/TV Character?">What is my Favorite Movie/TV Character?</asp:ListItem>
                                        <asp:ListItem Value="What is my Favorite Movie/TV Show?">What is my Favorite Movie/TV Show?</asp:ListItem>
                                        <asp:ListItem Value="What was the name of my first Boyfriend/Girlfriend?">What was the name of my first Boyfriend/Girlfriend?</asp:ListItem>
                                    </asp:DropDownList>
                                    <span style="color: red">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Security Question Answer:
                                </td>
                                <td style="text-align: left; width: 60%">
                                    <asp:TextBox ID="txtSecurityAnswer" runat="server" Width="50%" CssClass="form-control" ToolTip="Enter in the Answer to the question selected.  One Word Answers are strongly recommended.  This is not case sensistive" MaxLength="99"></asp:TextBox><span style="color: red">*</span> &nbsp; <span class="FontSmall">[One word answers are recommended]</span>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>

                <div class="panel panel-dark">
                    <div class="panel-heading">User Account Administrative Information</div>
                    <div class="panel-body">
                        <table class="TableSpacing">
                            <tr>
                                <td style="text-align: right; width: 40%">Account Disabled:</td>
                                <td style="text-align: left; width: 60%">
                                    <asp:CheckBox ID="chkDisabledFlag" runat="server" Enabled="False"></asp:CheckBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 40%">Date Last Logged In:
                                </td>
                                <td style="text-align: left; width: 60%">
                                    <asp:Label ID="lblLastLoginTime" runat="server" ForeColor="Gray"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 40%">Date Last Password Change:
                                </td>
                                <td style="text-align: left; width: 60%">
                                    <asp:Label ID="lblDateLastPasswordChange" runat="server" ForeColor="Gray"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 40%">Bad Login Count:
                                </td>
                                <td style="text-align: left; width: 60%">
                                    <asp:Label ID="lblBadLoginCount" runat="server" ForeColor="Gray"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 40%">Last Updated By:
                                </td>
                                <td style="text-align: left; width: 60%">
                                    <asp:Label ID="lblLastUpdateByName" runat="server" ForeColor="Gray"></asp:Label>&nbsp;on&nbsp;
                                        <asp:Label ID="lblLastUpdateByDate" runat="server" ForeColor="Gray"></asp:Label>
                                </td>
                            </tr>

                        </table>
                    </div>
                </div>

                <asp:RadioButtonList ID="rblEmailInfo" runat="server" Visible="False" CssClass="form-control listItemSpace" RepeatDirection="Horizontal" BorderColor="gold" BorderStyle="Solid">
                    <asp:ListItem Value="NoAction" Selected="True">Do not Email</asp:ListItem>
                    <asp:ListItem Value="EmailLoginID">Email Login ID only</asp:ListItem>
                    <asp:ListItem Value="EmailLoginAndNEWPassword">Email Login and Auto-Generate a NEW Password for User</asp:ListItem>
                </asp:RadioButtonList>

                <div class="TextCenter">
                    <asp:Button ID="btnUpdate" runat="server" OnClientClick="ScrollToTop();" CssClass="btn btn-gold" Text="Save"></asp:Button>
                    <asp:Button ID="btnChangePassword" runat="server" CssClass="btn btn-gold" Text="Change Password"></asp:Button>
                    <asp:Button ID="btnViewBrowserHistory" runat="server" Visible="False" CssClass="btn btn-gold" Text="View Browser History"></asp:Button>
                    <a href="AdminUserList.aspx" class="btn btn-gold" style="margin: 0.5em;">User List</a>
                </div>
            </asp:Panel>


            <asp:Panel ID="pnlChangePassword" Visible="False" runat="server">

                <div class="panel panel-dark">
                    <div class="panel-heading">User Account Security</div>
                    <div class="panel-body">
                        <table class="TableSpacing">
                            <tr>
                                <td align="right" width="50%">Login ID:
                                </td>
                                <td align="left" width="50%">
                                    <asp:Label ID="lblLoginID_ChangePassword" runat="server" Font-Bold="True"></asp:Label>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Panel ID="pnlUserChangePassword" runat="server">
                                        <table class="TableSpacing">
                                            <tr>
                                                <td align="right" width="50%">
                                                    <asp:Label ID="lblOldPassword" Visible="False" runat="server">Enter Old Password:</asp:Label>
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:TextBox ID="txtOldPassword" Visible="False" runat="server" Width="50%" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr align="center">
                                                <td align="right">
                                                    <asp:Label ID="lblNewPassword" runat="server">Enter New Password:</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtNewPassword" runat="server" Width="50%" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr align="center">
                                                <td align="right">
                                                    <asp:Label ID="lblConfirmNewPassword" runat="server">Confirm New Password:</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtConfirmPassword" runat="server" Width="50%" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblErrors_ChangePassword" runat="server" Visible="False" ForeColor="red"></asp:Label>
                                    <asp:Label ID="lblSuccess_ChangePassword" runat="server" Visible="False" ForeColor="Green">Password Successfully Changed</asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="TextCenter">
                    <asp:Button ID="btnUpdate_PasswordChange" runat="server" CssClass="btn btn-gold" Text="Update"></asp:Button>&nbsp;
                                            <asp:Button ID="btnCancel_PasswordChange" runat="server" CssClass="btn btn-gold" Text="Cancel"></asp:Button>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>
        function ScrollToTop() {
            window.scrollTo(0, 0);
        }
    </script>
</asp:Content>
