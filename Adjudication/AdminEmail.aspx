<%@ Page Language="vb" AutoEventWireup="false" ValidateRequest="false" EnableViewStateMac="True" MasterPageFile="~/MasterPage.Master" CodeBehind="AdminEmail.aspx.vb" Inherits="Adjudication.AdminEmail" Title="Email Management" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <table class="TableSpacing">
        <tr>
            <td valign="top" align="left" width="100%" colspan="2">
                <asp:Panel ID="pnlSendEmail" runat="server">
                    <table class="TableSpacing">
                        <tr align="center">
                            <td colspan="2">
                                <asp:Label ID="lblErrors" runat="server" Visible="False" CssClass="form-control" ForeColor="red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" width="20%">
                                <b>From:</b>
                            </td>
                            <td align="left" width="80%">
                                <asp:DropDownList ID="ddlPK_FromEmailID" runat="server" Font-Bold="true" CssClass="form-control" Style="width: 40%;">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top">
                                <br />
                                <b>Send To:</b>
                                <br />
                                <br />
                                <div role="alert" class="alert alert-info TextSmall" style="padding: 1em; text-align:center;"><b>InActive</b> Users/Companies will<br> not be emailed</div>
                                <br />
                            </td>
                            <td valign="top" align="left">
                                <table cellspacing="0" cellpadding="0" style="width:100%;">
                                    <tr>
                                        <td style="vertical-align: top; text-align: left; width: 40%;">
                                            <asp:CheckBoxList ID="chklstAddressTo" runat="server">
                                                <asp:ListItem Value="UsersAndCompanies">Users and Companies</asp:ListItem>
                                                <asp:ListItem Value="Users">Users</asp:ListItem>
                                                <asp:ListItem Value="Companies">Companies</asp:ListItem>
                                                <asp:ListItem Value="Administrators">Administrators</asp:ListItem>
                                                <asp:ListItem Value="Liaisons">Liaisons</asp:ListItem>
                                                <asp:ListItem Value="PrimaryAdjudicators">Primary Adjudicators</asp:ListItem>
                                                <asp:ListItem Value="BackupAdjudicators">Backup Adjudicators</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </td>
                                        <td rowspan="4" style="vertical-align: top; text-align:center; width:60%;">
                                            <table cellpadding="0" cellspacing="0" border="0">
                                                <tr>
                                                    <td align="center">Active Users &amp; Companies
                                                    </td>
                                                    <td align="center"></td>
                                                    <td align="center">Selected Users/Companies
                                                    </td>
                                                </tr>
                                               
                                                <tr>
                                                    <td>
                                                        <asp:ListBox ID="lstPK_UserID" runat="server" Width="220px" Height="140px" CssClass="form-control" SelectionMode="Multiple"></asp:ListBox>
                                                    </td>
                                                    <td valign="middle" align="center">
                                                        <asp:Button ID="btnEmailUsers_Add" class="btn btn-gold" runat="server" Text=">>"></asp:Button><br />
                                                        <br />
                                                        <asp:Button ID="btnEmailUsers_Remove" class="btn btn-gold" runat="server" Text="<<"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:ListBox ID="lstPK_UserID_Selected" runat="server" Width="220px" Height="140px" CssClass="form-control" SelectionMode="Multiple"></asp:ListBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="TextSmall" valign="top" align="center" colspan="3">
                                                        <em>Hold CTRL key to select multiple names</em>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">Other Addresses:<i><span class="FontSmaller">(separate with ",")</span></i>
                            </td>
                            <td valign="top">
                                <span class="FontSmaller"><em>
                                    <asp:TextBox ID="txtCustomEmailAddresses" runat="server" Width="100%" TextMode="MultiLine" Rows="2" CssClass="form-control"></asp:TextBox></em></span>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:LinkButton ID="lbtnViewEmailAddresses" runat="server" ForeColor="Blue">View All Recipients</asp:LinkButton>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtTOEmailAddresses" runat="server" ForeColor="Black" Width="100%" TextMode="MultiLine" Rows="3" CssClass="form-control" ReadOnly="True" BackColor="#E0E0E0"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top" width="160">
                                <br />
                                <b>Emailing Options:</b>
                            </td>
                            <td align="left" width="100%">
                                <table class="TableSpacing">
                                    <tr>
                                        <td valign="top" align="left" width="33%">
                                            <asp:RadioButtonList ID="rblReceipientType" runat="server" CellPadding="0" CellSpacing="0">
                                                <asp:ListItem Value="1" Selected="True">Blind Copy</asp:ListItem>
                                                <asp:ListItem Value="0">Normal (not recommended)</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td valign="top" align="left" width="33%">
                                            <asp:CheckBox ID="chkUseSecondaryEmailAddress" runat="server" Text="&nbsp;Include 2nd Email"></asp:CheckBox><br />
                                            <asp:CheckBox ID="chkHighPriority" runat="server" ForeColor="Maroon" Text="&nbsp;Send with High Priority"></asp:CheckBox>
                                        </td>
                                        <td valign="top" align="left" width="33%">
                                            <asp:RadioButtonList ID="rblEmailsPerRecipient" runat="server" Width="200px" CellPadding="0" CellSpacing="0">
                                                <asp:ListItem Value="0">Send 1 email to All recipients</asp:ListItem>
                                                <asp:ListItem Value="1" Selected="True">Send 1 email Per recipient</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <tr>
                                <td align="right">
                                    <b>Insert Production Information:</b>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlPK_ProductionID" runat="server" AutoPostBack="True" CssClass="form-control" Width="60%">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        <tr>
                            <td align="right">
                                <b>Email Subject:</b>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtEmailSubject" Font-Bold="True" runat="server" Width="100%" CssClass="form-control" MaxLength="200"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top">
                                <br />
                                <b>Email Body:</b>
                            </td>
                            <td align="left">
                                <FTB:FreeTextBox ID="ftbEmailBody" runat="Server" Width="100%" Height="280px" DesignModeCss="~\StyleSheets\NHTA.css" ToolbarLayout="ParagraphMenu, FontFacesMenu, FontSizesMenu, FontForeColorsMenu,  FontForeColorPicker, FontBackColorsMenu, FontBackColorPicker, Bold, Italic, Underline, Strikethrough, Superscript, Subscript, InsertImageFromGallery, CreateLink, Unlink, | RemoveFormat, JustifyLeft, JustifyRight, JustifyCenter, JustifyFull, BulletedList,  NumberedList, Indent, Outdent, Cut, Copy, Paste, Delete, Undo, Redo, Print, Save, | ieSpellCheck, StyleMenu, SymbolsMenu, InsertHtmlMenu, InsertRule, InsertDate,  InsertTime, WordClean, InsertImage, InsertTable, EditTable, InsertTableRowBefore,  InsertTableRowAfter, DeleteTableRow, InsertTableColumnBefore, InsertTableColumnAfter,  DeleteTableColumn, InsertForm, InsertForm, InsertTextBox, InsertTextArea,  InsertRadioButton, InsertCheckBox, InsertDropDownList, InsertButton, InsertDiv, | InsertImageFromGallery, Preview, SelectAll, EditStyle" OnSaveClick="btnSendEmail_Click">
                                </FTB:FreeTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Button ID="btnSendEmail" runat="server" Text="Send Email" CssClass="btn btn-gold"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td valign="top" align="left" width="100%" colspan="2">
                <asp:Panel ID="pnlFinished" runat="server" Visible="False">
                    <table style="width: 100%; border-spacing: 4px; border-collapse: separate; text-align:left;">
                        <tr>
                            <td align="center" colspan="2">
                                <p>
                                    <font color="seagreen"><strong>Successfully Sent EMail</strong></span>
                                </p>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="15%">Recipients:
                            </td>
                            <td align="left" width="85%">
                                <asp:Label ID="lblReciepients" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <div class="separatorBlack">
                                    &nbsp;
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="15%">Sent Email:
                            </td>
                            <td align="left" width="85%">
                                <p>
                                    <asp:Label ID="lblEmailSubject" runat="server"></asp:Label>
                                </p>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <div class="separatorBlack">
                                    &nbsp;
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Button ID="btnSendMoreEmails" runat="server" Width="140px" Text="Send More Emails" CssClass="btn btn-gold"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
