<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AdjLateConfirm.aspx.vb" Inherits="Adjudication.AdjLateConfirm_Web"%>
<!DOCTYPE html>
<HTML>
	<HEAD>
		<title>REPORT: Adjudicator Late Confirmations</title>
		
		
		
		
		<LINK href="../Styles.css" type="text/css" rel="STYLESHEET" />
	</HEAD>
	<body >
		<form class="container" id="Form1" method="post" runat="server">
			<table id="tblTitle" width="1240" border="0" cellpadding="0" cellspacing="0"  >
				<tr>
					<td colspan="3" class="ReportHeader1240" align="center" height="30">Late 
						Adjudication Confirmations
					</td>
				</tr>
				<tr>
					<td colSpan="3" class="separatorBlack"></td>
				</tr>
				<tr>
					<td colspan="3">
						<asp:datagrid id="gridMain" runat="server"   HorizontalAlign="Left" CellPadding="2"
							AutoGenerateColumns="False" DataKeyField="PK_ScoringID" BorderWidth="1px" AllowSorting="True"
							GridLines="Horizontal" BorderColor="Gainsboro" BorderStyle="None" Width="1240px">
							<AlternatingItemStyle BackColor="White"></AlternatingItemStyle>
							<ItemStyle BackColor="WhiteSmoke"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="Black" BorderColor="Black" BackColor="White"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Title" SortExpression="Title" HeaderText="Production">
									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CompanyName" SortExpression="CompanyName" HeaderText="Producing Company">
									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="FirstPerformanceDateTime" HeaderText="Open Date">
									<HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center" Width="80px"></ItemStyle>
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "FirstPerformanceDateTime","{0:MM/dd/yy}") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="LastPerformanceDateTime" HeaderText="Close Date">
									<HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center" Width="80px"></ItemStyle>
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "LastPerformanceDateTime","{0:MM/dd/yy}") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="ConfirmDueDate" HeaderText="Confirm Due Date">
									<HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center" Width="80px" ForeColor="red"></ItemStyle>
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "ConfirmDueDate","{0:MM/dd/yy}") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="FullName" SortExpression="FullName" HeaderText="Assigned Adjudicator">
									<HeaderStyle HorizontalAlign="left"></HeaderStyle>
									<ItemStyle HorizontalAlign="left"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="EmailPrimary" HeaderText="Email Address">
									<HeaderStyle HorizontalAlign="left"></HeaderStyle>
									<ItemStyle HorizontalAlign="left"></ItemStyle>
									<ItemTemplate>
										<a class=fontDataHyperlink href='mailto:<%# DataBinder.Eval(Container.DataItem, "EmailPrimary") %>'>
											<%# DataBinder.Eval(Container.DataItem, "EmailPrimary") %>
										</a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="PhonePrimary" SortExpression="PhonePrimary" HeaderText="Phone #">
									<HeaderStyle HorizontalAlign="left"></HeaderStyle>
									<ItemStyle HorizontalAlign="left"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid>
					</td>
				</tr>
				<tr>
					<td colSpan="3" class="separatorBlack"></td>
				</tr>
				<tr>
					<td><asp:Label id="lblTotalNumberOfRecords" runat="server" ForeColor="Black">Number of Late Confirmations: 0</asp:Label></td>
					<td></td>
					<td align="right"  >
						<asp:TextBox id="txtSortOrder" runat="server" Visible="False"    
							BorderStyle="Dotted" Width="64px">FullName, FirstPerformanceDateTime</asp:TextBox>
						<asp:TextBox id="txtSortColumnName" runat="server" Visible="False"   BorderStyle="Dotted"
							Width="64px"></asp:TextBox>Logged in as
						<asp:label id="lblLoginID" runat="server"  ></asp:label></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
