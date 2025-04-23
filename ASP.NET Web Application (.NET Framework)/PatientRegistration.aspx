<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PatientRegistration.aspx.cs" Inherits="ASP.NET_Web_Application__.NET_Framework_.PatientRegistration" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <h2 id="title"><%: Title %></h2>
        <link href="Content/PatientRegistration.css" rel="stylesheet" type="text/css" />

        <div class="form-container">
            <h2 class="text-left mb-4">Patient Registration</h2>

            <asp:Panel ID="pnlSuccess" runat="server" Visible="false" CssClass="alert alert-success">
                <asp:Literal ID="litSuccess" runat="server"></asp:Literal>
            </asp:Panel>

            <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert alert-danger">
                <asp:Literal ID="litError" runat="server"></asp:Literal>
            </asp:Panel>

            <!-- Registration Form -->
        <div class="form-section mb-4">
            <div class="mb-3">
          <label for="txtName" class="form-label">Full Name</label>
          <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Enter your full name" />
          <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" ErrorMessage="Name is required" Display="Dynamic" CssClass="text-danger" />
     </div>

      <div class="mb-3">
        <label for="txtEmail" class="form-label">Email</label>
        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="example@domain.com" />
        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required" Display="Dynamic" CssClass="text-danger" />
        <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Invalid email format" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic" CssClass="text-danger" />
    </div>

    <div class="mb-3">
        <label for="txtPhone" class="form-label">Phone Number</label>
        <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="e.g. +1234567890" />
        <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ControlToValidate="txtPhone" ErrorMessage="Phone number is required" Display="Dynamic" CssClass="text-danger" />
    </div>
    </div>

    <div class="form-section mb-4">
    <h4 class="mb-2">Notification Preferences</h4>
    <div class="form-check mb-2">
        <asp:CheckBox ID="chkEmail" runat="server" CssClass="form-check-input" />
        <label class="form-check-label" for="chkEmail">Email Notifications</label>
    </div>
    <div class="form-check mb-2">
        <asp:CheckBox ID="chkSms" runat="server" CssClass="form-check-input" />
        <label class="form-check-label" for="chkSms">SMS Notifications</label>
    </div>
    <div class="form-check mb-3">
        <asp:CheckBox ID="chkPush" runat="server" CssClass="form-check-input" />
        <label class="form-check-label" for="chkPush">Push Notifications</label>
    </div>
</div>

<div class="form-section">
    <asp:Button ID="btnSave" runat="server" Text="Register" OnClick="btnSave_Click" CssClass="btn btn-primary btn-lg w-100" />
</div>
            </div>

        <!-- Patients Table -->
        <div class="mt-5">
            <h3 class="mb-3">Registered Patients</h3>
            <asp:GridView ID="gvPatients" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered" 
                OnRowEditing="gvPatients_RowEditing" OnRowCancelingEdit="gvPatients_RowCancelingEdit" OnRowUpdating="gvPatients_RowUpdating"
                OnRowDeleting="gvPatients_RowDeleting" DataKeyNames="Email">
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="Name" ReadOnly="True" />
                    <asp:BoundField DataField="Email" HeaderText="Email" ReadOnly="True" />
                    <asp:BoundField DataField="PhoneNumber" HeaderText="Phone" ReadOnly="True" />
                    <asp:CheckBoxField DataField="EmailNotificationEnabled" HeaderText="Email Sub" />
                    <asp:CheckBoxField DataField="SmsNotificationEnabled" HeaderText="SMS Sub" />
                    <asp:CheckBoxField DataField="PushNotificationEnabled" HeaderText="Push Sub" />
                    <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
                </Columns>
            </asp:GridView>
        </div>
    </main>
</asp:Content>