'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated from a template.
'
'     Manual changes to this file may cause unexpected behavior in your application.
'     Manual changes to this file will be overwritten if the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Collections.Generic

Partial Public Class tblFamilies
    Public Property FamilyID As Integer
    Public Property FamilyUUID As Guid
    Public Property InsureeID As Integer
    Public Property LocationId As Integer
    Public Property Poverty As Nullable(Of Boolean)
    Public Property ValidityFrom As Date
    Public Property ValidityTo As Nullable(Of Date)
    Public Property LegacyID As Nullable(Of Integer)
    Public Property AuditUserID As Integer
    Public Property RowID As Byte()
    Public Property FamilyType As String
    Public Property FamilyAddress As String
    Public Property isOffline As Nullable(Of Boolean)
    Public Property Ethnicity As String
    Public Property ConfirmationNo As String
    Public Property ConfirmationType As String

    Public Overridable Property tblConfirmationTypes As tblConfirmationTypes
    Public Overridable Property tblInsuree As tblInsuree
    Public Overridable Property tblLocations As tblLocations
    Public Overridable Property tblFamilyTypes As tblFamilyTypes
    Public Overridable Property tblInsuree1 As ICollection(Of tblInsuree) = New HashSet(Of tblInsuree)
    Public Overridable Property tblPolicy As ICollection(Of tblPolicy) = New HashSet(Of tblPolicy)

End Class