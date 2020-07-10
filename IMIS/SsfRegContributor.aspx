<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/IMIS.Master" CodeBehind="SsfRegContributor.aspx.vb" Inherits="IMIS.SsfRegContributor" Title= '<%$ Resources:Resource,L_SSFCONTRIBUTOR%>' %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="contentHead" ContentPlaceHolderID="head" runat="server" >
    <script type="text/javascript" language="javascript">

    function pageLoad(sender, args) {
        $(document).ready(function () {

            $('#<%=btnBrowse.ClientID %>').click(function(e) {

                $("#SelectPic").show();

                e.preventDefault();
            });



            $("#btnCancel").click(function() {

                $("#SelectPic").hide();

            });


        });
    }

    $(document).ready(function() {
        $("#btnCancel").hide();
        $('#<%=btnBrowse.ClientID %>').click(function(e) {

            $("#SelectPic").show();
            $("#btnCancel").show();
            e.preventDefault();
        });



        $("#btnCancel").click(function() {

            $("#SelectPic").hide();
            $("#btnCancel").hide();
        });


    });
        function msgOkay(btn) {
            if (btn == "ok") {
                $("#<%=hfOK.ClientID%>").val(0);
                $("#<%=B_SAVE.ClientID %>").click();
            }
        }

    function promptInsureeAdd(btn) {
        if (btn === "ok") {
            $("#<%=hfActivate.ClientID%>").val(1);
            //theForm.__EVENTTARGET.value = $("#<%=B_SAVE.ClientID %>");
            //theForm.submit();
            
        } else {
           //var familyId = '<%=HttpContext.Current.Request.QueryString("f") %>';
            //window.location = "OverviewFamily.aspx?f=" + familyId;
            $("#<%=hfActivate.ClientID%>").val(0);
        }
      <%--  $("#<%=hfCheckMaxInsureeCount.ClientID %>").val(0);
        $("#<%=B_SAVE.ClientID %>").click();--%>
    }  
   

        
</script>
<style type="text/css">
        #SelectPic
        {
        	padding-top:10%;
            width: 100%;
        	margin:auto;
         	text-align:center;
            vertical-align:bottom;
        	position:fixed;
	        top:0;
	        left:0;
	        height:100%;
	        z-index:1001;
	        background-color:Gray;
	        opacity:0.9;
	        display:none;
	    }
    </style>
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="Body" Runat="Server">
    <asp:HiddenField ID="hfOK" Value="1" runat="server" />
    <asp:HiddenField ID="hfCheckMaxInsureeCount" Value="1" runat="server" />
    <asp:HiddenField ID="hfActivate" Value="0" runat="server" />
    <asp:HiddenField ID="HfLocationId" Value="0" runat="server" />

<style>
    .contentBox{
        min-height:399px !important;}
</style>
<div class="divBody" style="height:auto !important" >  
     <asp:Panel ID="L_SSF_SEARCH" runat="server" 
             CssClass="panel" 
         GroupingText='<%$ Resources:Resource,L_SSF_SEARCH%>' >
           
              <table class="style15">
                    <tr>
                       
                         <td class="DataEntry" style="width: 215px;">
                            <asp:TextBox ID="txtSSFNumber" runat="server" placeholder="Enter SSF Number"   Width="200px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldtxtSSFNumber" runat="server" ControlToValidate="txtSSFNumber" SetFocusOnError="True" ValidationGroup="check" ForeColor="Red" Display="Dynamic" Text='*'>
                            </asp:RequiredFieldValidator>                 
                        </td>

                        <td>
                          <asp:Button class="button" ID="btnSSFSearch" runat="server"
                          Text='<%$ Resources:Resource,SSF_B_SEARCH %>'></asp:Button>
                  
                        </td>
                        
                    </tr>
                    
                </table>      
                    
                    
         </asp:Panel>
     <asp:Panel ID="Panel2" runat="server" ScrollBars="Auto" CssClass="panelBody" style="height:auto; " GroupingText='<%$ Resources:Resource,L_INSUREE%>'>
        
                 <table width="100%">
                <tr>
                    <td valign="top">
                        <%--<asp:UpdatePanel ID="upCHFID" runat="server">
                            <ContentTemplate>--%>
                                <table width="100%">
                                    <tr>
                                        <td class="FormLabel">
                                            <asp:Label ID="L_CHFID" runat="server" Text="<%$ Resources:Resource,L_CHFID%>">
                                            </asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:UpdatePanel ID="up1" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox  ID="txtCHFID" runat="server" AutoPostBack="True" CssClass="numbersOnly" MaxLength="12" Width="150px" ReadOnly="true"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldCHFID0" runat="server" ControlToValidate="txtCHFID" SetFocusOnError="True" ValidationGroup="check" ForeColor="Red" Display="Dynamic" Text='*'>
                                                    </asp:RequiredFieldValidator>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            
                                        </td>
                                        <td class="FormLabel">&nbsp;</td>
                                        <td class="DataEntry">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="FormLabel">
                                            <asp:Label ID="L_OTHERNAMES0" runat="server" Text="<%$ Resources:Resource,L_OTHERNAMES %>">
                                            </asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:TextBox ID="txtOtherNames" runat="server"   Width="150px" ReadOnly="true"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldOtherNames1" runat="server" ControlToValidate="txtOtherNames" SetFocusOnError="True" ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                            Text='*'>
                                            </asp:RequiredFieldValidator>
                                             
                                        </td>
                                        <td class="FormLabel">&nbsp;</td>
                                        <td class="DataEntry">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="FormLabel">
                                            <asp:Label ID="L_LASTNAME0" runat="server" Text="<%$ Resources:Resource,L_LASTNAME %>">
                                            </asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:TextBox ID="txtLastName" runat="server"   Width="150px" ReadOnly="true"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldLastName2" runat="server" ControlToValidate="txtLastName" SetFocusOnError="True" ValidationGroup="check" ForeColor="Red" Display="Dynamic" Text='*'>
                                            </asp:RequiredFieldValidator>
                                             
                                        </td>
                                        <td class="FormLabel">&nbsp;</td>
                                        <td class="DataEntry">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="FormLabel">
                                            <asp:Label
                                                ID="L_BIRTHDATE"
                                                runat="server"
                                                Text='<%$ Resources:Resource,L_BIRTHDATE %>'>
                                            </asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:TextBox ID="txtBirthDate" runat="server" Width="150px" ReadOnly="true"></asp:TextBox>
                                           <%-- <asp:MaskedEditExtender ID="txtBirthDate_MaskedEditExtender" runat="server"
                                                CultureDateFormat="dd/MM/YYYY"
                                                TargetControlID="txtBirthDate" Mask="99/99/9999" MaskType="Date"
                                                UserDateFormat="DayMonthYear">
                                            </asp:MaskedEditExtender>--%>

                                          <%--  <asp:Button ID="Button1" runat="server" Height="20px" Width="20px" />--%>


                                            <%--<asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtBirthDate" PopupButtonID="Button1" Format="dd/MM/yyyy"></asp:CalendarExtender>--%>
                                            <asp:RequiredFieldValidator ID="RequiredFieldBirthDate0" runat="server" ControlToValidate="txtBirthDate" SetFocusOnError="True" ValidationGroup="check" ForeColor="Red" Display="Dynamic" Text='*'>
                                            </asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="txtBirthDate" ErrorMessage="*" SetFocusOnError="false" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$" ValidationGroup="check" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                                        </td>
                                        <td class="FormLabel">
                                            <br />
                                        </td>
                                        <td class="DataEntry">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="FormLabel">
                                            <asp:Label
                                                ID="L_GENDER"
                                                runat="server"
                                                Text='<%$ Resources:Resource,L_GENDER %>'>
                                            </asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:DropDownList ID="ddlGender" runat="server" Width="150px" Enabled="false" >
                                            </asp:DropDownList>

                                            <asp:RequiredFieldValidator ID="RequiredFieldGender0" runat="server" ControlToValidate="ddlGender" InitialValue="" SetFocusOnError="True" ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                                Text='*'>
                                            </asp:RequiredFieldValidator>

                                        </td>
                                        <td class="FormLabel">
                                            &nbsp;</td>
                                        <td class="DataEntry">
                                            &nbsp;</td>
                                    </tr>
                                
                                    <tr id="trCurrentAddress" runat="server"    >
                                        <td class="FormLabel" style="vertical-align: top">
                                            <asp:Label ID="lblCurrentAddress0" runat="server" Text="<%$ Resources:Resource, L_CURRENTADDRESS %>"></asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:TextBox ID="txtCurrentAddress" runat="server" Height="80px" MaxLength="25" style="resize:none;" TextMode="MultiLine" Width="150px" ReadOnly="true"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfCurrentAddress" runat="server" ControlToValidate="txtCurrentAddress" InitialValue="" SetFocusOnError="True" ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                                Text='*'>
                                            </asp:RequiredFieldValidator>
                                        </td>
                                        <td class="FormLabel">&nbsp;</td>
                                        <td class="DataEntry">&nbsp;</td>
                                    </tr>
                        
                                    <tr>
                                        <td class="FormLabel">
                                            <asp:Label
                                                ID="L_PHONE"
                                                runat="server"
                                                Text='<%$ Resources:Resource,L_PHONE%>'>
                                            </asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:TextBox ID="txtPhone" runat="server" Width="150px" ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td class="FormLabel" style="vertical-align: top">
                                            &nbsp;</td>
                                        <td class="DataEntry">
                                            &nbsp;</td>
                                    </tr>
                                    <tr id="trEmail" runat="server">
                                        <td class="FormLabel">
                                            <asp:Label ID="L_EMAIL" runat="server" Text="<%$ Resources:Resource, L_EMAIL %>"></asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:TextBox ID="txtEmail" runat="server" Width="150px" ReadOnly="true"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfEmail" runat="server" ControlToValidate="txtEmail" SetFocusOnError="True" ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                            Text='*'></asp:RequiredFieldValidator>
<%--                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtEmail" ErrorMessage="Invalid Email Id" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="check">*</asp:RegularExpressionValidator>--%>
                                        </td>
                                        <td class="FormLabel" style="vertical-align: top">&nbsp;</td>
                                        <td class="DataEntry">&nbsp;</td>
                                    </tr>
                                    <tr id="trIdentificationType" runat="server">
                                        <td class="FormLabel">
                                            <asp:Label ID="L_IDTYPE" runat="server" Text="<%$ Resources:Resource, L_IDTYPE %>"></asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:DropDownList ID="ddlIdType" runat="server" Width="150px" Enabled="false">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfIdType" runat="server" ControlToValidate="ddlIdType" InitialValue="" SetFocusOnError="True" ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                            Text='*'></asp:RequiredFieldValidator>
                                        </td>
                                        <td class="FormLabel">
                                            &nbsp;</td>
                                        <td class="DataEntry">
                                            &nbsp;</td>
                                    </tr>
                                    <tr id="trIdentificationNo" runat="server">
                                        <td class="FormLabel">
                                            <asp:Label ID="L_PASSPORT1" runat="server" Text="<%$ Resources:Resource,L_PASSPORT%>">
                                            </asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:TextBox ID="txtPassport" runat="server" MaxLength="12" Width="150px" ReadOnly="true"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfIdNo" runat="server" ControlToValidate="txtPassport" InitialValue="" SetFocusOnError="True" ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                            Text='*'></asp:RequiredFieldValidator>
                                        </td>
                                        <td class="FormLabel">
                                            &nbsp;</td>
                                        <td class="DataEntry">
                                            &nbsp;</td>
                                    </tr>
                                </table>
                            <%--</ContentTemplate>
                        </asp:UpdatePanel>    --%>   
                    </td>
                    <td valign="top" style="width:225px;">
                        <table width="200px">

                            <tr>
                                <td valign="top">
                                <asp:UpdatePanel ID="upImage" runat="server" style="width:200px; height:200px; text-align:center">
                                    <ContentTemplate>
                                        <asp:Image ID="Image1" runat="server"   style="max-height:200px" ImageAlign="Middle" onerror="NoImage(this);" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                    
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="display:none">
                                    <asp:Button runat="server" ID="btnBrowse" Text='<%$ Resources:Resource,B_BROWSE%>' />
                                 
                                    <asp:HiddenField ID="hfFamilyId" runat="server" />
                                 
                                    </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
              
                    
         </asp:Panel>
      </div>
     <asp:Panel ID="pnlButtons" runat="server"  CssClass="panelbuttons" >
                <table width="100%" cellpadding="10 10 10 10">
                 <tr>
                        
                         <td align="left" >
                        
                               <asp:Button 
                            
                            ID="B_SAVE" 
                            runat="server" 
                            Text='<%$ Resources:Resource,B_SAVE%>'
                            ValidationGroup="check1"  />
                        </td>
                        
                        
                        <td  align="right">
                       <asp:Button 
                            
                            ID="B_CANCEL" 
                            runat="server" 
                            Text='<%$ Resources:Resource,B_CANCEL%>'
                              />
                        </td>
                        
                    </tr>
                </table>             
         </asp:Panel>
               <asp:UpdatePanel ID="upDL" runat="server">
                    <ContentTemplate>
                    <table id="SelectPic">
                    <tr>
                        <td align="center">
                        <asp:Panel ID="pnlImages" runat="server" Width="500px" Height="450px" BackColor="White" ScrollBars="Auto">
                     <%--   <asp:DataList ID="dlImages" runat="server" RepeatColumns="4" RepeatDirection="Horizontal" DataKeyField="ImagePath" OnSelectedIndexChanged="dlImages_SelectedIndexChanged">
                            --%>
                            <ItemTemplate>
                                <table width="100px" style="height:100px">
                                <tr>
                                    <td align="center">
                                        
                                        On: <%#Eval("TakenDate")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <img alt="" width="100px" height="100px" src='Images\Submitted\<%#Eval("ImagePath") %>' />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                       <asp:LinkButton ID="lnkSelect" runat="server" CommandName="Select">Select</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            </ItemTemplate>
                            
                        </asp:DataList>
                           
                                                       
                        </asp:Panel>
                
                <br />
                
                            
                 <input type="button" id="btnCancel" value="Cancel" />
                        </td>
                    </tr>
                    
                </table>    
                    </ContentTemplate>
               </asp:UpdatePanel> 
                
                           

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Footer" Runat="Server">
    <asp:ValidationSummary ID="validationSummary" runat="server" HeaderText='<%$ Resources:Resource,V_SUMMARY%>' ValidationGroup="check" />
    <asp:label id="lblMsg" runat="server"></asp:label>
</asp:Content>
