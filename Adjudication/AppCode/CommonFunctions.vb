Imports Microsoft.VisualBasic
Imports System
Imports System.Web.UI
Imports System.Text
Imports System.Collections.Generic

Public Class CommonFunctions

    Public Shared Function FormatHyperLink(ByVal Link As String) As String
        If Link.Length > 6 Then
            If Not Link.StartsWith("HTTP://") Then
                Link = "HTTP://" & Link
            End If
            FormatHyperLink = Link
        Else
            FormatHyperLink = ""
        End If

    End Function

    Public Shared Function FormatHyperLinkEmail(ByVal Link As String) As String

        If Link.Length > 6 Then
            If Not Link.StartsWith("MAILTO:") Then
                Link = "MAILTO:" & Link
            End If
            FormatHyperLinkEmail = Link
        Else
            FormatHyperLinkEmail = ""
        End If

    End Function

    Public Shared Function TrimURL(Optional ByVal sLink As String = "", Optional ByVal iLength As Integer = 30) As String
        If sLink.Length Then
            sLink = sLink.Substring(0, IIf(sLink.Length < iLength, sLink.Length, iLength))
            If sLink.Length > (iLength - 1) Then sLink = sLink & "..."
            Return sLink
        Else
            Return ""
        End If

    End Function

    Public Shared Function RemoveHTMLTags(ByVal html As String) As String
        ' Remove HTML tags.
        html = Regex.Replace(html, "<.*?>", "")         'removes anything between the < and > tags, ie: </span>
        html = Regex.Replace(html, "&.*?;", " ")        'removes anything between the & and ; tags, ie: &nbsp;
        Return html
    End Function

    Public Shared Function RemoveInvalidMSWordFormatting(ByVal sMSWordText As String) As String
        'This will: 
        '1. Ignore CR/LF (important in HTML) 
        '2. Match all class attributes 
        '3. Match all '<!--[if *anything* endif]-->' sequences 
        '4. Match all '<![if !*someword*>' and '<![endif]>' instructions 
        '5. Match all '<o:p>anything</o:p>' sequences found at the end of paragraphs 
        '6. Match all <span> and </span> tags 
        '7. Match all font-family and font-size attributes 

        Dim sRegX As String = "(?s)( class=\w+(?=([^<]*>)))|(<!--\[if.*?<!\[endif\]-->)|(<!\[if !\w+\]>)|(<!\[endif\]>)|(<o:p>[^<]*</o:p>)|(<span[^>]*>)|(</span>)|(font-family:[^>]*[;'])|(font-size:[^>]*[;'])(?-s)"

        sMSWordText = Regex.Replace(sMSWordText, sRegX, "")         'removes anything between the < and > tags, ie: </span>
        Return sMSWordText

    End Function


    Public Shared Function AppendLEADINGSpaces(ByVal ItemValue As Object, ByVal NumOfSpaces As Integer) As String
        ' Function receives a string value [ItemValue] and the needed field length
        ' of the string [NumOfSpaces].
        ' Adds leading spaces as specified by value of [NumOfSpaces]
        ' Returns: A string containing the received string with any added leading spaces
        ' If an Error, the function returns the string "ERROR"

        Try
            If ItemValue Is Nothing Then ItemValue = " " ' Check for a Null value passed, set the Item Value to string value "0"

            If Len(ItemValue) < NumOfSpaces Then                'Check if leading zeros are needed
                Do While Len(ItemValue) < NumOfSpaces           ' Check if enough zeros are added
                    ItemValue = " " & ItemValue                 'Add leading zeros to string
                Loop                                            'Loop back to add more zeros (if needed)
            ElseIf Len(ItemValue) > NumOfSpaces Then            ' If the string is longer that the field then
                ItemValue = Mid(ItemValue, 1, NumOfSpaces)      ' >truncate the passed string
            End If

            Return ItemValue

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Shared Function AppendLEADINGZeros(ByVal ItemValue As Object, ByVal NumOfZeros As Integer) As String
        ' Function receives a string value [ItemValue] and the needed field length
        ' of the string [NumOfZeros].
        ' Adds leading spaces as specified by value of [NumOfZeros]
        ' Returns: A string containing the received string with any added leading spaces
        ' If an Error, the function returns the string "ERROR"

        Try
            If ItemValue Is Nothing Then ItemValue = "0" ' Check for a Null value passed, set the Item Value to string value "0"

            If Len(ItemValue) < NumOfZeros Then                'Check if leading zeros are needed
                Do While Len(ItemValue) < NumOfZeros           ' Check if enough zeros are added
                    ItemValue = "0" & ItemValue                 'Add leading zeros to string
                Loop                                            'Loop back to add more zeros (if needed)
            ElseIf Len(ItemValue) > NumOfZeros Then            ' If the string is longer that the field then
                ItemValue = Mid(ItemValue, 1, NumOfZeros)      ' >truncate the passed string
            End If

            Return ItemValue

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Shared Function AppendLEADINGSpacesDECIMAL(ByVal ItemValue As Object, ByVal NumOfSpaces As Integer) As String
        ' Function receives a string value [ItemValue] and the needed field length
        ' of the string [NumOfSpaces].
        ' Adds leading spaces as specified by value of [NumOfSpaces]
        ' Returns: A string containing the received string with any added leading spaces
        ' If an Error, the function returns the string "ERROR"

        Try
            Dim DecimalStart As Integer

            If ItemValue Is Nothing Then ItemValue = "0" ' Check for a Null value passed, set the Item Value to string value "0"

            DecimalStart = InStr(1, ItemValue, ".")             'check for decimal point in number

            If DecimalStart > 0 Then                            'If a decimal is found
                ' Check that 2 characters are after decimal point.  If not, add a 0 to end of String
                If (Len(ItemValue) - DecimalStart) = 1 Then ItemValue = ItemValue & "0"
                ' Make new string length without decimal point, and only 2 decimal places
                ItemValue = Left$(ItemValue, DecimalStart - 1) & Mid$(ItemValue, DecimalStart + 1, 2)
            ElseIf Len(ItemValue) >= 1 Then
                ItemValue = ItemValue & "00"
            End If

            If Len(ItemValue) < NumOfSpaces Then                'Check if leading zeros are needed
                Do While Len(ItemValue) < NumOfSpaces           ' Check if enough zeros are added
                    ItemValue = " " & ItemValue                 'Add leading zeros to string
                Loop                                            'Loop back to add more zeros (if needed)
            ElseIf Len(ItemValue) > NumOfSpaces Then            ' If the string is longer that the field then
                ItemValue = Mid(ItemValue, 1, NumOfSpaces)      ' >truncate the passed string
            End If

            Return ItemValue

        Catch ex As Exception
            Throw ex
        End Try


    End Function

    Public Shared Function AppendTRAILINGSpaces(ByVal ItemValue As Object, ByVal NumOfSpaces As Integer) As String
        ' Function receives a string value [ItemValue] and the needed field length
        ' of the string [NumOfSpaces].
        ' Adds leading spaces as specified by value of [NumOfSpaces]
        ' Returns: A string containing the received string with any added leading spaces
        ' If an Error, the function returns the string "ERROR"

        Try
            If ItemValue Is Nothing Then ItemValue = " " ' Check for a Null value passed, set the Item Value to string value "0"

            If Len(ItemValue) < NumOfSpaces Then                'Check if leading zeros are needed
                Do While Len(ItemValue) < NumOfSpaces           ' Check if enough zeros are added
                    ItemValue = ItemValue & " "                 'Add leading zeros to string
                Loop                                            'Loop back to add more zeros (if needed)
            ElseIf Len(ItemValue) > NumOfSpaces Then            ' If the string is longer that the field then
                ItemValue = Mid(ItemValue, 1, NumOfSpaces)      ' >truncate the passed string
            End If

            Return ItemValue

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Shared Function AppendTRAILINGZeros(ByVal ItemValue As Object, ByVal NumOfSpaces As Integer) As String
        ' Function receives a string value [ItemValue] and the needed field length
        ' of the string [NumOfSpaces].
        ' Adds leading spaces as specified by value of [NumOfSpaces]
        ' Returns: A string containing the received string with any added leading spaces
        ' If an Error, the function returns the string "ERROR"

        Try
            Dim DecimalStart As Integer

            If ItemValue Is Nothing Then ItemValue = "0" ' Check for a Null value passed, set the Item Value to string value "0"

            DecimalStart = InStr(1, ItemValue, ".")             'check for decimal point in number

            If DecimalStart > 0 Then                            'If a decimal is found
                ' Check that 2 characters are after decimal point.  If not, add a 0 to end of String
                If (Len(ItemValue) - DecimalStart) = 1 Then ItemValue = ItemValue & "0"
                ' Make new string length without decimal point, and only 2 decimal places
                ItemValue = Left$(ItemValue, DecimalStart - 1) & Mid$(ItemValue, DecimalStart + 1, 2)
            ElseIf Len(ItemValue) >= 1 Then
                ItemValue = ItemValue & "00"
            End If

            If Len(ItemValue) < NumOfSpaces Then                'Check if leading zeros are needed
                Do While Len(ItemValue) < NumOfSpaces           ' Check if enough zeros are added
                    ItemValue = ItemValue & "0"                 'Add leading zeros to string
                Loop                                            'Loop back to add more zeros (if needed)
            ElseIf Len(ItemValue) > NumOfSpaces Then            ' If the string is longer that the field then
                ItemValue = Mid(ItemValue, 1, NumOfSpaces)      ' >truncate the passed string
            End If

            Return ItemValue

        Catch ex As Exception
            Throw ex
        End Try

    End Function


    Public Shared Function AppendLEADINGZerosDECIMAL(ByVal ItemValue As Object, ByVal NumOfSpaces As Integer) As String
        ' Function receives a string value [ItemValue] and the needed field length
        ' of the string [NumOfSpaces].
        ' Adds leading spaces as specified by value of [NumOfSpaces]
        ' Returns: A string containing the received string with any added leading spaces
        ' If an Error, the function returns the string "ERROR"

        Try
            Dim DecimalStart As Integer

            If ItemValue Is Nothing Then ItemValue = "0" ' Check for a Null value passed, set the Item Value to string value "0"

            DecimalStart = InStr(1, ItemValue, ".")             'check for decimal point in number

            If DecimalStart > 0 Then                            'If a decimal is found
                ' Check that 2 characters are after decimal point.  If not, add a 0 to end of String
                If (Len(ItemValue) - DecimalStart) = 1 Then ItemValue = ItemValue & "0"
                ' Make new string length without decimal point, and only 2 decimal places
                ItemValue = Left$(ItemValue, DecimalStart - 1) & Mid$(ItemValue, DecimalStart + 1, 2)
            ElseIf Len(ItemValue) >= 1 Then
                ItemValue = ItemValue & "00"
            End If

            If Len(ItemValue) < NumOfSpaces Then                'Check if leading zeros are needed
                Do While Len(ItemValue) < NumOfSpaces           ' Check if enough zeros are added
                    ItemValue = "0" & ItemValue                 'Add leading zeros to string
                Loop                                            'Loop back to add more zeros (if needed)
            ElseIf Len(ItemValue) > NumOfSpaces Then            ' If the string is longer that the field then
                ItemValue = Mid(ItemValue, 1, NumOfSpaces)      ' >truncate the passed string
            End If

            Return ItemValue

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Shared Function AppendTRAILINGValue(ByVal ItemValue As Object, ByVal NumOfValues As Integer, ByVal AppendString As String) As String
        ' Function receives a string value [ItemValue] and the needed field length
        ' of the string [NumOfValues].
        ' Adds TRAILING Zeros as specified by value of [NumOfValues]
        ' Returns: A string containing the received string with any added leading spaces
        ' If an Error, the function returns the string "ERROR"

        Try
            If ItemValue Is Nothing Then ItemValue = AppendString ' Check for a Null value passed, set the Item Value to string value "0"

            If Len(ItemValue) < NumOfValues Then                'Check if leading zeros are needed
                Do While Len(ItemValue) < NumOfValues           ' Check if enough zeros are added
                    ItemValue = ItemValue & AppendString        ' Add leading zeros to string
                Loop                                            'Loop back to add more zeros (if needed)
            ElseIf Len(ItemValue) > NumOfValues Then            ' If the string is longer that the field then
                ItemValue = Mid(ItemValue, 1, NumOfValues)      ' >truncate the passed string
            End If

            Return ItemValue

        Catch ex As Exception
            Throw ex
        End Try

    End Function


    '<summary> 
    'A JavaScript alert 
    '</summary> 
    'Public Module Alert

    'Sub New()

    'End Sub

    ''<summary>Shows a client-side JavaScript alert in the browser.</summary> 
    ''<param name="message">The message to appear in the alert.</param> 

    'Public Sub Show(ByVal message As String)

    '    ' Cleans the message to allow single quotation marks 
    '    Dim cleanMessage As String = message.Replace("'", "'")
    '    Dim script As String = "<script type=""text/javascript"">alert('" & cleanMessage & "');</script>"

    '    ' Gets the executing web page 
    '    Dim page As Page = TryCast(HttpContext.Current.CurrentHandler, Page)

    '    ' Checks if the handler is a Page and that the script isn't allready on the Page 
    '    If page IsNot Nothing AndAlso Not page.ClientScript.IsClientScriptBlockRegistered("alert") Then
    '        page.ClientScript.RegisterClientScriptBlock(GetType(Alert), "alert", script)
    '    End If

    'End Sub

    'End Module

    '<summary>Shows a client-side JavaScript alert in the browser.</summary> 
    '<param name="message">The message to appear in the alert.</param> 
    Public Shared Sub ShowAlert(ByVal message As String)
        '' Cleans the message to allow single quotation marks 
        'Dim cleanMessage As String = message.Replace("'", "'")
        'Dim script As String = "<script type=""text/javascript"">alert('" & cleanMessage & "');</script>"

        '' Gets the executing web page 
        'Dim page As Page = TryCast(HttpContext.Current.CurrentHandler, Page)

        '' Checks if the handler is a Page and that the script isn't allready on the Page 
        'If page IsNot Nothing AndAlso Not page.ClientScript.IsClientScriptBlockRegistered("alert") Then
        '    page.ClientScript.RegisterClientScriptBlock(GetType(ShowAlert), "alert", script)
        'End If

    End Sub



End Class
