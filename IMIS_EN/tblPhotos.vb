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

Partial Public Class tblPhotos
    Public Property PhotoID As Integer
    Public Property PhotoUUID As Guid
    Public Property InsureeID As Nullable(Of Integer)
    Public Property CHFID As String
    Public Property PhotoFolder As String
    Public Property PhotoFileName As String
    Public Property OfficerID As Integer
    Public Property PhotoDate As Date
    Public Property ValidityFrom As Date
    Public Property ValidityTo As Nullable(Of Date)
    Public Property AuditUserID As Nullable(Of Integer)
    Public Property RowID As Byte()

    Public Overridable Property tblInsuree As ICollection(Of tblInsuree) = New HashSet(Of tblInsuree)

End Class
