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

Public Class PremiumDAL
    Dim data As New ExactSQL
    Public Sub LoadPremium(ByRef ePremium As IMIS_EN.tblPremium, ByRef PremiumContribution As Decimal)
        Dim ePolicy As New IMIS_EN.tblPolicy
        Dim ePayer As New IMIS_EN.tblPayer
        Dim eProduct As New IMIS_EN.tblProduct

        Dim data As New ExactSQL
        data.setSQLCommand("select pr.*,( SELECT SUM(Amount) FROM tblPremium WHERE PolicyID=@PolicyID AND tblPremium.ValidityTo IS NULL AND isPhotoFee = 0  ) AS PremiumContribution" & _
                           ",ISNULL(po.PolicyValue,0) PolicyValue,po.PolicyStatus,po.StartDate,po.EffectiveDate,po.InsurancePeriod,po.isOffline as PolicyIsOffline FROM tblPremium pr" & _
                           " INNER JOIN ( SELECT PolicyID,PolicyValue,startDate" & _
                           ",PolicyStatus" & _
                           ",EffectiveDate,pd.InsurancePeriod,isOffline  FROM tblPolicy" & _
                           " INNER JOIN tblProduct pd ON pd.ProdID=tblPolicy.ProdID" & _
                           " WHERE PolicyID=@PolicyID AND  tblPolicy.Validityto is null ) AS po" & _
                           " ON po.PolicyID = pr.PolicyID where PremiumId = @PremiumID and pr.validityto is null", CommandType.Text)
        data.params("@PremiumID", SqlDbType.Int, ePremium.PremiumId)
        data.params("@PolicyID", SqlDbType.Int, ePremium.tblPolicy.PolicyID)

        Dim dr As DataRow = data.Filldata()(0)
        If Not dr Is Nothing Then
            ePolicy.PolicyID = dr("PolicyID")
            ePayer.PayerID = if(dr.IsNull("PayerID"), Nothing, dr("PayerID"))
            ePremium.tblPayer = ePayer
            ePremium.Amount = dr("Amount")
            ePremium.Receipt = dr("Receipt")
            ePremium.PayDate = dr("PayDate")
            ePremium.PayType = dr("PayType")
            eProduct.InsurancePeriod = dr("InsurancePeriod")
            ePolicy.StartDate = dr("StartDate")
            If Not dr("isPhotoFee") Is DBNull.Value Then
                ePremium.isPhotoFee = dr("isPhotoFee")
            End If

            If Not dr("PolicyValue") Is DBNull.Value Then
                ePolicy.PolicyValue = dr("PolicyValue")
            End If
            If Not dr("PolicyStatus") Is DBNull.Value Then
                ePolicy.PolicyStatus = dr("PolicyStatus")
            End If
            If Not dr("EffectiveDate") Is DBNull.Value Then
                ePolicy.EffectiveDate = dr("EffectiveDate")
            End If

            If Not dr("PremiumContribution") Is DBNull.Value Then
                PremiumContribution = dr("PremiumContribution")
            End If
            If dr("isOffline") IsNot DBNull.Value Then
                ePremium.isOffline = dr("isOffline")
            End If
            If dr("PolicyIsOffline") IsNot DBNull.Value Then
                ePolicy.isOffline = dr("PolicyIsOffline")
            End If

            If Not dr("ValidityTo") Is DBNull.Value Then
                ePremium.ValidityTo = dr("ValidityTo")
            End If
        End If


        ePremium.tblPolicy = ePolicy
        ePremium.tblPolicy.tblProduct = eProduct
    End Sub

    'Corrected
    Public Function GetPremiums(ByVal ePremium As IMIS_EN.tblPremium, ByVal legacy As Boolean, dtPayType As DataTable, dtCategory As DataTable) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""

        sSQL = "select PremiumID,PremiumUUID,tblPremium.PolicyID,tblPremium.isOffline,tblFamilies.FamilyId, Amount, PayDate,Receipt, tblPremium.ValidityFrom,"
        sSQL += " tblPremium.ValidityTo,PayerName, PT.Name PayType,"
        sSQL += " C.Name PayCategory, tblPolicy.PolicyUUID, tblFamilies.FamilyUUID"
        sSQL += " FROM tblPremium"
        sSQL += " INNER JOIN tblPolicy ON tblPremium.PolicyID = tblPolicy.PolicyID"
        sSQL += " INNER JOIN tblFamilies ON tblPolicy.FamilyID = tblFamilies.FamilyID"
        sSQL += " INNER JOIN tblVillages V ON V.VillageId = tblFamilies.LocationId"
        sSQL += " INNER JOIN tblWards W ON W.WardId = V.WardId"
        sSQL += " INNER JOIN tblDistricts D ON D.DistrictId = W.DistrictID"
        sSQL += " INNER JOIN tblRegions R ON R.RegionId = D.Region"
        sSQL += " INNER JOIN tblUsersDistricts UD on UD.LocationId = D.districtid and UD.userid = @userid and UD.ValidityTo is null"
        sSQL += " left JOIN tblPayer ON tblPremium.PayerID = tblPayer.PayerID"
        sSQL += " LEFT OUTER JOIN @dtPayType PT ON PT.Code = tblPremium.PayType"
        sSQL += " LEFT OUTER JOIN @dtCategory C ON C.Code = CASE isPhotoFee WHEN 1 THEN 'P' ELSE 'C' END"
        sSQL += " WHERE CASE WHEN @PayerID = 0 THEN 0 ELSE tblPremium.PayerID END = @PayerID"
        sSQL += " AND CASE WHEN  @PayType = '' THEN '' ELSE PayType END = @PayType"
        sSQL += " AND CASE WHEN @Amount = 0 THEN 0 ELSE Amount END >= @Amount"


        If Not ePremium.PayDateFrom Is Nothing Then
            sSQL += " AND PayDate >= @PayDateFrom"
        End If
        If Not ePremium.PayDateTo Is Nothing Then
            sSQL += " AND PayDate <= @PayDateTo"
        End If
        If Not ePremium.tblPayer.tblLocations.RegionId = 0 Then
            sSQL += " AND (R.RegionId =  @RegionId)"
        End If
        If Not ePremium.tblPayer.tblLocations.DistrictID = 0 Then 'District Id
            sSQL += " AND (D.DistrictID =  @DistrictID)"
        End If
        If Not ePremium.Receipt = String.Empty Then
            sSQL += " AND Receipt like @Receipt"
        End If
        If Not legacy = True Then
            sSQL += " AND tblPremium.validityto is null "
        End If
        If ePremium.isOffline IsNot Nothing Then
            If ePremium.isOffline Then
                sSQL += " AND tblPremium.isOffline = 1"
            End If
        End If
        sSQL += " ORDER BY PayDate DESC,tblPremium.PolicyID, tblPremium.ValidityFrom desc, tblPremium.ValidityTo DESC"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("UserID", SqlDbType.Int, ePremium.AuditUserID)
        data.params("PayerID", SqlDbType.Int, ePremium.tblPayer.PayerID)
        data.params("Amount", SqlDbType.Decimal, ePremium.Amount)
        data.params("@Receipt", SqlDbType.NVarChar, 15, ePremium.Receipt & "%")
        If Not ePremium.PayDateFrom Is Nothing Then
            data.params("@PayDateFrom", SqlDbType.SmallDateTime, ePremium.PayDateFrom)
        End If
        If Not ePremium.PayDateTo Is Nothing Then
            data.params("@PayDateTo", SqlDbType.SmallDateTime, ePremium.PayDateTo)
        End If
        data.params("PayType", SqlDbType.NVarChar, 1, ePremium.PayType)
        data.params("@RegionId", SqlDbType.Int, ePremium.tblPayer.tblLocations.RegionId)
        data.params("@DistrictID", SqlDbType.Int, ePremium.tblPayer.tblLocations.DistrictID)
        data.params("@dtPayType", dtPayType, "xCareType")
        data.params("@dtCategory", dtPayType, "xCareType")

        Return data.Filldata
    End Function
    Public Function GetPremiumsByPolicy(ByVal PolicyId As Integer) As DataTable

        Dim data As New ExactSQL
        data.setSQLCommand("SELECT tblPremium.isOffline,PremiumID,PremiumUUID,tblFamilies.FamilyUUID,tblPolicy.policyid,tblPolicy.PolicyUUID,tblPolicy.FamilyId,PayDate,PayerName,Amount,CASE PayType WHEN 'M' THEN 'Mobile Phone' WHEN 'C' THEN 'Cash' WHEN 'B' THEN 'Bank Transfer' END as PayType,Receipt, tblPremium.PayerID,tblPayer.PayerUUID,tblpolicy.FamilyID,CASE tblPremium.isPhotoFee WHEN 1 THEN N'Photo Fee' ELSE N'Contribution' END PayCategory" &
                           " FROM tblPremium LEFT JOIN tblPayer ON tblPremium.PayerID = tblPayer.PayerID inner join tblpolicy on tblpremium.policyid = tblpolicy.policyid inner join tblfamilies on tblpolicy.familyId = tblfamilies.familyId where(tblpremium.PolicyId = @PolicyId  AND tblpremium.ValidityTo is null) ORDER BY PayerName", CommandType.Text)
        data.params("@PolicyId", SqlDbType.Int, PolicyId)
        Return data.Filldata
    End Function
    Public Function InsertPremium(ByRef ePremium As IMIS_EN.tblPremium) As Boolean
        Dim data As New ExactSQL
        Dim sSQL As String = "INSERT INTO tblPremium (PolicyID, PayerID, Amount, Receipt, PayDate, PayType,isOffline, AuditUserID,isPhotoFee)" & _
                    " VALUES (@PolicyID, @PayerID, @Amount, @Receipt, @PayDate, @PayType,@isOffline, @AuditUserID,@isPhotoFee);SET @PremiumID = SCOPE_IDENTITY()"
        data.setSQLCommand(sSQL, CommandType.Text)

        data.params("PolicyID", SqlDbType.Int, ePremium.tblPolicy.PolicyID)
        data.params("PayerID", SqlDbType.Int, if(ePremium.tblPayer.PayerID = 0, DBNull.Value, ePremium.tblPayer.PayerID))
        data.params("Amount", SqlDbType.Decimal, ePremium.Amount)
        data.params("Receipt", SqlDbType.NVarChar, 50, ePremium.Receipt)
        data.params("PayDate", SqlDbType.SmallDateTime, ePremium.PayDate)
        data.params("PayType", SqlDbType.NVarChar, 1, ePremium.PayType)
        data.params("isOffline", SqlDbType.Bit, ePremium.isOffline)
        data.params("AuditUserID", SqlDbType.Int, ePremium.AuditUserID)
        data.params("@PremiumID", SqlDbType.Int, 0, ParameterDirection.Output)
        data.params("@isPhotoFee", SqlDbType.Bit, ePremium.isPhotoFee)

        data.ExecuteCommand()

        ePremium.PremiumId = data.sqlParameters("@PremiumID")

        Return True
    End Function
    Public Function UpdatePremium(ByRef ePremium As IMIS_EN.tblPremium)
        Dim data As New ExactSQL
        Dim str As String = "INSERT INTO tblPremium (PolicyID, PayerID, Amount, Receipt, PayDate, PayType,isOffline, ValidityTo, LegacyID, AuditUserID,isPhotoFee)" _
                   & " SELECT PolicyID, PayerID, Amount, Receipt, PayDate, PayType,isOffline, getDate(), @PremiumID, AuditUserID,isPhotoFee from tblPremium where PremiumID = @PremiumID;" _
                & "UPDATE tblPremium set PolicyID=@PolicyID, PayerID=@PayerID, Amount=@Amount, Receipt=@Receipt, PayDate=@PayDate, PayType=@PayType" _
                & ",isOffline=@isOffline,ValidityFrom=GetDate(), LegacyID = @LegacyID, AuditUserID = @AuditUserID,isPhotoFee = @isPhotoFee where PremiumID=@PremiumID"

        data.setSQLCommand(str, CommandType.Text)

        data.params("PremiumID", SqlDbType.Int, ePremium.PremiumId)
        data.params("PolicyID", SqlDbType.Int, ePremium.tblPolicy.PolicyID)
        ' EMIS: 15223 data.params("PayerID", SqlDbType.Int, ePremium.tblPayer.PayerID)
        data.params("PayerID", SqlDbType.Int, if(ePremium.tblPayer.PayerID = 0, DBNull.Value, ePremium.tblPayer.PayerID))
        data.params("Amount", SqlDbType.Decimal, ePremium.Amount)
        data.params("Receipt", SqlDbType.NVarChar, 50, ePremium.Receipt)
        data.params("PayDate", SqlDbType.SmallDateTime, ePremium.PayDate)
        data.params("PayType", SqlDbType.NVarChar, 1, ePremium.PayType)
        data.params("@isOffline", SqlDbType.Bit, ePremium.isOffline)
        data.params("@LegacyID", SqlDbType.Int, 1, ParameterDirection.Output)
        data.params("AuditUserID", SqlDbType.Int, ePremium.AuditUserID)
        data.params("@isPhotoFee", SqlDbType.Bit, ePremium.isPhotoFee)

        data.ExecuteCommand()
        Return True
    End Function
    Public Function FindPremium(ByVal ePremium As IMIS_EN.tblPremium)
        Dim data As New ExactSQL
        Dim sSQL As String = "SELECT tblFamilies.FamilyID,PayerName,Amount,PayDate,CASE PayType WHEN 'M' THEN 'Mobile Phone' WHEN 'C' THEN 'Cash' WHEN 'B' THEN 'Bank Transfer' END PayType,tblPremium.isOffline,tblPremium.ValidityFrom,tblPremium.ValidityTo" & _
                " FROM tblPremium INNER JOIN tblPolicy ON tblPremium.PolicyID = tblPolicy.PolicyID" & _
                " INNER JOIN tblFamilies ON tblPolicy.FamilyID = tblFamilies.FamilyID" & _
                " INNER JOIN tblPayer ON tblPremium.PayerID = tblPayer.PayerID"

        data.setSQLCommand(sSQL, CommandType.Text)

        Return data.Filldata

    End Function
    Public Function CheckCanBeDeleted(ByVal PremiumID As Integer) As DataTable
        Dim str As String = ""

        data.setSQLCommand(str, CommandType.Text)
        Data.params("@PremiumID", SqlDbType.Int, PremiumID)
        Return Data.Filldata()
    End Function
    Public Function DeletePremium(ByVal epremium As IMIS_EN.tblPremium) As Boolean
        Dim data As New ExactSQL
        Dim str As String = "INSERT INTO tblPremium (PolicyID, PayerID, Amount, Receipt, PayDate, PayType,isOffline, ValidityTo, LegacyID, AuditUserID,isPhotoFee)" _
                   & "SELECT PolicyID, PayerID, Amount, Receipt, PayDate, PayType,isOffline, getDate(), @PremiumID, AuditUserID,isPhotoFee from tblPremium where PremiumID = @PremiumID AND ValidityTo IS NULL;" _
               & "UPDATE tblPremium SET [ValidityFrom] = GetDate(),[ValidityTo] = GetDate(),[AuditUserID] = @AuditUserID where PremiumID=@PremiumID AND ValidityTo IS NULL"

        data.setSQLCommand(str, CommandType.Text)

        data.params("@PremiumID", SqlDbType.Int, epremium.PremiumId)
        data.params("@AuditUserID", SqlDbType.Int, epremium.AuditUserID)
        data.ExecuteCommand()
        Return True
    End Function
    Public Sub GetPremiumContribution(ByRef ePremium As IMIS_EN.tblPremium, ByRef PremiumContribution As Decimal)
        Dim data As New ExactSQL
        Dim ePolicy As New IMIS_EN.tblPolicy
        Dim eProduct As New IMIS_EN.tblProduct

        Dim str As String = "SELECT isnull(pr.Amount,0) Contribution,isnull(po.PolicyValue,0) PolicyValue" & _
                            ",po.PolicyStatus" & _
                            ",po.StartDate,po.EffectiveDate,pd.InsurancePeriod FROM tblPolicy po" & _
                            " INNER JOIN tblProduct pd ON pd.ProdID=po.ProdID" & _
                            " LEFT JOIN (select policyId,Sum(amount) amount from tblpremium where policyid = @PolicyID and Validityto is null AND isPhotoFee = 0 group by policyId) pr ON pr.PolicyID = po.PolicyID WHERE po.PolicyID = @PolicyID AND po.ValidityTo IS NULL "

        data.setSQLCommand(str, CommandType.Text)

        data.params("@PolicyID", SqlDbType.Int, ePremium.tblPolicy.PolicyID)

        Dim dr As DataRow = data.Filldata()(0)
        If Not dr Is Nothing Then
            PremiumContribution = dr("Contribution")
            ePolicy.PolicyValue = dr("PolicyValue")
            eProduct.InsurancePeriod = dr("InsurancePeriod")
            ePolicy.StartDate = dr("StartDate")

            If Not dr("PolicyStatus") Is DBNull.Value Then
                ePolicy.PolicyStatus = dr("PolicyStatus")
            End If
            If Not dr("EffectiveDate") Is DBNull.Value Then
                ePolicy.EffectiveDate = dr("EffectiveDate")
            End If


            ePremium.tblPolicy = ePolicy
            ePremium.tblPolicy.tblProduct = eProduct
        End If
    End Sub
    Public Function GetLastDateForPayment(ByVal PolicyId As Integer) As Date
        Dim sSQL As String = "uspLastDateForPayment"
        data.setSQLCommand(sSQL, CommandType.StoredProcedure)

        data.params("@PolicyId", SqlDbType.Int, PolicyId)

        Return data.Filldata().Rows(0)("LastDate")
    End Function
    Public Function GetInstallmentsInfo(ByVal PolicyId As Integer) As DataTable
        Dim sSQL As String = ""
        sSQL = "SELECT TotalInstallments,MaxInstallments FROM (SELECT COUNT(PremiumId)TotalInstallments FROM tblPremium WHERE PolicyID = @PolicyID and ValidityTo is null AND isPhotoFee = 0)TotalInstallments, (SELECT MaxInstallments FROM tblProduct Prod INNER JOIN tblPolicy PL ON Prod.ProdID = PL.ProdID where Prod.ValidityTo IS NULL AND PL.ValidityTo IS NULL AND PL.PolicyID = @PolicyID)MaxInstallments"

        data.setSQLCommand(sSQL, CommandType.Text)

        data.params("@PolicyId", SqlDbType.Int, PolicyId)

        Return data.Filldata

    End Function
    Public Function isUniqueReceipt(ByVal ePremium As IMIS_EN.tblPremium) As Boolean
        Dim data As New ExactSQL
        Dim dt As New DataTable

        Dim sSQL As String = "SELECT 1" & _
                " FROM tblPremium PR INNER JOIN tblPolicy PL ON PR.PolicyID = PL.PolicyID" & _
                " INNER JOIN tblProduct Prod ON PL.ProdID = Prod.ProdId" & _
                " WHERE PR.ValidityTo Is NULL" & _
                " AND PR.Receipt = @Receipt" & _
                " AND PR.PremiumId <> @PremiumId" & _
                " AND Prod.ProdID = (SELECT ProdId FROM tblPolicy WHERE PolicyId = @PolicyID)"

        data.setSQLCommand(sSQL, CommandType.Text)

        data.params("PremiumID", SqlDbType.Int, ePremium.PremiumId)
        data.params("PolicyID", SqlDbType.Int, ePremium.tblPolicy.PolicyID)
        data.params("Receipt", SqlDbType.NVarChar, 50, ePremium.Receipt)

        dt = data.Filldata

        If dt.Rows.Count > 0 Then
            Return False
        Else
            Return True
        End If

    End Function
    Public Function AddFund(ePremium As IMIS_EN.tblPremium, ProdId As Integer) As Integer
        Dim sSQL As String = "uspAddFund"
        data.setSQLCommand(sSQL, CommandType.StoredProcedure)

        data.params("@ProductId", SqlDbType.Int, ProdId)
        data.params("@PayerId", SqlDbType.Int, If(ePremium.tblPayer.PayerID = 0, DBNull.Value, ePremium.tblPayer.PayerID))
        data.params("@PayDate", SqlDbType.Date, ePremium.PayDate)
        data.params("@Amount", SqlDbType.Decimal, ePremium.Amount)
        data.params("@Receipt", SqlDbType.NVarChar, 50, ePremium.Receipt)
        data.params("@AuditUserId", SqlDbType.Int, ePremium.AuditUserID)
        data.params("@isOffline", SqlDbType.Bit, ePremium.isOffline)
        data.params("@Return", SqlDbType.Int, 0, ParameterDirection.ReturnValue)

        data.ExecuteCommand()

        Return data.sqlParameters("@Return")

    End Function

    Public Function GetPremiumIdByUUID(ByVal uuid As Guid) As DataTable
        Dim sSQL As String = ""
        Dim data As New ExactSQL

        sSQL = "select PremiumID from tblPremium where PremiumUUID = @PremiumUUID"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@PremiumUUID", SqlDbType.UniqueIdentifier, uuid)

        Return data.Filldata
    End Function
    Public Function GetPremiumUUIDByID(ByVal id As Integer) As DataTable
        Dim sSQL As String = ""
        Dim data As New ExactSQL

        sSQL = "select PremiumUUID from tblPremium where PremiumID = @PremiumID"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@PremiumID", SqlDbType.Int, id)

        Return data.Filldata
    End Function
End Class

