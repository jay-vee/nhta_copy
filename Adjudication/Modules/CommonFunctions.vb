Imports Microsoft.VisualBasic
Imports System
Imports System.Web.UI
Imports System.Text

Module CommonFunctionsOLD

    Public Function IsState(StateText As String) As String
        Try
            StateText = StateText.Trim
            Dim StateAbbr As String = GetStateName(StateText)

            If StateAbbr.Length > 0 Then                                                                    'if State name returned, then this is a valid state
                Return StateText.ToUpper
            Else
                Dim StateName As String = GetStateAbbreviation(StateText)                                   'if state Abbreviation returned, then this is a valid state
                If StateName.Length > 0 Then
                    Return StateName.ToUpper
                Else
                    StateText = String.Empty
                End If
            End If

        Catch ex As Exception
            Throw ex                    'StateText = String.Empty
        End Try

        Return StateText

    End Function

    Public Function DateToShortTimeString(DateString As Object) As String
        If IsDBNull(DateString) Then
            Return ""
        Else
            Try
                Dim returnDate As String = CDate(DateString.ToString).ToShortTimeString
                Return returnDate
            Catch ex As Exception
                Return "Invalid Time"
            End Try
        End If
    End Function

    Public Function DateToShortDateString(DateString As Object) As String
        If IsDBNull(DateString) Then
            Return ""
        Else
            Try
                Dim returnDate As String = CDate(DateString.ToString).ToShortDateString
                Return returnDate
            Catch ex As Exception
                Return "Invalid Date"
            End Try
        End If
    End Function

    Public Function ParseMilitaryTime(ByVal time As String, ByVal year As Integer, ByVal month As Integer, ByVal day As Integer) As DateTime
        ' Convert hour part of string to integer.
        Dim TimeParts() As String = time.Split(New Char() {":"c})
        Dim hour As String = TimeParts(0)
        Dim hourInt As Integer = Integer.Parse(hour)
        If hourInt >= 24 Then
            Throw New ArgumentOutOfRangeException("Invalid hour")
        End If

        ' Convert minute part of string to integer.
        Dim minute As String
        If TimeParts.Count > 1 Then
            minute = TimeParts(1)
        Else
            minute = "00"
        End If

        Dim minuteInt As Integer = Integer.Parse(minute)
        If minuteInt >= 60 Then
            Throw New ArgumentOutOfRangeException("Invalid minute")
        End If

        ' Return the DateTime.
        Return New DateTime(year, month, day, hourInt, minuteInt, 0)

    End Function

    Public Function FormatHyperLink(ByVal Link As String) As String
        If Link.Length > 6 Then
            If Not Link.StartsWith("HTTP://") Then
                Link = "HTTP://" & Link
            End If
            FormatHyperLink = Link
        Else
            FormatHyperLink = ""
        End If
    End Function

    Public Function FormatHyperLinkEmail(ByVal Link As String) As String

        If Link.Length > 6 Then
            If Not Link.StartsWith("MAILTO:") Then
                Link = "MAILTO:" & Link
            End If
            FormatHyperLinkEmail = Link
        Else
            FormatHyperLinkEmail = ""
        End If
    End Function

    Public Function TrimURL(Optional ByVal sLink As String = "", Optional ByVal iLength As Integer = 30) As String
        If sLink.Length Then
            sLink = sLink.Substring(0, IIf(sLink.Length < iLength, sLink.Length, iLength))
            If sLink.Length > (iLength - 1) Then sLink = sLink & "..."
            Return sLink
        Else
            Return ""
        End If

    End Function

    Public Function RemoveCRFL(ByVal html As String) As String
        ' Remove HTML tags.
        html = Regex.Replace(html, "\r\n", " ")
        html = Regex.Replace(html, "\r", " ")
        html = Regex.Replace(html, "\n", " ")
        Return html.Trim()
    End Function

    Public Function RemoveHTMLTags(ByVal html As String) As String
        ' Remove HTML tags.
        html = Regex.Replace(html, "&#39;", "")         'replaces HTML (') with no space
        html = Regex.Replace(html, "<.*?>", "")         'removes anything between the < and > tags, ie: </font>
        html = Regex.Replace(html, "&.*?;", " ")        'removes anything between the & and ; tags, ie: &nbsp;
        Return html.Trim()
    End Function

    Public Function RemoveInvalidMSWordFormatting(ByVal sMSWordText As String) As String
        'This will:
        '1. Ignore CR/LF (important in HTML)
        '2. Match all class attributes
        '3. Match all '<!--[if *anything* endif]-->' sequences
        '4. Match all '<![if !*someword*>' and '<![endif]>' instructions
        '5. Match all '<o:p>anything</o:p>' sequences found at the end of paragraphs
        '6. Match all <span> and </span> tags
        '7. Match all font-family and font-size attributes

        Dim sRegX As String = "(?s)( class=\w+(?=([^<]*>)))|(<!--\[if.*?<!\[endif\]-->)|(<!\[if !\w+\]>)|(<!\[endif\]>)|(<o:p>[^<]*</o:p>)|(<span[^>]*>)|(</span>)|(font-family:[^>]*[;'])|(font-size:[^>]*[;'])(?-s)"

        sMSWordText = Regex.Replace(sMSWordText, sRegX, "")         'removes anything between the < and > tags, ie: </font>
        Return sMSWordText

    End Function

    Public Function RemoveInvalidSQLCharacters(ByVal sReviewText As String) As String
        '====================================================================================================
        ' Function is mainly used to stop SQL Injection attacks
        ' Removes the Following Character sets from the passed text
        '      '
        '      /*
        '      */
        '      --
        ' Also added code to remove astericks * used as wildcards in searches.
        ' Code will ignore an asterick anywhere in string.
        '====================================================================================================
        Dim iCount As Int16, sReturnText As String = String.Empty

        For iCount = 0 To sReviewText.Length - 1
            Select Case sReviewText.Substring(iCount, 1)
                Case "'"
                    ' Exclude this character
                Case "*"
                    If Right(sReviewText, 1) <> "*" Then
                        If sReviewText.Substring(iCount + 1, 1) = "/" Then
                            iCount = iCount + 1     'Skip this character  and the next char: /
                        ElseIf sReviewText.Substring(iCount, 1) = "*" Then
                            iCount = iCount     'Skip this character: *
                        Else
                            sReturnText = sReturnText & sReviewText.Substring(iCount, 1)
                        End If
                    Else
                        If sReviewText.Substring(iCount, 1) = "*" Then
                            iCount = iCount    'Skip this character: *
                        Else
                            sReturnText = sReturnText & sReviewText.Substring(iCount, 1)
                        End If
                    End If
                Case "/"
                    If sReviewText.Substring(iCount + 1, 1) = "*" Then
                        iCount = iCount + 1     'Skip this character and the next char: *
                    Else
                        sReturnText = sReturnText & sReviewText.Substring(iCount, 1)
                    End If
                Case "-"
                    If Right(sReviewText, 1) <> "-" Then
                        If sReviewText.Substring(iCount + 1, 1) = "-" Then
                            iCount = iCount + 1     'Skip this character  and the next char: -
                        Else
                            sReturnText = sReturnText & sReviewText.Substring(iCount, 1)
                        End If
                    End If
                Case " "
                    If sReviewText.Substring(iCount + 1, 1) = " " Then
                        iCount = iCount + 1     'Skip this character  and the next char: ' '
                    Else
                        sReturnText = sReturnText & sReviewText.Substring(iCount, 1)
                    End If
                Case Else
                    sReturnText = sReturnText & sReviewText.Substring(iCount, 1)
            End Select
        Next

        RemoveInvalidSQLCharacters = sReturnText

    End Function

    Public Function AppendLEADINGSpaces(ByVal ItemValue As Object, ByVal NumOfSpaces As Integer) As String
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

    Public Function AppendLEADINGZeros(ByVal ItemValue As Object, ByVal NumOfZeros As Integer) As String
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

    Public Function AppendLEADINGSpacesDECIMAL(ByVal ItemValue As Object, ByVal NumOfSpaces As Integer) As String
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

    Public Function AppendTRAILINGSpaces(ByVal ItemValue As Object, ByVal NumOfSpaces As Integer) As String
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

    Public Function AppendTRAILINGZeros(ByVal ItemValue As Object, ByVal NumOfSpaces As Integer) As String
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

    Public Function AppendLEADINGZerosDECIMAL(ByVal ItemValue As Object, ByVal NumOfSpaces As Integer) As String
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

    Public Function AppendTRAILINGValue(ByVal ItemValue As Object, ByVal NumOfValues As Integer, ByVal AppendString As String) As String
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

    Public Function GetStateName(StateAbbreviation As String) As String
        Select Case StateAbbreviation.ToUpper
            Case "AL"
                Return "Alabama"
            Case "AK"
                Return "Alaska"
            Case "AZ"
                Return "Arizona"
            Case "AR"
                Return "Arkansas"
            Case "CA"
                Return "California"
            Case "CO"
                Return "Colorado"
            Case "CT"
                Return "Connecticut"
            Case "DE"
                Return "Delaware"
            Case "FL"
                Return "Florida"
            Case "GA"
                Return "Georgia"
            Case "HI"
                Return "Hawaii"
            Case "ID"
                Return "Idaho"
            Case "IL"
                Return "Illinois"
            Case "IN"
                Return "Indiana"
            Case "IA"
                Return "Iowa"
            Case "KS"
                Return "Kansas"
            Case "KY"
                Return "Kentucky"
            Case "LA"
                Return "Louisiana"
            Case "ME"
                Return "Maine"
            Case "MD"
                Return "Maryland"
            Case "MA"
                Return "Massachusetts"
            Case "MI"
                Return "Michigan"
            Case "MN"
                Return "Minnesota"
            Case "MS"
                Return "Mississippi"
            Case "MO"
                Return "Missouri"
            Case "MT"
                Return "Montana"
            Case "NE"
                Return "Nebraska"
            Case "NV"
                Return "Nevada"
            Case "NH"
                Return "New Hampshire"
            Case "NJ"
                Return "New Jersey"
            Case "NM"
                Return "New Mexico"
            Case "NY"
                Return "New York"
            Case "NC"
                Return "North Carolina"
            Case "ND"
                Return "North Dakota"
            Case "OH"
                Return "Ohio"
            Case "OK"
                Return "Oklahoma"
            Case "OR"
                Return "Oregon"
            Case "PA"
                Return "Pennsylvania"
            Case "RI"
                Return "Rhode Island"
            Case "SC"
                Return "South Carolina"
            Case "SD"
                Return "South Dakota"
            Case "TN"
                Return "Tennessee"
            Case "TX"
                Return "Texas"
            Case "UT"
                Return "Utah"
            Case "VT"
                Return "Vermont"
            Case "VA"
                Return "Virginia"
            Case "WA"
                Return "Washington"
            Case "WV"
                Return "West Virginia"
            Case "WI"
                Return "Wisconsin"
            Case "WY"
                Return "Wyoming"
            Case Else
                Return String.Empty
        End Select

    End Function

    Public Function GetStateAbbreviation(StateName As String) As String
        Select Case StateName.ToUpper
            Case "Alabama".ToUpper
                Return "AL"
            Case "Alaska".ToUpper
                Return "AK"
            Case "Arizona".ToUpper
                Return "AZ"
            Case "Arkansas".ToUpper
                Return "AR"
            Case "California".ToUpper
                Return "CA"
            Case "Colorado".ToUpper
                Return "CO"
            Case "Connecticut".ToUpper
                Return "CT"
            Case "Delaware".ToUpper
                Return "DE"
            Case "Florida".ToUpper
                Return "FL"
            Case "Georgia".ToUpper
                Return "GA"
            Case "Hawaii".ToUpper
                Return "HI"
            Case "Idaho".ToUpper
                Return "ID"
            Case "Illinois".ToUpper
                Return "IL"
            Case "Indiana".ToUpper
                Return "IN"
            Case "Iowa".ToUpper
                Return "IA"
            Case "Kansas".ToUpper
                Return "KS"
            Case "Kentucky".ToUpper
                Return "KY"
            Case "Louisiana".ToUpper
                Return "LA"
            Case "Maine".ToUpper
                Return "ME"
            Case "Maryland".ToUpper
                Return "MD"
            Case "Massachusetts".ToUpper
                Return "MA"
            Case "Michigan".ToUpper
                Return "MI"
            Case "Minnesota".ToUpper
                Return "MN"
            Case "Mississippi".ToUpper
                Return "MS"
            Case "Missouri".ToUpper
                Return "MO"
            Case "Montana".ToUpper
                Return "MT"
            Case "Nebraska".ToUpper
                Return "NE"
            Case "Nevada".ToUpper
                Return "NV"
            Case "New Hampshire".ToUpper
                Return "NH"
            Case "New Jersey".ToUpper
                Return "NJ"
            Case "New Mexico".ToUpper
                Return "NM"
            Case "New York".ToUpper
                Return "NY"
            Case "North Carolina".ToUpper
                Return "NC"
            Case "North Dakota".ToUpper
                Return "ND"
            Case "Ohio".ToUpper
                Return "OH"
            Case "Oklahoma".ToUpper
                Return "OK"
            Case "Oregon".ToUpper
                Return "OR"
            Case "Pennsylvania".ToUpper
                Return "PA"
            Case "Rhode Island".ToUpper
                Return "RI"
            Case "South Carolina".ToUpper
                Return "SC"
            Case "South Dakota".ToUpper
                Return "SD"
            Case "Tennessee".ToUpper
                Return "TN"
            Case "Texas".ToUpper
                Return "TX"
            Case "Utah".ToUpper
                Return "UT"
            Case "Vermont".ToUpper
                Return "VT"
            Case "Virginia".ToUpper
                Return "VA"
            Case "Washington".ToUpper
                Return "WA"
            Case "West Virginia".ToUpper
                Return "WV"
            Case "Wisconsin".ToUpper
                Return "WI"
            Case "Wyoming".ToUpper
                Return "WY"
            Case Else
                Return String.Empty
        End Select

    End Function

End Module