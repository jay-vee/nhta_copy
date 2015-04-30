<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="AdminProduction.aspx.vb" Inherits="Adjudication.AdminProduction" Title="Production Administration" %>

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
    <asp:UpdatePanel runat="server" ID="UpdatePanelMain">
        <ContentTemplate>
          <div class="TextCenter">
                <asp:Label ID="lblErrors" runat="server" Visible="False" CssClass="alert alert-error" role="alert"></asp:Label>
                <asp:Label ID="lblSucessfulUpdate" runat="server" Visible="False" ForeColor="SeaGreen" CssClass="alert alert-success" role="alert">Update Successful!</asp:Label>
            </div>

            <asp:Panel ID="pnlEditSelection" Visible="True" runat="server">
                <table class="TableSpacing">
                    <tr>
                        <td>
                            <asp:LinkButton ID="lbtnAdd" runat="server" CssClass="btn btn-gold">Add Production</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:DataGrid ID="gridMain" runat="server" OnItemCommand="gridMain_ItemSelect" HorizontalAlign="Left" CellPadding="2" AllowSorting="false" AutoGenerateColumns="False" DataKeyField="PK_ProductionID" AllowPaging="False" BorderWidth="1px" GridLines="Horizontal" BorderColor="#000000" BorderStyle="Double" Width="100%">
                                <FooterStyle ForeColor="#000000"></FooterStyle>
                                <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#FFFF99"></SelectedItemStyle>
                                <AlternatingItemStyle HorizontalAlign="Left"></AlternatingItemStyle>
                                <ItemStyle ForeColor="#333333"></ItemStyle>
                                <HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000000"></HeaderStyle>
                                <Columns>
                                    <asp:BoundColumn Visible="False" DataField="PK_ProductionID" HeaderText="PK_ProductionID"></asp:BoundColumn>
                                    <asp:TemplateColumn ItemStyle-HorizontalAlign="left" ItemStyle-Width="80">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEdit" Text="Edit" CommandName="Edit_Command" ForeColor="blue" runat="server" />
                                            <asp:LinkButton ID="btnNomination" Text="Nominate" CommandName="Nomination_Command" ForeColor="blue" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="Title" SortExpression="Title" HeaderText="Production">
                                        <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="left"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="ProductionType" SortExpression="ProductionType" HeaderText="Production Type">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn SortExpression="OriginalProduction" HeaderText="Orig">
                                        <HeaderStyle HorizontalAlign="Center" Width="30"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" Width="30"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkGridOriginalProduction" Checked='<%# DataBinder.Eval(Container.DataItem, "OriginalProduction") %>' Enabled="false" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn SortExpression="FirstPerformanceDateTime" HeaderText="First Show">
                                        <HeaderStyle HorizontalAlign="Center" Width="70"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" Width="70"></ItemStyle>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "FirstPerformanceDateTime","{0:MM/dd/yy}") %>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn SortExpression="LastPerformanceDateTime" HeaderText="Last Show">
                                        <HeaderStyle HorizontalAlign="Center" Width="70"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" Width="70"></ItemStyle>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "LastPerformanceDateTime","{0:MM/dd/yy}") %>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <PagerStyle HorizontalAlign="Center" ForeColor="White" BackColor="#000000" Mode="NumericPages"></PagerStyle>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <asp:Panel ID="pnlSelectVenue" Visible="False" runat="server">
                <table style="width: 100%; border-spacing: 4px; border-collapse: separate; text-align:left;">
                    <tr>
                        <td class="LabelLarge" align="center" colspan="2">Select Venue for Production
                        </td>
                    </tr>
                </table>
                <table style="width: 100%; border-spacing: 4px; border-collapse: separate; text-align:left;">
                    <tr>
                        <td>
                            <asp:LinkButton ID="lbtnAddVenue" runat="server" CssClass="btn btn-gold">Add Venue</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DataGrid ID="gridVenue" runat="server" OnItemCommand="gridVenue_ItemSelect" HorizontalAlign="Left" CellPadding="2" AllowSorting="false" 
                                AutoGenerateColumns="False" DataKeyField="PK_VenueID" AllowPaging="False" BorderWidth="1px" GridLines="Horizontal" 
                                BorderStyle="Double" Width="100%">
                                <FooterStyle ForeColor="Black"></FooterStyle>
                                <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#FFFF99"></SelectedItemStyle>
                                <AlternatingItemStyle HorizontalAlign="Left"></AlternatingItemStyle>
                                <ItemStyle ForeColor="#333333"></ItemStyle>
                                <HeaderStyle Font-Bold="True" ForeColor="White" BackColor="Black"></HeaderStyle>
                                <Columns>
                                    <asp:BoundColumn Visible="False" DataField="PK_VenueID" HeaderText="PK_VenueID"></asp:BoundColumn>
                                    <asp:TemplateColumn>
                                        <ItemStyle></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnSelect" Text="Select Venue" CommandName="Select_Command" ForeColor="blue" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="VenueName" SortExpression="VenueName" HeaderText="Venue">
                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="City" SortExpression="City" HeaderText="City">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="State" SortExpression="State" HeaderText="State">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>
                                </Columns>
                                <PagerStyle HorizontalAlign="Center" ForeColor="White" BackColor="Black" Mode="NumericPages"></PagerStyle>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <asp:Panel ID="pnlAddEditData" Visible="False" runat="server">
                <asp:TextBox ID="txtPK_ProductionID" runat="server" Visible="False" BorderStyle="Dotted" Width="64px">0</asp:TextBox>
                <div class="panel panel-dark">
                    <div class="panel-heading">Production Information</div>
                    <div class="panel-body">
                        <asp:Label ID="lblMainpageApplicationNotes" runat="server"></asp:Label>
                        <table class="TableSpacing">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblEditInformation" runat="server" ForeColor="maroon"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 40%; text-align: right">Production Name:
                                </td>
                                <td style="width: 60%; text-align: left">
                                    <asp:TextBox ID="txtTitle" Font-Bold="True" runat="server" Width="65%" Enabled="False" CssClass="form-control"></asp:TextBox><span style="color: red;">*</span>
                                    <asp:DropDownList ID="ddlRequiresAdjudication" runat="server" Visible="False" CssClass="form-control">
                                        <asp:ListItem Value="1" Selected="True">Yes</asp:ListItem>
                                        <asp:ListItem Value="0">No</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 40%; text-align: right">Production Category:
                                </td>
                                <td style="width: 60%; text-align: left">
                                    <asp:DropDownList ID="ddlFK_ProductionCategoryID" runat="server" Width="65%" CssClass="form-control" Enabled="False">
                                    </asp:DropDownList>
                                    <span style="color: red;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 40%; text-align: right">Performing Company:
                                </td>
                                <td style="width: 60%; text-align: left">
                                    <asp:DropDownList ID="ddlFK_CompanyID" runat="server" Width="65%" CssClass="form-control" Enabled="False">
                                    </asp:DropDownList>
                                    <span style="color: red;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 40%; text-align: right">Venue:
                                </td>
                                <td style="width: 60%; text-align: left">
                                    <asp:DropDownList ID="ddlFK_VenueID" runat="server" Width="65%" CssClass="form-control" Enabled="False">
                                    </asp:DropDownList>
                                    <span style="color: red;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 40%; text-align: right">Production Type:
                                </td>
                                <td style="width: 60%; text-align: left">
                                    <asp:DropDownList ID="ddlFK_ProductionTypeID" runat="server" Width="65%" CssClass="form-control" Enabled="False">
                                    </asp:DropDownList>
                                    <span style="color: red;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Original Production:
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlOriginalProduction" runat="server" CssClass="form-control" Width="65%">
                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                        <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtAuthors" Visible="False" runat="server" Width="30px" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 40%; text-align: right">Age Appropriate:
                                </td>
                                <td style="width: 60%; text-align: left">
                                    <asp:DropDownList ID="ddlFK_AgeApproriateID" runat="server" Width="65%" CssClass="form-control">
                                    </asp:DropDownList>
                                    <span style="color: red;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Licensing Company:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtLicensingCompany" runat="server" Width="65%" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">First Performance Date:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtFirstPerformanceDateTime" runat="server" Width="25%" Enabled="False" CssClass="form-control"></asp:TextBox><span style="color: red;">*</span><span class="">[mm/dd/yy]</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Last Performance Date:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtLastPerformanceDateTime" runat="server" Width="25%" Enabled="False" CssClass="form-control"></asp:TextBox><span style="color: red;">*</span>&nbsp; <span size="1">[mm/dd/yy]</span>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="right">Please List ALL Performance<br />
                                    Dates and Times:<span style="color: red;">*</span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtAllPerformanceDatesTimes" runat="server" Width="75%" Rows="5" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Contact Name for Tickets:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtTicketContactName" runat="server" Width="65%" CssClass="form-control"></asp:TextBox><span style="color: red;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Contact Phone # for Tickets:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtTicketContactPhone" runat="server" Width="65%" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Contact Email for Tickets:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtTicketContactEmail" runat="server" Width="65%" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="right">Ticket Ordering Comments:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtTicketPurchaseDetails" runat="server" Width="75%" TextMode="MultiLine" Height="60px" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="right">Production Comments:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtComments" runat="server" Width="75%" TextMode="MultiLine" Height="60px" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>

                <div class="panel panel-dark">
                    <div class="panel-heading">Production Administrative Information</div>
                    <div class="panel-body">
                        <table class="TableSpacing">
                            <tr>
                                <td style="width: 40%; text-align: right">Last Updated By:
                                </td>
                                <td style="width: 60%; text-align: left">
                                    <asp:Label ID="lblLastUpdateByName" runat="server" ForeColor="Gray"></asp:Label>&nbsp;on&nbsp;
                                                <asp:Label ID="lblLastUpdateByDate" runat="server" ForeColor="Gray"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <table class="TableSpacing">
                            <tr>
                                <td style="width: 60%">Additional Comments for Producing Company & Assigned Adjudicators:<br />
                                    <asp:TextBox ID="txtAdminEmailComments" runat="server" Width="100%" TextMode="MultiLine" Rows="5" CssClass="form-control TextSmall"></asp:TextBox>
                                </td>
                                <td style="width: 40%">
                                    <asp:RadioButtonList ID="rblEmailInfo" runat="server" Width="100%">
                                        <asp:ListItem Value="NoAction">&nbsp;Do not Email Comments</asp:ListItem>
                                        <asp:ListItem Value="EmailAssignmentToAll" Selected="True">&nbsp;Email Comments</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>

                <div class="TextCenter">
                    <asp:Button ID="btnUpdate" runat="server" OnClientClick="ScrollToTop();" CssClass="btn btn-gold" Text="Save"></asp:Button>&nbsp;
                    <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-gold" Text="Return to Production List"></asp:Button>
                    <hr />
                    Note: Items with a red asterisk (<span style="color: red;">*</span>) are Required Fields.
                </div>
            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>
    <script id="scptDocumentReady" type="text/javascript">
        function ScrollToTop() {
            window.scrollTo(0, 0);
        }
        //=========== document.ready ===========
        $(document).ready(function () {

        });
    </script>

</asp:Content>
