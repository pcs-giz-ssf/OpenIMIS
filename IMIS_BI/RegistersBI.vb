''Copyright (c) 2016-2017 Swiss Agency for Development and Cooperation (SDC)
''
''The program users must agree to the following terms:
''
''Copyright notices
''This program is free software: you can redistribute it and/or modify it under the terms of the GNU AGPL v3 License as published by the 
''Free Software Foundation, version 3 of the License.
''This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of 
''MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU AGPL v3 License for more details www.gnu.org.
''
''Disclaimer of Warranty
''There is no warranty for the program, to the extent permitted by applicable law; except when otherwise stated in writing the copyright 
''holders and/or other parties provide the program "as is" without warranty of any kind, either expressed or implied, including, but not 
''limited to, the implied warranties of merchantability and fitness for a particular purpose. The entire risk as to the quality and 
''performance of the program is with you. Should the program prove defective, you assume the cost of all necessary servicing, repair or correction.
''
''Limitation of Liability 
''In no event unless required by applicable law or agreed to in writing will any copyright holder, or any other party who modifies and/or 
''conveys the program as permitted above, be liable to you for damages, including any general, special, incidental or consequential damages 
''arising out of the use or inability to use the program (including but not limited to loss of data or data being rendered inaccurate or losses 
''sustained by you or third parties or a failure of the program to operate with any other programs), even if such holder or other party has been 
''advised of the possibility of such damages.
''
''In case of dispute arising out or in relation to the use of the program, it is subject to the public law of Switzerland. The place of jurisdiction is Berne.
'
' 
'

Public Class RegistersBI
    Public Function checkRights(ByVal Right As IMIS_EN.Enums.Rights, ByVal UserID As Integer) As Boolean
        Dim UserRights As New IMIS_BL.UsersBL
        Return UserRights.CheckRights(Right, UserID)
    End Function
    Public Function RunPageSecurity(ByVal PageName As IMIS_EN.Enums.Pages, ByRef PageObj As System.Web.UI.Page) As Boolean
        Dim user As New IMIS_BL.UsersBL
        Return user.RunPageSecurity(PageName, PageObj)
    End Function
    Public Function UploadICD(ByVal FilePath As String, ByVal StratergyId As Integer, ByVal AuditUserID As Integer, ByRef dtResult As DataTable, ByVal dryRun As Boolean, registerName As String, ByRef LogFile As String) As Dictionary(Of String, Integer)
        Dim Locations As New IMIS_BL.LocationsBL
        Dim ICD As New IMIS_BL.ICDCodesBL
        Return ICD.UploadICDXML(FilePath, StratergyId, AuditUserID, dtResult, dryRun, registerName, LogFile)
    End Function

    Public Function UploadLocationsXML(ByVal File As String, ByVal StratergyId As Integer, ByVal AuditUserID As Integer, ByRef dtresult As DataTable, ByVal dryRun As Boolean, registerName As String, ByRef LogFile As String) As Dictionary(Of String, Integer)
        Dim Locations As New IMIS_BL.LocationsBL
        Return Locations.UploadLocationsXML(File, StratergyId, AuditUserID, dtresult, dryRun, registerName, LogFile)
    End Function
    Public Function DownLoadDiagnosis() As String
        Dim ICD As New IMIS_BL.ICDCodesBL
        Return ICD.DownLoadDiagnosisXML()
    End Function
    Public Function GetICDUploadStrategies() As DataTable
        Dim gen As New IMIS_BL.GeneralBL
        Return gen.GetUploadStrategies(True)
    End Function
    Public Function GetLocationUploadStrategies()
        Dim gen As New IMIS_BL.GeneralBL
        Return gen.GetUploadStrategies(False)
    End Function
    Public Function GetUploadStrategiesHF()
        Dim gen As New IMIS_BL.GeneralBL
        Return gen.GetUploadStrategies(False)
    End Function
    Public Function UploadHF(ByVal FilePath As String, ByVal StratergyId As Integer, ByVal AuditUserID As Integer, ByRef dtresult As DataTable, ByVal dryRun As Boolean, registerName As String, ByRef LogFile As String) As Dictionary(Of String, Integer)
        Dim HF As New IMIS_BL.HealthFacilityBL
        Return HF.UploadHF(FilePath, StratergyId, AuditUserID, dtresult, dryRun, registerName, LogFile)
    End Function
    Public Function downLoadHFXML() As String
        Dim BL As New IMIS_BL.HealthFacilityBL
        Return BL.downLoadHFXML
    End Function
    Public Function DownLoadLocationsXML() As String
        Dim BL As New IMIS_BL.LocationsBL
        Return BL.DownLoadLocationsXML
    End Function
End Class
