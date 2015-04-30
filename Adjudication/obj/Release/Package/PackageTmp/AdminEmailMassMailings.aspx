<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="AdminEmailMassMailings.aspx.vb" Inherits="Adjudication.AdminEmailMassMailings" Title="EMail Mass Communication" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:UpdatePanel runat="server" ID="UpdatePanelMain">
        <ContentTemplate>
            <table class="TableSpacing">
                <tr>
                    <td valign="top" align="left" width="100%" colspan="2">
                        <asp:Panel ID="pnlMain" runat="server">
                            <table class="TableSpacing">
                                <tr align="center">
                                    <td colspan="2">
                                        <asp:Label ID="lblErrors" runat="server" ForeColor="red" Visible="False"></asp:Label>
                                    </td>
                                </tr>

                                <tr>
                                    <td class="fontScoringHeader" align="center" colspan="2">Email Communication Choices
                                    </td>
                                </tr>

                                <tr>
                                    <td align="right" width="100">FROM:
                                    </td>
                                    <td align="left" width="560">
                                        <table cellspacing="0" cellpadding="0" width="560">
                                            <tr>
                                                <td valign="top" width="200">
                                                    <asp:DropDownList ID="ddlPK_FromEmailID" runat="server" Font-Bold="true">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>

                                <tr>
                                    <td align="right">OPTIONS:
                                    </td>
                                    <td valign="top" align="left">
                                        <asp:RadioButtonList ID="rblReceipientType" runat="server" Width="560">
                                            <asp:ListItem Value="0" Selected="True">Normal</asp:ListItem>
                                            <asp:ListItem Value="1">Blind Copy</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:CheckBox ID="chkUseSecondaryEmailAddress" runat="server" Checked="True" Text="&nbsp;Include 2nd Email Addresses"></asp:CheckBox><br />
                                        <asp:CheckBox ID="chkHighPriority" runat="server" ForeColor="Maroon" Text="&nbsp;Send with High Priority"></asp:CheckBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <div class="separatorBlack">
                                            &nbsp;
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <font color="olive"><strong>NOTES</strong>:
                                            <ol>
                                                <li><strong>Be Patient</strong> when sending emails - the processing can take many minutes
                                                    <li>EMails will go out for each production, for each assigned Adjudicator/Liaison -&nbsp;including their Liaison(s)
                                                        <li>Late Reminders include Adjudicating and Producing Theatre Company Liaisions</li>
                                            </ol></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:Label ID="lblSendEmailError" runat="server" ForeColor="red" Font-Bold="True" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" width="100%" colspan="2">
                                        <asp:DataGrid ID="gridMain" runat="server" CellPadding="2" OnItemCommand="gridMain_ItemSelect" Width="100%" AutoGenerateColumns="False" BorderColor="#000000" BorderStyle="Solid" HorizontalAlign="Left" DataKeyField="PK_EmailMessageTypesID" AllowPaging="False" BorderWidth="1px" GridLines="Horizontal">
                                            <FooterStyle ForeColor="#000000"></FooterStyle>
                                            <SelectedItemStyle ForeColor="White" BackColor="#FFFF99"></SelectedItemStyle>
                                            <ItemStyle ForeColor="#333333" BackColor="LightGoldenrodYellow"></ItemStyle>
                                            <HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000000"></HeaderStyle>
                                            <Columns>
                                                <asp:BoundColumn Visible="False" DataField="PK_EmailMessageTypesID"></asp:BoundColumn>
                                                <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Button runat="server" ID="btnSendEmail" CommandName="Select_Command" Width="76" Height="30" Text="Send Email"></asp:Button>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="EmailMessageType" HeaderText="Email Message Type">
                                                    <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="LastUpdateByName" HeaderText="Last Sent By">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" Width="80"></ItemStyle>
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="Last Sent Date*">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" Width="80"></ItemStyle>
                                                    <ItemTemplate>
                                                        <%# DataBinder.Eval(Container.DataItem, "LastUpdateByDate","{0:MM/dd/yy}") %>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn Visible="False" DataField="LastUpdateByDate"></asp:BoundColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">*Dates in <span style="color: red;">RED</span> indicate more than 30 days has passed.
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="left" width="100%" colspan="2">
                        <asp:Panel ID="pnlFinished" runat="server" Visible="False">
                            <table class="TableSpacing">
                                <tr>
                                    <td colspan="2">
                                        <div class="separatorBlack">
                                            &nbsp;
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="80">Count of Emails Sent:
                                    </td>
                                    <td align="left" width="540">
                                        <asp:Label ID="lblReciepients" runat="server" Font-Bold="True"></asp:Label>&nbsp; (review the <strong>Email Log </strong>to view the Details of each email Sent)
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="80">Subject Line of Last Email Sent:
                                    </td>
                                    <td align="left" width="540">
                                        <p>
                                            <asp:Label ID="lblEmailSubject" runat="server"></asp:Label>
                                        </p>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2">
                                        <asp:Label ID="lblHTMLEMailMessage" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <div class="separatorBlack">
                                            &nbsp;
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:Button ID="btnSendMoreEmails" runat="server" Width="140px" Text="Send More Emails"></asp:Button>&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
