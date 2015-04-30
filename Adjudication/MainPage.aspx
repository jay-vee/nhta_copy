<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="MainPage.aspx.vb" Inherits="Adjudication.MainPage" Title="Main Menu" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">   
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <div class="col-md-9" style="text-align: center;">
        <asp:Panel ID="pnlAdminRequests" Visible="False" runat="server">
            <div class="panel panel-admin">
                    <div class="panel-heading FontLargeBold" data-toggle="collapse" data-target="collapseAdmin" aria-expanded="true" aria-controls="collapseAdmin">
                        Administrator Adjudication Information
                    </div>
                <div class="panel-body collapse.in" id="collapseAdmin">
                    <table style="width: 100%;">
                        <tr>
                            <td align="right" style="width: 50%;"># of Pending Reassignment Requests:
                            </td>
                            <td align="left" style="width: 10%; padding-left: 1em;">
                                <asp:Label ID="lblCountReassignmentRequests" runat="server" Font-Bold="True" ForeColor="red"></asp:Label>&nbsp;                                
                            </td>
                            <td align="left" style="width: 40%;">
                                <asp:HyperLink ID="Hyperlink3" runat="server" CssClass="FontHyperLink" NavigateUrl="AdminReassignmentRequests.aspx" Target="_self">Process Requests</asp:HyperLink></td>
                        </tr>
                        <tr>
                            <td align="right"># of Late Ballots:
                            </td>
                            <td align="left" style="padding-left: 1em;">
                                <asp:Label ID="lblCountLateBallots" runat="server" Font-Bold="True" ForeColor="red"></asp:Label>&nbsp;                            
                            </td>
                            <td align="left">
                                <a href="Reports/AdjLateBallot30_60_90.aspx">View Late Ballots</a>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="2" style="padding-right: 1em;">
                                <asp:Label ID="lblAdminMessages" runat="server" CssClass="fontDataDesc" ForeColor="Firebrick" EnableViewState="False" Text=""></asp:Label>
                            </td>
                            <td align="left">
                                <asp:HyperLink ID="Hyperlink1" CssClass="FontHyperLink" runat="server" NavigateUrl="AdminMessages.aspx" Target="_self">View Messages</asp:HyperLink>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </asp:Panel>

        <div class="panel panel-dark">
            <div class="panel-heading">My Adjudications</div>
            <div class="panel-body">
                <asp:Panel ID="pnlMyAdjudications" Visible="False" runat="server">
                    <table style="width: 100%;">
                        <tr>
                            <td valign="top" align="center" style="width: 100%;" colspan="2">
                                <asp:Label ID="lblAdjudicationMessage" runat="server" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100%;">
                                <asp:DataGrid ID="gridMain" runat="server" Visible="False" Style="width: 100%;" BorderStyle="None" BorderColor="Gainsboro" GridLines="Horizontal" BorderWidth="1px" DataKeyField="PK_ScoringID" AutoGenerateColumns="False" CellPadding="2" HorizontalAlign="Left">
                                    <HeaderStyle ForeColor="Black" BorderColor="Black" BackColor="LemonChiffon"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn Visible="False" DataField="PK_NominationsID" HeaderText="PK_NominationsID"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="PK_ProductionID" HeaderText="PK_ProductionID"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="AdjudicatorToSeeProduction" SortExpression="AdjudicatorToSeeProduction" HeaderText="Confirmed Attendance">
                                            <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="80px"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="TotalScore" HeaderText="Ballot Status">
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Title" HeaderText="Title">
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="CompanyName" HeaderText="Company">
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:TemplateColumn HeaderText="Production Dates">
                                            <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "FirstPerformanceDateTime","{0:MM/dd/yy}") %>
                                                <%# DataBinder.Eval(Container.DataItem, "LastPerformanceDateTime","{0:MM/dd/yy}") %>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn Visible="False" DataField="FirstPerformanceDateTime" HeaderText="FirstPerformanceDateTime"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="LastPerformanceDateTime" HeaderText="LastPerformanceDateTime"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="DaysToConfirmAttendance" HeaderText="DaysToConfirmAttendance"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="DaysToWaitForScoring" HeaderText="DaysToWaitForScoring"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="PK_CompanyID" HeaderText="PK_CompanyID"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="Title" HeaderText="Title"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="LastUpdateByName" HeaderText="LastUpdateByName"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="LastUpdateByDate" HeaderText="LastUpdateByDate"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="PK_ScoringID" HeaderText="PK_ScoringID"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="ProductionDateAdjudicated_Planned" HeaderText="ProductionDateAdjudicated_Planned"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="AdjudicatorRequestsReassignment" HeaderText="AdjudicatorRequestsReassignment"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="FK_VenueID" HeaderText="FK_VenueID"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="ScoringStatus" HeaderText="Adjudication Status">
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" CssClass="small"></ItemStyle>
                                        </asp:BoundColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblDaysToConfirm" runat="server" CssClass="TextSmall" ForeColor="#404000" Visible="False">*Please confirm X days before the production's opening date.</asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </div>

        <div class="panel panel-dark">
            <div class="panel-heading">
                <asp:Label ID="lblProductionCompany" runat="server"></asp:Label>&nbsp;Productions&nbsp;&amp; Assigned Adjudicators
            </div>
            <div class="panel-body">
                <asp:Panel ID="pnlMyProductions" Visible="False" runat="server">
                    <table style="width: 100%;">
                        <tr>
                            <td valign="top" align="center" style="width: 100%;" colspan="2">
                                <asp:Label ID="lblProductionMessage" runat="server" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100%;">
                                <asp:DataGrid ID="gridSub" runat="server" Visible="False" Style="width: 100%;" BorderStyle="None" BorderColor="Gainsboro" GridLines="Horizontal" BorderWidth="1px" DataKeyField="PK_ScoringID" AutoGenerateColumns="False" CellPadding="2" HorizontalAlign="Left">
                                    <HeaderStyle ForeColor="Black" BorderColor="Black" BackColor="LemonChiffon"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn DataField="Title" HeaderText="Show Title">
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:TemplateColumn HeaderText="Adjudicator">
                                            <HeaderStyle HorizontalAlign="left" Width="140px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="left" Width="140px"></ItemStyle>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "Fullname","{0:MM/dd/yy}") %>
                                                <br />
                                                <a class="fontDataHyperlinkSmall" href='mailto:<%# DataBinder.Eval(Container.DataItem, "EmailPrimary") %>'>
                                                    <%# DataBinder.Eval(Container.DataItem, "EmailPrimary") %>
                                                </a>
                                                <br />
                                                <%# DataBinder.Eval(Container.DataItem, "PhonePrimary") %>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Confirmed Attendance">
                                            <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "ProductionDateAdjudicated_Planned","{0:MM/dd/yy}") %>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Confirmed at Show">
                                            <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "ProductionDateAdjudicated_Actual","{0:MM/dd/yy}") %>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn DataField="CompanyName" HeaderText="Representing Company">
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Wrap="true"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:TemplateColumn HeaderText="Production Dates">
                                            <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "FirstPerformanceDateTime","{0:MM/dd/yy}") %>
                                                <%# DataBinder.Eval(Container.DataItem, "LastPerformanceDateTime","{0:MM/dd/yy}") %>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn Visible="False" DataField="ProductionDateAdjudicated_Actual"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="LastPerformanceDateTime"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="PK_UserID"></asp:BoundColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </div>

        <div class="panel panel-dark">
            <div class="panel-heading">
                <asp:Label ID="lblCompany" runat="server"></asp:Label>&nbsp;Registered Users
            </div>
            <div class="panel-body">
                <asp:Panel ID="pnlCompanyUsers" runat="server">
                    <table style="width: 100%;">
                        <tr>
                            <td valign="top" align="center" style="width: 100%;" colspan="2">
                                <asp:Label ID="lblCompanyUsersMessage" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100%;">
                                <asp:DataGrid ID="gridCompanyUsers" runat="server" Style="width: 100%;" BorderStyle="None" BorderColor="Gainsboro" GridLines="Horizontal" BorderWidth="1px" DataKeyField="PK_UserID" AutoGenerateColumns="False" CellPadding="2" HorizontalAlign="Left">
                                    <HeaderStyle ForeColor="Black" BorderColor="Black" BackColor="LemonChiffon"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn DataField="Fullname" HeaderText="Registered User">
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="ActiveUser" HeaderText="Active" Visible="False">
                                            <HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="AccessLevelName" HeaderText="Role">
                                            <HeaderStyle HorizontalAlign="Left" Width="70px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left" Width="70px"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="PhonePrimary" HeaderText="Phone #">
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:TemplateColumn SortExpression="EmailPrimary" HeaderText="Email Address">
                                            <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="left"></ItemStyle>
                                            <ItemTemplate>
                                                <a class="fontDataHyperlink" href='mailto:<%# DataBinder.Eval(Container.DataItem, "EmailPrimary") %>'>
                                                    <%# DataBinder.Eval(Container.DataItem, "EmailPrimary") %>
                                                </a>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Last Training">
                                            <HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "LastTrainingDate","{0:MM/dd/yy}") %>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Last Login">
                                            <HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "LastLoginTime","{0:MM/dd/yy}") %>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </div>
    </div>

    <div class="col-md-3" style="text-align: center;">
        <div class="panel panel-dark">
            <div class="panel-heading">News & Notes</div>
            <div class="panel-body">
                <asp:Label ID="lblMainpageApplicationNotes" runat="server"></asp:Label>
            </div>
        </div>

        <div class="panel panel-dark">
            <div class="panel-heading">Administrator Contact</div>
            <div class="panel-body">
                <div>
                    <asp:Label ID="lblAdminContactName" runat="server" CssClass="fontDataDesc" Text="" EnableViewState="False"></asp:Label>
                </div>
                <div>
                    <asp:Label ID="lblAdminContactPhoneNum" runat="server" CssClass="fontDataDesc" Text="" EnableViewState="False"></asp:Label>&nbsp;
                </div>
                <div>
                    <asp:HyperLink ID="lnkAdminContactEmail" CssClass="fontDataDesc" Target="_blank" runat="server" ForeColor="Blue"></asp:HyperLink>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
