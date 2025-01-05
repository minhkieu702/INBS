import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import '../css/Home.css';
import '../data.json';
import { PieChart, Pie, Cell, Legend, Tooltip, BarChart, Bar, XAxis, YAxis, CartesianGrid } from 'recharts';

function Home() {
  const [bookings, setBookings] = useState([]);
  const [showSidebar, setShowSidebar] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');
  const [sortConfig, setSortConfig] = useState({ key: null, direction: 'ascending' });
  const [filters, setFilters] = useState({
    service: '',
    store: '',
    price: ''
  });
  const [selectedImage, setSelectedImage] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    fetchBookings();
  }, []);

  const fetchBookings = async () => {
    try {
      const response = await fetch('https://6772b9a7ee76b92dd49333cb.mockapi.io/Booking');
      const data = await response.json();
      // Ensure data is an array before setting state
      if (Array.isArray(data)) {
        setBookings(data);
      } else {
        setBookings([]); // Set empty array if data is not an array
        console.error('Fetched data is not an array:', data);
      }
    } catch (error) {
      console.error('Error fetching bookings:', error);
      setBookings([]); // Set empty array on error
    }
  };
  // const fetchBookings = async () => {
  //   try {
  //     const data = require('../data.json');
  //     // Ensure data is an array before setting state
  //     if (Array.isArray(data)) {
  //       setBookings(data);
  //     } else {
  //       setBookings([]); // Set empty array if data is not an array
  //       console.error('Fetched data is not an array:', data);
  //     }
  //   } catch (error) {
  //     console.error('Error fetching bookings:', error);
  //     setBookings([]); // Set empty array on error
  //   }
  // };
  

  const handleLogout = () => {
    navigate('/');
  };

  const handleStore = () => {
    navigate('/store');
  };

  const handleArtist = () => {
    navigate('/artist');
  };

  // Search function
  const handleSearch = (event) => {
    setSearchTerm(event.target.value);
  };

  // Sort function
  const requestSort = (key) => {
    let direction = 'ascending';
    if (sortConfig.key === key && sortConfig.direction === 'ascending') {
      direction = 'descending';
    }
    setSortConfig({ key, direction });
  };

  // Filter function
  const handleFilterChange = (event) => {
    const { name, value } = event.target;
    setFilters(prev => ({
      ...prev,
      [name]: value
    }));
  };

  
  const getUniqueValues = (key) => {
    return [...new Set(bookings.map(booking => booking[key]))];
  };

  // Get data for pie chart
  const getPieChartData = () => {
    const serviceCount = {};
    bookings.forEach(booking => {
      serviceCount[booking.service] = (serviceCount[booking.service] || 0) + 1;
    });
    
    return Object.entries(serviceCount).map(([name, value]) => ({
      name,
      value
    }));
  };

  // Get data for price bar chart
  const getPriceChartData = () => {
    const priceRanges = {
      '200,000-250,000': 0,
      '250,001-500,000': 0,
      '500,001-1,000,000': 0,
      '1,000,001-2,000,000': 0,
      '2,000,000+': 0
    };

    bookings.forEach(booking => {
      const price = Number(booking.price);
      if (price <= 250000) {
        priceRanges['200,000-250,000']++;
      } else if (price <= 500000) {
        priceRanges['250,001-500,000']++;
      } else if (price <= 10000000) {
        priceRanges['500,001-1,000,000']++;
      } else if (price <= 20000000) {
        priceRanges['1,000,001-2,000,000']++;
      } else {
        priceRanges['2,000,000+']++;
      }
    });

    return Object.entries(priceRanges).map(([range, count]) => ({
      range,
      count
    }));
  };

  const COLORS = ['#FF6B6B', '#4ECDC4', '#45B7D1', '#96CEB4', '#FFEEAD'];
  
  const filteredAndSortedBookings = bookings
    .filter(booking => {
      const matchesSearch = Object.values(booking)
        .join(' ')
        .toLowerCase()
        .includes(searchTerm.toLowerCase());
      
      const matchesServiceFilter = filters.service === '' || booking.service === filters.service;
      const matchesStoreFilter = filters.store === '' || booking.store === filters.store;
      
      return matchesSearch && matchesServiceFilter && matchesStoreFilter;
    })
    .sort((a, b) => {
      if (!sortConfig.key) return 0;
      
      if (a[sortConfig.key] < b[sortConfig.key]) {
        return sortConfig.direction === 'ascending' ? -1 : 1;
      }
      if (a[sortConfig.key] > b[sortConfig.key]) {
        return sortConfig.direction === 'ascending' ? 1 : -1;
      }
      return 0;
    });

  return (
    <div className="layout">
      <div 
        className={`sidebar ${showSidebar ? 'expanded' : ''}`}
        onMouseEnter={() => setShowSidebar(true)}
        onMouseLeave={() => setShowSidebar(false)}
      >
        <div className="sidebar-header">
          <h1 className="sidebar-title">{showSidebar ? 'INBS' : 'IB'}</h1>
        </div>

        <div className="sidebar-buttons">
          <button className="sidebar-button" onClick={handleStore}>
            <span className="sidebar-button-icon">📊</span>
            {showSidebar && 'Store'}
          </button>
          <button className="sidebar-button" onClick={handleArtist}>
            <span className="sidebar-button-icon">🎨</span>
            {showSidebar && 'Artist'}
          </button>
          <button className="sidebar-button" onClick={handleLogout}>
            <span className="sidebar-button-icon">⬅️</span>
            {showSidebar && 'Logout'}
          </button>
        </div>
      </div>

      <div className={`main-content ${showSidebar ? 'sidebar-expanded' : ''}`}>
        <div className="home-container" style={{ padding: '40px' }}>
          <div style={{ 
            display: 'flex', 
            justifyContent: 'space-between', 
            alignItems: 'center', 
            marginBottom: '35px',
            background: 'rgba(255,255,255,0.95)',
            padding: '30px 35px',
            boxShadow: '0 10px 30px rgba(0,0,0,0.08)',
            backdropFilter: 'blur(10px)',
            border: '1px solid rgba(255,255,255,0.2)'
          }}>
            <h1 style={{
              color: '#1e3c72',
              fontSize: '36px',
              fontWeight: '800',
              margin: 0,
              textShadow: '2px 2px 4px rgba(0,0,0,0.1)'
            }}>Booking Management</h1>
          </div>

          {/* Analytics Section */}
          <div style={{
            display: 'grid',
            gridTemplateColumns: '1fr 1fr',
            gap: '20px',
            marginBottom: '20px'
          }}>
            {/* Service Distribution Chart */}
            <div style={{
              background: 'rgba(255,255,255,0.95)',
              padding: '30px',
              borderRadius: '15px',
              boxShadow: '0 10px 30px rgba(0,0,0,0.08)'
            }}>
              <h2 style={{
                color: '#1e3c72',
                marginBottom: '20px',
                fontSize: '24px',
                fontWeight: '600'
              }}>Service Distribution</h2>
              <div style={{
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'center',
                padding: '20px'
              }}>
                <PieChart width={500} height={400}>
                  <Pie
                    data={getPieChartData()}
                    cx={250}
                    cy={200}
                    innerRadius={80}
                    outerRadius={160}
                    fill="#8884d8"
                    paddingAngle={5}
                    dataKey="value"
                    label={({name, percent}) => `${name} (${(percent * 100).toFixed(0)}%)`}
                    labelLine={{stroke: '#666', strokeWidth: 1}}
                  >
                    {getPieChartData().map((entry, index) => (
                      <Cell 
                        key={`cell-${index}`} 
                        fill={COLORS[index % COLORS.length]}
                        stroke="#fff"
                        strokeWidth={2}
                      />
                    ))}
                  </Pie>
                  <Tooltip 
                    contentStyle={{
                      background: 'rgba(255, 255, 255, 0.95)',
                      border: 'none',
                      borderRadius: '8px',
                      boxShadow: '0 4px 12px rgba(0,0,0,0.1)',
                      padding: '10px 15px'
                    }}
                  />
                  <Legend 
                    verticalAlign="bottom" 
                    height={36}
                    iconType="circle"
                    iconSize={10}
                    wrapperStyle={{
                      padding: '20px 0',
                      fontSize: '14px'
                    }}
                  />
                </PieChart>
              </div>
            </div>

            {/* Price Distribution Chart */}
            <div style={{
              background: 'rgba(255,255,255,0.95)',
              padding: '30px',
              borderRadius: '15px',
              boxShadow: '0 10px 30px rgba(0,0,0,0.08)'
            }}>
              <h2 style={{
                color: '#1e3c72',
                marginBottom: '20px',
                fontSize: '24px',
                fontWeight: '600'
              }}>Price Distribution</h2>
              <div style={{
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'center',
                padding: '20px'
              }}>
                <BarChart width={500} height={400} data={getPriceChartData()}>
                  <CartesianGrid strokeDasharray="3 3" />
                  <XAxis dataKey="range" />
                  <YAxis />
                  <Tooltip 
                    contentStyle={{
                      background: 'rgba(255, 255, 255, 0.95)',
                      border: 'none',
                      borderRadius: '8px',
                      boxShadow: '0 4px 12px rgba(0,0,0,0.1)',
                      padding: '10px 15px'
                    }}
                  />
                  <Bar dataKey="count" fill="#24BFDD">
                    {getPriceChartData().map((entry, index) => (
                      <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                    ))}
                  </Bar>
                </BarChart>
              </div>
            </div>
          </div>

          {/* Search and Filter Controls */}
          <div style={{
            background: 'rgba(255,255,255,0.95)',
            padding: '20px',
            borderRadius: '15px',
            marginBottom: '20px',
            display: 'flex',
            gap: '20px',
            alignItems: 'center',
            flexWrap: 'wrap'
          }}>
            <input
              type="text"
              placeholder="Search anything..."
              value={searchTerm}
              onChange={handleSearch}
              style={{
                padding: '10px 15px',
                borderRadius: '8px',
                border: '1px solid #ddd',
                width: '250px'
              }}
            />
            
            <select
              name="service"
              value={filters.service}
              onChange={handleFilterChange}
              style={{
                padding: '10px 15px',
                borderRadius: '8px',
                border: '1px solid #ddd'
              }}
            >
              <option value="">All Services</option>
              {getUniqueValues('service').map(service => (
                <option key={service} value={service}>{service}</option>
              ))}
            </select>

            <select
              name="store"
              value={filters.store}
              onChange={handleFilterChange}
              style={{
                padding: '10px 15px',
                borderRadius: '8px',
                border: '1px solid #ddd'
              }}
            >
              <option value="">All Stores</option>
              {getUniqueValues('store').map(store => (
                <option key={store} value={store}>{store}</option>
              ))}
            </select>

            <select
              name="price"
              value={filters.price}
              onChange={handleFilterChange}
              style={{
                padding: '10px 15px',
                borderRadius: '8px',
                border: '1px solid #ddd'
              }}
            >
              <option value="">All Prices</option>
              {getUniqueValues('price').sort((a,b) => Number(a)-Number(b)).map(price => (
                <option key={price} value={price}>{price}</option>
              ))}
            </select>
          </div>

          <div className="booking-list" style={{
            background: 'rgba(255,255,255,0.95)',
            padding: '35px',
            borderRadius: '25px',
            boxShadow: '0 15px 40px rgba(0,0,0,0.08)',
            backdropFilter: 'blur(10px)',
            border: '1px solid rgba(255,255,255,0.2)'
          }}>
            <h2 style={{
              color: '#1e3c72',
              fontSize: '28px',
              marginBottom: '30px',
              borderBottom: '4px solid #24BFDD',
              paddingBottom: '12px',
              display: 'inline-block',
              fontWeight: '700'
            }}>Appointment List</h2>
            <table style={{ width: '100%', borderCollapse: 'separate', borderSpacing: '0 12px' }}>
              <thead>
                <tr style={{ background: 'rgba(248,249,250,0.8)' }}>
                  <th 
                    onClick={() => requestSort('name')}
                    style={{ 
                      padding: '18px', 
                      color: '#1e3c72', 
                      fontWeight: '700', 
                      borderBottom: '2px solid #e9ecef', 
                      fontSize: '15px',
                      cursor: 'pointer'
                    }}
                  >
                    Customer Name {sortConfig.key === 'name' && (sortConfig.direction === 'ascending' ? '↑' : '↓')}
                  </th>
                  <th style={{ padding: '18px', color: '#1e3c72', fontWeight: '700', borderBottom: '2px solid #e9ecef', fontSize: '15px' }}>Phone Number</th>
                  <th 
                    onClick={() => requestSort('date')}
                    style={{ 
                      padding: '18px', 
                      color: '#1e3c72', 
                      fontWeight: '700', 
                      borderBottom: '2px solid #e9ecef', 
                      fontSize: '15px',
                      cursor: 'pointer'
                    }}
                  >
                    Date {sortConfig.key === 'date' && (sortConfig.direction === 'ascending' ? '↑' : '↓')}
                  </th>
                  <th style={{ padding: '18px', color: '#1e3c72', fontWeight: '700', borderBottom: '2px solid #e9ecef', fontSize: '15px' }}>Time</th>
                  <th style={{ padding: '18px', color: '#1e3c72', fontWeight: '700', borderBottom: '2px solid #e9ecef', fontSize: '15px' }}>Service</th>
                  <th 
                    onClick={() => requestSort('price')}
                    style={{ 
                      padding: '18px', 
                      color: '#1e3c72', 
                      fontWeight: '700', 
                      borderBottom: '2px solid #e9ecef', 
                      fontSize: '15px',
                      cursor: 'pointer'
                    }}
                  >
                    Price {sortConfig.key === 'price' && (sortConfig.direction === 'ascending' ? '↑' : '↓')}
                  </th>
                  <th style={{ padding: '18px', color: '#1e3c72', fontWeight: '700', borderBottom: '2px solid #e9ecef', fontSize: '15px' }}>Store Address</th>
                  <th style={{ padding: '18px', color: '#1e3c72', fontWeight: '700', borderBottom: '2px solid #e9ecef', fontSize: '15px' }}>Service Image</th>
                </tr>
              </thead>
              <tbody>
                {filteredAndSortedBookings.map((booking, index) => (
                  <tr key={index} style={{
                    background: 'white',
                    transition: 'all 0.4s ease',
                    borderRadius: '15px',
                    boxShadow: '0 4px 15px rgba(0,0,0,0.03)',
                    cursor: 'pointer'
                  }}
                  onMouseOver={(e) => {
                    e.currentTarget.style.transform = 'translateY(-3px)';
                    e.currentTarget.style.boxShadow = '0 8px 25px rgba(0,0,0,0.1)';
                  }}
                  onMouseOut={(e) => {
                    e.currentTarget.style.transform = 'translateY(0)';
                    e.currentTarget.style.boxShadow = '0 4px 15px rgba(0,0,0,0.03)';
                  }}>
                    <td style={{ padding: '22px', borderBottom: '1px solid #e9ecef', fontWeight: '500' }}>{booking.name}</td>
                    <td style={{ padding: '22px', borderBottom: '1px solid #e9ecef', color: '#666' }}>{booking.phone}</td>
                    <td style={{ padding: '22px', borderBottom: '1px solid #e9ecef', color: '#666' }}>{booking.date}</td>
                    <td style={{ padding: '22px', borderBottom: '1px solid #e9ecef', color: '#666' }}>{booking.time} hours</td>
                    <td style={{ padding: '22px', borderBottom: '1px solid #e9ecef', fontWeight: '500', color: '#1e3c72' }}>{booking.service}</td>
                    <td style={{ padding: '22px', borderBottom: '1px solid #e9ecef', fontWeight: '600', color: '#24BFDD' }}>${booking.price}</td>
                    <td style={{ padding: '22px', borderBottom: '1px solid #e9ecef', fontWeight: '600', color: '#24BFDD' }}>{booking.store}</td>
                    <td style={{ padding: '22px', borderBottom: '1px solid #e9ecef' }}>
                      {booking.serviceImage && (
                        <div>
                          <img 
                            src={booking.serviceImage} 
                            alt={booking.service}
                            onClick={() => setSelectedImage(booking)}
                            style={{
                              width: '120px', 
                              height: '120px', 
                              objectFit: 'cover',
                              borderRadius: '15px',
                              boxShadow: '0 8px 20px rgba(0,0,0,0.1)',
                              transition: 'all 0.4s ease',
                              border: '3px solid rgba(255,255,255,0.2)',
                              cursor: 'pointer'
                            }}
                            onMouseOver={(e) => {
                              e.target.style.transform = 'scale(1.08)';
                              e.target.style.boxShadow = '0 12px 30px rgba(0,0,0,0.15)';
                            }}
                            onMouseOut={(e) => {
                              e.target.style.transform = 'scale(1)';
                              e.target.style.boxShadow = '0 8px 20px rgba(0,0,0,0.1)';
                            }}
                          />
                        </div>
                      )}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      </div>

      {/* Image Modal */}
      {selectedImage && (
        <div 
          style={{
            position: 'fixed',
            top: 0,
            left: 0,
            right: 0,
            bottom: 0,
            backgroundColor: 'rgba(0,0,0,0.85)', // Slightly darker overlay
            display: 'flex',
            justifyContent: 'center',
            alignItems: 'center',
            zIndex: 1100,
            padding: '20px',
            backdropFilter: 'blur(8px)' // Add blur effect
          }}
          onClick={() => setSelectedImage(null)}
        >
          <div 
            style={{
              background: 'linear-gradient(145deg, #ffffff, #f5f7fa)',
              padding: '30px',
              borderRadius: '20px',
              maxWidth: '90%',
              maxHeight: '90%',
              overflow: 'auto',
              position: 'relative',
              boxShadow: '0 25px 50px -12px rgba(0,0,0,0.25)',
              border: '1px solid rgba(255,255,255,0.18)',
              animation: 'modalFadeIn 0.3s ease-out'
            }}
            onClick={e => e.stopPropagation()}
          >
            <img 
              src={selectedImage.serviceImage}
              alt={selectedImage.service}
              style={{
                width: '100%',
                maxHeight: '70vh',
                objectFit: 'cover',
                borderRadius: '18px',
                marginBottom: '30px',
                boxShadow: '0 15px 35px rgba(0,0,0,0.2)',
                transition: 'all 0.4s ease',
                cursor: 'zoom-in',
                border: '3px solid rgba(255,255,255,0.2)'
              }}
              onMouseOver={(e) => {
                e.target.style.transform = 'scale(1.03)';
                e.target.style.boxShadow = '0 20px 40px rgba(0,0,0,0.25)';
              }}
              onMouseOut={(e) => {
                e.target.style.transform = 'scale(1)';
                e.target.style.boxShadow = '0 15px 35px rgba(0,0,0,0.2)';
              }}
            />
            <div style={{ 
              textAlign: 'center',
              padding: '25px 30px',
              background: 'linear-gradient(135deg, #ffffff, #f8f9fa)',
              borderRadius: '15px',
              boxShadow: '0 8px 25px rgba(0,0,0,0.08)',
              border: '1px solid rgba(255,255,255,0.4)'
            }}>
              <h3 style={{ 
                color: '#005FA3', 
                marginBottom: '20px',
                fontSize: '2rem',
                fontWeight: '700',
                textShadow: '2px 2px 4px rgba(0,0,0,0.1)',
                letterSpacing: '0.5px'
              }}>{selectedImage.service}</h3>
              <p style={{ 
                color: '#4a4a4a', 
                margin: '12px 0',
                fontSize: '1.2rem',
                fontWeight: '500',
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'center',
                gap: '8px'
              }}>Price: <span style={{
                color: '#00A5F5',
                background: 'rgba(0, 165, 245, 0.1)',
                padding: '4px 12px',
                borderRadius: '20px',
                fontWeight: '600'
              }}>${selectedImage.price}</span></p>
              <p style={{ 
                color: '#4a4a4a', 
                margin: '12px 0',
                fontSize: '1.2rem',
                fontWeight: '500',
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'center',
                gap: '8px'
              }}>Duration: <span style={{
                color: '#24BFDD',
                background: 'rgba(36, 191, 221, 0.1)',
                padding: '4px 12px',
                borderRadius: '20px',
                fontWeight: '600'
              }}>{selectedImage.time} hours</span></p>
              <p style={{ 
                color: '#4a4a4a', 
                margin: '12px 0',
                fontSize: '1.2rem',
                fontWeight: '500',
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'center',
                gap: '8px'
              }}>Store: <span style={{
                color: '#53CEF8',
                background: 'rgba(83, 206, 248, 0.1)',
                padding: '4px 12px',
                borderRadius: '20px',
                fontWeight: '600'
              }}>{selectedImage.store}</span></p>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}

export default Home;