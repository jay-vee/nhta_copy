Imports System.Web
Imports System.Web.Security
Imports System.Diagnostics
Imports System.Collections
Imports System
Imports System.Collections.Generic

Imports Adjudication.DataAccess

Public Class UpcomingProductions
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Session.Item("AccessLevel") = 0         ' Reset login access level
        'Session.Item("LoginID") = ""            ' Reset login access level
        Session.Item("PK_UserID") = ""            ' Reset login access level

        '============================================================================================
        'PK_AccessLevelID	AccessLevelName
        '       1	    Administrator
        '       2       Liaison & Adjudicator
        '       3	    Liaison
        '       4	    Primary Adjudicator
        '       5	    Backup Adjudicator
        '============================================================================================
        If Not IsPostBack Then
            Call Get_ProductionInfo()
        End If
    End Sub

    Private Sub Get_ProductionInfo()
        '====================================================================================================
        '=== Get all productions opening in the next 100 days
        '====================================================================================================
        Me.gridCommunity.DataSource = DataAccess.Get_Upcoming_Productions(365)
        Me.gridCommunity.DataBind()
    End Sub

    Sub gridCommunity_Edit(ByVal Sender As Object, ByVal E As DataListCommandEventArgs)
        '====================================================================================================
        gridCommunity.EditItemIndex = CInt(E.Item.ItemIndex)
        Get_ProductionInfo()
    End Sub

    Sub gridCommunity_Cancel(ByVal Sender As Object, ByVal E As DataListCommandEventArgs)
        '====================================================================================================
        gridCommunity.EditItemIndex = -1
        Get_ProductionInfo()
    End Sub

End Class
