<%@ Page Language="vb" MasterPageFile="~/MasterPageNoNav.Master" AutoEventWireup="false" CodeBehind="UpcomingProductions.aspx.vb" Inherits="Adjudication.UpcomingProductions" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <!--[if IE]>
<style>
#WebsitePreview {
    zoom: 0.2;
}
</style>
<![endif]-->
    <style>
        #wrap {
            width: 100%;
            height: 180px;
            padding: 0;
            overflow: hidden;
            text-align:center;
        }

        #frame {
            -ms-zoom: 0.36;
            -ms-transform-origin: 0 0;
            -moz-transform: scale(0.36);
            -moz-transform-origin: 0px 50px;
            -o-transform: scale(0.36);
            -o-transform-origin: 0px 50px;
            -webkit-transform: scale(0.36);
            -webkit-transform-origin: 0 0;
            width: 1000px;
            height: 500px;
            overflow: hidden;
        }
    </style>
    <div class="col-md-12" style="text-align: center;">
        <div class="panel panel-dark">
            <div class="panel-heading">Upcoming Productions for NH Theatre Awards Adjudication</div>
            <div class="panel-body">
                <asp:DataList ID="gridCommunity" runat="server" Width="100%" BorderStyle="none" GridLines="None" BorderWidth="0px" DataKeyField="PK_ProductionID" CellPadding="3" CellSpacing="3" OnEditCommand="gridCommunity_Edit" OnCancelCommand="gridCommunity_Cancel" HorizontalAlign="Center">
                    <AlternatingItemStyle HorizontalAlign="Center"></AlternatingItemStyle>
                    <ItemTemplate>
                        <div class="panel panel-gold">
                            <div class="panel-heading">
                                <h3 class="panel-title">
                                    <asp:Label runat="server" ID="Label1" Font-Bold="True" Text='<%# DataBinder.Eval(Container.DataItem, "Title") %>' /></h3>
                            </div>
                            <div class="panel-body">
                                <div class="col-md-7">
                                    <a style="font-size: large;" href='http://<%# DataBinder.Eval(Container.DataItem, "Website") %>' target="_blank">
                                        <asp:Label runat="server" ID="Title" Font-Bold="True" Text='<%# DataBinder.Eval(Container.DataItem, "Title") %>' />
                                    </a>
                                    <br />
                                    <asp:Label Font-Size="Larger" runat="server" ID="AllPerformanceDatesTimes" Text='<%# DataBinder.Eval(Container.DataItem, "AllPerformanceDatesTimes") %>' /></3>
                                    <br />
                                    Performed at<br />
                                    <asp:Label CssClass="LabelLarge" runat="server" ID="VenueName" Text='<%# DataBinder.Eval(Container.DataItem, "VenueName") %>' />
                                    <br />
                                    on 
                                    <asp:Label runat="server" ID="Address" Text='<%# DataBinder.Eval(Container.DataItem, "Address") %>' />, 
                                    <asp:Label runat="server" ID="City" Text='<%# DataBinder.Eval(Container.DataItem, "City") %>' />
                                    <asp:Label runat="server" ID="State" Text='<%# DataBinder.Eval(Container.DataItem, "State") %>' />
                                    <br />
                                    Produced by
                                <br />
                                    <asp:Label Font-Bold="True" runat="server" ID="Label11" Text='<%# DataBinder.Eval(Container.DataItem, "CompanyName") %>' />
                                    <br />
                                    <a class="fontDataHyperlink" target="_blank" href='http://<%# DataBinder.Eval(Container.DataItem, "Website") %>'>
                                        <%# DataBinder.Eval(Container.DataItem, "Website") %>
                                    </a>
                                </div>
                                <div class="col-md-5">
                                    <div id="wrap">
                                        <iframe id="frame" src="http://<%# DataBinder.Eval(Container.DataItem, "Website") %>"></iframe>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:DataList>
            </div>
        </div>
    </div>
</asp:Content>
