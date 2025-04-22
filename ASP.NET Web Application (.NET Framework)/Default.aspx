<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ASP.NET_Web_Application__.NET_Framework_._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
          <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css">
 <style type="text/css">
     /* Global Styles */
     :root {
         --primary: #3a86ff;
         --primary-light: #e9f5ff;
         --secondary: #4cc9f0;
         --dark: #2b2d42;
         --light: #f8f9fa;
         --success: #4caf50;
         --warning: #ff9800;
         --danger: #f44336;
     }
     
     * {
         margin: 0;
         padding: 0;
         box-sizing: border-box;
         font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
     }
     
     body {
         line-height: 1.6;
         color: #333;
         background-color: #fff;
     }
     
     .container {
         width: 100%;
         max-width: 1200px;
         margin: 0 auto;
         padding: 0 20px;
     }
     
     section {
         padding: 60px 0;
     }
     
     .section-title {
         text-align: center;
         margin-bottom: 50px;
     }
     
         .section-title h2 {
             font-size: 2.5rem;
             color: var(--dark);
             margin-bottom: 15px;
         }
     
         .section-title p {
             font-size: 1.1rem;
             color: #666;
             max-width: 700px;
             margin: 0 auto;
         }
     
     .btn {
         display: inline-block;
         padding: 12px 30px;
         background-color: var(--primary);
         color: white;
         border: none;
         border-radius: 5px;
         text-decoration: none;
         font-weight: 600;
         transition: all 0.3s ease;
         cursor: pointer;
     }
     
         .btn:hover {
             background-color: #2a75e6;
             transform: translateY(-2px);
             box-shadow: 0 5px 15px rgba(58, 134, 255, 0.3);
         }
     
     .btn-outline {
         background-color: transparent;
         border: 2px solid var(--primary);
         color: var(--primary);
     }
     
         .btn-outline:hover {
             background-color: var(--primary);
             color: white;
         }
     
     /* Header Styles */
     header {
         background-color: white;
         box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
         position: fixed;
         width: 100%;
         z-index: 1000;
     }
     
     nav {
         display: flex;
         justify-content: space-between;
         align-items: center;
         padding: 20px 0;
     }
     
     .logo {
         font-size: 1.8rem;
         font-weight: 700;
         color: var(--primary);
         text-decoration: none;
     }
     
     .nav-links {
         display: flex;
         list-style: none;
     }
     
         .nav-links li {
             margin-left: 30px;
         }
     
         .nav-links a {
             text-decoration: none;
             color: var(--dark);
             font-weight: 500;
             transition: color 0.3s ease;
         }
     
             .nav-links a:hover {
                 color: var(--primary);
             }
     
     .mobile-menu-btn {
         display: none;
         background: none;
         border: none;
         font-size: 1.5rem;
         cursor: pointer;
         color: var(--dark);
     }
     
     /* Hero Section */
     .hero {
         background: linear-gradient(135deg, #f5f7fa 0%, #e9f5ff 100%);
         padding: 180px 0 100px;
         text-align: center;
     }
     
         .hero h1 {
             font-size: 3rem;
             margin-bottom: 20px;
             color: var(--dark);
         }
     
         .hero p {
             font-size: 1.2rem;
             color: #666;
             max-width: 700px;
             margin: 0 auto 40px;
         }
     
     .hero-btns {
         display: flex;
         justify-content: center;
         gap: 20px;
     }
     
     .hero-image {
         max-width: 100%;
         height: auto;
         margin-top: 50px;
         border-radius: 10px;
         box-shadow: 0 20px 40px rgba(0, 0, 0, 0.1);
     }
     
     /* Features Section */
     .features-grid {
         display: grid;
         grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
         gap: 30px;
         margin-top: 50px;
     }
     
     .feature-card {
         background-color: white;
         border-radius: 10px;
         padding: 30px;
         box-shadow: 0 5px 15px rgba(0, 0, 0, 0.05);
         transition: transform 0.3s ease, box-shadow 0.3s ease;
     }
     
         .feature-card:hover {
             transform: translateY(-10px);
             box-shadow: 0 15px 30px rgba(0, 0, 0, 0.1);
         }
     
     .feature-icon {
         font-size: 2.5rem;
         color: var(--primary);
         margin-bottom: 20px;
     }
     
     .feature-card h3 {
         font-size: 1.5rem;
         margin-bottom: 15px;
         color: var(--dark);
     }
     
     /* How It Works Section */
     .steps {
         display: flex;
         flex-direction: column;
         gap: 40px;
         max-width: 800px;
         margin: 0 auto;
     }
     
     .step {
         display: flex;
         gap: 30px;
     }
     
     .step-number {
         flex-shrink: 0;
         width: 60px;
         height: 60px;
         background-color: var(--primary-light);
         color: var(--primary);
         border-radius: 50%;
         display: flex;
         align-items: center;
         justify-content: center;
         font-size: 1.5rem;
         font-weight: 700;
     }
     
     .step-content h3 {
         font-size: 1.5rem;
         margin-bottom: 10px;
         color: var(--dark);
     }
    
     /* Responsive Styles */
     @media (max-width: 992px) {
         .section-title h2 {
             font-size: 2rem;
         }
         
         .hero h1 {
             font-size: 2.5rem;
         }
     }
     
     @media (max-width: 768px) {
         .mobile-menu-btn {
             display: block;
         }
         
         .nav-links {
             position: fixed;
             top: 80px;
             left: -100%;
             width: 100%;
             height: calc(100vh - 80px);
             background-color: white;
             flex-direction: column;
             align-items: center;
             justify-content: center;
             transition: left 0.3s ease;
         }
         
         .nav-links.active {
             left: 0;
         }
         
         .nav-links li {
             margin: 15px 0;
         }
         
         .hero-btns, .cta-btns {
             flex-direction: column;
             gap: 15px;
         }
         
         .step {
             flex-direction: column;
         }
     }
     
     @media (max-width: 576px) {
         .section-title h2 {
             font-size: 1.8rem;
         }
         
         .hero h1 {
             font-size: 2rem;
         }
         
         section {
             padding: 40px 0;
         }
     }
 </style>       
    <main>

    <!-- Hero Section -->
    <section class="hero">
        
            <h1>Never Miss a Healthcare Appointment Again</h1>
            <p>Automated SMS, Email & App Notifications that reduce no-shows by up to 30% and improve patient satisfaction.</p>
            <div class="hero-btns">
                <a href="#" class="btn">Register Patient</a>
                <a href="#" class="btn btn-outline">Learn More</a>
            </div>
            <!-- With additional attributes for better performance -->
            <img src='<%= ResolveUrl("~/Content/Images/img1.svg") %>' alt="Healthcare System" class="hero-image" width="700"  height="400"  />            
     
      
    </section>

    <!-- Features Section -->
    <section id="features">

            <div class="section-title">
                <h2>Key Features</h2>
                <p>Our comprehensive solution helps healthcare providers and patients stay connected</p>
            </div>
            <div class="features-grid">
                <div class="feature-card">
                    <div class="feature-icon">
                        <i class="fas fa-user-plus"></i>
                    </div>
                    <h3>Patient Registration</h3>
                    <p>Patients can easily sign up and choose their preferred notification method (email, SMS, or app notifications).</p>
                </div>
                <div class="feature-card">
                    <div class="feature-icon">
                        <i class="fas fa-calendar-check"></i>
                    </div>
                    <h3>Smart Scheduling</h3>
                    <p>Single calendar system prevents double bookings and automatically generates reminders based on patient preferences.</p>
                </div>
                <div class="feature-card">
                    <div class="feature-icon">
                        <i class="fas fa-bell"></i>
                    </div>
                    <h3>Multi-Channel Alerts</h3>
                    <p>Send reminders via Email, SMS, or App Push Notifications to ensure patients receive them in their preferred format.</p>
                </div>
                <div class="feature-card">
                    <div class="feature-icon">
                        <i class="fas fa-truck"></i>
                    </div>
                    <h3>Flexible Delivery</h3>
                    <p>Send reminders instantly, on schedule, or in batches to optimize your notification workflow.</p>
                </div>
                <div class="feature-card">
                    <div class="feature-icon">
                        <i class="fas fa-shield-alt"></i>
                    </div>
                    <h3>HIPAA Compliant</h3>
                    <p>Secure patient data handling with enterprise-grade security and compliance measures.</p>
                </div>
                <div class="feature-card">
                    <div class="feature-icon">
                        <i class="fas fa-chart-line"></i>
                    </div>
                    <h3>Analytics Dashboard</h3>
                    <p>Track appointment attendance rates, notification delivery success, and patient response patterns.</p>
                </div>
            </div>
 
    </section>

    <!-- How It Works Section -->
    <section id="how-it-works" class="bg-light">
        <div class="container">
            <div class="section-title">
                <h2>How It Works</h2>
                <p>Our simple 4-step process ensures patients never miss an appointment</p>
            </div>
            <div class="steps">
                <div class="step">
                    <div class="step-number">1</div>
                    <div class="step-content">
                        <h3>Patient Registration & Preference Setup</h3>
                        <p>Patients sign up with their name, contact information, and choose how they want to receive reminders (email, text message, or app notification). These preferences are saved securely in our system.</p>
                    </div>
                </div>
                <div class="step">
                    <div class="step-number">2</div>
                    <div class="step-content">
                        <h3>Appointment Creation</h3>
                        <p>Healthcare providers schedule appointments through our single-calendar system that prevents double bookings. The system automatically sets up reminders based on the patient's preferences.</p>
                    </div>
                </div>
                <div class="step">
                    <div class="step-number">3</div>
                    <div class="step-content">
                        <h3>Notification Preparation</h3>
                        <p>Our system formats the perfect reminder message for each patient's preferred communication channel, whether that's email, SMS, or app notification.</p>
                    </div>
                </div>
                <div class="step">
                    <div class="step-number">4</div>
                    <div class="step-content">
                        <h3>Message Delivery</h3>
                        <p>Reminders are sent at the optimal time before the appointment (1 day before, 1 hour before, etc.) using the patient's preferred method. Delivery status is tracked in real-time.</p>
                    </div>
                </div>
            </div>
        </div>
    </section>

    </main>

</asp:Content>
