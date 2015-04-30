<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="AdminAssignAdjudicators.aspx.vb" Inherits="Adjudication.AdminAssignAdjudicators" Title="Assign Adjudicators" %>

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
                <asp:Label ID="lblErrors" runat="server" Visible="False" CssClass="alert alert-warning" role="alert"></asp:Label>
                <asp:Label ID="lblSucessfulUpdate" runat="server" Visible="False" CssClass="alert alert-success" role="alert">Update Successful!</asp:Label>
            </div>

            <asp:Panel ID="pnlGrid" Visible="True" runat="server">
                <asp:TextBox ID="txtSortOrder" runat="server" Visible="False" BorderStyle="Dotted" Width="64px"></asp:TextBox>
                <asp:TextBox ID="txtSortColumnName" runat="server" Visible="False" BorderStyle="Dotted" Width="64px"></asp:TextBox>

                <div class="TextCenter">
                    Number of Nominations:<asp:Label ID="lblTotalNumberOfRecords" runat="server" Font-Bold="true">0</asp:Label>
                </div>
                <asp:DataGrid ID="gridMain" runat="server" BorderColor="#000000" BorderStyle="Double" OnItemCommand="gridMain_ItemSelect" HorizontalAlign="Left" CellPadding="2" AllowSorting="True" AutoGenerateColumns="False" DataKeyField="PK_ProductionID" AllowPaging="False" BorderWidth="1px" GridLines="Horizontal" Width="100%">
                    <FooterStyle ForeColor="Black"></FooterStyle>
                    <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#FFFF99"></SelectedItemStyle>
                    <AlternatingItemStyle HorizontalAlign="Left" BackColor="LemonChiffon"></AlternatingItemStyle>
                    <ItemStyle ForeColor="#333333"></ItemStyle>
                    <HeaderStyle Font-Bold="True" ForeColor="White" BackColor="Black"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="PK_NominationsID" HeaderText="PK_NominationsID"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="PK_ProductionID" HeaderText="PK_ProductionID"></asp:BoundColumn>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnAssign" Text="Assign" CommandName="Edit_Command" ForeColor="blue" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="Title" SortExpression="Title" HeaderText="Production">
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="UsersAssigned" SortExpression="UsersAssigned" HeaderText="# Adj. Assigned">
                            <HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" SortExpression="CompanyName" HeaderText="Company">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionCategory" SortExpression="ProductionCategory" HeaderText="Production Category">
                            <HeaderStyle HorizontalAlign="Center" Width="140px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="140px"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:TemplateColumn SortExpression="FirstPerformanceDateTime" HeaderText="First Performance">
                            <HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="80px"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "FirstPerformanceDateTime","{0:MM/dd/yy}") %>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn SortExpression="LastPerformanceDateTime" HeaderText="Last Performance">
                            <HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="80px"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "LastPerformanceDateTime","{0:MM/dd/yy}") %>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="FirstPerformanceDateTime" HeaderText="FirstPerformanceDateTime"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="LastPerformanceDateTime" HeaderText="LastPerformanceDateTime"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="VenueName" HeaderText="VenueName"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="PK_CompanyID" HeaderText="PK_CompanyID"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="EmailAddress"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="Address"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="City"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="State"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="Zip"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="Website"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="TicketContactName"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="TicketContactPhone"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="TicketContactEmail"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="CompanyWebsite" HeaderText="22"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="CompanyEmailAddress" HeaderText="23"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Center" ForeColor="White" BackColor="Black" Mode="NumericPages"></PagerStyle>
                </asp:DataGrid>
            </asp:Panel>

            <asp:Panel ID="pnlSelectedProductionDetail" Visible="False" runat="server">
                <asp:TextBox ID="txtPK_NominationID" runat="server" Visible="False" BorderStyle="Dotted" Width="64px">0</asp:TextBox>
                <asp:TextBox ID="txtFK_CompanyID" runat="server" Visible="False" BorderStyle="Dotted" Width="64px">0</asp:TextBox>
                <asp:TextBox ID="txtPK_ProductionID" runat="server" Visible="False" BorderStyle="Dotted" Width="64px">0</asp:TextBox>
                <asp:TextBox ID="txtFullName" runat="server" Visible="False" BorderStyle="Dotted" Width="64px">0</asp:TextBox>

                <div class="panel panel-dark">
                    <div class="panel-heading">Production Details</div>
                    <div class="panel-body">
                        <table class="TableSpacing">
                            <tr>
                                <td valign="top" style="text-align: right; width: 40%;">Production Name:
                                </td>
                                <td style="text-align: left; width: 60%;">
                                    <asp:Label ID="lblTitle" runat="server" Font-Bold="True" ForeColor="#404040"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" style="text-align: right; width: 40%;">Company:
                                </td>
                                <td style="text-align: left; width: 60%;">
                                    <asp:Label ID="lblCompanyName" runat="server" Font-Bold="True" ForeColor="#404040"></asp:Label><br />
                                    <asp:Label ID="lblCompanyEmailAddress" runat="server" ForeColor="#404040"></asp:Label><br />
                                    <asp:HyperLink ID="hlnkCompanyWebsite" runat="server"></asp:HyperLink>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" style="text-align: right; width: 40%;">Venue:
                                </td>
                                <td style="text-align: left; width: 60%;">
                                    <asp:Label ID="lblVenueName" runat="server" Font-Bold="True" ForeColor="#404040"></asp:Label><br />
                                    <asp:Label ID="lblAddress" runat="server" ForeColor="#404040"></asp:Label><br />
                                    <asp:Label ID="lblCity" runat="server" ForeColor="#404040"></asp:Label>&nbsp;
                                        <asp:Label ID="lblState" runat="server" ForeColor="#404040"></asp:Label>&nbsp;
                                        <asp:Label ID="lblZIP" runat="server" ForeColor="#404040"></asp:Label><br />
                                    <asp:HyperLink ID="hlnkWebsite" runat="server"></asp:HyperLink>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="right">Performance Dates:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblFirstPerformanceDateTime" runat="server" Font-Bold="True" ForeColor="#404040"></asp:Label>&nbsp;thru
                                        <asp:Label ID="lblLastPerformanceDateTime" runat="server" Font-Bold="True" ForeColor="#404040"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="right">Ticket Contact Information:
                                </td>
                                <td align="left">
                                    <p>
                                        Name:
                                            <asp:Label ID="lblTicketContactName" runat="server" ForeColor="#404040"></asp:Label><br />
                                        Phone#:
                                            <asp:Label ID="lblTicketContactPhone" runat="server" ForeColor="#404040"></asp:Label><br />
                                        Email:
                                            <asp:Label ID="lblTicketContactEmail" runat="server" ForeColor="#404040"></asp:Label>
                                    </p>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlAdjudictorList" Visible="False" runat="server">
                <asp:LinkButton ID="lbtnAdd" runat="server" CssClass="btn btn-gold">Assign Adjudicator</asp:LinkButton>

                <asp:DataGrid ID="gridSub" runat="server" BorderColor="#000000" BorderStyle="Double" OnItemCommand="gridSub_ItemSelect" HorizontalAlign="Left" CellPadding="2" AutoGenerateColumns="False" DataKeyField="FK_NominationsID" AllowPaging="False" BorderWidth="1px" GridLines="Horizontal" Width="100%">
                    <FooterStyle ForeColor="Black"></FooterStyle>
                    <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#FFFF99"></SelectedItemStyle>
                    <AlternatingItemStyle HorizontalAlign="Left"></AlternatingItemStyle>
                    <ItemStyle ForeColor="#333333"></ItemStyle>
                    <HeaderStyle Font-Bold="True" ForeColor="White" BackColor="Black"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="PK_ScoringID" HeaderText="PK_ScoringID"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="FK_NominationsID" HeaderText="FK_NominationsID"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="PK_UserID" HeaderText="PK_UserID"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="PK_CompanyID" HeaderText="PK_CompanyID"></asp:BoundColumn>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" Text="Edit" CommandName="Edit_Command" ForeColor="blue" runat="server" />
                                <asp:LinkButton ID="btnDelete" Text="Delete" CommandName="Delete_Command" ForeColor="blue" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="FullName" SortExpression="FullName" HeaderText="Adjudicator">
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" SortExpression="CompanyName" HeaderText="Representing Company">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ScoringStatus" SortExpression="ScoringStatus" HeaderText="Status">
                            <HeaderStyle HorizontalAlign="Center" Width="160px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="160px" CssClass="TextSmall"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="TotalScore" SortExpression="TotalScore" HeaderText="Scoring">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:TemplateColumn SortExpression="LastUpdateByDate" HeaderText="Last Updated">
                            <HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="80px"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "LastUpdateByDate","{0:MM/dd/yy}") %>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="ProductionDateAdjudicated_Planned" HeaderText="11"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="ProductionDateAdjudicated_Actual" HeaderText="12"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="AdjudicatorRequestsReassignment" HeaderText="13"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="LastUpdateByName" HeaderText="14"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="LastUpdateByDate" HeaderText="15"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="CreateByName" HeaderText="16"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="CreateByDate" HeaderText="17"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="ReserveAdjudicator" HeaderText="ReserveAdjudicator"></asp:BoundColumn>
                        <asp:TemplateColumn SortExpression="ReserveAdjudicator" HeaderText="Rsrv" Visible="false">
                            <HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                            <ItemTemplate>
                                <%# IIF(DataBinder.Eval(Container.DataItem, "ReserveAdjudicator")=0,"No", "Yes") %>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="FK_ScoringStatusID" HeaderText="19"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Center" ForeColor="White" BackColor="Black" Mode="NumericPages"></PagerStyle>
                </asp:DataGrid>
            </asp:Panel>

            <asp:Panel ID="pnlAddEdit" Visible="False" runat="server">
                <asp:TextBox ID="txtPK_ScoringID" runat="server" Visible="False" BorderStyle="Dotted" Width="64px">0</asp:TextBox>
                <asp:Label ID="lblTotalScore" runat="server" ForeColor="#404040">0</asp:Label>
                <div class="panel panel-dark">
                    <div class="panel-heading">Select Adjudicator to Assign</div>
                    <div class="panel-body">
                        <table style="width: 100%; border-spacing: 4px; border-collapse: separate; background-color: lemonchiffon;">
                            <tr>
                                <td align="right">Number of Adjudicators required for Production:
                                </td>
                                <td>
                                    <asp:Label ID="lblNumAdjudicatorsPerShow" runat="server" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Maximum Shows Per Adjudicator:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblMaxShowsPerAdjudicator" runat="server" Font-Bold="true"></asp:Label>&nbsp;&nbsp; <span class="FontSmall">[not excluded from Adjudicator list]</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Adjudicator must have received Training after:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblValidTrainingStartDate" runat="server" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                        </table>

                        <table class="TableSpacing">
                            <tr>
                                <td align="right">Adjudicator Representing Company:
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlFK_CompanyID" runat="server" Width="65%" CssClass="form-control" Enabled="False">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 40%;">Trained Adjudicators<strong><font color="#0066ff">*</span></strong>:
                                </td>
                                <td style="text-align: left; width: 60%;">
                                    <asp:DropDownList ID="ddlPK_UserID" runat="server" Width="65%" CssClass="form-control" AutoPostBack="True">
                                    </asp:DropDownList><br />
                                    <span class="FontSmaller">*The Number in parentheses () is the # of Productions already assigned to that Adjudicator</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; text-align: right; padding-right: 0.5em;"></td>
                                <td align="left">
                                    <asp:CheckBox ID="chkShowAssignmentCounts" runat="server" CssClass="TextSmall" AutoPostBack="True" Checked="True" Text="&nbsp;Sort by Assignment Count" ToolTip="Shows (#) in the drop down list, indicating the # of Adjudications already assigned"></asp:CheckBox><br />
                                    <asp:CheckBox ID="chkIncludeBackupAdjudicators" runat="server" CssClass="TextSmall" AutoPostBack="True" Text="&nbsp;Include Backup Adjudicators" ToolTip='Will list Backup Adjudicators in the "Adjudicator" drop down list.'></asp:CheckBox><br />
                                    <asp:CheckBox ID="chkShowAllAdjudicators" runat="server" CssClass="TextSmall" AutoPostBack="True" Text="&nbsp;Show ALL Active Adjudicators  (WARNING!)" ToolTip='Will list Backup Adjudicators in the "Adjudicator" drop down list.'></asp:CheckBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2"></td>
                            </tr>
                            <tr style="display: none;">
                                <td style="text-align: right; width: 40%;">Reserve Adjudicator:
                                </td>
                                <td style="text-align: left; width: 60%;">
                                    <asp:DropDownList ID="ddlReserveAdjudicator" runat="server" Width="70px">
                                        <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 40%;">Adjudication Status:
                                </td>
                                <td style="text-align: left; width: 60%;">
                                    <asp:DropDownList ID="ddlPK_ScoringStatusID" runat="server" Width="65%" CssClass="form-control">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 100%; border-spacing: 4px; border-collapse: separate; background-color: gainsboro;">
                            <tr>
                                <td style="text-align: right; width: 40%;">Confirmation Date for Adjudication:
                                </td>
                                <td style="text-align: left; width: 60%;">
                                    <asp:TextBox ID="txtProductionDateAdjudicated_Planned" runat="server" Width="25%" CssClass="form-control"></asp:TextBox><span class="FontSmall">&nbsp;[mm/dd/yy] (Adjudicator)</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 40%;">Actual Date Adjudicated:
                                </td>
                                <td style="text-align: left; width: 60%;">
                                    <asp:TextBox ID="txtProductionDateAdjudicated_Actual" runat="server" Width="25%" CssClass="form-control"></asp:TextBox><span class="FontSmall">&nbsp;[mm/dd/yy] (Producing Liaison)</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Adjudicator Requests Reassignment:
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlAdjudicatorRequestsReassignment" runat="server" Width="15%" CssClass="form-control">
                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                        <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>

                        </table>
                    </div>
                </div>

                <div class="panel panel-dark">
                    <div class="panel-heading">Assignment Administrative Information</div>
                    <div class="panel-body">
                        <table class="TableSpacing">
                            <tr>
                                <td style="text-align: right; width: 40%;">Last Updated By:
                                </td>
                                <td style="text-align: left; width: 60%;">
                                    <asp:Label ID="lblLastUpdateByName" runat="server" ForeColor="Gray"></asp:Label>&nbsp;on&nbsp;
                                        <asp:Label ID="lblLastUpdateByDate" runat="server" ForeColor="Gray"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 40%;">Created By:
                                </td>
                                <td style="text-align: left; width: 60%;">
                                    <asp:Label ID="lblCreateByName" runat="server" ForeColor="Gray"></asp:Label>&nbsp;on&nbsp;
                                        <asp:Label ID="lblCreateByDate" runat="server" ForeColor="Gray"></asp:Label>
                                </td>
                            </tr>
                        </table>

                        <table class="TableSpacing">
                            <tr>
                                <td style="width: 60%">Additional Comments for Producing Company & Assigned Adjudicators:<br />
                                    <asp:TextBox ID="txtAdminEmailComments_Assign" runat="server" Width="100%" TextMode="MultiLine" Rows="5" CssClass="form-control TextSmall"></asp:TextBox>
                                </td>
                                <td style="width: 40%">
                                    <asp:RadioButtonList ID="rblEmailInfo" runat="server" Width="100%">
                                        <asp:ListItem Value="NoAction">&nbsp;Do not Email Comments</asp:ListItem>
                                        <asp:ListItem Value="EmailAssignmentToAll" Selected="True">&nbsp;Email Comments</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>

                        <div class="TextCenter">
                            <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-gold" Text="Save"></asp:Button>
                            <asp:Button ID="btnCancelUpdate" runat="server" CssClass="btn btn-gold" Text="Cancel"></asp:Button>
                        </div>
            </asp:Panel>

            <asp:Panel ID="pnlDeleteConfirm" runat="server" Visible="False" CssClass="Text">
                <div style="text-align: center">
                    <asp:Label ID="lblConfirmDelete" runat="server" CssClass="Text"></asp:Label>
                </div>
                <div class="alert alert-danger" role="alert" style="text-align: center">
                    <b>NOTE:</b> Deletions cannot be undone!
                </div>
                <table style="width: 100%; border-spacing: 4px; border-collapse: separate; text-align: center;">
                    <tr>
                        <td class="form-control" style="text-align: center; margin-bottom: 1em; width: 90%;">
                            <asp:RadioButtonList ID="rblEmailInfo_Delete" runat="server" Width="80%" RepeatDirection="Horizontal">
                                <asp:ListItem Value="NoAction">&nbsp;Do not Email</asp:ListItem>
                                <asp:ListItem Value="EmailDeletionToAll" Selected="True">&nbsp;Email Deletion to Representing Company and Producing Company members</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; width: 100%;">
                                Administrator Comments to include in Email:<br />
                                <asp:TextBox ID="txtAdminEmailComments_Delete" runat="server" TextMode="MultiLine" CssClass="form-control" Width="90%" Rows="4"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <hr style="width: 80%; margin: 1em;" />
                            <asp:Button ID="btnDeleteConfirm" runat="server" CssClass="btn btn-gold" Text="Delete"></asp:Button>&nbsp;
                            <asp:Button ID="btnDeleteCancel" runat="server" CssClass="btn btn-gold" Text="Cancel"></asp:Button>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
