import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import '../css/Artist.css';

function Artist() {
  const [artists, setArtists] = useState([]);
  const [showSidebar, setShowSidebar] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');
  const [sortConfig, setSortConfig] = useState({ key: null, direction: 'ascending' });
  const navigate = useNavigate();

  useEffect(() => {
    fetchArtists();
  }, []);

  const fetchArtists = async () => {
    try {
      const response = await fetch('https://6772b9a7ee76b92dd49333cb.mockapi.io/Artist');
      const data = await response.json();
      // Ensure data is an array before setting state
      if (Array.isArray(data)) {
        setArtists(data);
      } else {
        setArtists([]); // Set empty array if data is not an array
        console.error('Fetched data is not an array:', data);
      }
    } catch (error) {
      console.error('Error fetching artists:', error);
      setArtists([]); // Set empty array on error
    }
  };

  const handleLogout = () => {
    navigate('/');
  };

  const handleHome = () => {
    navigate('/home');
  };

  const handleStore = () => {
    navigate('/store');
  };

  // Search and sort functions
  const handleSearch = (event) => {
    setSearchTerm(event.target.value);
  };

  const requestSort = (key) => {
    let direction = 'ascending';
    if (sortConfig.key === key && sortConfig.direction === 'ascending') {
      direction = 'descending';
    }
    setSortConfig({ key, direction });
  };

  // Filter and sort artists
  const filteredAndSortedArtists = Array.isArray(artists) ? artists
    .filter(artist => 
      Object.values(artist)
        .join(' ')
        .toLowerCase()
        .includes(searchTerm.toLowerCase())
    )
    .sort((a, b) => {
      if (!sortConfig.key) return 0;
      
      if (a[sortConfig.key] < b[sortConfig.key]) {
        return sortConfig.direction === 'ascending' ? -1 : 1;
      }
      if (a[sortConfig.key] > b[sortConfig.key]) {
        return sortConfig.direction === 'ascending' ? 1 : -1;
      }
      return 0;
    }) : [];

  return (
    <div style={{ display: 'flex', minHeight: '100vh' }}>
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
        {/* Logo */}
        <div style={{ padding: '0 25px', marginBottom: '50px', whiteSpace: 'nowrap' }}>
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

        {/* Navigation Buttons */}
        <div style={{ display: 'flex', flexDirection: 'column', gap: '15px', padding: '0 25px' }}>
          <button
            className="nav-button"
            onClick={handleHome}
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
          >
            <span style={{ fontSize: '24px', filter: 'drop-shadow(2px 2px 4px rgba(0,0,0,0.2))' }}>🏠</span>
            {showSidebar && 'Home'}
          </button>

          <button
            className="nav-button"
            onClick={handleStore}
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
          >
            <span style={{ fontSize: '24px', filter: 'drop-shadow(2px 2px 4px rgba(0,0,0,0.2))' }}>📊</span>
            {showSidebar && 'Store'}
          </button>

          <button
            className="nav-button"
            onClick={handleLogout}
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
        transition: 'margin-left 0.4s cubic-bezier(0.4, 0, 0.2, 1)',
        padding: '40px'
      }}>
        {/* Header */}
        <div style={{
          background: 'rgba(255,255,255,0.95)',
          padding: '30px 35px',
          marginBottom: '35px',
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
          }}>Artist Management</h1>
        </div>

        {/* Search Bar */}
        <div style={{
          background: 'rgba(255,255,255,0.95)',
          padding: '20px',
          borderRadius: '15px',
          marginBottom: '20px'
        }}>
          <input
            type="text"
            placeholder="Search artists..."
            value={searchTerm}
            onChange={handleSearch}
            style={{
              padding: '10px 15px',
              borderRadius: '8px',
              border: '1px solid #ddd',
              width: '250px'
            }}
          />
        </div>

        {/* Artists List */}
        <div style={{
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
          }}>Artists List</h2>

          <table style={{ width: '100%', borderCollapse: 'separate', borderSpacing: '0 12px' }}>
            <thead>
              <tr style={{ background: 'rgba(248,249,250,0.8)' }}>
                <th onClick={() => requestSort('name')} style={{ padding: '18px', cursor: 'pointer' }}>
                  Name {sortConfig.key === 'name' && (sortConfig.direction === 'ascending' ? '↑' : '↓')}
                </th>
                <th onClick={() => requestSort('specialty')} style={{ padding: '18px', cursor: 'pointer' }}>
                  Specialty {sortConfig.key === 'specialty' && (sortConfig.direction === 'ascending' ? '↑' : '↓')}
                </th>
                <th onClick={() => requestSort('experience')} style={{ padding: '18px', cursor: 'pointer' }}>
                  Experience (Years) {sortConfig.key === 'experience' && (sortConfig.direction === 'ascending' ? '↑' : '↓')}
                </th>
                <th>Contact</th>
                <th>Profile Image</th>
              </tr>
            </thead>
            <tbody>
              {filteredAndSortedArtists.map((artist, index) => (
                <tr key={index} style={{
                  background: 'white',
                  transition: 'all 0.3s ease',
                  boxShadow: '0 4px 15px rgba(0,0,0,0.03)',
                  '&:hover': {
                    transform: 'translateY(-2px)',
                    boxShadow: '0 8px 25px rgba(0,0,0,0.08)'
                  }
                }}>
                  <td style={{ padding: '22px' }}>{artist.name}</td>
                  <td style={{ padding: '22px' }}>{artist.specialty}</td>
                  <td style={{ padding: '22px' }}>{artist.experience}</td>
                  <td style={{ padding: '22px' }}>{artist.contact}</td>
                  <td style={{ padding: '22px' }}>
                    {artist.profileImage && (
                      <img 
                        src={artist.profileImage} 
                        alt={artist.name}
                        style={{
                          width: '100px',
                          height: '100px',
                          objectFit: 'cover',
                          borderRadius: '50%',
                          border: '3px solid #24BFDD',
                          transition: 'transform 0.3s ease',
                          '&:hover': {
                            transform: 'scale(1.1)'
                          }
                        }}
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
  );
}

export default Artist;