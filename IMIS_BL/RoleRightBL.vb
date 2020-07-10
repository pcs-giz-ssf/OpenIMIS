﻿Public Class RoleRightBL
    Public Function GetRoleRights(RoleID As Integer) As DataSet

        Dim RoleRightDAL As New IMIS_DAL.RoleRightDAL
        Dim BL As New UsersBL
        Dim dsRole As DataSet = RoleRightDAL.GetRoleRights(RoleID)
        If dsRole.Tables.Count > 0 Then
            If dsRole.Tables(0).Rows.Count > 0 Then
                For Each role As DataRow In dsRole.Tables(0).Rows
                    If role("IsSystem") > 0 Then
                        role("RoleName") = BL.ReturnRole(role("IsSystem"))
                    End If
                Next
            End If
        End If
        Return dsRole

    End Function

    Public Function IsRoleNameUnique(ByVal roleName As String) As Boolean
        Dim RoleDAL As New IMIS_DAL.RoleDAL
        Return RoleDAL.IsRoleNameUnique(roleName)
    End Function

    Public Function SaveRights(dtRights As DataTable, eRole As IMIS_EN.tblRole) As Integer
        Dim RoleRightDAL As New IMIS_DAL.RoleRightDAL
        Dim RoleDAL As New IMIS_DAL.RoleDAL
        If eRole.RoleID = 0 Then
            RoleDAL.InsertRole(eRole)
        Else
            Dim eRoleOrg As New IMIS_EN.tblRole
            eRoleOrg.RoleID = eRole.RoleID
            RoleDAL.GetRole(eRoleOrg)
            If isDirty(eRole, eRoleOrg) Then
                RoleDAL.UpdateRole(eRole)
            End If

        End If
        RoleRightDAL.SaveRights(dtRights, eRole)
        Return 0
    End Function
    Private Function isDirty(eRole As IMIS_EN.tblRole, eRoleOrg As IMIS_EN.tblRole) As Boolean
        isDirty = True

        If eRole.RoleID.ToString() <> eRoleOrg.RoleID.ToString() Then Exit Function
        If IIf(eRole.AltLanguage Is Nothing, DBNull.Value, eRole.AltLanguage).ToString() <> IIf(eRoleOrg.AltLanguage Is Nothing, DBNull.Value, eRoleOrg.AltLanguage).ToString() Then Exit Function
        If IIf(eRole.RoleName Is Nothing, DBNull.Value, eRole.RoleName).ToString() <> IIf(eRoleOrg.RoleName Is Nothing, DBNull.Value, eRoleOrg.RoleName).ToString() Then Exit Function
        If IIf(eRole.IsSystem Is Nothing, False, eRole.IsSystem).ToString() <> IIf(eRoleOrg.IsSystem Is Nothing, False, eRoleOrg.IsSystem).ToString() Then Exit Function
        If IIf(eRole.IsBlocked Is Nothing, False, eRole.IsBlocked).ToString() <> IIf(eRoleOrg.IsBlocked Is Nothing, False, eRoleOrg.IsBlocked).ToString() Then Exit Function

        isDirty = False
    End Function
End Class
