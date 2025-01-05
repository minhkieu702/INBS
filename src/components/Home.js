import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import '../css/Home.css';

function Home() {
  const [bookings, setBookings] = useState([]);
  const [showSidebar, setShowSidebar] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');
  const [sortConfig, setSortConfig] = useState({ key: null, direction: 'ascending' });
  const [filters, setFilters] = useState({
    service: '',
    store: ''
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

  // Get unique values for filters
  const getUniqueValues = (key) => {
    return [...new Set(bookings.map(booking => booking[key]))];
  };

  // Apply all filters and sorting
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
    <div style={{
      display: 'flex',
      minHeight: '100vh'
    }}>
      {/* Sidebar */}
      <div 
        onMouseEnter={() => setShowSidebar(true)}
        onMouseLeave={() => setShowSidebar(false)}
        style={{
          width: showSidebar ? '280px' : '70px',
          background: 'linear-gradient(180deg, #1a237e, #0d47a1)',
          padding: '35px 0',
          display: 'flex',
          flexDirection: 'column',
          position: 'fixed',
          height: '100vh',
          transition: 'all 0.4s cubic-bezier(0.4, 0, 0.2, 1)',
          overflow: 'hidden',
          boxShadow: '4px 0 15px rgba(0,0,0,0.15)',
          zIndex: 1000
        }}>
        <div style={{
          padding: '0 25px',
          marginBottom: '50px',
          whiteSpace: 'nowrap'
        }}>
          <h1 style={{
            color: 'white',
            fontSize: showSidebar ? '38px' : '34px',
            fontWeight: '900',
            letterSpacing: '4px',
            margin: 0,
            textShadow: '2px 2px 8px rgba(0,0,0,0.3)',
            fontFamily: "'Poppins', sans-serif",
            background: 'linear-gradient(45deg, #fff, #64b5f6)',
            WebkitBackgroundClip: 'text',
            WebkitTextFillColor: 'transparent',
            transition: 'all 0.3s ease'
          }}>{showSidebar ? 'INBS' : 'IB'}</h1>
        </div>

        <div style={{
          display: 'flex',
          flexDirection: 'column',
          gap: '15px',
          padding: '0 25px'
        }}>
          <button
            style={{
              background: 'rgba(255,255,255,0.1)',
              border: '1px solid rgba(255,255,255,0.1)',
              color: 'white',
              padding: '18px',
              cursor: 'pointer',
              fontSize: '16px',
              fontWeight: '600',
              display: 'flex',
              alignItems: 'center',
              gap: '15px',
              transition: 'all 0.3s ease',
              whiteSpace: 'nowrap',
              backdropFilter: 'blur(10px)',
              boxShadow: '0 4px 15px rgba(0,0,0,0.1)'
            }}
            onClick={handleStore}
          >
            <span style={{ fontSize: '24px', filter: 'drop-shadow(2px 2px 4px rgba(0,0,0,0.2))' }}>📊</span>
            {showSidebar && 'Store'}
          </button>
          <button
            style={{
              background: 'rgba(255,255,255,0.1)',
              border: '1px solid rgba(255,255,255,0.1)', 
              color: 'white',
              padding: '18px',
              cursor: 'pointer',
              fontSize: '16px',
              fontWeight: '600',
              display: 'flex',
              alignItems: 'center',
              gap: '15px',
              transition: 'all 0.3s ease',
              whiteSpace: 'nowrap',
              backdropFilter: 'blur(10px)',
              boxShadow: '0 4px 15px rgba(0,0,0,0.1)'
            }}
            onClick={handleArtist}
          >
            <span style={{ fontSize: '24px', filter: 'drop-shadow(2px 2px 4px rgba(0,0,0,0.2))' }}>🎨</span>
            {showSidebar && 'Artist'}
          </button>

          <button
            style={{
              background: 'rgba(255,255,255,0.1)',
              border: '1px solid rgba(255,255,255,0.1)',
              color: 'white',
              padding: '18px',
              cursor: 'pointer',
              fontSize: '16px',
              fontWeight: '600',
              display: 'flex',
              alignItems: 'center',
              gap: '15px',
              transition: 'all 0.3s ease',
              marginTop: 'auto',
              whiteSpace: 'nowrap',
              backdropFilter: 'blur(10px)',
              boxShadow: '0 4px 15px rgba(0,0,0,0.1)'
            }}
            onClick={handleLogout}
          >
            <span style={{ fontSize: '24px', filter: 'drop-shadow(2px 2px 4px rgba(0,0,0,0.2))' }}>⬅️</span>
            {showSidebar && 'Logout'}
          </button>
        </div>
      </div>

      {/* Main Content */}
      <div style={{
        flex: 1,
        marginLeft: showSidebar ? '280px' : '70px',
        background: 'linear-gradient(135deg, #f6f9fc 0%, #e9f2f9 100%)',
        transition: 'margin-left 0.4s cubic-bezier(0.4, 0, 0.2, 1)'
      }}>
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
            backgroundColor: 'rgba(0,0,0,0.8)',
            display: 'flex',
            justifyContent: 'center',
            alignItems: 'center',
            zIndex: 1100,
            padding: '20px'
          }}
          onClick={() => setSelectedImage(null)}
        >
          <div 
            style={{
              background: 'white',
              padding: '20px',
              borderRadius: '15px',
              maxWidth: '90%',
              maxHeight: '90%',
              overflow: 'auto',
              position: 'relative'
            }}
            onClick={e => e.stopPropagation()}
          >
            <img 
              src={selectedImage.serviceImage}
              alt={selectedImage.service}
              style={{
                maxWidth: '100%',
                maxHeight: '70vh',
                objectFit: 'contain',
                borderRadius: '15px',
                marginBottom: '25px',
                boxShadow: '0 10px 30px rgba(0,0,0,0.15)',
                transition: 'transform 0.3s ease',
                cursor: 'pointer'
              }}
              onMouseOver={(e) => {
                e.target.style.transform = 'scale(1.02)';
              }}
              onMouseOut={(e) => {
                e.target.style.transform = 'scale(1)';
              }}
            />
            <div style={{ 
              textAlign: 'center',
              padding: '0 20px 20px',
              background: 'linear-gradient(to bottom, #ffffff, #f8f9fa)',
              borderRadius: '12px',
              boxShadow: '0 4px 15px rgba(0,0,0,0.05)'
            }}>
              <h3 style={{ 
                color: '#1e3c72', 
                marginBottom: '15px',
                fontSize: '1.8rem',
                fontWeight: '600',
                textShadow: '1px 1px 2px rgba(0,0,0,0.1)'
              }}>{selectedImage.service}</h3>
              <p style={{ 
                color: '#4a4a4a', 
                margin: '8px 0',
                fontSize: '1.1rem',
                fontWeight: '500'
              }}>Price: <span style={{color: '#2ecc71'}}>${selectedImage.price}</span></p>
              <p style={{ 
                color: '#4a4a4a', 
                margin: '8px 0',
                fontSize: '1.1rem',
                fontWeight: '500'
              }}>Duration: <span style={{color: '#3498db'}}>{selectedImage.time} hours</span></p>
              <p style={{ 
                color: '#4a4a4a', 
                margin: '8px 0',
                fontSize: '1.1rem',
                fontWeight: '500'
              }}>Store: <span style={{color: '#9b59b6'}}>{selectedImage.store}</span></p>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}

export default Home;