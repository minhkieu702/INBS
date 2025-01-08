import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Calendar from 'react-calendar';
import 'react-calendar/dist/Calendar.css';
import '../css/Waitlist.css';

function Waitlist() {
  const [showSidebar, setShowSidebar] = useState(false);
  const [waitlist, setWaitlist] = useState([]);
  const [selectedDate, setSelectedDate] = useState(new Date());
  const navigate = useNavigate();

  useEffect(() => {
    fetchWaitlist();
  }, []);

  const fetchWaitlist = async () => {
    try {
      const response = await fetch('https://6772b9a7ee76b92dd49333cb.mockapi.io/Booking');
      const data = await response.json();
      if (Array.isArray(data)) {
        setWaitlist(data);
      } else {
        setWaitlist([]);
        console.error('Fetched data is not an array:', data);
      }
    } catch (error) {
      console.error('Error fetching waitlist:', error);
      setWaitlist([]);
    }
  };

  const handleDateChange = (date) => {
    setSelectedDate(date);
  };

  const filteredWaitlist = waitlist.filter(customer => {
    const customerDate = new Date(customer.date);
    return customerDate.toDateString() === selectedDate.toDateString();
  });

  const handleHome = () => navigate('/home');
  const handleStore = () => navigate('/store');
  const handleArtist = () => navigate('/artist');
  const handleLogout = () => navigate('/');

  return (
    <div className="waitlist-container">
      <div 
        className={`sidebar ${showSidebar ? 'expanded' : ''}`}
        onMouseEnter={() => setShowSidebar(true)}
        onMouseLeave={() => setShowSidebar(false)}
      >
        <div className="logo-container">
          <h1 className="logo">{showSidebar ? 'INBS' : 'IB'}</h1>
        </div>

        <div className="sidebar-buttons">
          <button className="sidebar-button" onClick={handleHome}>
            <span className="sidebar-button-icon" style={{marginRight: "12px", marginLeft: "-12px", fontSize: "20px"}}>🏠</span>
            {showSidebar && 'Home'}
          </button>
          <button className="sidebar-button" onClick={handleStore}>
            <span className="sidebar-button-icon" style={{marginRight: "12px", marginLeft: "-12px", fontSize: "20px"}}>📊</span>
            {showSidebar && 'Store'}
          </button>
          <button className="sidebar-button" onClick={handleArtist}>
            <span className="sidebar-button-icon" style={{marginRight: "12px", marginLeft: "-12px", fontSize: "20px"}}>🎨</span>
            {showSidebar && 'Artist'}
          </button>
          <button className="sidebar-button" onClick={handleLogout}>
            <span className="sidebar-button-icon" style={{marginRight: "12px", marginLeft: "-12px", fontSize: "20px"}}>⬅️</span>
            {showSidebar && 'Logout'}
          </button>
        </div>
      </div>

      <div className={`main-content ${showSidebar ? 'sidebar-expanded' : ''}`}>
        <div className="header">
          <h1 className="page-title">Waitlist Management</h1>
        </div>

        <div className="content-grid">
          <div className="calendar-section">
            <h2 className="section-title">Select Date</h2>
            <Calendar
              onChange={handleDateChange}
              value={selectedDate}
              className="custom-calendar"
              tileClassName={({ date }) => {
                const hasAppointment = waitlist.some(customer => 
                  new Date(customer.date).toDateString() === date.toDateString()
                );
                return hasAppointment ? 'has-appointment' : null;
              }}
            />
          </div>

          <div className="waitlist-table-container">
            <h2 className="section-title">
              Appointments for {selectedDate.toLocaleDateString('en-US', { 
                weekday: 'long', 
                year: 'numeric', 
                month: 'long', 
                day: 'numeric' 
              })}
            </h2>
            <div className="table-wrapper">
              <table className="waitlist-table">
                <thead>
                  <tr>
                    <th>Name</th>
                    <th>Phone</th>
                    <th>Service</th>
                    <th>Time</th>
                    <th>Store</th>
                    <th>Price</th>
                    <th>Service Image</th>
                  </tr>
                </thead>
                <tbody>
                  {filteredWaitlist.map((customer) => (
                    <tr key={customer.id}>
                      <td className="customer-name">{customer.name}</td>
                      <td className="customer-phone">{customer.phone}</td>
                      <td className="customer-service">{customer.service}</td>
                      <td className="customer-time">{customer.time} hours</td>
                      <td className="customer-store">{customer.store}</td>
                      <td className="customer-price">${customer.price}</td>
                      <td className="customer-image">
                        {customer.serviceImage && (
                          <img 
                            src={customer.serviceImage} 
                            alt={customer.service}
                            className="service-image"
                          />
                        )}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default Waitlist;
