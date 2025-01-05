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
      if (Array.isArray(data)) {
        setArtists(data);
      } else {
        setArtists([]);
        console.error('Fetched data is not an array:', data);
      }
    } catch (error) {
      console.error('Error fetching artists:', error);
      setArtists([]);
    }
  };

  const handleLogout = () => navigate('/');
  const handleHome = () => navigate('/home'); 
  const handleStore = () => navigate('/store');

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

  const filteredAndSortedArtists = Array.isArray(artists) ? artists
    .filter(artist => 
      Object.values(artist)
        .join(' ')
        .toLowerCase()
        .includes(searchTerm.toLowerCase())
    )
    .sort((a, b) => {
      if (!sortConfig.key) return 0;
      
      const aValue = a[sortConfig.key];
      const bValue = b[sortConfig.key];
      
      if (aValue < bValue) return sortConfig.direction === 'ascending' ? -1 : 1;
      if (aValue > bValue) return sortConfig.direction === 'ascending' ? 1 : -1;
      return 0;
    }) : [];

  return (
    <div className="artist-container">
      <div 
        className={`sidebar ${showSidebar ? 'expanded' : ''}`}
        onMouseEnter={() => setShowSidebar(true)}
        onMouseLeave={() => setShowSidebar(false)}
      >
        <div className="logo">
          <h1 className={showSidebar ? 'expanded' : ''}>
            {showSidebar ? 'INBS' : 'IB'}
          </h1>
        </div>

        <div className="nav-buttons">
          <button onClick={handleHome} className="nav-button">
            <span>🏠</span>
            {showSidebar && 'Home'}
          </button>

          <button onClick={handleStore} className="nav-button">
            <span>📊</span>
            {showSidebar && 'Store'}
          </button>

          <button onClick={handleLogout} className="nav-button logout">
            <span>⬅️</span>
            {showSidebar && 'Logout'}
          </button>
        </div>
      </div>

      <div className={`main-content ${showSidebar ? 'sidebar-expanded' : ''}`}>
        <div className="header">
          <h1>Artist Management</h1>
        </div>

        <div className="search-bar">
          <input
            type="text"
            className="search-input"
            placeholder="Search artists..."
            value={searchTerm}
            onChange={handleSearch}
          />
        </div>

        <div className="artists-list">
          <h2>Artists List</h2>

          <table className="artists-table">
            <thead>
              <tr>
                <th onClick={() => requestSort('name')} className="table-header">
                  Name {sortConfig.key === 'name' && (sortConfig.direction === 'ascending' ? '↑' : '↓')}
                </th>
                <th onClick={() => requestSort('contact')} className="table-header">
                  Contact {sortConfig.key === 'contact' && (sortConfig.direction === 'ascending' ? '↑' : '↓')}
                </th>
                <th onClick={() => requestSort('experience')} className="table-header">
                  Experience {sortConfig.key === 'experience' && (sortConfig.direction === 'ascending' ? '↑' : '↓')}
                </th>
                <th onClick={() => requestSort('breakTime')} className="table-header">
                  Break Time {sortConfig.key === 'breakTime' && (sortConfig.direction === 'ascending' ? '↑' : '↓')}
                </th>
                <th onClick={() => requestSort('totalBooking')} className="table-header">
                  Total Booking {sortConfig.key === 'totalBooking' && (sortConfig.direction === 'ascending' ? '↑' : '↓')}
                </th>
                <th onClick={() => requestSort('storeAddress')} className="table-header">
                  Store Address {sortConfig.key === 'storeAddress' && (sortConfig.direction === 'ascending' ? '↑' : '↓')}
                </th>
                <th onClick={() => requestSort('status')} className="table-header">
                  Status {sortConfig.key === 'status' && (sortConfig.direction === 'ascending' ? '↑' : '↓')}
                </th>
              </tr>
            </thead>
            <tbody>
              {filteredAndSortedArtists.map((artist, index) => (
                <tr key={index} className="table-row">
                  <td className="table-cell name">{artist.name}</td>
                  <td className="table-cell">{artist.contact}</td>
                  <td className="table-cell">{artist.experience}</td>
                  <td className="table-cell">{artist.breakTime}</td>
                  <td className="table-cell">
                    <span className={`status-badge ${
                      artist.totalBooking > 30 ? 'success' : 
                      artist.totalBooking > 20 ? 'warning' : 
                      'danger'
                    }`}>
                      {artist.totalBooking}
                    </span>
                  </td>
                  <td className="table-cell">{artist.storeAddress}</td>
                  <td className="table-cell">
                    <span className={`status-badge ${artist.status ? 'success' : 'danger'}`}>
                      {artist.status ? 'Available' : 'Unavailable'}
                    </span>
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