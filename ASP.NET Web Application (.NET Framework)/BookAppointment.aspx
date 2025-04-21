<%@ Page Title="Book Appointment" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BookAppointment.aspx.cs" Inherits="ASP.NET_Web_Application__.NET_Framework_.BookAppointment" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <h2 id="title">Book Appointment</h2>
        <link href="Content/Appointment.css" rel="stylesheet" type="text/css" />
        <h3>Please fill in the details below to schedule your appointment</h3>
        
       <div class="form-group">
    <asp:Label runat="server" Text="Select Patient: " AssociatedControlID="ddlPatient" CssClass="control-label" />
    <asp:DropDownList ID="ddlPatient" runat="server" CssClass="form-control">
        <asp:ListItem Text="-- Select Patient --" Value="" />
    </asp:DropDownList>
    <asp:RequiredFieldValidator ID="rfvPatient" runat="server" ControlToValidate="ddlPatient"
        InitialValue="" ErrorMessage="Please select a patient." CssClass="text-danger" Display="Dynamic" />
</div>

        
        <div class="form-group">
            <asp:Label runat="server" Text="Appointment Date: " AssociatedControlID="calendar" CssClass="control-label" />
            <div class="calendar-container">
                <asp:Calendar ID="calendar" runat="server" CssClass="calendar" BackColor="White" BorderColor="#999999"
                    CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt"
                    ForeColor="Black" Height="180px" Width="100%">
                    <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                    <NextPrevStyle VerticalAlign="Bottom" />
                    <OtherMonthDayStyle ForeColor="#808080" />
                    <SelectedDayStyle BackColor="#007bff" Font-Bold="True" ForeColor="White" />
                    <SelectorStyle BackColor="#CCCCCC" />
                    <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
                    <TodayDayStyle BackColor="#CCCCCC" />
                    <WeekendDayStyle BackColor="#EFEFEF" />
                </asp:Calendar>
            </div>
        </div>
        
        <div class="form-group">
            <asp:Label runat="server" Text="Preferred Time: " AssociatedControlID="ddlTime" CssClass="control-label" />
            <asp:DropDownList ID="ddlTime" runat="server" CssClass="form-control">
                <asp:ListItem Text="-- Select Time --" Value="" />
                <asp:ListItem Text="9:00 AM" Value="9:00" />
                <asp:ListItem Text="10:00 AM" Value="10:00" />
                <asp:ListItem Text="11:00 AM" Value="11:00" />
                <asp:ListItem Text="1:00 PM" Value="13:00" />
                <asp:ListItem Text="2:00 PM" Value="14:00" />
                <asp:ListItem Text="3:00 PM" Value="15:00" />
                <asp:ListItem Text="4:00 PM" Value="16:00" />
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvTime" runat="server" ControlToValidate="ddlTime"
                InitialValue="" ErrorMessage="Please select a time." CssClass="text-danger" Display="Dynamic" />
        </div>
        
        <div class="form-group">
            <asp:Label runat="server" Text="Reason for Visit: " AssociatedControlID="txtReason" CssClass="control-label" />
            <asp:TextBox ID="txtReason" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" 
                placeholder="Please briefly describe your reason for the appointment" />
            <asp:RequiredFieldValidator ID="rfvReason" runat="server" ControlToValidate="txtReason"
                ErrorMessage="Reason is required." CssClass="text-danger" Display="Dynamic" />
        </div>
        
        <div class="form-group">
            <asp:Label runat="server" Text="Select Doctor: " AssociatedControlID="ddlDoctor" CssClass="control-label" />
            <asp:DropDownList ID="ddlDoctor" runat="server" CssClass="form-control">
                <asp:ListItem Text="-- Select Doctor --" Value="" />
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvDoctor" runat="server" ControlToValidate="ddlDoctor"
                InitialValue="" ErrorMessage="Please select a doctor." CssClass="text-danger" Display="Dynamic" />
        </div>
        
        
        
        <div class="form-group">
            <asp:Label runat="server" Text="Appointment Reminders: " CssClass="control-label" />
            <div class="checkbox-group">
                <asp:CheckBox ID="chkNotify24h" runat="server" Text="24 hours before appointment" Checked="true" />
                <asp:CheckBox ID="chkNotify1h" runat="server" Text="1 hour before appointment" />
            </div>
        </div>
       
        
        <div class="form-actions">
            <asp:Button ID="btnSubmit" runat="server" Text="Book Appointment" CssClass="btn btn-primary" OnClick="BtnSubmit_Click" />
            <asp:Button ID="btnClear" runat="server" Text="Clear Form" CssClass="btn btn-secondary" OnClick="BtnClear_Click" CausesValidation="false" />
        </div>
        
       <div class="message-container">
    <asp:Label ID="lblMessage" runat="server" CssClass="appointment-message" EnableViewState="true" />
</div>

    </main>
</asp:Content>