<%@ Page Language="vb" MasterPageFile="~/MasterPageNoNav.Master" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="Adjudication.Login" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Panel runat="server" ID="pnlLogin" Width="100%">
        <div class="col-md-5" style="text-align: center;">
            <div class="panel panel-dark">
                <div class="panel-heading">Login to the Adjudication Website</div>
                <div class="panel-body">
                    <div class="form-group">
                        <label for="txtLoginID" class="col-md-3 control-label" style="text-align: right;">
                            Login ID</label>
                        <div class="col-md-9" style="margin-bottom: 0.5em;">
                            <asp:TextBox ID="txtLoginID" CssClass="form-control" runat="server" Width="100%" MaxLength="50" ToolTip="please enter in your Login ID as specified by the NH Theatre Awards"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="txtPassword" class="col-md-3 control-label" style="text-align: right;">
                            Password</label>
                        <div class="col-md-9">
                            <asp:TextBox ID="txtPassword" CssClass="form-control" runat="server" Width="70%" TextMode="Password"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-12" style="text-align: center;">
                        <hr style="width: 60%;" />
                        <asp:Label ID="lblErrors" runat="server" Font-Bold="True" Visible="False" ForeColor="Tomato">An Error was encountered!</asp:Label>
                    </div>
                    <div class="form-group">
                        <div class="col-md-12" style="text-align: center;">
                            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-gold"></asp:Button>
                            <div class="col-md-12" style="text-align: center; padding: 1em;">
                                <a href="ForgotPassword.aspx" target="_self" class="fontDataHyperlink">I forgot my Password/Login ID</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-7" style="text-align: center;">
            <div class="panel panel-dark">
                <div class="panel-heading">Upcoming Productions for Adjudication</div>
                <div class="panel-body">
                    <asp:DataList ID="gridCommunity" runat="server" Width="100%" BorderStyle="none" GridLines="None" BorderWidth="0px" DataKeyField="PK_ProductionID" CellPadding="3" CellSpacing="3" OnEditCommand="gridCommunity_Edit" OnCancelCommand="gridCommunity_Cancel" HorizontalAlign="Center">
                        <AlternatingItemStyle HorizontalAlign="Center"></AlternatingItemStyle>
                        <ItemTemplate>
                            <div class="well well-sm">
                                <asp:Label CssClass="LabelLarge" runat="server" ID="Title" Font-Bold="True" Text='<%# DataBinder.Eval(Container.DataItem, "Title") %>' />
                                <br />
                                <asp:Label runat="server" ID="FirstPerformanceDateTime" Text='<%# DataBinder.Eval(Container.DataItem, "FirstPerformanceDateTime","{0:MM/dd/yy}") %>' />
                                thru
                            <asp:Label runat="server" ID="LastPerformanceDateTime" Text='<%# DataBinder.Eval(Container.DataItem, "LastPerformanceDateTime","{0:MM/dd/yy}") %>' />
                                <br />
                                performed at 
                            <asp:Label CssClass="LabelLarge" runat="server" ID="VenueName" Text='<%# DataBinder.Eval(Container.DataItem, "VenueName") %>' />
                                on 
                            <asp:Label runat="server" ID="Address" Text='<%# DataBinder.Eval(Container.DataItem, "Address") %>' />, 
                            <asp:Label runat="server" ID="City" Text='<%# DataBinder.Eval(Container.DataItem, "City") %>' />
                                <asp:Label runat="server" ID="State" Text='<%# DataBinder.Eval(Container.DataItem, "State") %>' />
                                <br />
                                Produced by 
                            <asp:Label Font-Bold="True" runat="server" ID="Label11" Text='<%# DataBinder.Eval(Container.DataItem, "CompanyName") %>' />
                                <br />
                                <asp:LinkButton CommandName="Edit" runat="server" ID="Linkbutton1">View Details</asp:LinkButton>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="width: 100%; text-align: center">
                                <asp:Label CssClass="LabelLarge" runat="server" ID="Label1" Font-Bold="True" Text='<%# DataBinder.Eval(Container.DataItem, "Title") %>' />
                                <br />
                                <asp:Label runat="server" ID="Label2" Text='<%# DataBinder.Eval(Container.DataItem, "FirstPerformanceDateTime","{0:MM/dd/yy}") %>' />
                                thru 
                                        <asp:Label runat="server" ID="Label3" Text='<%# DataBinder.Eval(Container.DataItem, "LastPerformanceDateTime","{0:MM/dd/yy}") %>' />
                                <br />
                                performed at 
                                        <asp:Label CssClass="LabelMediumBold" runat="server" ID="Label4" Text='<%# DataBinder.Eval(Container.DataItem, "VenueName") %>' />
                                on 
                                        <asp:Label runat="server" ID="Label5" Text='<%# DataBinder.Eval(Container.DataItem, "Address") %>' />,
                                                                    <asp:Label runat="server" ID="Label6" Text='<%# DataBinder.Eval(Container.DataItem, "City") %>' />
                                <asp:Label runat="server" ID="Label7" Text='<%# DataBinder.Eval(Container.DataItem, "State") %>' />
                                <br />
                                <asp:Label runat="server" ID="Label8" Text='<%# DataBinder.Eval(Container.DataItem, "ProductionType") %>' />
                                -
                                                                    <asp:Label runat="server" ID="Label9" Text='<%# DataBinder.Eval(Container.DataItem, "AgeAppropriateName") %>' />
                                <br />
                                Produced by
                                                                    <asp:Label Font-Bold="True" runat="server" ID="Label10" Text='<%# DataBinder.Eval(Container.DataItem, "CompanyName") %>' />
                                <br />
                                <a class="fontDataHyperlink" href='http://<%# DataBinder.Eval(Container.DataItem, "Website") %>'>
                                    <%# DataBinder.Eval(Container.DataItem, "Website") %>
                                </a>
                                <br />
                                <asp:Label runat="server" ID="AllPerformanceDatesTimes" Text='<%# DataBinder.Eval(Container.DataItem, "AllPerformanceDatesTimes") %>' />
                                <br />
                                <asp:Label runat="server" ID="TicketPurchaseDetails" Text='<%# DataBinder.Eval(Container.DataItem, "TicketPurchaseDetails") %>' />                                
                            </div>
                        </EditItemTemplate>
                    </asp:DataList>
                </div>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel runat="server" ID="pnlContactInfo" Visible="False" Width="100%">
        <table style="width: 100%; border-spacing: 4px; border-collapse: separate; text-align:center;">
            <tr>
                <td align="left" colspan="2">
                    <p>
                        To request site access, please send an email to
                        <asp:HyperLink ID="lnkAdminContactEmail" runat="server" CssClass="fontDataDesc" ForeColor="Blue" Target="_blank"></asp:HyperLink>&nbsp;with your:
                    </p>
                    <p>
                        <u>For Adjudicators/Liaisons:</u>
                    </p>
                    <p>
                        Currently only people who have attended NHTA training are granted access to the Adjudication Website.&nbsp; If you attended training but have not yet been emailed your login ID, please email us with the following required information:
                    </p>
                    <p>
                        <ol>
                            <li>
                            Date and Location that you last&nbsp;attended NH Theatre Award Training
                                <li>
                            Full Name
                                    <li>
                            Associated Theatre Company Name
                                        <li>
                            Access Level (Liaison, Adjudicator, or Backup Adjudicator)
                                            <li>
                            Contact Phone Number
                                                <li>Unique Primary Email Address <span class="FontSmaller">(cannot be shared with another Adjudicator or Liaison)</span>
                            <li>Mailing Address</li>
                        </ol>
                        <p>
                        </p>
                    <p>
                        <u>For Theatre Companies:</u>
                    </p>
                    <p>
                        Theatre companies are required to fill out an application form.&nbsp; Please email us at the above email address to request the application form.&nbsp; Expect to supply the following information and more:
                    </p>
                    <ol>
                        <li>
                        Company Name
                                <li>
                        Company Contact person
                                    <li>
                        Company Mailing Address
                                        <li>
                        Company Email Address
                                            <li>
                        Company Website
                                                <li>Year of incorporation</li>
                    </ol>
                    <p>
                        Thank you!
                    </p>
                    <p>
                        &nbsp;
                    </p>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <script>
        function setFocus() {
            //set focus to login ID box
            Login.txtLoginID.focus();
        }
    </script>

</asp:Content>
