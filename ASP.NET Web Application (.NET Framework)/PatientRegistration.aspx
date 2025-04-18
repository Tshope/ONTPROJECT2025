<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PatientRegistration.aspx.cs" Inherits="ASP.NET_Web_Application__.NET_Framework_.PatientRegistration" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server"> 
    <main aria-labelledby="title">
        <h2 id="title"><%: Title %></h2>
         <div class="form-container">
            <h2 class="mb-4">Patient Registration</h2>

            <asp:Panel ID="pnlSuccess" runat="server" Visible="false" CssClass="alert alert-success">
                <asp:Literal ID="litSuccess" runat="server"></asp:Literal>
            </asp:Panel>

            <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert alert-danger">
                <asp:Literal ID="litError" runat="server"></asp:Literal>
            </asp:Panel>

            <div class="mb-3">
                <label for="txtName" class="form-label">Full Name</label>
                <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvName" runat="server" 
                    ControlToValidate="txtName" 
                    ErrorMessage="Name is required" 
                    Display="Dynamic"
                    CssClass="text-danger">
                </asp:RequiredFieldValidator>
            </div>    
             
            <div class="mb-3">
                <label for="txtEmail" class="form-label">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                    ControlToValidate="txtEmail" 
                    ErrorMessage="Email is required" 
                    Display="Dynamic"
                    CssClass="text-danger">
                </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revEmail" runat="server" 
                    ControlToValidate="txtEmail"
                    ErrorMessage="Invalid email format"
                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                    Display="Dynamic"
                    CssClass="text-danger">
                </asp:RegularExpressionValidator>
            </div> 

             <div class="mb-3">
                <label for="txtPhone" class="form-label">Phone Number</label>
                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPhone" runat="server" 
                    ControlToValidate="txtPhone" 
                    ErrorMessage="Phone number is required" 
                    Display="Dynamic"
                    CssClass="text-danger">
                </asp:RequiredFieldValidator>
            </div>

            <div class="mb-3">
                <h4>Notification Preferences</h4>
                <div class="form-check">
                    <asp:CheckBox ID="chkEmail" runat="server" CssClass="form-check-input" />
                    <label class="form-check-label" for="chkEmail">Email Notifications</label>
                </div>
                <div class="form-check">
                    <asp:CheckBox ID="chkSms" runat="server" CssClass="form-check-input" />
                    <label class="form-check-label" for="chkSms">SMS Notifications</label>
                </div>
                <div class="form-check">
                    <asp:CheckBox ID="chkPush" runat="server" CssClass="form-check-input" />
                    <label class="form-check-label" for="chkPush">Push Notifications</label>
                </div> 
                <div class="mb-3">
            <asp:Button ID="btnSave" runat="server" Text="Register" Class="btn btn-default" OnClick="btnSave_Click"
                     Style="margin-top: 20px;
                     background-color: dodgerblue;
                     color: white;
                     padding: 10px 20px;
                     border: none;
                     border-radius: 5px;
                     font-size: 16px;
                     cursor: pointer;" />
            </div>
         </div>
         </div>
    </main>
</asp:Content>
