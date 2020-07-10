

Imports System.IO

Partial Public Class SsfRegContributor
    Inherits System.Web.UI.Page
    Private efamily As New IMIS_EN.tblFamilies
    Private eInsuree As New IMIS_EN.tblInsuree
    Private Insuree As New IMIS_BI.InsureeBI

    Private dtImage As New DataTable
    Private imisgen As New IMIS_Gen
    Private userBI As New IMIS_BI.UserBI
    Private Family As New IMIS_BI.FamilyBI
    Private SsfRegContributorUUID As Guid

    Private Sub FormatForm()
        Dim Adjustibility As String = ""

        Adjustibility = General.getControlSetting("ConfirmationNo")

        'CurrentAddress
        Adjustibility = General.getControlSetting("CurrentAddress")
        trCurrentAddress.Visible = Not (Adjustibility = "N")
        rfCurrentAddress.Enabled = (Adjustibility = "M")

        'Current District
        'Adjustibility = General.getControlSetting("CurrentDistrict")
        'trCurrentDistrict.Visible = Not (Adjustibility = "N")
        ''trCurrentRegion.visible = Not (Adjustibility = "N")
        'rfCurrentDistrict.Enabled = (Adjustibility = "M")
        ''rfCurrentRegion.Enabled = (Adjustibility = "M")

        'Current Municipality
        'Adjustibility = General.getControlSetting("CurrentMunicipality")
        'trCurrentMunicipality.Visible = Not (Adjustibility = "N")
        'rfCurrentVDC.Enabled = (Adjustibility = "M")

        'Current Village
        'Adjustibility = General.getControlSetting("CurrentVillage")
        'trCurrentVillage.Visible = Not (Adjustibility = "N")
        'rfCurrentVillage.Enabled = (Adjustibility = "M")



        'Identification Type
        Adjustibility = General.getControlSetting("IdentificationType")
        trIdentificationType.Visible = Not (Adjustibility = "N")
        rfIdType.Enabled = (Adjustibility = "M")

        'Identification Number
        Adjustibility = General.getControlSetting("IdentificationNumber")
        trIdentificationNo.Visible = Not (Adjustibility = "N")
        rfIdNo.Enabled = (Adjustibility = "M")

        'Email
        Adjustibility = General.getControlSetting("InsureeEmail")
        trEmail.Visible = Not (Adjustibility = "N")
        rfEmail.Enabled = (Adjustibility = "M")

        'Marital Status
        'Adjustibility = General.getControlSetting("MaritalStatus")
        'trMaritalStatus.Visible = Not (Adjustibility = "N")
        'L_MARITAL.Visible = Not (Adjustibility = "N")
        'ddlMarital.Visible = Not (Adjustibility = "N")
        'rfMaritalStatus.Enabled = (Adjustibility = "M")

        'Permanent Address
        Adjustibility = General.getControlSetting("PermanentAddress")

        'Poverty
        Adjustibility = General.getControlSetting("Poverty")




    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If HttpContext.Current.Request.QueryString("i") IsNot Nothing Then
            eInsuree.InsureeUUID = Guid.Parse(HttpContext.Current.Request.QueryString("i"))
            eInsuree.InsureeID = Insuree.GetInsureeIdByUUID(eInsuree.InsureeUUID)
        End If

        'If HttpContext.Current.Request.QueryString("f") IsNot Nothing Then
        '    efamily.FamilyUUID = Guid.Parse(HttpContext.Current.Request.QueryString("f"))
        '    efamily.FamilyID = Family.GetFamilyIdByUUID(efamily.FamilyUUID)
        'End If

        lblMsg.Text = ""
        If IsPostBack = True Then Return

        FormatForm()
        RunPageSecurity()

        Try
            ddlGender.DataSource = Insuree.GetGender
            ddlGender.DataValueField = "Code"
            ddlGender.DataTextField = "Gender"
            ddlGender.DataBind()
            'ddlMarital.DataSource = Insuree.GetMaritalStatus
            'ddlMarital.DataValueField = "Code"
            'ddlMarital.DataTextField = "Status"
            'ddlMarital.DataBind()
            'ddlCardIssued.DataSource = Insuree.GetYesNO
            'ddlCardIssued.DataValueField = "Code"
            'ddlCardIssued.DataTextField = "Status"
            'ddlCardIssued.DataBind()

            'ddlProfession.DataSource = Insuree.GetProfession
            'ddlProfession.DataValueField = "ProfessionId"
            'ddlProfession.DataTextField = If(Request.Cookies("CultureInfo").Value = "en", "Profession", "AltLanguage")
            'ddlProfession.DataBind()
            'ddlEducation.DataSource = Insuree.GetEducation
            'ddlEducation.DataValueField = "EducationId"
            'ddlEducation.DataTextField = If(Request.Cookies("CultureInfo").Value = "en", "Education", "AltLanguage")
            'ddlEducation.DataBind()

            ddlIdType.DataSource = Insuree.GetTypeOfIdentity
            ddlIdType.DataValueField = "IdentificationCode"
            ddlIdType.DataTextField = If(Request.Cookies("CultureInfo").Value = "en", "IdentificationTypes", "AltLanguage")
            ddlIdType.DataBind()

            'Dim dtFSPRegion As DataTable = Insuree.GetRegionsAll(imisgen.getUserId(Session("User")), True)
            'ddlFSPRegion.DataSource = dtFSPRegion
            'ddlFSPRegion.DataValueField = "RegionId"
            'ddlFSPRegion.DataTextField = "RegionName"
            'ddlFSPRegion.DataBind()

            'If dtFSPRegion.Rows.Count = 1 Then
            '    'FillFSPDistricts()
            'End If

            'ddlFSPCateogory.DataSource = Insuree.GetHFLevel
            'ddlFSPCateogory.DataValueField = "Code"
            'ddlFSPCateogory.DataTextField = "HFLevel"
            'ddlFSPCateogory.DataBind()

            'FillHF()

            Dim UpdatedFolder As String
            UpdatedFolder = System.Web.Configuration.WebConfigurationManager.AppSettings("UpdatedFolder").ToString()
            'Get Current Region, District, Wards and Villages

            'Dim dtCurrentRegion As DataTable = Insuree.GetRegionsAll(imisgen.getUserId(Session("User")), True)
            'ddlCurrentRegion.DataSource = dtCurrentRegion
            'ddlCurrentRegion.DataValueField = "RegionId"
            'ddlCurrentRegion.DataTextField = "RegionName"
            'ddlCurrentRegion.DataBind()

            'If dtCurrentRegion.Rows.Count = 1 Then
            '    'FillCurrentDistricts()
            'End If

            If Not eInsuree.InsureeID = 0 Then
                Insuree.LoadInsuree(eInsuree)
                txtCHFID.Text = eInsuree.CHFID.Trim
                txtLastName.Text = eInsuree.LastName
                txtOtherNames.Text = eInsuree.OtherNames
                txtBirthDate.Text = eInsuree.DOB
                ddlGender.SelectedValue = eInsuree.Gender

                'ddlMarital.SelectedValue = eInsuree.Marital
                'ddlCardIssued.SelectedValue = If(eInsuree.CardIssued = True, "1", "0")
                txtPassport.Text = eInsuree.passport
                txtPhone.Text = eInsuree.Phone
                Image1.ImageUrl = UpdatedFolder & eInsuree.tblPhotos.PhotoFileName.ToString 'if(eInsuree.tblPhotos.PhotoFileName.ToString <> String.Empty, eInsuree.tblPhotos.PhotoFolder & eInsuree.tblPhotos.PhotoFileName.ToString, "")
                efamily.FamilyID = eInsuree.tblFamilies1.FamilyID


                'ddlProfession.SelectedValue = eInsuree.Profession
                'ddlEducation.SelectedValue = eInsuree.Education
                txtEmail.Text = eInsuree.Email
                If eInsuree.ValidityTo.HasValue Or ((IMIS_Gen.offlineHF Or IMIS_Gen.OfflineCHF) And Not If(eInsuree.isOffline Is Nothing, False, eInsuree.isOffline)) Then
                    Panel2.Enabled = False
                    pnlImages.Enabled = False
                    B_SAVE.Visible = False
                    btnBrowse.Enabled = False

                End If
                ddlIdType.SelectedValue = eInsuree.TypeOfId

                'ddlFSPRegion.SelectedValue = eInsuree.FSPRegion
                ''FillFSPDistricts()
                'ddlFSPDistrict.SelectedValue = eInsuree.FSPDistrict
                'ddlFSPCateogory.SelectedValue = eInsuree.FSPCategory
                ''FillHF()
                'ddlFSP.SelectedValue = eInsuree.tblHF.HfID


                'ddlCurrentRegion.SelectedValue = eInsuree.CurrentRegion
                'FillCurrentDistricts()
                'ddlCurrentDistrict.SelectedValue = eInsuree.CurDistrict

                'getWards()

                'ddlCurentWard.SelectedValue = eInsuree.CurWard
                'getVillages()

                'ddlCurrentVillage.SelectedValue = eInsuree.CurrentVillage


            End If

            'hfFamilyId.Value = efamily.FamilyID
            'Insuree.GetFamilyHeadInfo(efamily)



            'txtHeadPhone.Text = efamily.tblInsuree.Phone

            txtCurrentAddress.Text = eInsuree.CurrentAddress


            ''Hide the ralationship if it is the head of the family
            'L_Relation.Visible = Not eInsuree.IsHead And (Not General.getControlSetting("Relationship") = "N")
            'ddlRelation.Visible = Not eInsuree.IsHead And (Not General.getControlSetting("Relationship") = "N")
            'rfRelation.Enabled = Not eInsuree.IsHead And (General.getControlSetting("Relationship") = "M")
            'ddlRelation.CausesValidation = Not eInsuree.IsHead And (Not General.getControlSetting("Relationship") = "N")


            'FillImageDL()

        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 1)
            Return
        End Try
    End Sub


    'Private Sub FillCurrentDistricts()
    '    Dim dtCurDistrict As DataTable = Insuree.GetDistrictsAll(imisgen.getUserId(Session("User")), Val(ddlCurrentRegion.SelectedValue), True)
    '    ddlCurrentDistrict.DataSource = dtCurDistrict
    '    ddlCurrentDistrict.DataValueField = "DistrictId"
    '    ddlCurrentDistrict.DataTextField = "DistrictName"
    '    ddlCurrentDistrict.DataBind()

    '    If dtCurDistrict.Rows.Count = 1 Then
    '        getWards()
    '    End If

    'End Sub
    'Private Sub FillFSPDistricts()
    '    ddlFSPDistrict.DataSource = Insuree.GetDistrictsAll(imisgen.getUserId(Session("User")), ddlFSPRegion.SelectedValue, True)
    '    ddlFSPDistrict.DataValueField = "DistrictId"
    '    ddlFSPDistrict.DataTextField = "DistrictName"
    '    ddlFSPDistrict.DataBind()
    'End Sub
    'Private Sub getWards()
    '    Dim dtWards As DataTable = Insuree.GetWards(ddlCurrentDistrict.SelectedValue)
    '    Dim wards As Integer = dtWards.Rows.Count
    '    If wards > 0 Then
    '        ddlCurentWard.DataSource = dtWards
    '        ddlCurentWard.DataValueField = "WardId"
    '        ddlCurentWard.DataTextField = "WardName"
    '        ddlCurentWard.DataBind()
    '    Else
    '        ddlCurentWard.Items.Clear()
    '    End If
    '    getVillages(wards)
    'End Sub
    'Private Sub getVillages(Optional ByVal Wards As Integer = 1)
    '    If ddlCurentWard.SelectedIndex < 0 Then Exit Sub
    '    If Wards > 0 And Not ddlCurentWard.SelectedValue = 0 Then
    '        ddlCurrentVillage.DataSource = Insuree.GetVillages(ddlCurentWard.SelectedValue)
    '        ddlCurrentVillage.DataValueField = "VillageId"
    '        ddlCurrentVillage.DataTextField = "VillageName"
    '        ddlCurrentVillage.DataBind()
    '    Else
    '        ddlCurrentVillage.Items.Clear()
    '    End If

    'End Sub


    'Private Sub FillHF()
    '    'If ddlFSPDistrict.SelectedValue = 0 Or ddlFSPCateogory.SelectedValue = "" Then Exit Sub

    '    ddlFSP.DataSource = Insuree.GetFSPHF(Val(ddlFSPDistrict.SelectedValue), ddlFSPCateogory.SelectedValue)
    '    ddlFSP.DataValueField = "HFID"
    '    ddlFSP.DataTextField = "HFCode"
    '    ddlFSP.DataBind()

    'End Sub
    Private Sub RunPageSecurity()
        Dim RoleID As Integer = imisgen.getRoleId(Session("User"))
        Dim UserID As Integer = imisgen.getUserId(Session("User"))
        If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.Insuree, Page) Then
            B_SAVE.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.InsureeEdit, UserID) Or userBI.checkRights(IMIS_EN.Enums.Rights.InsureeAdd, UserID)
            If Not B_SAVE.Visible Then
                pnlImages.Enabled = False
                Panel2.Enabled = False
            End If
        Else
            Dim RefUrl = Request.Headers("Referer")
            Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.Insuree.ToString & "&retUrl=" & RefUrl)
        End If
    End Sub
    'Private Sub FillImageDL()
    '    Dim dt As New DataTable
    '    dt = Insuree.FetchNewImages(Server.MapPath(IMIS_EN.AppConfiguration.SubmittedFolder), eInsuree.CHFID)
    '    dlImages.DataSource = dt
    '    dlImages.DataBind()
    'End Sub
    Private Sub FetchNewImage()
        If Len(Trim(eInsuree.CHFID)) > 0 Then
            dtImage = Insuree.FetchNewImages(Server.MapPath(IMIS_EN.AppConfiguration.SubmittedFolder), eInsuree.CHFID)
            If dtImage.Rows.Count > 0 Then
                Image1.ImageUrl = IMIS_EN.AppConfiguration.SubmittedFolder & dtImage.Rows(0)("ImagePath")
            Else
                Image1.ImageUrl = ""
            End If

        Else
            Image1.ImageUrl = ""

        End If
    End Sub

    Private Sub btnSSFSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSSFSearch.Click

        Dim errMsg As String = ""
        If String.IsNullOrEmpty(txtSSFNumber.Text.Trim) Then
            imisgen.Alert("Please Enter SSF Number !", pnlButtons, alertPopupTitle:="IMIS")
            Return
        End If

        eInsuree.CHFID = txtSSFNumber.Text.Trim
        Try
            If Insuree.InsureeExists(eInsuree) = True Then
                'imisgen.Confirm("This " & imisgen.getMessage("L_CHFID", True) & " is already exist!", pnlButtons, "promptInsureeAdd", "", AcceptButtonText:=imisgen.getMessage("L_YES"), RejectButtonText:=imisgen.getMessage("L_NO"))
                imisgen.Alert("This " & imisgen.getMessage("L_CHFID", True) & " is already exist!", pnlButtons, alertPopupTitle:="IMIS")
                Return

            End If
            'Dim Activate As Boolean = If(hfActivate.Value = 0, False, True)
            'If Not Activate Then
            Insuree.LoadContributor(txtSSFNumber.Text.Trim, errMsg, eInsuree)
                If String.IsNullOrEmpty(errMsg) Then
                    txtCHFID.Text = eInsuree.CHFID
                    txtOtherNames.Text = eInsuree.OtherNames
                    txtLastName.Text = eInsuree.LastName
                    txtBirthDate.Text = eInsuree.DOB.ToString()
                    ddlGender.Text = eInsuree.Gender
                    txtCurrentAddress.Text = eInsuree.CurrentAddress
                    txtEmail.Text = eInsuree.Email
                    ddlIdType.Text = eInsuree.TypeOfId
                    txtPassport.Text = eInsuree.passport
                txtPhone.Text = eInsuree.Phone
                Dim tblPhoto = eInsuree.tblPhotos
                Image1.ImageUrl = "data:image/jpeg;base64," + tblPhoto.PhotoFileName
                HfLocationId.Value = eInsuree.tblFamilies1.LocationId



            Else
                    txtCHFID.Text = String.Empty
                    txtOtherNames.Text = String.Empty
                    txtLastName.Text = String.Empty
                    txtBirthDate.Text = String.Empty
                    ddlGender.Text = String.Empty
                    txtCurrentAddress.Text = String.Empty
                    txtEmail.Text = String.Empty
                    ddlIdType.Text = String.Empty
                    txtPassport.Text = String.Empty
                txtPhone.Text = String.Empty
                HfLocationId.Value = 0
                imisgen.Alert(errMsg, pnlButtons, alertPopupTitle:="IMIS")
                End If
            'End If



        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_SAVE.Click

        If String.IsNullOrEmpty(txtCHFID.Text.Trim) Then
            imisgen.Alert("Please Search SSF Number !", pnlButtons, alertPopupTitle:="IMIS")
            Return
        End If
        eInsuree.CHFID = txtCHFID.Text.Trim

        If Insuree.InsureeExists(eInsuree) = True Then
            imisgen.Alert("This " & imisgen.getMessage("L_CHFID", True) & " is already exist!", pnlButtons, alertPopupTitle:="IMIS")
            Return
        End If

        Try
            Dim ePhotos As New IMIS_EN.tblPhotos

            Dim msg As String = ""

            Dim dt As New DataTable
            dt = DirectCast(Session("User"), DataTable)

            eInsuree.CHFID = txtCHFID.Text.Trim
            eInsuree.LastName = txtLastName.Text
            eInsuree.OtherNames = txtOtherNames.Text
            eInsuree.DOB = Convert.ToDateTime(txtBirthDate.Text)
            'Date.ParseExact(txtBirthDate.Text, "dd/MM/yyyy", Nothing)
            eInsuree.Gender = ddlGender.SelectedValue
            eInsuree.passport = txtPassport.Text
            eInsuree.Phone = txtPhone.Text
            eInsuree.Email = txtEmail.Text
            eInsuree.isOffline = IMIS_Gen.offlineHF Or IMIS_Gen.OfflineCHF
            ePhotos.PhotoID = 0
            ePhotos.InsureeID = eInsuree.InsureeID
            ePhotos.CHFID = eInsuree.CHFID.Trim
            ePhotos.PhotoFolder = IMIS_EN.AppConfiguration.UpdatedFolder

            Dim ImageName As String = Mid(Image1.ImageUrl, Image1.ImageUrl.LastIndexOf("\") + 2, Image1.ImageUrl.Length)

            ePhotos.PhotoFileName = ImageName
            ePhotos.OfficerID = 0
            ePhotos.ValidityFrom = Now
            ePhotos.AuditUserID = dt.Rows(0)("UserID")
            eInsuree.PhotoDate = Now
            ePhotos.PhotoDate = eInsuree.PhotoDate

            'Addition for Nepal >> Start
            If ddlIdType.SelectedIndex > 0 Then eInsuree.TypeOfId = ddlIdType.SelectedValue
            Dim eHF As New IMIS_EN.tblHF


            eInsuree.tblHF = eHF

            eInsuree.CurrentAddress = txtCurrentAddress.Text

            If ImageName.Length > 0 Then
                eInsuree.GeoLocation = Family.ExtractLatitude(ImageName) & " " & Family.ExtractLongitude(ImageName)
            End If
            'Addition for Nepal >> End

            efamily.AuditUserID = dt.Rows(0)("UserID")
            efamily.isOffline = IMIS_Gen.offlineHF Or IMIS_Gen.OfflineCHF
            eInsuree.tblPhotos = ePhotos
            efamily.tblInsuree = eInsuree
            efamily.LocationId = HfLocationId.Value

            'Dim Activate As Boolean = If(hfActivate.Value = 0, False, True)

            Family.SaveFamilySSF(efamily)

            'If Not Activate Then
            '    Family.SaveFamilySSF(efamily)
            'Else
            '    imisgen.Alert("Need to Update!", pnlButtons, alertPopupTitle:="IMIS")
            'End If

            imisgen.Alert("Insuree details were Saved successfully!", pnlButtons, alertPopupTitle:="IMIS")

        Catch ex As Exception
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return

        End Try

    End Sub



    'Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
    '    If B_SAVE.Visible = True Then
    '        If eInsuree.InsureeID > 0 Then
    '            SsfRegContributorUUID = Insuree.GetInsureeUUIDByID(eInsuree.InsureeID)
    '            Response.Redirect("OverviewFamily.aspx?f=" & efamily.FamilyUUID.ToString() & "&i=" & InsureeUUID.ToString())
    '        Else
    '            Response.Redirect("OverviewFamily.aspx?f=" & efamily.FamilyUUID.ToString())
    '        End If
    '    Else
    '        Response.Redirect("FindInsuree.aspx")
    '    End If
    'End Sub
    'Protected Sub dlImages_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dlImages.SelectedIndexChanged
    '    Image1.ImageUrl = IMIS_EN.AppConfiguration.SubmittedFolder & (dlImages.SelectedValue)
    'End Sub
    'Private Sub UpdateImage(ByRef ePhotos As IMIS_EN.tblPhotos)
    '    Insuree.UpdateImage(ePhotos)
    '    Image1.ImageUrl = IMIS_EN.AppConfiguration.UpdatedFolder & Mid(Image1.ImageUrl, Image1.ImageUrl.LastIndexOf("\") + 2, Image1.ImageUrl.Length)
    'End Sub
    'Protected Sub txtCHFID_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtCHFID.TextChanged

    '    eInsuree.CHFID = txtCHFID.Text.Trim
    '    ' If txtCHFID.Text.Length = 9 Then
    '    If Insuree.CheckCHFID(txtCHFID.Text) = True Then
    '        FetchNewImage()
    '        ''FillImageDL()
    '        Return
    '    Else

    '    End If
    '    '  End If
    '    imisgen.Alert(eInsuree.CHFID & imisgen.getMessage("M_NOTVALIDCHFNUMBER"), pnlButtons, alertPopupTitle:="IMIS")
    'End Sub

    'Private Sub ddlFSPDistrict_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlFSPDistrict.SelectedIndexChanged
    '    Try
    '        FillHF()
    '    Catch ex As Exception
    '        imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
    '        EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
    '    End Try
    'End Sub

    'Private Sub ddlFSPCateogory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlFSPCateogory.SelectedIndexChanged
    '    Try
    '        FillHF()
    '    Catch ex As Exception
    '        imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
    '        EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
    '    End Try
    'End Sub

    'Private Sub ddlCurDistrict_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCurrentDistrict.SelectedIndexChanged
    '    Try
    '        'getWards()
    '    Catch ex As Exception
    '        imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
    '        EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
    '    End Try
    'End Sub
    'Private Sub ddlCurVDC_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCurentWard.SelectedIndexChanged
    '    'getVillages()
    'End Sub
    'Private Sub ddlFSPRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlFSPRegion.SelectedIndexChanged
    '    'FillFSPDistricts()
    'End Sub

    'Private Sub ddlCurrentRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCurrentRegion.SelectedIndexChanged
    '    'FillCurrentDistricts()
    'End Sub

    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    'End Sub

    Private Sub UpdateImage(ByRef ePhotos As IMIS_EN.tblPhotos)
        Insuree.UpdateImage(ePhotos)
        Image1.ImageUrl = IMIS_EN.AppConfiguration.UpdatedFolder & Mid(Image1.ImageUrl, Image1.ImageUrl.LastIndexOf("\") + 2, Image1.ImageUrl.Length)
    End Sub

End Class